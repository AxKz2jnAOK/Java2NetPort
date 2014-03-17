namespace Java2NetPort.Tests.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addesseqnumb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Faculties",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        BirthDay = c.DateTime(nullable: false),
                        SequenceNumber = c.Int(nullable: false),
                        Faculty_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Faculties", t => t.Faculty_Id)
                .Index(t => t.Faculty_Id);
            
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Level = c.String(),
                        Message = c.String(),
                        InsertedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Students", "Faculty_Id", "dbo.Faculties");
            DropIndex("dbo.Students", new[] { "Faculty_Id" });
            DropTable("dbo.Logs");
            DropTable("dbo.Students");
            DropTable("dbo.Faculties");
        }
    }
}
