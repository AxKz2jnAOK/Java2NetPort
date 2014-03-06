using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityDataAccess
{
    /********************************************
     * Database model
     * *******************************************/
   

    public class UniversityContext : DbContext
    {
        public UniversityContext()
            : base("Magistrinis.Tests.UniversityContext")
        {
        }

        public DbSet<Student> Students { get; set; }

        public DbSet<Faculty> Faculties { get; set; }

        public DbSet<Log> Logs { get; set; }
    }

    public class UniversityInitializer : System.Data.Entity.CreateDatabaseIfNotExists<UniversityContext>
                                        //System.Data.Entity.DropCreateDatabaseAlways<UniversityContext> 
    {
        protected override void Seed(UniversityContext context)
        {    
        }
    }
}
