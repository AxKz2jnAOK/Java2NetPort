using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UniversityDataAccess;

namespace Java2NetPort.Tests
{
    [TestClass]
    public class CriteriaApi
    {
        [ClassInitialize]
        public static void TestClassInit(TestContext textContext)
        {
            Database.SetInitializer(new UniversityInitializer());

            EJBContainer c = EJBContainer.Instance;
            c.Configuration.SetDBContextCreationFuncForEntityManager(() => new UniversityContext());
            c.Init();

            using (UniversityContext context = new UniversityContext())
            {
                foreach (Student s in context.Students)
                {
                    context.Students.Remove(s);
                }
                context.SaveChanges();
            }
        }

        [Ignore]
        [TestMethod]
        public void CreateCriteria()
        {
            EJBContainer c = EJBContainer.Instance;
            c.Configuration.SetFullAssebliesNamesForSessionBeans(new[] { "abc" });
            c.Init();
            Assert.IsNotNull(c.EntityManager);
        }
    }
}
