namespace BELibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init_Database : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Account",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FullName = c.String(maxLength: 50),
                        PhoneNumber = c.String(maxLength: 15),
                        UserName = c.String(maxLength: 50),
                        LinkAvata = c.String(maxLength: 250),
                        Gender = c.Boolean(nullable: false),
                        Password = c.String(maxLength: 250),
                        Role = c.Int(nullable: false),
                        PatientId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Patient", t => t.PatientId)
                .Index(t => t.PatientId);
            
            CreateTable(
                "dbo.Patient",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FullName = c.String(nullable: false, maxLength: 100),
                        DateOfBirth = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Address = c.String(nullable: false, maxLength: 250),
                        Gender = c.Boolean(nullable: false),
                        IndentificationCardId = c.String(nullable: false, maxLength: 12, unicode: false),
                        Phone = c.String(nullable: false, maxLength: 12, unicode: false),
                        RecordId = c.Guid(),
                        Status = c.Boolean(nullable: false),
                        ImageProfile = c.String(),
                        PatientCode = c.String(nullable: false),
                        JoinDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        IndentificationCardDate = c.DateTime(nullable: false),
                        Job = c.String(),
                        WorkPlace = c.String(),
                        HistoryOfIllnessFamily = c.String(),
                        HistoryOfIllnessYourself = c.String(),
                        Email = c.String(nullable: false, maxLength: 250),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Record", t => t.RecordId)
                .Index(t => t.RecordId);
            
            CreateTable(
                "dbo.MedicalSupplies",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Amount = c.Int(nullable: false),
                        AvailableAmount = c.Int(nullable: false),
                        PatientId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Patient", t => t.PatientId)
                .Index(t => t.PatientId);
            
            CreateTable(
                "dbo.Record",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        StartTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        EndTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Doctoe = c.String(nullable: false, maxLength: 100),
                        Note = c.String(nullable: false),
                        Result = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DetailRecord",
                c => new
                    {
                        DetailRecordId = c.Guid(nullable: false),
                        DiseaseName = c.String(nullable: false, maxLength: 200),
                        Status = c.String(nullable: false),
                        PrescriptionId = c.Guid(nullable: true),
                        RecordId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.DetailRecordId)
                .ForeignKey("dbo.Prescription", t => t.PrescriptionId)
                .ForeignKey("dbo.Record", t => t.RecordId)
                .Index(t => t.PrescriptionId)
                .Index(t => t.RecordId);
            
            CreateTable(
                "dbo.Prescription",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DetailPrescriptionId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DetailPrescription", t => t.DetailPrescriptionId)
                .Index(t => t.DetailPrescriptionId);
            
            CreateTable(
                "dbo.DetailPrescription",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Amount = c.Int(nullable: false),
                        Note = c.String(nullable: false),
                        MedicineId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Medicine", t => t.MedicineId)
                .Index(t => t.MedicineId);
            
            CreateTable(
                "dbo.Medicine",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 200),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 200),
                        Unit = c.String(nullable: false, maxLength: 50),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Item",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 250),
                        Amount = c.Int(nullable: false),
                        Description = c.String(),
                        CategoryId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Category", t => t.CategoryId)
                .Index(t => t.CategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Item", "CategoryId", "dbo.Category");
            DropForeignKey("dbo.Patient", "RecordId", "dbo.Record");
            DropForeignKey("dbo.DetailRecord", "RecordId", "dbo.Record");
            DropForeignKey("dbo.DetailRecord", "PrescriptionId", "dbo.Prescription");
            DropForeignKey("dbo.Prescription", "DetailPrescriptionId", "dbo.DetailPrescription");
            DropForeignKey("dbo.DetailPrescription", "MedicineId", "dbo.Medicine");
            DropForeignKey("dbo.MedicalSupplies", "PatientId", "dbo.Patient");
            DropForeignKey("dbo.Account", "PatientId", "dbo.Patient");
            DropIndex("dbo.Item", new[] { "CategoryId" });
            DropIndex("dbo.DetailPrescription", new[] { "MedicineId" });
            DropIndex("dbo.Prescription", new[] { "DetailPrescriptionId" });
            DropIndex("dbo.DetailRecord", new[] { "RecordId" });
            DropIndex("dbo.DetailRecord", new[] { "PrescriptionId" });
            DropIndex("dbo.MedicalSupplies", new[] { "PatientId" });
            DropIndex("dbo.Patient", new[] { "RecordId" });
            DropIndex("dbo.Account", new[] { "PatientId" });
            DropTable("dbo.Item");
            DropTable("dbo.Category");
            DropTable("dbo.Medicine");
            DropTable("dbo.DetailPrescription");
            DropTable("dbo.Prescription");
            DropTable("dbo.DetailRecord");
            DropTable("dbo.Record");
            DropTable("dbo.MedicalSupplies");
            DropTable("dbo.Patient");
            DropTable("dbo.Account");
        }
    }
}
