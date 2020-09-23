namespace BELibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_DoctorId_To_DetailRecord_Table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DetailRecord", "DoctorId", c => c.Guid(nullable: false));
            AddColumn("dbo.DetailRecord", "FacultyId", c => c.Guid(nullable: false));
            CreateIndex("dbo.DetailRecord", "FacultyId");
            AddForeignKey("dbo.DetailRecord", "FacultyId", "dbo.Faculty", "Id", cascadeDelete: true);
            DropColumn("dbo.DetailRecord", "DoctorName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DetailRecord", "DoctorName", c => c.String());
            DropForeignKey("dbo.DetailRecord", "FacultyId", "dbo.Faculty");
            DropIndex("dbo.DetailRecord", new[] { "FacultyId" });
            DropColumn("dbo.DetailRecord", "FacultyId");
            DropColumn("dbo.DetailRecord", "DoctorId");
        }
    }
}
