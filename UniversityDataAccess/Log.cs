using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityDataAccess
{
    public class Log
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public virtual string Level { get; set; }
        public virtual string Message { get; set; }
        public virtual DateTime InsertedAt{ get; set; }
    }
}