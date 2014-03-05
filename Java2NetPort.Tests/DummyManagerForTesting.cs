using Java2NetPort.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityDataAccess;

namespace Java2NetPort.Tests
{
    //Todo: rename
    [Stateless]
    public class DummyManagerForTesting
    {
        [Transaction(TransactionAttributeType.MANDATORY)]
        public void Mandatory()
        {

        }

        [Transaction(TransactionAttributeType.REQUIRED)]
        public void Required()
        {
            Assert.IsNotNull(System.Transactions.Transaction.Current);
        }

        [Transaction(TransactionAttributeType.REQUIRES_NEW)]
        public void RequiredNew()
        {
            Assert.IsNotNull(System.Transactions.Transaction.Current);
        }

        [Transaction(TransactionAttributeType.REQUIRED)]
        public void Required_RequiredNew_InsertNewStudent()
        {
            Assert.IsNotNull(System.Transactions.Transaction.Current);

            IEntytiManager em = EJBContainer.Instance.EntityManager;

            Student s = new Student();
            s.Id = 3;
            s.FirstName = "Vardenis3";
            s.LastName = "Pavardenis3";
            s.BirthDay = new DateTime(1991, 01, 01);
            em.Persist(s);

            UniversityContext uc = new UniversityContext();
            Assert.IsNull(uc.Students.SingleOrDefault(e => e.Id == 3
                && e.FirstName == "Vardenis3"
                && e.LastName == "Pavardenis3"
                && e.BirthDay == new DateTime(1991, 01, 01)));


            RequiredNew_InsertNewStudent();

            Assert.IsNotNull(System.Transactions.Transaction.Current);

            UniversityContext uc2 = new UniversityContext();

            Assert.IsNull(uc2.Students.SingleOrDefault(e => e.Id == 3
                && e.FirstName == "Vardenis3"
                && e.LastName == "Pavardenis3"
                && e.BirthDay == new DateTime(1991, 01, 01)));

            Assert.IsNotNull(uc2.Students.SingleOrDefault(e => e.Id == 2
                && e.FirstName == "Vardenis"
                && e.LastName == "Pavardenis"
                && e.BirthDay == new DateTime(1990, 01, 01)));
        }

        //Insertion of student 3 occours after returning from inner method.
        [Transaction(TransactionAttributeType.REQUIRED)]
        public void Required_RequiredNew_InsertNewStudent2()
        {
            Assert.IsNotNull(System.Transactions.Transaction.Current);

            IEntytiManager em = EJBContainer.Instance.EntityManager;

            RequiredNew_InsertNewStudent();

            Student s = new Student();
            s.Id = 3;
            s.FirstName = "Vardenis3";
            s.LastName = "Pavardenis3";
            s.BirthDay = new DateTime(1991, 01, 01);
            em.Persist(s);

            Assert.IsNotNull(System.Transactions.Transaction.Current);

            UniversityContext uc = new UniversityContext();
            Assert.IsNull(uc.Students.SingleOrDefault(e => e.Id == 3
                && e.FirstName == "Vardenis3"
                && e.LastName == "Pavardenis3"
                && e.BirthDay == new DateTime(1991, 01, 01)));

            Assert.IsNotNull(uc.Students.SingleOrDefault(e => e.Id == 2
                && e.FirstName == "Vardenis"
                && e.LastName == "Pavardenis"
                && e.BirthDay == new DateTime(1990, 01, 01)));
        }

        [Transaction(TransactionAttributeType.REQUIRES_NEW)]
        internal void RequiredNew_InsertNewStudent()
        {
            IEntytiManager em = EJBContainer.Instance.EntityManager;

            Student s = new Student();
            s.Id = 2;
            s.FirstName = "Vardenis";
            s.LastName = "Pavardenis";
            s.BirthDay = new DateTime(1990, 01, 01);

            em.Persist(s);

            Assert.IsNotNull(System.Transactions.Transaction.Current);

            UniversityContext uc = new UniversityContext();
            Assert.IsNull(uc.Students.SingleOrDefault(e => e.Id == 2
                && e.FirstName == "Vardenis"
                && e.LastName == "Pavardenis"
                && e.BirthDay == new DateTime(1990, 01, 01)));


        }

        [Transaction(TransactionAttributeType.REQUIRED)]
        internal void Required_InsertNewStudent()
        {
            IEntytiManager em = EJBContainer.Instance.EntityManager;

            Student s = new Student();
            s.Id = 2;
            s.FirstName = "Vardenis";
            s.LastName = "Pavardenis";
            s.BirthDay = new DateTime(1990, 01, 01);

            em.Persist(s);



            Assert.IsNotNull(System.Transactions.Transaction.Current);

            UniversityContext uc = new UniversityContext();
            Assert.IsNull(uc.Students.SingleOrDefault(e => e.Id == 2
                && e.FirstName == "Vardenis"
                && e.LastName == "Pavardenis"
                && e.BirthDay == new DateTime(1990, 01, 01)));


        }

        [Transaction(TransactionAttributeType.REQUIRED)]
        public void Required_Required_InsertNewStudent()
        {
            Required_InsertNewStudent();

            Assert.IsNotNull(System.Transactions.Transaction.Current);

            UniversityContext uc = new UniversityContext();
            Assert.IsNull(uc.Students.SingleOrDefault(e => e.Id == 2
                && e.FirstName == "Vardenis"
                && e.LastName == "Pavardenis"
                && e.BirthDay == new DateTime(1990, 01, 01)));
        }

        [Transaction(TransactionAttributeType.SUPPORTS)]
        public void Supports_NoAmbientTransaction_InsertNewStudent()
        {
            IEntytiManager em = EJBContainer.Instance.EntityManager;

            Student s = new Student();
            s.Id = 2;
            s.FirstName = "Vardenis";
            s.LastName = "Pavardenis";
            s.BirthDay = new DateTime(1990, 01, 01);

            em.Persist(s);

            Assert.IsNull(System.Transactions.Transaction.Current);
        }

        [Transaction(TransactionAttributeType.SUPPORTS)]
        public void Supports_WithAmbientTransaction_InsertNewStudent()
        {
            IEntytiManager em = EJBContainer.Instance.EntityManager;

            Student s = new Student();
            s.Id = 2;
            s.FirstName = "Vardenis";
            s.LastName = "Pavardenis";
            s.BirthDay = new DateTime(1990, 01, 01);

            em.Persist(s);

            Assert.IsNotNull(System.Transactions.Transaction.Current);
        }

        [Transaction(TransactionAttributeType.REQUIRED)]
        public void Required_Supports_InsertNewStudent()
        {
            Assert.IsNotNull(System.Transactions.Transaction.Current);

            Supports_WithAmbientTransaction_InsertNewStudent();

            UniversityContext uc = new UniversityContext();
            Assert.IsNull(uc.Students.SingleOrDefault(e => e.Id == 2
                && e.FirstName == "Vardenis"
                && e.LastName == "Pavardenis"
                && e.BirthDay == new DateTime(1990, 01, 01)));
        }


        [Transaction(TransactionAttributeType.NOT_SUPPORTED)]
        public void NotSupports_InsertNewStudent()
        {
            IEntytiManager em = EJBContainer.Instance.EntityManager;

            Student s = new Student();
            s.Id = 2;
            s.FirstName = "Vardenis2";
            s.LastName = "Pavardenis2";
            s.BirthDay = new DateTime(1990, 01, 02);

            em.Persist(s);

            Assert.IsNull(System.Transactions.Transaction.Current);
        }

        [Transaction(TransactionAttributeType.REQUIRED)]
        public void Requried_NotSupports_InsertNewStudent()
        {
            IEntytiManager em = EJBContainer.Instance.EntityManager;

            Student s = new Student();
            s.Id = 3;
            s.FirstName = "Vardenis3";
            s.LastName = "Pavardenis3";
            s.BirthDay = new DateTime(1990, 01, 03);

            em.Persist(s);

            NotSupports_InsertNewStudent();

            UniversityContext uc = new UniversityContext();
            Assert.IsNotNull(uc.Students.SingleOrDefault(e => e.Id == 2
                && e.FirstName == "Vardenis2"
                && e.LastName == "Pavardenis2"
                && e.BirthDay == new DateTime(1990, 01, 02)));


            Assert.IsNotNull(System.Transactions.Transaction.Current);

            Assert.IsNull(uc.Students.SingleOrDefault(e => e.Id == 3
                && e.FirstName == "Vardenis3"
                && e.LastName == "Pavardenis3"
                && e.BirthDay == new DateTime(1990, 01, 03)));
        }
    }
}
