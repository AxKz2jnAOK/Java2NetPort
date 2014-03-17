using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Java2NetPort.CriteriaAPI.Interfaces;

namespace Java2NetPort.CriteriaAPI
{
    public class JoinItem : IJoin
    {
        public IRoot Root
        {
            get;
            private set;
        }

        public IPath Path
        {
            get;
            private set;
        }

        public JoinType JoinType
        {
            get;
            private set;
        }

        public IExpression _onExpression;

        public JoinItem(IRoot root, JoinType joinType, IExpression joinOnExpression)
        {
            this.From = root.From;
            Root = root;
            JoinType = joinType;
            _onExpression = joinOnExpression;
        }


        public string ExpressionValue
        {
            get
            {
                return " " + JoinType.ToString().ToUpper() + " JOIN " + Root.From + " ON " + _onExpression.ExpressionValue;
            }
        }

        public string From
        {
            get;
            private set;
        }

        public IEnumerable<IJoin> Joins
        {
            get { throw new NotImplementedException(); }
        }

        public IPath Get(string selector)
        {
            return new Path(this.From, selector);
        }

        public IJoin Join(string joinObjectName, JoinType joinType, string leftSideColumnName, string rightSideColumnName)
        {
            throw new NotImplementedException();
        }

        //public Subquery Subquery()
        //{
        //    throw new NotImplementedException();
        //}

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
