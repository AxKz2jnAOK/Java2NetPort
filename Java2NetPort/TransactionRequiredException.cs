using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Java2NetPort
{
    public class TransactionRequiredException : Exception
    {
        public TransactionRequiredException(string msg) : base(msg) { }

        public TransactionRequiredException() : base() { }
    }
}
