namespace BELibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Status_To_Record_Table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Record", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Record", "Status");
        }
    }
}
