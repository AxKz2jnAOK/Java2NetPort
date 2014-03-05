using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Java2NetPort.JNDI
{
    public interface IName : IComparable
    {
        void Insert(int index, string item);
        void Add(string item);
        void Insert(int index, IName name);
        void Add(IName name);

        string Get(int index);
        
        bool IsEmpty();

        IEnumerable<string> GetAll();

        bool EndsWith(IName name);

        string Remove(int index);
    }
}
