using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Java2NetPort.CriteriaAPI
{
    public static class Extensions
    {
        public static string GetSqlExpression(this BinaryOperators predicateOperation)
        {
            string result;
            
            switch(predicateOperation)
            {
                case BinaryOperators.Equal:
                    result = " = ";
                    break;
                case BinaryOperators.NotEqual:
                    result = " <> ";
                    break;
                case BinaryOperators.GreaterThen:
                    result = " > ";
                    break;
                case BinaryOperators.GreaterOrEqualThen:
                    result = " >= ";
                    break;
                case BinaryOperators.LessThen:
                    result = " < ";
                    break;
                case BinaryOperators.LessOrEqualThen:
                    result = " <= ";
                    break;
                case BinaryOperators.Like:
                    result = " LIKE ";
                    break;
                case BinaryOperators.NotLike:
                    result = " NOT LIKE ";
                    break;
                case BinaryOperators.And:
                    result = " AND ";
                    break;
                case BinaryOperators.Or:
                    result = " OR ";
                    break;
                default: throw new ArgumentOutOfRangeException();
            }

            return result;
        }
    }
}
