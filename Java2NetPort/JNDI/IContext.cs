using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Java2NetPort.JNDI
{
    public interface IContext : 
                            IContextLookup,
                            IContextUpdate,
                            IContextListing,
                            IContextNameParser
    {
    }

    public interface IContextLookup
    {
        object Lookup(string name);
        object Lookup(IName name);
    }

    public interface IContextUpdate
    {
        void Bind(string name, object obj);
        void Bind(IName name, object obj);
        void Rename(string oldName, string newName);
        void Rename(IName oldName, IName newName);
        void Unbind(string name);
        void Unbind(IName name);
        void Rebind(string name, object obj);
        void Rebind(IName name, object obj);
    }

    public interface IContextListing
    {   
        IList<NameClassPair> List(string name);
        IList<Binding> ListBindings(string name);
    }

    public interface IContextNameParser
    {
        INameParser GetNameParser(string name);
    }
}
