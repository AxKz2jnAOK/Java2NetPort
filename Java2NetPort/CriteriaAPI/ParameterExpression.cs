using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Java2NetPort.CriteriaAPI.Interfaces;

namespace Java2NetPort.CriteriaAPI
{
    public class ParameterExpression : IParameterExpression
    {
        public ParameterExpression(string name)
        {
            ExpressionValue = name;
        }

        public string GetName()
        {
            throw new NotImplementedException();
        }

        public string Value
        {
            get { throw new NotImplementedException(); }
        }

        public string From
        {
            get { throw new NotImplementedException(); }
        }

        public string AliasValue
        {
            get { throw new NotImplementedException(); }
        }

        public ISelection Alias(string alias)
        {
            throw new NotImplementedException();
        }

        public string ExpressionValue
        {
            get;
            private set;
        }
    }
}
