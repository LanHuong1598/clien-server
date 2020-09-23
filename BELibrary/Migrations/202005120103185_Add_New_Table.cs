namespace BELibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_New_Table : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.DetailRecord");
            CreateTable(
                "dbo.AttachmentAssigns",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AttachmentId = c.Guid(nullable: false),
                        DetailRecordId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Attachments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Type = c.String(),
                        Url = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Record", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Record", "CreatedBy", c => c.String());
            AddColumn("dbo.Record", "ModifiedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Record", "ModifiedBy", c => c.String());
            AddColumn("dbo.Record", "Doctor", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.DetailRecord", "Id", c => c.Guid(nullable: false));
            AddColumn("dbo.DetailRecord", "Note", c => c.String(nullable: false));
            AddColumn("dbo.DetailRecord", "Result", c => c.String(nullable: false));
            AddColumn("dbo.DetailRecord", "DoctorName", c => c.String());
            AddColumn("dbo.DetailRecord", "Process", c => c.Int(nullable: false));
            AlterColumn("dbo.DetailRecord", "Status", c => c.Boolean(nullable: false));
            AddPrimaryKey("dbo.DetailRecord", "Id");
            DropColumn("dbo.Record", "StartTime");
            DropColumn("dbo.Record", "EndTime");
            DropColumn("dbo.Record", "Doctoe");
            DropColumn("dbo.DetailRecord", "DetailRecordId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DetailRecord", "DetailRecordId", c => c.Guid(nullable: false));
            AddColumn("dbo.Record", "Doctoe", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Record", "EndTime", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.Record", "StartTime", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            DropPrimaryKey("dbo.DetailRecord");
            AlterColumn("dbo.DetailRecord", "Status", c => c.String(nullable: false));
            DropColumn("dbo.DetailRecord", "Process");
            DropColumn("dbo.DetailRecord", "DoctorName");
            DropColumn("dbo.DetailRecord", "Result");
            DropColumn("dbo.DetailRecord", "Note");
            DropColumn("dbo.DetailRecord", "Id");
            DropColumn("dbo.Record", "Doctor");
            DropColumn("dbo.Record", "ModifiedBy");
            DropColumn("dbo.Record", "ModifiedDate");
            DropColumn("dbo.Record", "CreatedBy");
            DropColumn("dbo.Record", "CreatedDate");
            DropTable("dbo.Attachments");
            DropTable("dbo.AttachmentAssigns");
            AddPrimaryKey("dbo.DetailRecord", "DetailRecordId");
        }
    }
}
