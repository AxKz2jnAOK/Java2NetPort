using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Java2NetPort.CriteriaAPI.Interfaces;

namespace Java2NetPort.CriteriaAPI
{
    public class Order : IOrder
    {

        public IExpression OrderExpression
        {
            get;
            private set;
        }

        public bool IsAscending
        {
            get;
            private set;
        }
        public Order(IExpression orderExpression, bool isAscending)
        {
            OrderExpression = orderExpression;
            IsAscending     = isAscending;
        }
    }
}
