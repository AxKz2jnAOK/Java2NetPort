using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Java2NetPort.JNDI
{
    public class Binding : NameClassPair
    {
        public object Object
        {
            get;
            private set;
        }

        public Binding(string name, Type type, object obj) : base(name, type)
        {
            Object = obj;
        }
    }
}
