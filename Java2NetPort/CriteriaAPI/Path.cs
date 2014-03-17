using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Java2NetPort.CriteriaAPI
{
    public class Path : IPath
    {
        public Path(string from, string value)
        {
            From = from;
            Value = value;
        }


        public string ExpressionValue
        {
            get { return From + "." + Value; }
        }

        public string Value
        {
            get;
            private set;
        }

        public string From
        {
            get;
            private set;
        }

        public string AliasValue
        {
            get;
            private set;
        }

        public ISelection Alias(string alias)
        {
            AliasValue = alias;
            return this;
        }
    }
}
