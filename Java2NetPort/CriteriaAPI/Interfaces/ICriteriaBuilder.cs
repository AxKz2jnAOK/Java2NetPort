using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Java2NetPort.CriteriaAPI.Interfaces;

namespace Java2NetPort.CriteriaAPI
{
    public interface ICriteriaBuilder
    {
        ICriteriaQuery CreateQuery();
        IPredicate Equal(IExpression left, IExpression right);
        IPredicate NotEqual(IExpression left, IExpression right);
        IPredicate GreaterThen(IExpression left, IExpression right);
        IPredicate GreaterOrEqualThen(IExpression left, IExpression right);        
        IPredicate LessThen(IExpression left, IExpression right);
        IPredicate LessOrEqualThen(IExpression left, IExpression right);
        IPredicate Between(IExpression value, IExpression minComponent, IExpression maxComponent);
        IPredicate Like(IExpression left, IExpression right);
        IPredicate NotLike(IExpression left, IExpression right);
        IPredicate Not(IPredicate predicate);
        IPredicate IsNull(IExpression operand);
        IPredicate IsNotNull(IExpression operand);
        IPredicate And(IExpression left, IExpression right);
        IPredicate Or(IExpression left, IExpression right);
        IParameterExpression Parameter(System.Data.SqlDbType sqlDbType, string parameterValue);

        IPredicate Conjunction();
        IPredicate Disjunction();

        IExpression Literal(string value);

        IOrder Asc(IExpression orderingExpression);        
        IOrder Desc(IExpression orderingExpression);
        
    }
}
