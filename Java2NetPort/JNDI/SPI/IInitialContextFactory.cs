using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Java2NetPort.JNDI.SPI
{
    public interface IInitialContextFactory
    {
        IContext GetInitialContext(IReadOnlyDictionary<string, string> enviroment);
    }
}
