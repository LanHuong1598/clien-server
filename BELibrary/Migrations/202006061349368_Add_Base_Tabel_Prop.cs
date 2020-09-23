namespace BELibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Base_Tabel_Prop : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Item", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Item", "CreatedBy", c => c.String());
            AddColumn("dbo.Item", "ModifiedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Item", "ModifiedBy", c => c.String());
            AddColumn("dbo.Prescription", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Prescription", "CreatedBy", c => c.String());
            AddColumn("dbo.Prescription", "ModifiedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Prescription", "ModifiedBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Prescription", "ModifiedBy");
            DropColumn("dbo.Prescription", "ModifiedDate");
            DropColumn("dbo.Prescription", "CreatedBy");
            DropColumn("dbo.Prescription", "CreatedDate");
            DropColumn("dbo.Item", "ModifiedBy");
            DropColumn("dbo.Item", "ModifiedDate");
            DropColumn("dbo.Item", "CreatedBy");
            DropColumn("dbo.Item", "CreatedDate");
        }
    }
}
