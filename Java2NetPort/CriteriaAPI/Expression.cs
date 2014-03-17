using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Java2NetPort.CriteriaAPI
{
    internal class Expression : IExpression, ISelection
    {
        public Expression (string expression)
        {
            this.ExpressionValue = expression;
        }

        public string ExpressionValue
        {
            get;
            private set;
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

        public IExpression Alias(string alias)
        {
            throw new NotImplementedException();
        }


        ISelection ISelection.Alias(string alias)
        {
            throw new NotImplementedException();
        }
    }
}
