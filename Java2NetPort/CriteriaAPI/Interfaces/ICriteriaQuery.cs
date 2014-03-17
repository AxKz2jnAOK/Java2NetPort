using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Java2NetPort.CriteriaAPI.Interfaces;

namespace Java2NetPort.CriteriaAPI.Interfaces
{
    public interface ICriteriaQuery : IExpression, IAbstractQuery
    {
        
        
        ICriteriaQuery Select(ISelection selection);

        ICriteriaQuery Select(params ISelection[] selections);

        ICriteriaQuery Select(params string[] selections);

        

        

        ICriteriaQuery OrderBy(IOrder ordering);

        ICriteriaQuery OrderBy(params IOrder[] orderings);

    }
}
