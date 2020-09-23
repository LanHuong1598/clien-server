namespace BELibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Prop_To_MedicalSupply : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MedicalSupplies", "DateOfHire", c => c.DateTime(nullable: false));
            AddColumn("dbo.MedicalSupplies", "Status", c => c.Int(nullable: false));
            DropColumn("dbo.MedicalSupplies", "AvailableAmount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MedicalSupplies", "AvailableAmount", c => c.Int(nullable: false));
            DropColumn("dbo.MedicalSupplies", "Status");
            DropColumn("dbo.MedicalSupplies", "DateOfHire");
        }
    }
}
