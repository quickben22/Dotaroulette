namespace WebApplication5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfileInfoes", "About", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfileInfoes", "About");
        }
    }
}
