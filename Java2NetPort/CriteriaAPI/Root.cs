using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Java2NetPort.CriteriaAPI.Interfaces;

namespace Java2NetPort.CriteriaAPI
{

    internal class Root : IRoot
    {
        private string _select = string.Empty;
        
        private IList<IJoin> _joins;
        
        public IEnumerable<IJoin> Joins
        {
            get
            {
                return new List<IJoin>(_joins);
            }            
        }
        
        public Root(string from)
        {   
            From = from;
            _joins = new List<IJoin>();
        }

        public string From
        {
            get;
            private set;
        }


        public IPath Get(string selector)
        {
            return new Path(this.From, selector);
        }

        //public Subquery Subquery()
        //{
        //    throw new NotImplementedException();
        //}

        public IJoin Join(string joinObjectName, JoinType joinType, string leftSideColumnName, string rightSideColumnName)
        {
            IJoin join = new JoinItem(new Root(joinObjectName), joinType
                , new Expression(From + "." + leftSideColumnName + " = " + joinObjectName + "." + rightSideColumnName));
            _joins.Add(join);

            return join;
        }

        public string ExpressionValue
        {
            get { throw new NotImplementedException(); }
        }

        public string Value
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
    }
}
