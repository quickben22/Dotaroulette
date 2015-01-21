namespace WebApplication5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfileInfoes", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfileInfoes", "Email");
        }
    }
}
