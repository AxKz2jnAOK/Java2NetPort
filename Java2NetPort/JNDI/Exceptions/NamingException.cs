using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Java2NetPort.JNDI.Exceptions
{
    public abstract class NamingException : Exception
    {
        public NamingException(string msg) : base(msg) { }

        public NamingException() { }
    }
}
