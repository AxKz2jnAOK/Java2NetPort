using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using UniversityDataAccess;
using System.Diagnostics;

namespace Java2NetPort.Tests
{
    [TestClass]
    public class EJBContainerTests
    {
        [TestInitialize]
        public void TestsInit()
        {
        }

        [ClassInitialize]
        public static void TestClassInit(TestContext textContext)
        {
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));

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

        [TestMethod]
        public void GetEntityManager_ReturnNotNull()
        {
            EJBContainer c = EJBContainer.Instance;
            c.Configuration.SetFullAssebliesNamesForSessionBeans(new[] { "abc" });
            c.Init();
            Assert.IsNotNull(c.EntityManager);
        }

        [TestMethod]
        public void CreateEJBContainer()
        {
            EJBContainer c = EJBContainer.Instance;
            Assert.IsTrue(c != null);
        }

        [TestMethod]
        public void InitEJBContainerEntityManagerDataContext()
        {
            EJBContainer c = EJBContainer.Instance;
            c.Configuration.SetDBContextCreationFuncForEntityManager(()=>new UniversityContext());
            c.Init();
        }
    }
}
