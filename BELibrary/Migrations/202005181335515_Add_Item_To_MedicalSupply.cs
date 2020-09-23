namespace BELibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Item_To_MedicalSupply : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MedicalSupplies", "ItemId", c => c.Guid(nullable: false));
            CreateIndex("dbo.MedicalSupplies", "ItemId");
            AddForeignKey("dbo.MedicalSupplies", "ItemId", "dbo.Item", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MedicalSupplies", "ItemId", "dbo.Item");
            DropIndex("dbo.MedicalSupplies", new[] { "ItemId" });
            DropColumn("dbo.MedicalSupplies", "ItemId");
        }
    }
}
