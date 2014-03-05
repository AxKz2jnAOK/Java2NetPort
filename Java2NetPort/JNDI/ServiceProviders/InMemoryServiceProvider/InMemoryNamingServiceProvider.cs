using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Java2NetPort.JNDI.Exceptions;

namespace Java2NetPort.JNDI.ServiceProviders.InMemoryServiceProvider
{
    public interface IClearContext
    {
        void Clear();
    }

    public class InMemoryNamingServiceProvider : IContext, IClearContext
    {
        static INameParser NameParser
        { get; set; }

        private static InMemoryNamingServiceProvider _instance;

        public static InMemoryNamingServiceProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new InMemoryNamingServiceProvider();
                }
                return _instance;
            }
        }

        private InMemoryNamingServiceProvider() 
        {
            _storage = new Dictionary<string, object>();
            NameParser = new InMemoryHierarchicalNameParser();
        }
        
        private Dictionary<string, object> _storage;

        public object Lookup(string name)
        {
            object result = null;
            if(!string.IsNullOrWhiteSpace(name))                
            {
                if(!_storage.Keys.Contains(name))
                {
                    throw new NameNotFoundException();
                }
                result = _storage[name];
            }
            else
            {
                return this;
            }
            return result;
        }

        public object Lookup(IName name)
        {
            return Lookup(name.ToString());
        }

        public void Bind(string name, object obj)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidNameException();
            }
            _storage.Add(name, obj);
        }

        public void Bind(IName name, object obj)
        {
            Bind(name.ToString(), obj);
        }

        public void Rename(string oldName, string newName)
        {
            if(string.IsNullOrWhiteSpace(newName))
            {
                throw new InvalidNameException();
            }

            if (string.IsNullOrWhiteSpace(oldName))
            {
                throw new InvalidNameException();
            }
            
            if(!_storage.Keys.Contains(oldName))
            {
                throw new NameNotFoundException();
            }

            if (_storage.Keys.Contains(newName))
            {
                throw new NameAlreadyBoundException();
            }

            object value = _storage[oldName];
            _storage.Remove(oldName);
            _storage.Add(newName, value);
        }

        public void Rename(IName oldName, IName newName)
        {
            Rename(oldName.ToString(), newName.ToString());
        }

        public void Unbind(string name)
        {
            _storage.Remove(name);
        }

        public void Unbind(IName name)
        {
            this.Unbind(name.ToString());
        }

        public void Rebind(string name, object obj)
        {
            this.Unbind(name);
            _storage.Add(name, obj);
        }

        public void Rebind(IName name, object obj)
        {
            Rebind(name.ToString(), obj);
        }

        public void Clear()
        {
            _storage = new Dictionary<string, object>();
        }
        
        public IList<NameClassPair> List(string name)
        {
            IList<NameClassPair> result = null;
            
            result = _storage.Select(e => new NameClassPair(e.Key, e.Value.GetType())).ToArray();
            
            return result;
        }

        public IList<Binding> ListBindings(string name)
        {
            IList<Binding> result = null;

            result = _storage.Select(e => new Binding(e.Key, e.Value.GetType(), e.Value)).ToArray();

            return result;
        }

        public INameParser GetNameParser(string name)
        {
            return NameParser;
        }
    }
}
