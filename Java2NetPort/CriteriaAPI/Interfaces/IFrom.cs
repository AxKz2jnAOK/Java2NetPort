using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Java2NetPort.CriteriaAPI.Interfaces
{
    public interface IFrom : IPath
    {
        string From { get; }
        IEnumerable<IJoin> Joins { get; }

        IPath Get(string selector);

        IJoin Join(string joinObjectName, JoinType joinType, string leftSideColumnName, string rightSideColumnName);

        //Subquery Subquery();
    }
}
