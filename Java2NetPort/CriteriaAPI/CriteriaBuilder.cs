using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Java2NetPort.CriteriaAPI.Interfaces;

namespace Java2NetPort.CriteriaAPI
{
    internal class CriteriaBuilder : ICriteriaBuilder
    {
        public ICriteriaQuery CreateQuery()
        {
            return new CriteriaQuery();
        }

        public IPredicate Equal(IExpression left, IExpression right)
        {   
            return new Predicate(left.ExpressionValue, right.ExpressionValue, BinaryOperators.Equal);
        }



        public IPredicate NotEqual(IExpression left, IExpression right)
        {
            return new Predicate(left.ExpressionValue, right.ExpressionValue, BinaryOperators.NotEqual);
        }

        public IPredicate GreaterThen(IExpression left, IExpression right)
        {
            return new Predicate(left.ExpressionValue, right.ExpressionValue, BinaryOperators.GreaterThen);
        }


        public IPredicate GreaterOrEqualThen(IExpression left, IExpression right)
        {
            return new Predicate(left.ExpressionValue, right.ExpressionValue, BinaryOperators.GreaterOrEqualThen);
        }

        public IPredicate LessThen(IExpression left, IExpression right)
        {
            return new Predicate(left.ExpressionValue, right.ExpressionValue, BinaryOperators.LessThen);
        }

        public IPredicate LessOrEqualThen(IExpression left, IExpression right)
        {
            return new Predicate(left.ExpressionValue, right.ExpressionValue, BinaryOperators.LessOrEqualThen);
        }

        public IPredicate Like(IExpression left, IExpression right)
        {
            return new Predicate(left.ExpressionValue, right.ExpressionValue, BinaryOperators.Like);
        }

        public IPredicate NotLike(IExpression left, IExpression right)
        {
            return new Predicate(left.ExpressionValue, right.ExpressionValue, BinaryOperators.NotLike);
        }
        
        public IPredicate Between(IExpression value, IExpression minComponent, IExpression maxComponent)
        {
            return new Predicate(value.ExpressionValue, minComponent.ExpressionValue, maxComponent.ExpressionValue, TernaryOperators.Between);
        }

        public IPredicate And(IExpression left, IExpression right)
        {
            return new Predicate(left.ExpressionValue, right.ExpressionValue, BinaryOperators.And);
        }

        public IPredicate Or(IExpression left, IExpression right)
        {
            return new Predicate(left.ExpressionValue, right.ExpressionValue, BinaryOperators.Or);
        }

        public IPredicate Not(IPredicate predicate)
        {
            return new Predicate(predicate.ExpressionValue, UnaryOperators.Not);
        }
        
        public IPredicate IsNull(IExpression operand)
        {
            return new Predicate(operand.ExpressionValue, UnaryOperators.IsNull);
        }


        public IPredicate IsNotNull(IExpression operand)
        {
            return new Predicate(operand.ExpressionValue, UnaryOperators.IsNotNull);
        }


        public IParameterExpression Parameter(System.Data.SqlDbType sqlDbType, string parameterValue)
        {
            if (sqlDbType == System.Data.SqlDbType.NChar
                || sqlDbType == System.Data.SqlDbType.NText
                || sqlDbType == System.Data.SqlDbType.NVarChar
                || sqlDbType == System.Data.SqlDbType.VarChar)
                return new ParameterExpression("'" + parameterValue + "'");
            else
                return new ParameterExpression(parameterValue);
        }   
        public IPredicate Conjunction()
        {
            return new Predicate();
        }

        public IPredicate Disjunction()
        {
            return new Predicate();
        }


        public IExpression Literal(string value)
        {
            return new Literal(value);
        }


        public IOrder Asc(IExpression orderingExpression)
        {
            return new Order(orderingExpression, true);

        }

        public IOrder Desc(IExpression orderingExpression)
        {
            return new Order(orderingExpression, false);
        }


        public IEnumerable<IOrder> Asc(params IExpression[] orderingExpressions)
        {
            foreach(IExpression expre in orderingExpressions)
            {
                yield return new Order(expre, true);
            }
        }

        public IEnumerable<IOrder> Desc(params IExpression[] orderingExpressions)
        {
            foreach (IExpression expre in orderingExpressions)
            {
                yield return new Order(expre, false);
            }
        }
    }
}
