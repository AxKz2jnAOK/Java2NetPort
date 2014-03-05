using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Java2NetPort.JNDI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Java2NetPort.Tests
{
    [TestClass]
    public class JNDI_CompoundName
    {
        [TestMethod]
        public void CreateName_ok()
        {
            Dictionary<string, string> syntax = new Dictionary<string, string>();
            IName n = new CompoundName(string.Empty, syntax);
        }

        [TestMethod]
        public void InsertStringAtPosition_ok()
        {
            Dictionary<string, string> syntax = new Dictionary<string, string>();
            IName n = new CompoundName(string.Empty, syntax);
            n.Insert(0, "item1");

            IEnumerable<string> r = n.GetAll();
            IEnumerator<string> enumerator = r.GetEnumerator();

            enumerator.MoveNext();
            Assert.AreSame("item1", enumerator.Current);

            Assert.IsFalse(enumerator.MoveNext());
        }

        [TestMethod]
        public void AddString_ok()
        {
            Dictionary<string, string> syntax = new Dictionary<string, string>();
            IName n = new CompoundName(string.Empty, syntax);
            n.Add("item1");
            n.Insert(1, "item2");
            n.Add("item3");

            IEnumerable<string> r = n.GetAll();
            IEnumerator<string> enumerator = r.GetEnumerator();

            enumerator.MoveNext();
            Assert.AreSame("item1", enumerator.Current);

            enumerator.MoveNext();
            Assert.AreSame("item2", enumerator.Current);

            enumerator.MoveNext();
            Assert.AreSame("item3", enumerator.Current);

            Assert.IsFalse(enumerator.MoveNext());
        }


        [TestMethod]
        public void InsertNameAtPosition_ok()
        {
            Dictionary<string, string> syntax = new Dictionary<string, string>();

            IName n2 = new CompoundName("item2", syntax);

            IName n = new CompoundName(string.Empty, syntax);
            n.Add("item1");
            n.Insert(1, n2);

            IEnumerable<string> r = n.GetAll();
            IEnumerator<string> enumerator = r.GetEnumerator();

            enumerator.MoveNext();
            Assert.AreSame("item1", enumerator.Current);

            enumerator.MoveNext();
            Assert.AreSame("item2", enumerator.Current);

            Assert.IsFalse(enumerator.MoveNext());
        }

        [TestMethod]
        public void AddName_ok()
        {
            Dictionary<string, string> syntax = new Dictionary<string, string>();
            IName n2 = new CompoundName("item2", syntax);

            IName n = new CompoundName(string.Empty, syntax);
            n.Add(n2);
            n.Add("item1");

            IEnumerable<string> r = n.GetAll();
            IEnumerator<string> enumerator = r.GetEnumerator();

            enumerator.MoveNext();
            Assert.AreSame("item2", enumerator.Current);

            enumerator.MoveNext();
            Assert.AreSame("item1", enumerator.Current);

            Assert.IsFalse(enumerator.MoveNext());
        }

        [TestMethod]
        public void EndsWithName_Success()
        {
            Dictionary<string, string> syntax = new Dictionary<string, string>();
            IName nEnd = new CompoundName("item2", syntax);

            IName n = new CompoundName(string.Empty, syntax);
            n.Add("item1");
            n.Add("item2");

            Assert.IsTrue(n.EndsWith(nEnd));
        }

        [TestMethod]
        public void EndsWithName_Fail()
        {
            Dictionary<string, string> syntax = new Dictionary<string, string>();
            IName nEnd = new CompoundName("item1", syntax);

            IName n = new CompoundName(string.Empty, syntax);
            n.Add("item1");
            n.Add("item2");

            Assert.IsFalse(n.EndsWith(nEnd));
        }

        [TestMethod]
        public void Get_Success()
        {
            Dictionary<string, string> syntax = new Dictionary<string, string>();
            IName n = new CompoundName(string.Empty, syntax);
            n.Add("item1");
            n.Add("item2");
            n.Add("item3");

            Assert.AreSame("item2", n.Get(1));
        }

        [TestMethod]
        public void IsEmpty_True()
        {
            Dictionary<string, string> syntax = new Dictionary<string, string>();
            IName n = new CompoundName(string.Empty, syntax);
            Assert.IsTrue(n.IsEmpty());
        }

        [TestMethod]
        public void IsEmpty_False()
        {
            Dictionary<string, string> syntax = new Dictionary<string, string>();
            IName n = new CompoundName(string.Empty, syntax);
            n.Add("item1");
            Assert.IsFalse(n.IsEmpty());
        }

        [TestMethod]
        public void DivideNameBySeparator_FoundCoupleNameComponents()
        {
            Dictionary<string, string> syntax = new Dictionary<string, string>()
            {
                {"separator", ";"}
            };

            IName n = new CompoundName("itemA;itemB;itemC", syntax);

            IEnumerable<string> r = n.GetAll();
            IEnumerator<string> enumerator = r.GetEnumerator();

            enumerator.MoveNext();
            Assert.AreEqual("itemA", enumerator.Current);

            enumerator.MoveNext();
            Assert.AreEqual("itemB", enumerator.Current);

            enumerator.MoveNext();
            Assert.AreEqual("itemC", enumerator.Current);

            Assert.IsFalse(enumerator.MoveNext());
        }

        [TestMethod]
        public void DivideNameWithSpacesBySeparator_DonotTrim_FoundCoupleNameComponents()
        {
            Dictionary<string, string> syntax = new Dictionary<string, string>()
            {
                {"separator", ";"}
            };

            IName n = new CompoundName("itemA ; itemB; itemC ", syntax);

            IEnumerable<string> r = n.GetAll();
            IEnumerator<string> enumerator = r.GetEnumerator();

            enumerator.MoveNext();
            Assert.AreEqual("itemA ", enumerator.Current);

            enumerator.MoveNext();
            Assert.AreEqual(" itemB", enumerator.Current);

            enumerator.MoveNext();
            Assert.AreEqual(" itemC ", enumerator.Current);

            Assert.IsFalse(enumerator.MoveNext());
        }

        [TestMethod]
        public void DivideNameWithSpacesBySeparator_Trim_FoundCoupleNameComponents()
        {
            Dictionary<string, string> syntax = new Dictionary<string, string>()
            {
                {NameSyntaxKeys.separator.ToString(), ";"},
                {NameSyntaxKeys.trimblanks.ToString(), "true"}
            };

            IName n = new CompoundName("itemA ; itemB; itemC ", syntax);

            IEnumerable<string> r = n.GetAll();
            IEnumerator<string> enumerator = r.GetEnumerator();

            enumerator.MoveNext();
            Assert.AreEqual("itemA", enumerator.Current);

            enumerator.MoveNext();
            Assert.AreEqual("itemB", enumerator.Current);

            enumerator.MoveNext();
            Assert.AreEqual("itemC", enumerator.Current);

            Assert.IsFalse(enumerator.MoveNext());
        }

        [TestMethod]
        public void remove_Success()
        {
            Dictionary<string, string> env = new Dictionary<string, string>();
            env.Add(Context.INITIAL_CONTEXT_FACTORY, "Java2NetPort.JNDI.ServiceProviders.InMemoryServiceProvider.InitialInMemoryContextFactory");

            IContext ctx = new InitialContext(env);

            INameParser np = ctx.GetNameParser("");

            IName n = np.Parse("item1;item2;item3");

            string rez = n.Remove(1);
            Assert.AreEqual("item2", rez);

            IEnumerable<string> r = n.GetAll();
            IEnumerator<string> enumerator = r.GetEnumerator();

            enumerator.MoveNext();
            Assert.AreEqual("item1", enumerator.Current);

            enumerator.MoveNext();
            Assert.AreEqual("item3", enumerator.Current);


            Assert.IsFalse(enumerator.MoveNext());
        }

        //todo test: flat
        //todo test: left to right
        //todo test: right to left

        
    }
}
