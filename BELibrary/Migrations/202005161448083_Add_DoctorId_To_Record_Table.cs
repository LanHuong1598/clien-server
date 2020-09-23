namespace BELibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_DoctorId_To_Record_Table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Record", "DoctorId", c => c.Guid());
            CreateIndex("dbo.Record", "DoctorId");
            AddForeignKey("dbo.Record", "DoctorId", "dbo.Doctor", "Id");
            DropColumn("dbo.Record", "Doctor");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Record", "Doctor", c => c.String(maxLength: 100));
            DropForeignKey("dbo.Record", "DoctorId", "dbo.Doctor");
            DropIndex("dbo.Record", new[] { "DoctorId" });
            DropColumn("dbo.Record", "DoctorId");
        }
    }
}
