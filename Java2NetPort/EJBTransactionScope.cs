using Java2NetPort.Attributes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Java2NetPort
{
    internal class EJBTransactionScope
    {
        public TransactionScope TransactionScope { get; set; }
        public TransactionAttributeType TransactionAttributeType { get; set; }
        public DbContext DbContext { get; set; }
    }
}
