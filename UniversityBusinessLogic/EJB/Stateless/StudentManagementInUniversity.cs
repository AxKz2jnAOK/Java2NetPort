using Java2NetPort;
using Java2NetPort.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityBusinessLogic.EJB.Stateless
{
    [Stateless]
    public class StudentManagementInUniversity
    {
        public void HelloWorld()
        {
            Console.WriteLine("Hello");
        }

        [Transaction(TransactionAttributeType.MANDATORY)]
        public void AddFaculty(int id, string name)
        {
        }


    }
}
