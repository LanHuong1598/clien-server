namespace BELibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Name_Attachment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Attachments", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Attachments", "Name");
        }
    }
}
