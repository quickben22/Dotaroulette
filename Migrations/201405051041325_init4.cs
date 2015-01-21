namespace WebApplication5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MemberGroups", "Language", c => c.String());
            AddColumn("dbo.MemberGroups", "About", c => c.String());
            AddColumn("dbo.MemberGroups", "Goal", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MemberGroups", "Goal");
            DropColumn("dbo.MemberGroups", "About");
            DropColumn("dbo.MemberGroups", "Language");
        }
    }
}
