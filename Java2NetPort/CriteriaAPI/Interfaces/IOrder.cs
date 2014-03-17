using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Java2NetPort.CriteriaAPI.Interfaces
{
    public interface IOrder
    {
        IExpression OrderExpression {get;}
        bool IsAscending { get; }
        //public Order(string orderExpression)
        //{

        //}
    }
}
