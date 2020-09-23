namespace BELibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Modify_DetailPrescription_Prescription : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DetailRecord", "PrescriptionId", "dbo.Prescription");
            DropForeignKey("dbo.Prescription", "DetailPrescriptionId", "dbo.DetailPrescription");
            DropIndex("dbo.DetailRecord", new[] { "PrescriptionId" });
            AddColumn("dbo.Prescription", "DetailRecordId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Prescription", "DetailRecordId");
            AddForeignKey("dbo.Prescription", "DetailRecordId", "dbo.DetailRecord", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Prescription", "DetailPrescriptionId", "dbo.DetailPrescription", "Id", cascadeDelete: true);
            DropColumn("dbo.DetailRecord", "PrescriptionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DetailRecord", "PrescriptionId", c => c.Guid(nullable: false));
            DropForeignKey("dbo.Prescription", "DetailPrescriptionId", "dbo.DetailPrescription");
            DropForeignKey("dbo.Prescription", "DetailRecordId", "dbo.DetailRecord");
            DropIndex("dbo.Prescription", new[] { "DetailRecordId" });
            DropColumn("dbo.Prescription", "DetailRecordId");
            CreateIndex("dbo.DetailRecord", "PrescriptionId");
            AddForeignKey("dbo.Prescription", "DetailPrescriptionId", "dbo.DetailPrescription", "Id");
            AddForeignKey("dbo.DetailRecord", "PrescriptionId", "dbo.Prescription", "Id");
        }
    }
}
