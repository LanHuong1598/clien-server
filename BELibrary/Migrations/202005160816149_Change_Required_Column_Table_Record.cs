namespace BELibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_Required_Column_Table_Record : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Record", "Doctor", c => c.String(maxLength: 100));
            AlterColumn("dbo.Record", "Note", c => c.String());
            AlterColumn("dbo.Record", "Result", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Record", "Result", c => c.String(nullable: false));
            AlterColumn("dbo.Record", "Note", c => c.String(nullable: false));
            AlterColumn("dbo.Record", "Doctor", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
