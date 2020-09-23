namespace BELibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Edit_Status_To_Record_Table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Record", "StatusRecord", c => c.Int(nullable: false));
            DropColumn("dbo.Record", "Status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Record", "Status", c => c.Int(nullable: false));
            DropColumn("dbo.Record", "StatusRecord");
        }
    }
}
