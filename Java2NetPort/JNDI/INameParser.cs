using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Java2NetPort.JNDI
{
    public interface INameParser
    {
        IName Parse(string name);
    }
}
