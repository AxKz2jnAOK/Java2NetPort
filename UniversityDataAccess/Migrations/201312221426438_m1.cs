namespace Java2NetPort.Tests.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m1 : DbMigration
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
            
            AddColumn("dbo.Students", "Faculty_Id", c => c.Int());
            CreateIndex("dbo.Students", "Faculty_Id");
            AddForeignKey("dbo.Students", "Faculty_Id", "dbo.Faculties", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Students", "Faculty_Id", "dbo.Faculties");
            DropIndex("dbo.Students", new[] { "Faculty_Id" });
            DropColumn("dbo.Students", "Faculty_Id");
            DropTable("dbo.Faculties");
        }
    }
}
