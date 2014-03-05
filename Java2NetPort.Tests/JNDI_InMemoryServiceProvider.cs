using System;
using System.Collections.Generic;
using Java2NetPort.JNDI;
using Java2NetPort.JNDI.Exceptions;
using Java2NetPort.JNDI.ServiceProviders.InMemoryServiceProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Java2NetPort.Tests
{
    [TestClass]
    public class JNDI_InMemoryNamingServiceProvider
    {
        [TestCleanup]
        public void TestCleanup()
        {
            InMemoryNamingServiceProvider.Instance.Clear();
        }

        [TestMethod]
        [ExpectedException(typeof(NoInitialContextException))]
        public void CreateNonExistingServiceProvider()
        {
            Dictionary<string, string> env = new Dictionary<string, string>();
            env.Add(Context.INITIAL_CONTEXT_FACTORY, "notExisting");

            IContext ctx = new InitialContext(env);
        }
        
        [TestMethod]
        public void CreateInMemoryServiceProvider_Ok()
        {
            Dictionary<string, string> env = new Dictionary<string, string>();
            env.Add(Context.INITIAL_CONTEXT_FACTORY, "Java2NetPort.JNDI.ServiceProviders.InMemoryServiceProvider.InitialInMemoryContextFactory");

            IContext ctx = new InitialContext(env);
        }

        [TestMethod]
        public void LookupEmptyStringName_ShouldReturnContext()
        {
            Dictionary<string, string> env = new Dictionary<string, string>();
            env.Add(Context.INITIAL_CONTEXT_FACTORY, "Java2NetPort.JNDI.ServiceProviders.InMemoryServiceProvider.InitialInMemoryContextFactory");
            
            IContext ctx = new InitialContext(env);

            object result = ctx.Lookup(string.Empty);

            Assert.IsTrue(result is IContext);
            Assert.IsTrue(result is InMemoryNamingServiceProvider);
        }

        [TestMethod]
        public void LookupEmptyName_ShouldReturnContext()
        {
            Dictionary<string, string> env = new Dictionary<string, string>();
            env.Add(Context.INITIAL_CONTEXT_FACTORY, "Java2NetPort.JNDI.ServiceProviders.InMemoryServiceProvider.InitialInMemoryContextFactory");

            IContext ctx = new InitialContext(env);
            INameParser np = ctx.GetNameParser(string.Empty);

            IName name = np.Parse("");

            object result = ctx.Lookup(name);

            Assert.IsTrue(result is IContext);
            Assert.IsTrue(result is InMemoryNamingServiceProvider);
        }

        [TestMethod]
        [ExpectedException(typeof(NameNotFoundException))]
        public void LookupNonExistingItemByStringName_ThrowNamingException()
        {   
            Dictionary<string, string> env = new Dictionary<string, string>();
            env.Add(Context.INITIAL_CONTEXT_FACTORY, "Java2NetPort.JNDI.ServiceProviders.InMemoryServiceProvider.InitialInMemoryContextFactory");

            IContext ctx = new InitialContext(env);

            ctx.Lookup("itemNotExisting");
        }

        [TestMethod]
        public void LookupExistingItemByStringName_Ok()
        {
            Dictionary<string, string> env = new Dictionary<string, string>();
            env.Add(Context.INITIAL_CONTEXT_FACTORY, "Java2NetPort.JNDI.ServiceProviders.InMemoryServiceProvider.InitialInMemoryContextFactory");

            IContext ctx = new InitialContext(env);

            ctx.Bind("stringItem", "stringItemValue");

            object returnedObj = ctx.Lookup("stringItem");

            Assert.IsNotNull(returnedObj);
            Assert.IsTrue(returnedObj is string);
            Assert.AreEqual("stringItemValue", returnedObj.ToString());
        }

        [TestMethod]
        public void LookupExistingItemByName_Ok()
        {
            Dictionary<string, string> env = new Dictionary<string, string>();
            env.Add(Context.INITIAL_CONTEXT_FACTORY, "Java2NetPort.JNDI.ServiceProviders.InMemoryServiceProvider.InitialInMemoryContextFactory");

            IContext ctx = new InitialContext(env);

            IName name = ctx.GetNameParser("").Parse("stringItem;key");

            ctx.Bind(name, "stringItemValue");

            object returnedObj = ctx.Lookup(name);

            Assert.IsNotNull(returnedObj);
            Assert.IsTrue(returnedObj is string);
            Assert.AreEqual("stringItemValue", returnedObj.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void BindWithEmptyNameString_InvalidNameException()
        {
            Dictionary<string, string> env = new Dictionary<string, string>();
            env.Add(Context.INITIAL_CONTEXT_FACTORY, "Java2NetPort.JNDI.ServiceProviders.InMemoryServiceProvider.InitialInMemoryContextFactory");

            IContext ctx = new InitialContext(env);

            ctx.Bind("", "stringItemValue");
        }

        [TestMethod]
        public void RebindByString_Ok()
        {
            Dictionary<string, string> env = new Dictionary<string, string>();
            env.Add(Context.INITIAL_CONTEXT_FACTORY, "Java2NetPort.JNDI.ServiceProviders.InMemoryServiceProvider.InitialInMemoryContextFactory");

            IContext ctx = new InitialContext(env);

            ctx.Bind("stringItem", "stringItemValue");
            object returnedObj = ctx.Lookup("stringItem");
            
            Assert.IsNotNull(returnedObj);
            Assert.IsTrue(returnedObj is string);
            Assert.IsTrue(returnedObj.ToString() == "stringItemValue");

            

            
            ctx.Rebind("stringItem", "stringItemValueChanged");
            returnedObj = ctx.Lookup("stringItem");
            
            Assert.IsNotNull(returnedObj);
            Assert.IsTrue(returnedObj is string);
            Assert.AreEqual("stringItemValueChanged", returnedObj.ToString());
        }

        [TestMethod]
        public void RebindByName_Ok()
        {
            Dictionary<string, string> env = new Dictionary<string, string>();
            env.Add(Context.INITIAL_CONTEXT_FACTORY, "Java2NetPort.JNDI.ServiceProviders.InMemoryServiceProvider.InitialInMemoryContextFactory");

            IContext ctx = new InitialContext(env);

            IName name = ctx.GetNameParser("").Parse("stringItem");
            
            ctx.Bind(name, "stringItemValue");
            object returnedObj = ctx.Lookup("stringItem");

            Assert.IsNotNull(returnedObj);
            Assert.IsTrue(returnedObj is string);
            Assert.IsTrue(returnedObj.ToString() == "stringItemValue");




            ctx.Rebind(name, "stringItemValueChanged");
            returnedObj = ctx.Lookup(name);

            Assert.IsNotNull(returnedObj);
            Assert.IsTrue(returnedObj is string);
            Assert.AreEqual("stringItemValueChanged", returnedObj.ToString());
        }




        [TestMethod]
        [ExpectedException(typeof(NameNotFoundException))]
        public void UnbindByString_ThrowNameNotFoundExceptionOnLookup()
        {
            Dictionary<string, string> env = new Dictionary<string, string>();
            env.Add(Context.INITIAL_CONTEXT_FACTORY, "Java2NetPort.JNDI.ServiceProviders.InMemoryServiceProvider.InitialInMemoryContextFactory");

            IContext ctx = new InitialContext(env);

            ctx.Bind("stringItem", "stringItemValue");

            ctx.Unbind("stringItem");

            object returnedObj = ctx.Lookup("stringItem");
        }

        [TestMethod]
        [ExpectedException(typeof(NameNotFoundException))]
        public void UnbindByName_ThrowNameNotFoundExceptionOnLookup()
        {
            Dictionary<string, string> env = new Dictionary<string, string>();
            env.Add(Context.INITIAL_CONTEXT_FACTORY, "Java2NetPort.JNDI.ServiceProviders.InMemoryServiceProvider.InitialInMemoryContextFactory");

            IContext ctx = new InitialContext(env);

            IName name = ctx.GetNameParser("").Parse("stringItem");

            ctx.Bind(name, "stringItemValue");

            ctx.Unbind(name);

            object returnedObj = ctx.Lookup(name);
        }

        [TestMethod]
        public void RenameByString_Ok()
        {
            Dictionary<string, string> env = new Dictionary<string, string>();
            env.Add(Context.INITIAL_CONTEXT_FACTORY, "Java2NetPort.JNDI.ServiceProviders.InMemoryServiceProvider.InitialInMemoryContextFactory");

            IContext ctx = new InitialContext(env);

            ctx.Bind("stringItem1", "stringItem1Value");

            ctx.Rename("stringItem1", "stringItem2");

            object obj = ctx.Lookup("stringItem2");
            
            Assert.IsNotNull(obj);
            Assert.IsTrue(obj is string);
            Assert.IsTrue(obj.ToString() == "stringItem1Value");
        }


        [TestMethod]
        public void RenameByName_Ok()
        {
            Dictionary<string, string> env = new Dictionary<string, string>();
            env.Add(Context.INITIAL_CONTEXT_FACTORY, "Java2NetPort.JNDI.ServiceProviders.InMemoryServiceProvider.InitialInMemoryContextFactory");

            IContext ctx = new InitialContext(env);

            IName name = ctx.GetNameParser("").Parse("stringItem1");
            IName name2 = ctx.GetNameParser("").Parse("stringItem2");

            ctx.Bind(name, "stringItem1Value");

            ctx.Rename(name, name2);

            object obj = ctx.Lookup(name2);

            Assert.IsNotNull(obj);
            Assert.IsTrue(obj is string);
            Assert.IsTrue(obj.ToString() == "stringItem1Value");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void RenameToEmptyString_InvalidNameException()
        {
            Dictionary<string, string> env = new Dictionary<string, string>();
            env.Add(Context.INITIAL_CONTEXT_FACTORY, "Java2NetPort.JNDI.ServiceProviders.InMemoryServiceProvider.InitialInMemoryContextFactory");

            IContext ctx = new InitialContext(env);

            ctx.Bind("stringItem", "stringItemValue");
            ctx.Rename("stringItem", string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void RenameFromEmptyString_InvalidNameException()
        {
            Dictionary<string, string> env = new Dictionary<string, string>();
            env.Add(Context.INITIAL_CONTEXT_FACTORY, "Java2NetPort.JNDI.ServiceProviders.InMemoryServiceProvider.InitialInMemoryContextFactory");

            IContext ctx = new InitialContext(env);

            ctx.Bind("stringItem", "stringItemValue");
            ctx.Rename(string.Empty, "stringItem");
        }

        [TestMethod]
        [ExpectedException(typeof(NameAlreadyBoundException))]
        public void RenameToAlreadyExistingName_NameAlreadyBoundException()
        {
            Dictionary<string, string> env = new Dictionary<string, string>();
            env.Add(Context.INITIAL_CONTEXT_FACTORY, "Java2NetPort.JNDI.ServiceProviders.InMemoryServiceProvider.InitialInMemoryContextFactory");

            IContext ctx = new InitialContext(env);

            ctx.Bind("stringItem1", "stringItem1Value");
            ctx.Bind("stringItem2", "stringItem2Value");

            ctx.Rename("stringItem1", "stringItem2");
        }

        [TestMethod]
        [ExpectedException(typeof(NameNotFoundException))]
        public void RenameNameThatDoesNotExists_NameNotFoundException()
        {
            Dictionary<string, string> env = new Dictionary<string, string>();
            env.Add(Context.INITIAL_CONTEXT_FACTORY, "Java2NetPort.JNDI.ServiceProviders.InMemoryServiceProvider.InitialInMemoryContextFactory");

            IContext ctx = new InitialContext(env);

            ctx.Rename("notExistingName", "notExistingName");
        }

        [TestMethod]        
        public void MultipleCreationOfInitialContext_ShouldReturnTheSameContext()
        {
            Dictionary<string, string> env = new Dictionary<string, string>();
            env.Add(Context.INITIAL_CONTEXT_FACTORY, "Java2NetPort.JNDI.ServiceProviders.InMemoryServiceProvider.InitialInMemoryContextFactory");

            IContext ctx = new InitialContext(env);
            ctx.Bind("item1", "item1Value");

            IContext ctx2 = new InitialContext(env);
            object result = ctx2.Lookup("item1");
            Assert.AreSame("item1Value", result.ToString());
        }

        [TestMethod]
        public void List_Ok()        
        {
            Dictionary<string, string> env = new Dictionary<string, string>();
            env.Add(Context.INITIAL_CONTEXT_FACTORY, "Java2NetPort.JNDI.ServiceProviders.InMemoryServiceProvider.InitialInMemoryContextFactory");

            IContext ctx = new InitialContext(env);
            ctx.Bind("key1", 1);
            ctx.Bind("key2", "test");

            IList<NameClassPair> d  = ctx.List("");

            Assert.IsTrue(d.Count == 2);

            NameClassPair r1 = d[0];
            NameClassPair r2 = d[1];


            Assert.IsTrue(r1.Name == "key1" && r1.Type == typeof(int));
            Assert.IsTrue(r2.Name == "key2" && r2.Type == typeof(string));
        }

        [TestMethod]
        public void ListBinding_Ok()
        {
            Dictionary<string, string> env = new Dictionary<string, string>();
            env.Add(Context.INITIAL_CONTEXT_FACTORY, "Java2NetPort.JNDI.ServiceProviders.InMemoryServiceProvider.InitialInMemoryContextFactory");

            IContext ctx = new InitialContext(env);
            ctx.Bind("key1", 1);
            ctx.Bind("key2", "test");

            IList<Binding> d = ctx.ListBindings("");

            Assert.IsTrue(d.Count == 2);

            Binding r1 = d[0];
            Binding r2 = d[1];
            
            Assert.IsTrue(r1.Name == "key1" && r1.Type == typeof(int) && (int)r1.Object == 1);
            Assert.IsTrue(r2.Name == "key2" && r2.Type == typeof(string) && r2.Object.ToString() == "test");
        }
        
        #region name parser

        [TestMethod]
        public void GetNameParserFromContext_Ok()
        {
            Dictionary<string, string> env = new Dictionary<string, string>();
            env.Add(Context.INITIAL_CONTEXT_FACTORY, "Java2NetPort.JNDI.ServiceProviders.InMemoryServiceProvider.InitialInMemoryContextFactory");

            IContext ctx = new InitialContext(env);

            INameParser np = ctx.GetNameParser(string.Empty);
            
            Assert.IsNotNull(np);

            Assert.IsTrue(np is InMemoryHierarchicalNameParser);
        }

        [TestMethod]
        public void GetNameParserAndParseStringName_Success()
        {
            Dictionary<string, string> env = new Dictionary<string, string>();
            env.Add(Context.INITIAL_CONTEXT_FACTORY, "Java2NetPort.JNDI.ServiceProviders.InMemoryServiceProvider.InitialInMemoryContextFactory");

            IContext ctx = new InitialContext(env);

            INameParser np = ctx.GetNameParser("");

            Assert.IsNotNull(np);

            Assert.IsTrue(np is InMemoryHierarchicalNameParser);

            IName n = np.Parse("itemA;itemB;itemC");

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

        #endregion
    }
}
