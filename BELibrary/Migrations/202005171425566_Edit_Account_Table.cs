namespace BELibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Edit_Account_Table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Account", "Phone", c => c.String(maxLength: 15));
            AddColumn("dbo.Account", "LinkAvatar", c => c.String(maxLength: 250));
            DropColumn("dbo.Account", "PhoneNumber");
            DropColumn("dbo.Account", "LinkAvata");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Account", "LinkAvata", c => c.String(maxLength: 250));
            AddColumn("dbo.Account", "PhoneNumber", c => c.String(maxLength: 15));
            DropColumn("dbo.Account", "LinkAvatar");
            DropColumn("dbo.Account", "Phone");
        }
    }
}
