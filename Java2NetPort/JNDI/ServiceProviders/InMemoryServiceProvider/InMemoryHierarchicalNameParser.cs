using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Java2NetPort.JNDI.ServiceProviders.InMemoryServiceProvider
{
    public class InMemoryHierarchicalNameParser : INameParser
    {
        public IName Parse(string name)
        {
            Dictionary<string, string> syntax = new Dictionary<string, string>()
            {
                {NameSyntaxKeys.separator.ToString(), ";"},
                {NameSyntaxKeys.trimblanks.ToString(), "true"}
            };

            return new CompoundName(name, syntax);
        }
    }
}
