namespace BELibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_Prescription_Allow_Null_DetailRecord_Table : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DetailRecord", "Note", c => c.String());
            AlterColumn("dbo.DetailRecord", "Result", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DetailRecord", "Result", c => c.String(nullable: false));
            AlterColumn("dbo.DetailRecord", "Note", c => c.String(nullable: false));
        }
    }
}
