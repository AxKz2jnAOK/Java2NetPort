using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Java2NetPort.JNDI.SPI;

namespace Java2NetPort.JNDI.ServiceProviders.InMemoryServiceProvider
{
    public class InitialInMemoryContextFactory : IInitialContextFactory
    {
        public IContext GetInitialContext(IReadOnlyDictionary<string, string> enviroment)
        {
            return InMemoryNamingServiceProvider.Instance;
        }
    }
}
