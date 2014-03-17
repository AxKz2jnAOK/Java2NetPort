using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Java2NetPort.CriteriaAPI
{
    public enum BinaryOperators
    {
        NotSet,
        Equal,
        NotEqual,
        GreaterThen,
        GreaterOrEqualThen,
        LessThen,
        LessOrEqualThen,

        Between,
        Like,
        NotLike,


        And,
        Or
    }       
}
