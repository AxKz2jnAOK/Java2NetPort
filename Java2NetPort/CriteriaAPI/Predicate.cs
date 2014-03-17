using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Java2NetPort.CriteriaAPI
{
    public class Predicate : IPredicate
    {
        public Predicate()
        {   
        }
        public Predicate(string firstOperand, string secondOperand, BinaryOperators binaryOperator)
        {   
            if(string.IsNullOrWhiteSpace(firstOperand))
            {
                ExpressionValue = secondOperand;
            }
            else
            { 
                ExpressionValue = string.Concat(firstOperand, binaryOperator.GetSqlExpression(), secondOperand);
            }
        }

        public Predicate(string operand, UnaryOperators unaryOperator)
        {
            if (unaryOperator == UnaryOperators.Not)
            {
                ExpressionValue = "NOT (" + operand + ")";
            }
            else if(unaryOperator == UnaryOperators.IsNull)
            {
                ExpressionValue = operand + " IS NULL";
            }
            else if (unaryOperator == UnaryOperators.IsNotNull)
            {
                ExpressionValue = operand + " IS NOT NULL";
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public Predicate(string firstOperand, string secondOperand, string thirdOperand, TernaryOperators ternaryOperator)
        {
            if(ternaryOperator == TernaryOperators.Between)
            {
                ExpressionValue = string.Format("{0} BETWEEN {1} AND {2}", firstOperand, secondOperand, thirdOperand);
            }
            else
            {
                throw new NotImplementedException();
            }
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

        public ISelection Alias(string alias)
        {
            throw new NotImplementedException();
        }
    }
}
