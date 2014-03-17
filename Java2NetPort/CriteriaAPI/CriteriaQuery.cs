using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Java2NetPort.CriteriaAPI.Interfaces;

namespace Java2NetPort.CriteriaAPI
{
    internal class CriteriaQuery : ICriteriaQuery
    {   
        private IList<IRoot> _roots = null;
        private IList<ISelection> _selections;
        private bool _distinct = false;
        private IExpression _whereExpression;
        private List<IOrder> _orderings;

        public CriteriaQuery()
        {
            _selections = new List<ISelection>();
            _roots = new List<IRoot>();
            _orderings = new List<IOrder>();
        }
        
        public IRoot From(string tableName)
        {
            IRoot root = new Root(tableName);
            _roots.Add(root);
            return root;
        }

        public override string ToString()
        {
            string result;

            string s = string.Empty;
            string roots = string.Empty;
            foreach(ISelection selection in _selections)
            {
                if (!string.IsNullOrEmpty(selection.From) && !string.IsNullOrEmpty(selection.AliasValue))
                {
                    s += selection.From + "." + selection.Value + " AS " + selection.AliasValue;
                }
                else if (!string.IsNullOrEmpty(selection.From))
                {
                    s += selection.From + "." + selection.Value;
                }
                else
                {
                    s += selection.Value;
                }
                s += ", ";
            }
            s = s.Substring(0, s.Length - 2);

            foreach(IRoot root in _roots)
            {
                roots += root.From;
                roots += ", ";
            }
            roots = roots.Substring(0, roots.Length - 2);

            if(_distinct)
            {
                result = string.Format("SELECT DISTINCT {0} FROM {1}", s, roots);
            }
            else
            {
                result = string.Format("SELECT {0} FROM {1}", s, roots);
            }

            foreach(IJoin join in _roots.SelectMany(e=>e.Joins))
            {
                result += join.ExpressionValue;
            }


            if(_whereExpression != null)
            {
                string queryRestrictions = string.Empty;
                result += " WHERE " + _whereExpression.ExpressionValue;
            }

            if (_orderings.Count > 0)
            {
                string orderings = " ORDER BY ";
                
                foreach (IOrder ordering in _orderings)
                {
                    if(ordering.IsAscending)
                    {
                        orderings += ordering.OrderExpression.ExpressionValue + " ASC, ";
                    }
                    else
                    {
                        orderings += ordering.OrderExpression.ExpressionValue + " DESC, ";
                    }
                }

                orderings = orderings.Substring(0, orderings.Length - 2);

                result += orderings;
            }

            return result;
            
        }

        public ICriteriaQuery OrderBy(IOrder order)
        {
            _orderings.Add(order);
            return this;
        }

        public ICriteriaQuery OrderBy(params IOrder[] orderings)
        {
            _orderings.AddRange(orderings);
            return this;
        }

        public ICriteriaQuery Select(ISelection selection)
        {
            _selections.Add(selection);
            return this;
        }

        public ICriteriaQuery Select(params ISelection[] selections)
        {
            foreach (ISelection selection in selections)
            {
                _selections.Add(selection);
            }

            return this;  
        }

        public ICriteriaQuery Select(params string[] selections)
        {
            foreach (string selection in selections)
            {
                _selections.Add(new Selection(selection));
            }
            return this;
        }

        public string ExpressionValue
        {
            get { return this.ToString(); }
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

        

        public ICriteriaQuery Where(IPredicate expression)
        {
            _whereExpression = expression;
            return this;
        }

        public void Distinct(bool distinct)
        {
            _distinct = distinct;
        }


        string ISelection.From
        {
            get { throw new NotImplementedException(); }
        }
    }
}
