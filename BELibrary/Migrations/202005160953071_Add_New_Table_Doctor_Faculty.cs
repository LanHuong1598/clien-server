namespace BELibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_New_Table_Doctor_Faculty : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Doctor",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Address = c.String(),
                        Gender = c.Boolean(nullable: false),
                        Phone = c.String(),
                        Email = c.String(),
                        FacultyId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Faculty", t => t.FacultyId, cascadeDelete: true)
                .Index(t => t.FacultyId);
            
            CreateTable(
                "dbo.Faculty",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Doctor", "FacultyId", "dbo.Faculty");
            DropIndex("dbo.Doctor", new[] { "FacultyId" });
            DropTable("dbo.Faculty");
            DropTable("dbo.Doctor");
        }
    }
}
