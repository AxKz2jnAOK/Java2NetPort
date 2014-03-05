using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using System.Linq;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using UniversityDataAccess;

namespace Java2NetPort.Tests
{
    [TestClass]
    public class EntityManagerTests
    {
        protected UniversityContext DbContext { get; set; }

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
    

        [TestInitialize]
        public void TestInit()
        {
            using (UniversityContext context = new UniversityContext())
            {
                Student s = new Student()
                {
                    Id = 1,
                    BirthDay = new DateTime(1980, 05, 05),
                    FirstName = "Petras",
                    LastName = "Petraitis"
                };

                context.Students.Add(s);

                context.SaveChanges();
            }
        }
        

        [TestCleanup]
        public void TestCleanup()
        {
            using (UniversityContext context = new UniversityContext())
            {
                foreach(Student s in context.Students)
                {
                    context.Students.Remove(s);
                }
                context.SaveChanges();
            }
        }


        [TestMethod]
        [Description("Synchronize the persistence context to the underlying database.")]
        public void FlushMethod()
        {
            //EJBContainer c = EJBContainer.Instance;
            //IEntytiManager em = c.EntityManager;

            //em.Flush();
            Assert.IsTrue(true);
        }

        //TODO: test ContextBoundObject not null


        [TestMethod]
        public void Find_Student_By_PK_1_OK()
        {            
            IEntytiManager em = new EntityManager(new UniversityContext());

            Student stud = em.Find<Student>(1);
            Assert.IsNotNull(stud);
            Assert.IsTrue(stud.Id == 1);
        }

        [TestMethod]
        public void Find_Student_By_PK_101_NULL()
        {
            IEntytiManager em = new EntityManager(new UniversityContext());

            Student stud = em.Find<Student>(101);
            Assert.IsNull(stud);
            
        }

        [TestMethod]
        public void Clear_All_entities_should_become_detached()
        {
            UniversityContext context = new UniversityContext();

            IEntytiManager em = new EntityManager(context);

            Student stud = em.Find<Student>(1);
            Assert.IsNotNull(stud);
            
            Assert.IsTrue(context.Entry<Student>(stud).State == EntityState.Unchanged);
            
            em.Clear();
            
            Assert.IsTrue(context.Entry<Student>(stud).State == EntityState.Detached);
        }

        #region Flush
        [TestMethod]
        public void ChangeENtity_ThenFlush_ShouldChangeInDB()
        {
            IEntytiManager em = new EntityManager(new UniversityContext());

            Student stud = em.Find<Student>(1);

            stud.FirstName = "Jonas";

            using(UniversityContext ctx = new UniversityContext())
            {
                Student s = ctx.Students.Where(e => e.Id == 1).Single();
                Assert.IsTrue(s.FirstName == "Petras");
            }

            em.Flush();


            using (UniversityContext ctx = new UniversityContext())
            {
                Student s = ctx.Students.Where(e => e.Id == 1).Single();
                Assert.IsTrue(s.FirstName == "Jonas");
            }
        }

        [TestMethod]
        [Ignore]
        public void Flush_TransactionRequiredException()
        {
            //Todo: pasžiūrėti ir kitus metodus, kurie gali mesti tranzakcijų exception'us.
            Assert.Inconclusive();
        }

        [TestMethod]
        [Ignore]
        public void Flush_PersistenceException()
        {
            Assert.Inconclusive();
        }
        #endregion

        [TestMethod]
        public void Contains_EntityGotFromEntityManager_Ok()
        {
            UniversityContext context = new UniversityContext();            
            
            IEntytiManager em = new EntityManager(context);
            Student student = em.Find<Student>(1);

            Assert.IsTrue(em.Contains<Student>(student));            
        }

        [TestMethod]
        public void Contains_EntityGotFromDbContext_Ok()
        {
            UniversityContext context = new UniversityContext();

            IEntytiManager em = new EntityManager(context);
            Student student = context.Students.Single(e => e.Id == 1);

            Assert.IsTrue(em.Contains<Student>(student));
        }

        [TestMethod]
        public void Contains_EntityGotFromDifferentDbContext_False()
        {
            IEntytiManager em = new EntityManager(new UniversityContext());
         
            using(UniversityContext context = new UniversityContext())
            {
                Student student = context.Students.Single(e => e.Id == 1);
                Assert.IsFalse(em.Contains<Student>(student));
            }
        }

        [TestMethod]
        public void Contains_EntityGotFromDifferentEntityManager_False()
        {
            IEntytiManager em = new EntityManager(new UniversityContext());
            Student student = em.Find<Student>(1);

            IEntytiManager em2 = new EntityManager(new UniversityContext());
            Student student2 = em2.Find<Student>(1);

            Assert.IsFalse(em.Contains<Student>(student2));
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Contains_BadArgumentSupplied__throws_IllegalArgumentException()
        {
            IEntytiManager em = new EntityManager(new UniversityContext());
            BadArgument ba = new BadArgument();
            em.Contains<BadArgument>(ba);
        }
        public class BadArgument {}


        [TestMethod]
        public void Detach_Entity()
        {
            UniversityContext context = new UniversityContext();

            IEntytiManager em = new EntityManager(context);

            Student stud = em.Find<Student>(1);
            Assert.IsTrue(context.Entry<Student>(stud).State == EntityState.Unchanged);

            em.Detach<Student>(stud);

            Assert.IsTrue(context.Entry<Student>(stud).State == EntityState.Detached);
        }


        [TestMethod]
        [Ignore]
        public void GetReference()
        {
            UniversityContext context = new UniversityContext();

            context.Configuration.LazyLoadingEnabled = true;

            IEntytiManager em = new EntityManager(context);

            Student stud = em.GetReference<Student>(1);
            
            Assert.IsTrue(stud.Id == 1
                && stud.BirthDay == DateTime.MinValue
                && stud.FirstName == string.Empty
                && stud.LastName == string.Empty);
        }

        #region persist
        
        [TestMethod]
        public void Persist()
        {
            UniversityContext context = new UniversityContext();
            IEntytiManager em = new EntityManager(context);

            Student stud2 = new Student();
            stud2.Id = 2;
            stud2.FirstName = "Robertas";
            stud2.LastName = "Garteris";

            Assert.IsTrue(context.Entry<Student>(stud2).State == EntityState.Detached);
            
            em.Persist(stud2);

            Assert.IsTrue(context.Entry<Student>(stud2).State == EntityState.Added);
        }

        #endregion

        [ExpectedException(typeof(DbUpdateException))]
        [TestMethod]
        public void Persist_EntityAlreadyExists_Exception()
        {
            UniversityContext context = new UniversityContext();
            IEntytiManager em = new EntityManager(context);

            Student stud2 = new Student();
            stud2.Id = 1;
            stud2.BirthDay = DateTime.Now;
            
            em.Persist(stud2);

            context.SaveChanges();
        }


        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void Persist_BadArgument_Exception()
        {
            UniversityContext context = new UniversityContext();
            IEntytiManager em = new EntityManager(context);

            BadArgument ba = new BadArgument();

            em.Persist(ba);

            context.SaveChanges();
        }

        #region Refresh

        [TestMethod]
        public void Refresh_ChangedEntity()
        {
            UniversityContext context = new UniversityContext();
            IEntytiManager em = new EntityManager(context);

            Student stud = em.Find<Student>(1);
            Assert.IsTrue(stud.Id == 1);
            Assert.IsTrue(stud.FirstName == "Petras");
            Assert.IsTrue(stud.LastName == "Petraitis");
            Assert.IsTrue(stud.BirthDay == new DateTime(1980, 05, 05));

            using(UniversityContext context2 = new UniversityContext())
            {
                Student s = context2.Students.Single(e => e.Id == 1);

                s.FirstName = "Tammie";
                s.LastName = "Edwards";
                s.BirthDay = new DateTime(1981, 06, 06);

                context2.SaveChanges();
            }

            em.Refresh(stud);

            Assert.IsTrue(stud.Id == 1);
            Assert.IsTrue(stud.FirstName == "Tammie");
            Assert.IsTrue(stud.LastName == "Edwards");
            Assert.IsTrue(stud.BirthDay == new DateTime(1981, 06, 06));
        }

        [TestMethod]
        [Ignore]
        public void Refresh_TransactionRequiredException(){ }


        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void Refresh_BadArgument()
        {   
            IEntytiManager em = new EntityManager(new UniversityContext());            
            em.Refresh(new BadArgument());   
        }

        [TestMethod]
        [Ignore]
        public void Refresh_EntityNotFoundException()
        {//Nežinau ar čia prasimingas toks elgesys. tam kad patikrinti reikalingas papildomas kreipinys į DB.
            UniversityContext context = new UniversityContext();
            
            IEntytiManager em = new EntityManager(context);
            
            Student stud = em.Find<Student>(1);

            using (UniversityContext context2 = new UniversityContext())
            {
                Student s = context2.Students.Single(e => e.Id == 1);
                context2.Students.Remove(s);
                context2.SaveChanges();
            }

            em.Refresh(stud);
        }
        
        #endregion

        #region remove

        [TestMethod]
        public void Remove()
        {
            UniversityContext context = new UniversityContext();
            IEntytiManager em = new EntityManager(context);

            Student stud = em.Find<Student>(1);

            Assert.IsTrue(context.Entry<Student>(stud).State == EntityState.Unchanged);
            em.Remove(stud);
            Assert.IsTrue(context.Entry<Student>(stud).State == EntityState.Deleted);
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void Remove_BadArgumentException()
        {
            UniversityContext context = new UniversityContext();
            IEntytiManager em = new EntityManager(context);

            em.Remove(new BadArgument());
        }       
 
        [TestMethod]
        public void TestPostSharp()
        {
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));


            //SayHello();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EntityManagerGotFromEJBContainer_NoDefinedDataContext_ShouldThrowExceptionOnContainerInitMethod()
        {
            EJBContainer c = EJBContainer.Instance;
            c.Configuration.SetDBContextCreationFuncForEntityManager(null);
            c.Init();
        }

        #endregion
    }
}