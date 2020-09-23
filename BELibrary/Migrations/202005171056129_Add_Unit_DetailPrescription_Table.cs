namespace BELibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Unit_DetailPrescription_Table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DetailPrescription", "Unit", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DetailPrescription", "Unit");
        }
    }
}
