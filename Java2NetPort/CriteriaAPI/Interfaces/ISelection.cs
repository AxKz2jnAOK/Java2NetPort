using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Java2NetPort.CriteriaAPI
{
    public interface ISelection
    {
        string Value { get; }
        string From  { get; }

        string AliasValue { get; }

        ISelection Alias(string alias);
    }
}
