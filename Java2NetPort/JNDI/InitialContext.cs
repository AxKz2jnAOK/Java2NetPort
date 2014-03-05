using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Java2NetPort.JNDI.Exceptions;
using Java2NetPort.JNDI.ServiceProviders.InMemoryServiceProvider;
using Java2NetPort.JNDI.SPI;

namespace Java2NetPort.JNDI
{
    public class InitialContext : IContext
    {
        private IContext _context = null;

        public InitialContext(IReadOnlyDictionary<string, string> enviroment)
        {
            if (enviroment.Keys.Contains(Context.INITIAL_CONTEXT_FACTORY))
            {
                Type t =  Type.GetType(enviroment[Context.INITIAL_CONTEXT_FACTORY]);
                if(t == null)
                {
                    throw new NoInitialContextException("Provider \"" + enviroment[Context.INITIAL_CONTEXT_FACTORY] + "\" - not found.");
                }
                IInitialContextFactory f = (IInitialContextFactory)Activator.CreateInstance(t);

                _context = f.GetInitialContext(enviroment);                
            }
            else
            {
                throw new Exception("Context.INITIAL_CONTEXT_FACTORY value in enviroment parameter - not found.");
            }
        }

        #region IContextLookup
        public object Lookup(string name)
        {
            return _context.Lookup(name);
        }

        public object Lookup(IName name)
        {
            return _context.Lookup(name);
        }

        #endregion

        #region update
        public void Bind(string name, object obj)
        {
            _context.Bind(name, obj);
        }
        public void Bind(IName name, object obj)
        {
            _context.Bind(name, obj);
        }
        public void Rename(string oldName, string newName)
        {
            _context.Rename(oldName, newName);
        }

        public void Rename(IName oldName, IName newName)
        {
            _context.Rename(oldName, newName);
        }

        public void Unbind(string name)
        {
            _context.Unbind(name);
        }
        public void Unbind(IName name)
        {
            _context.Unbind(name);
        }
        public void Rebind(string name, object obj)
        {
            _context.Rebind(name, obj);
        }
        public void Rebind(IName name, object obj)
        {
            _context.Rebind(name, obj);
        }

        #endregion

        

        public IList<NameClassPair> List(string name)
        {
            return _context.List(name);
        }

        public IList<Binding> ListBindings(string name)
        {
            return _context.ListBindings(name);
        }

        public INameParser GetNameParser(string name)
        {
            return _context.GetNameParser(name);
        }
    }
}
