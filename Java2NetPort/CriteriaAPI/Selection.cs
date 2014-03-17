using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Java2NetPort.CriteriaAPI
{
    public class Selection : ISelection
    {
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

        public Selection(string value)
        {
            Value = value;
        }

        public Selection(string value, string from)
        {
            Value = value;
            From = from;
        }


        ISelection ISelection.Alias(string alias)
        {
            AliasValue = alias;
            return this;
        }
    }
}   
