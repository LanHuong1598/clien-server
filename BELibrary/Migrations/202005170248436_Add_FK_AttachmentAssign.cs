namespace BELibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_FK_AttachmentAssign : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.AttachmentAssigns", "AttachmentId");
            CreateIndex("dbo.AttachmentAssigns", "DetailRecordId");
            AddForeignKey("dbo.AttachmentAssigns", "AttachmentId", "dbo.Attachments", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AttachmentAssigns", "DetailRecordId", "dbo.DetailRecord", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AttachmentAssigns", "DetailRecordId", "dbo.DetailRecord");
            DropForeignKey("dbo.AttachmentAssigns", "AttachmentId", "dbo.Attachments");
            DropIndex("dbo.AttachmentAssigns", new[] { "DetailRecordId" });
            DropIndex("dbo.AttachmentAssigns", new[] { "AttachmentId" });
        }
    }
}
