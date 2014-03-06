using Java2NetPort.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using UniversityBusinessLogic.EJB.Stateless;
using UniversityDataAccess;
using System.Data.Entity;

namespace Java2NetPort.Tests
{
    //java example: http://www.javabeat.net/managing-transactions-in-ejb-3-0/
    [TestClass]
    public class ContainerManagedTransactions
    {
        [ClassInitialize]
        public static void TestClassInit(TestContext textContext)
        {
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));

            Trace.WriteLine("Executing ContainerManagedTransactions::TestClassInit(..)");

            Database.SetInitializer(new UniversityInitializer());

            //EJBContainer c = EJBContainer.Instance;
            //c.Configuration.SetDBContextCreationFuncForEntityManager(() => new UniversityContext());
            //c.Init();

            //using (UniversityContext context = new UniversityContext())
            //{
            //    foreach (Student s in context.Students)
            //    {
            //        context.Students.Remove(s);
            //    }

            //    foreach (Log l in context.Logs)
            //    {
            //        context.Logs.Remove(l);
            //    }

            //    context.SaveChanges();
            //}
        }

        [TestInitialize]
        public void TestsInit()
        {
            Trace.WriteLine("Executing ContainerManagedTransactions::TestsInit(..)");
            
            EJBContainer c = EJBContainer.Instance;
            c.Configuration.SetDBContextCreationFuncForEntityManager(() => new UniversityContext());
            c.Init();

            using (UniversityContext context = new UniversityContext())
            {
                foreach (Student s in context.Students)
                {
                    context.Students.Remove(s);
                }

                foreach (Log l in context.Logs)
                {
                    context.Logs.Remove(l);
                }

                context.SaveChanges();
            }
        }

        #region HelperMethods
        
        public int HowManyActiveTransactionExistsInSqlServer()
        {
            int result = 0;
            using(TransactionScope ts = new TransactionScope(TransactionScopeOption.Suppress))
            {
                using(SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Magistrinis.Tests.UniversityContext"].ToString()))
                {
                    using(SqlCommand command = new SqlCommand("SELECT count(*) FROM sys.dm_tran_session_transactions", con))
                    {
                        result = (int)command.ExecuteScalar();
                    }
                }
            }
            return result;
        }

        #endregion



        /// <summary>           
        /// Nesant tranzakcijai iškviečiamas metodas su TransactionAttributeType.MANDATORY atributu. Turibūti išmetama TransactionRequiredException
        /// MANDATORY   parent transaction: No javax.ejb.EJBTransactionRequiredException is thrown
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TransactionRequiredException))]
       public void Test_MandatoryAttribute_ThrowException()
       {   
            DummyManagerForTesting m = new DummyManagerForTesting();
            m.Mandatory();
       }

        
        [TestMethod]
        public void Request_Active_transaction_should_exist_insideTheMethod()
        {
            //Kviečiamas metodas su request atributu. vidutj patikrinama ar yra aktyvi tranzakcija.
            DummyManagerForTesting m = new DummyManagerForTesting();
            m.Required();

            Assert.IsNull(System.Transactions.Transaction.Current);
        }

        [TestMethod]
        public void InsertNewStudentInRequestTransactionScope_ShouldExistInDbAfterMethodCall()
        {
            Trace.WriteLine("Executing ContainerManagedTransactions::InsertNewStudentInRequestTransactionScope_ShouldExistInDbAfterMethodCall()");

            try
            {
                //EJBContainer c = EJBContainer.Instance;
                //c.Configuration.SetDBContextCreationFuncForEntityManager(() => new UniversityContext());
                //c.Init();

                DummyManagerForTesting m = new DummyManagerForTesting();
                m.Required_InsertNewStudent();

                Assert.IsNull(System.Transactions.Transaction.Current);

                UniversityContext uc = new UniversityContext();
                Assert.IsNotNull(uc.Students.SingleOrDefault(e => e.Id == 2
                    && e.FirstName == "Vardenis"
                    && e.LastName == "Pavardenis"
                    && e.BirthDay == new DateTime(1990, 01, 01)));
            }
            catch(Exception exc)
            {
                throw;
            }
        }

        /// <summary>
        /// Yes                             Method joins the callers transaction
        /// </summary>
        [TestMethod]
        public void InsertNewStudentInSecondRequestTransactionMethod()
        {
            //iškviečiamas metodas su request atributu. sukuriama tranzakcija.
            //iškviečiam request metodas, jis įrašo duomenis. grįžus iš metodo, pirmame metode patikriname ar objektas egzistuoja DB - turi neegzistuot.
            //Po to kai grįžtama iš antrojo metodo, tuomet turi jau būti objektas įrašytas į DB.
            DummyManagerForTesting m = new DummyManagerForTesting();
            m.Required_Required_InsertNewStudent();


            Assert.IsNull(System.Transactions.Transaction.Current);

            UniversityContext uc = new UniversityContext();
            Assert.IsNotNull(uc.Students.SingleOrDefault(e => e.Id == 2
                && e.FirstName == "Vardenis"
                && e.LastName == "Pavardenis"
                && e.BirthDay == new DateTime(1990, 01, 01)));
        }


        [TestMethod]
        public void RequestNew_Active_transaction_should_exist_insideTheMethod()
        {
            //Kviečiamas metodas su requestNew atributu. viduj patikrinama ar yra aktyvi tranzakcija.
            DummyManagerForTesting m = new DummyManagerForTesting();
            m.RequiredNew();

            Assert.IsNull(System.Transactions.Transaction.Current);
        }

        //Container creates a new transaction and the callers transaction is suspended 
        [TestMethod]
        public void WhenInnerRequestNewIsCalled_ItShouldBeCommitedIndependentlyOfTheOutterTransaction()
        {
            DummyManagerForTesting m = new DummyManagerForTesting();
            m.Required_RequiredNew_InsertNewStudent();

            //Also tests:
            /*
             * [ ] New test: //patikrinti ar kai naudojamas request ir requestNew, request irgi veikia teisingai.
             *      Request_InsertNewStudent (id:3)
             *           RequestNew_InsertNewStudent (id:2)
             *           should not exist id:2
             *      should exist id:2
             *      Should not exist id3
             *  should exist id:3*/

            UniversityContext uc = new UniversityContext();
            Assert.IsNotNull(uc.Students.SingleOrDefault(e => e.Id == 3
                && e.FirstName == "Vardenis3"
                && e.LastName == "Pavardenis3"
                && e.BirthDay == new DateTime(1991, 01, 01)));

        }

        //Container creates a new transaction and the callers transaction is suspended 
        [TestMethod]
        public void WhenInnerRequestNewIsCalled_ItShouldBeCommitedIndependentlyOfTheOutterTransaction2()
        {
            DummyManagerForTesting m = new DummyManagerForTesting();
            m.Required_RequiredNew_InsertNewStudent2();

            //Also tests:
            /*
             * [ ] New test: //patikrinti ar kai naudojamas request ir requestNew, request irgi veikia teisingai.
             *      Request_InsertNewStudent 
             *           RequestNew_InsertNewStudent (id:2)
             *           should not exist id:2             *           
             *      should exist id:2
             *      student (id:3) inserted
             *      Should not exist id3
             *  should exist id:3*/

            UniversityContext uc = new UniversityContext();
            Assert.IsNotNull(uc.Students.SingleOrDefault(e => e.Id == 3
                && e.FirstName == "Vardenis3"
                && e.LastName == "Pavardenis3"
                && e.BirthDay == new DateTime(1991, 01, 01)));

        }


        /// <summary>
        /// tests support attribute
        /// </summary>
        [TestMethod]
        public void WhenInsertsStudentWithSupportAttribute_NoActiveTransactionExists()
        {
            DummyManagerForTesting m = new DummyManagerForTesting();

            m.Supports_NoAmbientTransaction_InsertNewStudent();

            UniversityContext uc = new UniversityContext();
            Assert.IsNotNull(uc.Students.SingleOrDefault(e => e.Id == 2
                && e.FirstName == "Vardenis"
                && e.LastName == "Pavardenis"
                && e.BirthDay == new DateTime(1990, 01, 01)));
        }

        [TestMethod]
        public void WhenInsertsStudentWithSupportAttribute_JoinsAmbientTransactionIfItExists()
        {
            DummyManagerForTesting m = new DummyManagerForTesting();

            m.Required_Supports_InsertNewStudent();

            UniversityContext uc = new UniversityContext();
            Assert.IsNotNull(uc.Students.SingleOrDefault(e => e.Id == 2
                && e.FirstName == "Vardenis"
                && e.LastName == "Pavardenis"
                && e.BirthDay == new DateTime(1990, 01, 01)));
        }


        [TestMethod]
        public void InsertNewStudentWithNotSupport_NoAmbientTransaction_ShouldInsertButWithoutActiveTransaction()
        {
            DummyManagerForTesting m = new DummyManagerForTesting();

            m.NotSupports_InsertNewStudent();

            UniversityContext uc = new UniversityContext();
            Assert.IsNotNull(uc.Students.SingleOrDefault(e => e.Id == 2
                && e.FirstName == "Vardenis2"
                && e.LastName == "Pavardenis2"
                && e.BirthDay == new DateTime(1990, 01, 02)));
        }

        [TestMethod]
        public void InsertNewStudentWithNotSupport_WithAmbientTransaction_ShouldInsertButWithoutActiveTransaction()
        {
            DummyManagerForTesting m = new DummyManagerForTesting();

            m.Requried_NotSupports_InsertNewStudent();

            UniversityContext uc = new UniversityContext();
            Assert.IsNotNull(uc.Students.SingleOrDefault(e => e.Id == 3
                && e.FirstName == "Vardenis3"
                && e.LastName == "Pavardenis3"
                && e.BirthDay == new DateTime(1990, 01, 03)));
        }

        //new test:
        /*
         * [ ] New test: //patikrinti ar kai naudojamas request ir requestNew, request irgi veikia teisingai.
         *      Request_InsertNewStudent (id:3)
         *           RequestNew_InsertNewStudent (id:2)
         *           should not exist id:2
         *      should exist id:2
         *      Should not exist id3
         *  should exist id:3*/

        /*
//        The defined transaction attributes choices are as follows:

        //• MANDATORY: If this attribute is specified for a method, a transaction is expected to
//have already been started and be active when the method is called. If no
//transaction is active, an exception is thrown. This attribute is seldom used, but
//can be a development tool to catch transaction demarcation errors when it is
//expected that a transaction should already have been started.

        //• REQUIRED: This attribute is the most common case in which a method is expected to
//be in a transaction. The container provides a guarantee that a transaction is
//active for the method. If one is already active, it is used; if one does not exist, a
//new transaction is created for the method execution.
        //• REQUIRES_NEW: This attribute is used when the method always needs to be in its
//own transaction; that is, the method should be committed or rolled back
//independently of methods further up the call stack. It should be used with caution
//because it can lead to excessive transaction overhead.
         
         NOT_SUPPORTED: A method marked to not support transactions will cause the
container to suspend the current transaction if one is active when the method is
called. It implies that the method does not perform transactional operations, but
might fail in other ways that could undesirably affect the outcome of a
transaction. This is not a commonly used attribute.

         * 
         * • NEVER: A method marked to never support transactions will cause the container to
throw an exception if a transaction is active when the method is called. This
attribute is very seldom used, but can be a development tool to catch transaction
demarcation errors when it is expected that transactions should already have
been completed.
         */
        //---------------------------------------------------------------------------
        /*
        Transaction Attribute       |       Caller Transaction Exists   |   Effect    
          REQUIRED                          No                              Container creates a new transaction 
                                            Yes                             Method joins the callers transaction 
         * REQUIRES_NEW                     No                              Container creates a new transaction 
         *                                  Yes                             Container creates a new transaction and the callers transaction is suspended 
         *  SUPPORTS 	                    No                              No transaction is used
  	                                        Yes                             Method joins the callers transaction
         * MANDATORY 	                    No                              javax.ejb.EJBTransactionRequiredException is thrown
         *                                  Yes                             Method joins the callers transaction
            NOT_SUPPORTED 	                No                              No transaction is used
         *                                  Yes                             The callers transaction is suspended and the method is called without a transaction
         NEVER 	                            No                              No transaction is used
                                            Yes                             javax.ejb.EJBException is thrown 
         * 
         * + testai, kai patys metodai (viduje) išmeta exception'us.
         * */

        [Ignore]
        [TestMethod]
        public void TestTransactionScopes()
        {
            using(var ts1 = new TransactionScope())
            {
                using (UniversityContext uda1 = new UniversityContext())
                {
                    Student s = new Student();
                    s.Id = 3;
                    s.BirthDay = new DateTime(1900, 01, 01);
                    s.FirstName = "jonas";
                    s.LastName = "Jonelis";

                    uda1.Students.Add(s);

                    uda1.SaveChanges();
                }

                using(var ts2 = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    using(UniversityContext uda2 = new UniversityContext())
                    {
                        Log l        = new Log();
                        l.Id         = 1;
                        l.InsertedAt = DateTime.Now;
                        l.Message    = "message txt";
                        l.Level      = "I";
                        uda2.Logs.Add(l);
                        uda2.SaveChanges();
                    }
                    ts2.Complete();
                }
            }
        }
    }
}
