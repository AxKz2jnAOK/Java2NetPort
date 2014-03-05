using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Java2NetPort.JNDI
{
    public class NameClassPair
    {
        public string Name
        {
            get;            
            private set;
        }

        public Type Type
        {
            get;
            private set;
        }

        public NameClassPair(string name, Type type)
        {
            Name = name;
            Type = type;
        }
    }
}
