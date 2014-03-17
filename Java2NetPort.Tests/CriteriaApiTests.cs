using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Java2NetPort.CriteriaAPI;
using Java2NetPort.CriteriaAPI.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UniversityDataAccess;

namespace Java2NetPort.Tests
{
    [TestClass]
    public class CriteriaApiTests
    {
        [ClassInitialize]
        public static void TestClassInit(TestContext textContext)
        {
            Database.SetInitializer(new UniversityInitializer());
        }

        [TestInitialize]
        public void TestInit()
        {
            EJBContainer c = EJBContainer.Instance;
            c.Configuration.SetDBContextCreationFuncForEntityManager(() => new UniversityContext());
            c.Init();
        }

        [TestMethod]
        public void CreateCriteriaBuilder_Success()
        {
            ICriteriaBuilder cb = EJBContainer.Instance.EntityManager.GetCriteriaBuilder();

            Assert.IsNotNull(cb);            
        }


        [TestMethod]
        public void CreateCriteriaQuery_Success()
        {
            ICriteriaQuery cq = EJBContainer.Instance.EntityManager.GetCriteriaBuilder()
                                                                    .CreateQuery();
            Assert.IsNotNull(cq);
        }

        [TestMethod]
        public void CreateRoot_Success()
        {
            IRoot r = EJBContainer.Instance.EntityManager.GetCriteriaBuilder()
                                                            .CreateQuery()
                                                            .From("Students");
            Assert.IsNotNull(r);
        }

        //todo: test without from
        //todo: test without select

        #region simple select

        [TestMethod]
        public void SelectOneFieldStatement_Success()
        {
            ICriteriaQuery cq = EJBContainer.Instance.EntityManager.GetCriteriaBuilder().CreateQuery();
            IRoot r = cq.From("Students");
            cq.Select(new Selection("FirstName"));

            Assert.AreEqual(cq.ExpressionValue, "SELECT FirstName FROM Students");            
        }

        [TestMethod]
        public void SelectOneFieldFromRootStatement_Success()
        {
            ICriteriaQuery cq = EJBContainer.Instance.EntityManager.GetCriteriaBuilder().CreateQuery();
            IRoot r = cq.From("Students");
            cq.Select(r.Get("FirstName"));

            Assert.AreEqual(cq.ExpressionValue, "SELECT Students.FirstName FROM Students");
        }

        [TestMethod]
        public void SelectMultipleFieldsStatement_Success()
        {
            ICriteriaQuery cc = EJBContainer.Instance.EntityManager.GetCriteriaBuilder().CreateQuery();
            IRoot r = cc.From("Students");
            cc.Select("FirstName", "LastName");

            string s = cc.ToString();

            Assert.AreEqual(s, "SELECT FirstName, LastName FROM Students");
        }

        [TestMethod]
        public void SelectFieldWithAliasStatement_Success()
        {
            ICriteriaQuery cc = EJBContainer.Instance.EntityManager.GetCriteriaBuilder().CreateQuery();
            IRoot r = cc.From("Students");
            cc.Select(r.Get("FirstName").Alias("Name"));

            string s = cc.ToString();

            Assert.AreEqual(s, "SELECT Students.FirstName AS Name FROM Students");
        }

        [TestMethod]
        public void SelectAllFieldsStatement_Success()
        {
            ICriteriaQuery cc = EJBContainer.Instance.EntityManager.GetCriteriaBuilder().CreateQuery();
            IRoot r = cc.From("Students");
            cc.Select("*");

            string s = cc.ToString();

            Assert.AreEqual(s, "SELECT * FROM Students");
        }

        [TestMethod]
        public void SelectAllFieldsFromRootStatement_Success()
        {
            ICriteriaQuery cq = EJBContainer.Instance.EntityManager.GetCriteriaBuilder().CreateQuery();
            IRoot r = cq.From("Students");
            cq.Select(r.Get("*"));

            Assert.AreEqual(cq.ExpressionValue, "SELECT Students.* FROM Students");
        }

        [TestMethod]
        public void DistinctSelectOneFieldStatement_Success()
        {
            ICriteriaQuery cq = EJBContainer.Instance.EntityManager.GetCriteriaBuilder().CreateQuery();
            IRoot r = cq.From("Students");
            cq.Select("FirstName").Distinct(true);

            Assert.AreEqual(cq.ExpressionValue, "SELECT DISTINCT FirstName FROM Students");
        }

        [TestMethod]
        public void DistinctSelectMultipleFieldsStatement_Success()
        {
            ICriteriaQuery cc = EJBContainer.Instance.EntityManager.GetCriteriaBuilder().CreateQuery();
            IRoot r = cc.From("Students");
            cc.Select("FirstName", "LastName").Distinct(true);

            string s = cc.ToString();

            Assert.AreEqual(s, "SELECT DISTINCT FirstName, LastName FROM Students");
        }

        [TestMethod]
        public void SelectMultipleRootsStatement_Success()
        {
            ICriteriaQuery cc = EJBContainer.Instance.EntityManager.GetCriteriaBuilder().CreateQuery();
            IRoot r = cc.From("Students");
            IRoot r2 = cc.From("Faculties");
            cc.Select("FirstName", "LastName");

            string s = cc.ToString();

            Assert.AreEqual(s, "SELECT FirstName, LastName FROM Students, Faculties");
        }

        [TestMethod]
        public void SelectMultipleFieldsMultipleRootsStatement_Success()
        {
            ICriteriaQuery cc = EJBContainer.Instance.EntityManager.GetCriteriaBuilder().CreateQuery();
            IRoot r = cc.From("Students");
            IRoot r2 = cc.From("Faculties");
            cc.Select(r.Get("FirstName"), r2.Get("Name"));

            string s = cc.ToString();

            Assert.AreEqual(s, "SELECT Students.FirstName, Faculties.Name FROM Students, Faculties");
        }

        #endregion
        
        #region joins

        [TestMethod]
        public void InnerJoin()
        {
            ICriteriaBuilder cb = EJBContainer.Instance.EntityManager.GetCriteriaBuilder();
            ICriteriaQuery cc = cb.CreateQuery();
            
            IRoot r = cc.From("Students");
            
            IJoin facultiesJoin = r.Join("Faculties", JoinType.Inner, "Faculty_Id", "Id");

            cc.Select(r.Get("Firstname"), facultiesJoin.Get("Name"));

            Assert.AreEqual("SELECT Students.Firstname, Faculties.Name FROM Students INNER JOIN Faculties ON Students.Faculty_Id = Faculties.Id", cc.ExpressionValue);
        }

        [TestMethod]
        public void LeftJoin()
        {
            ICriteriaBuilder cb = EJBContainer.Instance.EntityManager.GetCriteriaBuilder();
            ICriteriaQuery cc = cb.CreateQuery();

            IRoot r = cc.From("Students");

            IJoin facultiesJoin = r.Join("Faculties", JoinType.Left, "Faculty_Id", "Id");

            cc.Select(r.Get("Firstname"), facultiesJoin.Get("Name"));

            Assert.AreEqual("SELECT Students.Firstname, Faculties.Name FROM Students LEFT JOIN Faculties ON Students.Faculty_Id = Faculties.Id", cc.ExpressionValue);
        }

        [TestMethod]
        public void RightJoin()
        {
            ICriteriaBuilder cb = EJBContainer.Instance.EntityManager.GetCriteriaBuilder();
            ICriteriaQuery cc = cb.CreateQuery();

            IRoot r = cc.From("Students");

            IJoin facultiesJoin = r.Join("Faculties", JoinType.Right, "Faculty_Id", "Id");

            cc.Select(r.Get("Firstname"), facultiesJoin.Get("Name"));

            Assert.AreEqual("SELECT Students.Firstname, Faculties.Name FROM Students RIGHT JOIN Faculties ON Students.Faculty_Id = Faculties.Id", cc.ExpressionValue);
        }

        #endregion

        #region criteria builder. building expressions

        [TestMethod]
        public void And()
        {
            ICriteriaBuilder cb = EJBContainer.Instance.EntityManager.GetCriteriaBuilder();

            ICriteriaQuery cc = cb.CreateQuery();

            IRoot r = cc.From("Students");

            IPredicate left = cb.Equal(r.Get("Name") , cb.Literal("'Jonas'"));

            IPredicate right = cb.Equal(r.Get("Faculty_Id"), cb.Literal("3"));

            IExpression expr = cb.And(left, right);

            Assert.AreEqual("Students.Name = 'Jonas' AND Students.Faculty_Id = 3", expr.ExpressionValue);
        }

        [TestMethod]
        public void Or()
        {
            ICriteriaBuilder cb = EJBContainer.Instance.EntityManager.GetCriteriaBuilder();

            ICriteriaQuery cc = cb.CreateQuery();

            IRoot r = cc.From("Students");

            IPredicate left = cb.Equal(r.Get("Name"), cb.Literal("'Jonas'"));

            IPredicate right = cb.Equal(r.Get("Faculty_Id"), cb.Literal("3"));

            IExpression expr = cb.Or(left, right);

            Assert.AreEqual("Students.Name = 'Jonas' OR Students.Faculty_Id = 3", expr.ExpressionValue);
        }

        [TestMethod]
        public void EqualSingleParamWithQuotes()
        {
            ICriteriaBuilder cb = EJBContainer.Instance.EntityManager.GetCriteriaBuilder();
            ICriteriaQuery cq = cb.CreateQuery();
            IRoot r = cq.From("Students");

            IPredicate exp = cb.Equal(r.Get("FirstName"), cb.Literal("'Jonas'"));

            Assert.AreEqual("Students.FirstName = 'Jonas'", exp.ExpressionValue);
        }

        [TestMethod]
        public void EqualWithQuotes()
        {
            ICriteriaBuilder cb = EJBContainer.Instance.EntityManager.GetCriteriaBuilder();
            ICriteriaQuery cq = cb.CreateQuery();
            IRoot r = cq.From("Students");

            IPredicate exp = cb.Equal(cb.Literal("'FirstName'"), cb.Literal("'Jonas'"));

            Assert.AreEqual("'FirstName' = 'Jonas'", exp.ExpressionValue);
        }

        [TestMethod]
        public void EqualWithoutQuotes()
        {
            ICriteriaBuilder cb = EJBContainer.Instance.EntityManager.GetCriteriaBuilder();
            ICriteriaQuery cq = cb.CreateQuery();
            IRoot r = cq.From("Students");

            IPredicate exp = cb.Equal(r.Get("FirstName"), r.Get("LastName"));

            Assert.AreEqual("Students.FirstName = Students.LastName", exp.ExpressionValue);
        }
        
        [TestMethod]
        public void NotEqual_Success()
        {
            ICriteriaBuilder cb = EJBContainer.Instance.EntityManager.GetCriteriaBuilder();
            ICriteriaQuery cq = cb.CreateQuery();
            IRoot r = cq.From("Students");

            IPredicate exp = cb.NotEqual(r.Get("SequenceNumber"), cb.Literal("2"));

            Assert.AreEqual("Students.SequenceNumber <> 2", exp.ExpressionValue);
        }

        [TestMethod]
        public void GreaterThen_Success()
        {
            ICriteriaBuilder cb = EJBContainer.Instance.EntityManager.GetCriteriaBuilder();
            ICriteriaQuery cq = cb.CreateQuery();
            IRoot r = cq.From("Students");

            IPredicate exp = cb.GreaterThen(r.Get("SequenceNumber"), cb.Literal("2"));

            Assert.AreEqual("Students.SequenceNumber > 2", exp.ExpressionValue);
        }

        [TestMethod]
        public void GreaterOrEqualThen_Success()
        {
            ICriteriaBuilder cb = EJBContainer.Instance.EntityManager.GetCriteriaBuilder();
            ICriteriaQuery cq = cb.CreateQuery();
            IRoot r = cq.From("Students");

            IPredicate exp = cb.GreaterOrEqualThen(r.Get("SequenceNumber"), cb.Literal("2"));

            Assert.AreEqual("Students.SequenceNumber >= 2", exp.ExpressionValue);
        }

        [TestMethod]
        public void LessThen_Success()
        {
            ICriteriaBuilder cb = EJBContainer.Instance.EntityManager.GetCriteriaBuilder();
            ICriteriaQuery cq = cb.CreateQuery();
            IRoot r = cq.From("Students");

            IPredicate exp = cb.LessThen(r.Get("SequenceNumber"), cb.Literal("2"));

            Assert.AreEqual("Students.SequenceNumber < 2", exp.ExpressionValue);
        }

        [TestMethod]
        public void LessOrEqualThen_Success()
        {
            ICriteriaBuilder cb = EJBContainer.Instance.EntityManager.GetCriteriaBuilder();
            ICriteriaQuery cq = cb.CreateQuery();
            IRoot r = cq.From("Students");

            IPredicate exp = cb.LessOrEqualThen(r.Get("SequenceNumber"), cb.Literal("2"));

            Assert.AreEqual("Students.SequenceNumber <= 2", exp.ExpressionValue);
        }

        [TestMethod]
        public void Between_Success()
        {
            ICriteriaBuilder cb = EJBContainer.Instance.EntityManager.GetCriteriaBuilder();
            ICriteriaQuery cq = cb.CreateQuery();
            IRoot r = cq.From("Students");

            IPredicate exp = cb.Between(r.Get("SequenceNumber"), cb.Literal("2"), cb.Literal("3"));

            Assert.AreEqual("Students.SequenceNumber BETWEEN 2 AND 3", exp.ExpressionValue);
        }

        [TestMethod]
        public void Like_Success()
        {
            ICriteriaBuilder cb = EJBContainer.Instance.EntityManager.GetCriteriaBuilder();
            ICriteriaQuery cq = cb.CreateQuery();
            IRoot r = cq.From("Students");

            IPredicate exp = cb.Like(r.Get("FirstName"), cb.Literal("'Jon%'"));

            Assert.AreEqual("Students.FirstName LIKE 'Jon%'", exp.ExpressionValue);
        }

        [TestMethod]
        public void Not()
        {
            ICriteriaBuilder cb = EJBContainer.Instance.EntityManager.GetCriteriaBuilder();
            ICriteriaQuery cq = cb.CreateQuery();
            IRoot r = cq.From("Students");

            IPredicate exp = cb.Equal(r.Get("SequenceNumber"), cb.Literal("2"));

            IPredicate pred = cb.Not(exp);

            Assert.AreEqual("NOT (Students.SequenceNumber = 2)", pred.ExpressionValue);
        }

        [TestMethod]
        public void IsNull()
        {
            ICriteriaBuilder cb = EJBContainer.Instance.EntityManager.GetCriteriaBuilder();
            ICriteriaQuery cq = cb.CreateQuery();
            IRoot r = cq.From("Students");

            IPredicate exp = cb.IsNull(r.Get("LastName"));

            Assert.AreEqual("Students.LastName IS NULL", exp.ExpressionValue);
        }

        [TestMethod]
        public void IsNotNull()
        {
            ICriteriaBuilder cb = EJBContainer.Instance.EntityManager.GetCriteriaBuilder();
            ICriteriaQuery cq = cb.CreateQuery();
            IRoot r = cq.From("Students");

            IPredicate exp = cb.IsNotNull(r.Get("LastName"));

            Assert.AreEqual("Students.LastName IS NOT NULL", exp.ExpressionValue);
        }

        [TestMethod]
        public void NotLike_Success()
        {
            ICriteriaBuilder cb = EJBContainer.Instance.EntityManager.GetCriteriaBuilder();
            ICriteriaQuery cq = cb.CreateQuery();
            IRoot r = cq.From("Students");

            IPredicate exp = cb.NotLike(r.Get("FirstName"), cb.Literal("'Jon%'"));

            Assert.AreEqual("Students.FirstName NOT LIKE 'Jon%'", exp.ExpressionValue);
        }

        [TestMethod]
        public void ParameterExpressoin_StringType()
        {
            ICriteriaBuilder cb = EJBContainer.Instance.EntityManager.GetCriteriaBuilder();
            ICriteriaQuery cq = cb.CreateQuery();
            IRoot r = cq.From("Students");

            IParameterExpression pr = cb.Parameter(System.Data.SqlDbType.VarChar, "Jonas");

            IPredicate exp = cb.Equal(r.Get("FirstName"), pr);

            Assert.AreEqual("Students.FirstName = 'Jonas'", exp.ExpressionValue);
        }

        [TestMethod]
        public void ParameterExpressoin_IntType()
        {
            ICriteriaBuilder cb = EJBContainer.Instance.EntityManager.GetCriteriaBuilder();
            ICriteriaQuery cq = cb.CreateQuery();
            IRoot r = cq.From("Students");

            IParameterExpression pr = cb.Parameter(System.Data.SqlDbType.Int, "3");

            IPredicate exp = cb.Equal(r.Get("SequenceNumber"), pr);

            Assert.AreEqual("Students.SequenceNumber = 3", exp.ExpressionValue);
        }

        #endregion

        #region PredicateExpression

        [TestMethod]
        public void Conjunction()
        {
            ICriteriaBuilder cb = EJBContainer.Instance.EntityManager.GetCriteriaBuilder();
            ICriteriaQuery cq = cb.CreateQuery();
            IRoot r = cq.From("Students");

            IPredicate criteria = cb.Conjunction();

            IParameterExpression pr = cb.Parameter(System.Data.SqlDbType.Int, "3");
            criteria = cb.And(criteria, 
                cb.Equal(r.Get("SequenceNumber"), pr)
                );

            pr = cb.Parameter(System.Data.SqlDbType.Int, "'Jonas'");

            criteria = cb.And(criteria,
                cb.Equal(r.Get("FirstName"), pr)
                );

            pr = cb.Parameter(System.Data.SqlDbType.Int, "'Petraitis'");

            criteria = cb.And(criteria,
                cb.Equal(r.Get("LastName"), pr)
                );

            Assert.AreEqual("Students.SequenceNumber = 3 AND Students.FirstName = 'Jonas' AND Students.LastName = 'Petraitis'", criteria.ExpressionValue);
        }

        [TestMethod]
        public void Disjunction()
        {
            ICriteriaBuilder cb = EJBContainer.Instance.EntityManager.GetCriteriaBuilder();
            ICriteriaQuery cq = cb.CreateQuery();
            IRoot r = cq.From("Students");

            IPredicate criteria = cb.Disjunction();

            IParameterExpression pr = cb.Parameter(System.Data.SqlDbType.Int, "3");
            criteria = cb.Or(criteria,
                cb.Equal(r.Get("SequenceNumber"), pr)
                );

            pr = cb.Parameter(System.Data.SqlDbType.Int, "'Jonas'");

            criteria = cb.Or(criteria,
                cb.Equal(r.Get("FirstName"), pr)
                );

            pr = cb.Parameter(System.Data.SqlDbType.Int, "'Petraitis'");

            criteria = cb.Or(criteria,
                cb.Equal(r.Get("LastName"), pr)
                );

            Assert.AreEqual("Students.SequenceNumber = 3 OR Students.FirstName = 'Jonas' OR Students.LastName = 'Petraitis'", criteria.ExpressionValue);
        }

        #endregion

        #region Order by

        [TestMethod]
        public void OrderByOneFieldDesc()
        {
            ICriteriaBuilder cb = EJBContainer.Instance.EntityManager.GetCriteriaBuilder();
            ICriteriaQuery cc = cb.CreateQuery();
            IRoot r = cc.From("Students");
            cc.Select(r.Get("FirstName"), r.Get("LastName"));

            cc.OrderBy(cb.Desc(r.Get("FirstName")));

            Assert.AreEqual("SELECT Students.FirstName, Students.LastName FROM Students ORDER BY Students.FirstName DESC", cc.ExpressionValue);
        }

        [TestMethod]
        public void OrderByOneFieldAsc()
        {
            ICriteriaBuilder cb = EJBContainer.Instance.EntityManager.GetCriteriaBuilder();
            ICriteriaQuery cc = cb.CreateQuery();
            IRoot r = cc.From("Students");
            cc.Select(r.Get("FirstName"), r.Get("LastName"));

            cc.OrderBy(cb.Asc(r.Get("FirstName")));

            Assert.AreEqual("SELECT Students.FirstName, Students.LastName FROM Students ORDER BY Students.FirstName ASC", cc.ExpressionValue);
        }

        [TestMethod]
        public void OrderByMultipleFieldsDesc()
        {
            ICriteriaBuilder cb = EJBContainer.Instance.EntityManager.GetCriteriaBuilder();
            ICriteriaQuery cc = cb.CreateQuery();
            IRoot r = cc.From("Students");
            cc.Select(r.Get("FirstName"), r.Get("LastName"));

            cc.OrderBy(cb.Desc(r.Get("FirstName")), cb.Desc(r.Get("LastName")));

            Assert.AreEqual("SELECT Students.FirstName, Students.LastName FROM Students ORDER BY Students.FirstName DESC, Students.LastName DESC", cc.ExpressionValue);
        }


        [TestMethod]
        public void OrderByMultipleFieldsAsc()
        {
            ICriteriaBuilder cb = EJBContainer.Instance.EntityManager.GetCriteriaBuilder();
            ICriteriaQuery cc = cb.CreateQuery();
            IRoot r = cc.From("Students");
            cc.Select(r.Get("FirstName"), r.Get("LastName"));

            cc.OrderBy(cb.Asc(r.Get("FirstName")), cb.Asc(r.Get("LastName")));

            Assert.AreEqual("SELECT Students.FirstName, Students.LastName FROM Students ORDER BY Students.FirstName ASC, Students.LastName ASC", cc.ExpressionValue);
        }

        [TestMethod]
        public void OrderByMultipleFieldsAscDesc()
        {

            ICriteriaBuilder cb = EJBContainer.Instance.EntityManager.GetCriteriaBuilder();
            ICriteriaQuery cc = cb.CreateQuery();
            IRoot r = cc.From("Students");
            cc.Select(r.Get("FirstName"), r.Get("LastName"));

            cc.OrderBy(cb.Asc(r.Get("FirstName")), cb.Desc(r.Get("LastName")));

            Assert.AreEqual("SELECT Students.FirstName, Students.LastName FROM Students ORDER BY Students.FirstName ASC, Students.LastName DESC", cc.ExpressionValue);
        }

        [TestMethod]
        public void OrderByMultipleFieldsDescAsc()
        {
            ICriteriaBuilder cb = EJBContainer.Instance.EntityManager.GetCriteriaBuilder();
            ICriteriaQuery cc = cb.CreateQuery();
            IRoot r = cc.From("Students");
            cc.Select(r.Get("FirstName"), r.Get("LastName"));

            cc.OrderBy(cb.Desc(r.Get("FirstName")), cb.Asc(r.Get("LastName")));

            Assert.AreEqual("SELECT Students.FirstName, Students.LastName FROM Students ORDER BY Students.FirstName DESC, Students.LastName ASC", cc.ExpressionValue);
        }

        #endregion

        #region where 

        [TestMethod]
        public void SelectWhere()
        {
            ICriteriaBuilder cb = EJBContainer.Instance.EntityManager.GetCriteriaBuilder();
            ICriteriaQuery cc = cb.CreateQuery();
            IRoot r = cc.From("Students");
            cc.Select(r.Get("FirstName"));

            cc.Where(cb.Equal(r.Get("FirstName"), cb.Literal("'Jonas'")));

            Assert.AreEqual("SELECT Students.FirstName FROM Students WHERE Students.FirstName = 'Jonas'", cc.ExpressionValue);
        }

        [TestMethod]
        public void SelecRenewtWhere()
        {
            ICriteriaBuilder cb = EJBContainer.Instance.EntityManager.GetCriteriaBuilder();
            ICriteriaQuery cc = cb.CreateQuery();
            IRoot r = cc.From("Students");
            cc.Select(r.Get("FirstName"));

            cc.Where(cb.Equal(r.Get("FirstName"), cb.Literal("'Jonas'")));
            cc.Where(cb.NotEqual(r.Get("LastName"), cb.Literal("'Kybartas'")));

            Assert.AreEqual("SELECT Students.FirstName FROM Students WHERE Students.LastName <> 'Kybartas'", cc.ExpressionValue);
        }

        #endregion
    }
}
