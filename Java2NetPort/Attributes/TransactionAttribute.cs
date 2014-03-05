using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Java2NetPort.Attributes
{
    public class TransactionAttribute : Attribute 
    {
        public TransactionAttributeType TransactionAttributeType {get; private set;}
        public TransactionAttribute(TransactionAttributeType transactionAttributeType)
        {
            TransactionAttributeType = transactionAttributeType;
        }
    }
}
