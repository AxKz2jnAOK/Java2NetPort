using Java2NetPort.Attributes;
using Java2NetPort.Patterns;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Java2NetPort
{
    public class EJBContainer
    {
        #region class Members

        private IList<object> _statefulSesionBeans;
        private IList<object> _statelessSessionBeans;
        Dictionary<Type, object> _dicPoolOfStatelessBeans = null;
        protected IEnumerable<string> _fullAssebliesNamesForSessionBeans;
        
        protected Func<DbContext> _dbContextCreationFunc;
        #endregion

        #region Properties

        public IEntytiManager EntityManager
        {
            get
            {
                IEntytiManager entityManager;
                if(this.TransactionScopes.Any())
                {
                    EJBTransactionScope ts = EJBContainer.Instance.TransactionScopes.Peek();
                    entityManager = new EntityManager(ts.DbContext);
                }
                else
                {
                    entityManager = new EntityManager(_dbContextCreationFunc.Invoke());
                }
                return entityManager;
            }
        }

        internal Stack<EJBTransactionScope> TransactionScopes { get; set; }

        #endregion

        #region Constructor    

        private EJBContainer()
        {
            TransactionScopes = new Stack<EJBTransactionScope>();
        }

        #endregion

        #region Singleton instance

        private static EJBContainer _instance;

        public static EJBContainer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EJBContainer();
                    _instance.Configuration = new Config(_instance);
                }
                return _instance;
            }
        }

        #endregion

        public Config Configuration
        {
            get;protected set;
        }
        
        #region Config
        public class Config
        {
            private EJBContainer _eJBContainer;
            public Config(EJBContainer eJBContainer)
            {
                _eJBContainer = eJBContainer;
            }

            public EJBContainer SetFullAssebliesNamesForSessionBeans(IEnumerable<string> names)
            {
                _eJBContainer._fullAssebliesNamesForSessionBeans= names;
                return _eJBContainer;
            }

            public EJBContainer SetDBContextCreationFuncForEntityManager(Func<DbContext> dbContextCreationFunc)
            {
                _eJBContainer._dbContextCreationFunc = dbContextCreationFunc; 
                return _eJBContainer;
            }
        }

        #endregion

        internal DbContext GetNewDbContext()
        {
            Init();
            return _dbContextCreationFunc.Invoke();
        }

        bool _initiated = false;
        public void Init()
        {
            if (!_initiated)
            {
                if (_dbContextCreationFunc == null) throw new ArgumentNullException("EJBContainer: dbContextCreationFunc must be defined through configuration.");
                //_entityManager = new EntityManager(_dbContextCreationFunc.Invoke());
            }
        }


        public void PassivateStatefulSessionBeans()
        {
            //for(int i=_statefulSesionBeans.Count()-1; i>=0; i--)
            //{
            //    //_statefulSesionBeans.RemoveAt(i);
            //    _statefulSesionBeans[i] = null;
            //}
            _ssb = null;
        }

        public T FetchStatelessSessionBean<T>()
        {
            //ToDo: make thread safe
            T result = default(T);

            result = (T)_dicPoolOfStatelessBeans[typeof(T)];

            return result;
        }


        private object _ssb;
        private void ScanStatefulSessionBeans()
        {
            throw new NotImplementedException();

            _statefulSesionBeans = new List<object>();
            IEnumerable<Type> statefulSessionBeanTypes = GetClassesByAttributeFromAssemblies(typeof(StatefulAttribute)).ToList();

            //check if stateful session beans implement serialazable interfacę
            foreach (Type t in statefulSessionBeanTypes)
            {
                if(!(t.GetInterface("ISerializable") != null 
                    || Attribute.GetCustomAttribute(t, typeof(SerializableAttribute)) != null))
                {
                    throw new Exception(t.Name + " does not implement neither ISerializable interface, nor has Serializable attribute.");
                }
            }

            //for(int i=0; i<5; i++)
            //{
            //    _statefulSesionBeans.Add(Activator.CreateInstance(statefulSessionBeanTypes.First()));
            //}

            if (statefulSessionBeanTypes.Count() > 0)
            {
                _ssb = Activator.CreateInstance(statefulSessionBeanTypes.First());
            }

            //ToDo: surasti būdus kaip interceptinti metodų kvietinius. stebėdami kvietinius, galime matuoti kiek objektas yra gyvas, aktyvus - jį aktyvuoti, pasyvuoti.
            //ToDo: [ ] implement prePassivate, preActivateAtributus
        }

        private void ScanStatelessSessionBeans()
        {
            throw new NotImplementedException();

             _dicPoolOfStatelessBeans = new Dictionary<Type, object>();

            foreach (Type type in GetClassesByAttributeFromAssemblies(typeof(StatelessAttribute)))
            {
                var instance = Activator.CreateInstance(type);
                //Todo: atlikti priklausomybių injekciją

                var methods = type.GetMethods();

                foreach(var method in methods)
                {
                    var attributes = method.GetCustomAttributes( typeof( PostConstructAttribute ), true );
                    if (attributes != null && attributes.Length > 0)
                    {
                        method.Invoke(instance, new object[] { });
                    }
                }

                _dicPoolOfStatelessBeans.Add(type, instance);
            }
        }

        private IEnumerable<Type> GetClassesByAttributeFromAssemblies(Type attributeType)
        {
            IEnumerable<Assembly> assembliesToSearch;
            if(_fullAssebliesNamesForSessionBeans != null && _fullAssebliesNamesForSessionBeans.Count() > 0)
            {
                assembliesToSearch = GetLoadedAndRelatedAssemblies().Where(e => _fullAssebliesNamesForSessionBeans.Contains(e.FullName));
            }
            else
            {
                assembliesToSearch = GetLoadedAndRelatedAssemblies();
            }
             
            foreach (Assembly assembly in assembliesToSearch)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.GetCustomAttributes(attributeType, true).Length > 0)
                    {
                        yield return type;
                    }
                }
            }
        }

        protected IList<Assembly> _listOfAssemblies;
        public IEnumerable<Assembly> GetLoadedAndRelatedAssemblies()
        {
            if (_listOfAssemblies == null)
            { 
                _listOfAssemblies = new List<Assembly>();

                var stack = new Stack<Assembly>();

                foreach (Assembly asb in AppDomain.CurrentDomain.GetAssemblies())
                {
                    _listOfAssemblies.Add(asb);
                    stack.Push(asb);
                }

                do
                {
                    var asm = stack.Pop();
                    
                    foreach (var reference in asm.GetReferencedAssemblies())
                    if (!_listOfAssemblies.Select(e => e.FullName).Contains(reference.FullName))
                    {
                        Assembly a = Assembly.Load(reference);
                        stack.Push(a);
                        _listOfAssemblies.Add(a);
                    }
                }
                while (stack.Count > 0);
                //Todo: optimize to avoid this call
                _listOfAssemblies = _listOfAssemblies.Distinct().ToList();
            }
             return _listOfAssemblies;
        }

        public object FetchStatefullSessionBean()
        {
            //return _statefulSesionBeans.First();
            return _ssb;
        }       
    }
}
