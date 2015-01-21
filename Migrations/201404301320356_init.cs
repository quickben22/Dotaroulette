namespace WebApplication5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        LanguageID = c.Int(nullable: false, identity: true),
                        Jezik = c.String(),
                    })
                .PrimaryKey(t => t.LanguageID);
            
            CreateTable(
                "dbo.UserProfileInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        avatar_url = c.String(),
                        avatar_url2 = c.String(),
                        steam_url = c.String(),
                        MinRating = c.Int(nullable: false),
                        Rating = c.Int(nullable: false),
                        MaxRating = c.Int(nullable: false),
                        FavPosition = c.Int(nullable: false),
                        TimeZone = c.Int(nullable: false),
                        MinTime = c.Int(nullable: false),
                        MaxTime = c.Int(nullable: false),
                        Microphone = c.Int(nullable: false),
                        Goal = c.Int(nullable: false),
                        Search = c.Int(nullable: false),
                        MinAge = c.Int(nullable: false),
                        Age = c.Int(nullable: false),
                        MaxAge = c.Int(nullable: false),
                        Team_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MemberGroups", t => t.Team_Id)
                .Index(t => t.Team_Id);
            
            CreateTable(
                "dbo.Positions",
                c => new
                    {
                        PositionID = c.Int(nullable: false, identity: true),
                        PositionNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PositionID);
            
            CreateTable(
                "dbo.MemberGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OwnerId = c.Int(nullable: false),
                        Broj_clanova = c.Int(nullable: false),
                        Url = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MemberGroupmembers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Aktivnost = c.Int(nullable: false),
                        Role = c.Int(nullable: false),
                        Votes = c.Int(nullable: false),
                        Group_Id = c.Int(),
                        MemberDetails_Id = c.Int(),
                        Vote_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MemberGroups", t => t.Group_Id)
                .ForeignKey("dbo.UserProfileInfoes", t => t.MemberDetails_Id)
                .ForeignKey("dbo.MemberGroupmembers", t => t.Vote_Id)
                .Index(t => t.Group_Id)
                .Index(t => t.MemberDetails_Id)
                .Index(t => t.Vote_Id);
            
            CreateTable(
                "dbo.MemberAlerts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Version = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Ishidden = c.Int(),
                        Message = c.String(),
                        PUser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfileInfoes", t => t.PUser_Id)
                .Index(t => t.PUser_Id);
            
            CreateTable(
                "dbo.Alerttypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Paramcount = c.Int(nullable: false),
                        SupportEmail = c.String(),
                        Template = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MemberRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Guid = c.String(),
                        Email = c.String(),
                        Status = c.Int(nullable: false),
                        TargetPageid = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Rola = c.Int(nullable: false),
                        Recipientid_Id = c.Int(),
                        Senderid_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfileInfoes", t => t.Recipientid_Id)
                .ForeignKey("dbo.UserProfileInfoes", t => t.Senderid_Id)
                .Index(t => t.Recipientid_Id)
                .Index(t => t.Senderid_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        UserProfileInfo_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfileInfoes", t => t.UserProfileInfo_Id)
                .Index(t => t.UserProfileInfo_Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.LoginProvider, t.ProviderKey })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserProfileInfoLanguages",
                c => new
                    {
                        UserProfileInfo_Id = c.Int(nullable: false),
                        Language_LanguageID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserProfileInfo_Id, t.Language_LanguageID })
                .ForeignKey("dbo.UserProfileInfoes", t => t.UserProfileInfo_Id, cascadeDelete: true)
                .ForeignKey("dbo.Languages", t => t.Language_LanguageID, cascadeDelete: true)
                .Index(t => t.UserProfileInfo_Id)
                .Index(t => t.Language_LanguageID);
            
            CreateTable(
                "dbo.PositionUserProfileInfoes",
                c => new
                    {
                        Position_PositionID = c.Int(nullable: false),
                        UserProfileInfo_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Position_PositionID, t.UserProfileInfo_Id })
                .ForeignKey("dbo.Positions", t => t.Position_PositionID, cascadeDelete: true)
                .ForeignKey("dbo.UserProfileInfoes", t => t.UserProfileInfo_Id, cascadeDelete: true)
                .Index(t => t.Position_PositionID)
                .Index(t => t.UserProfileInfo_Id);
            
            CreateTable(
                "dbo.AlerttypeMemberAlerts",
                c => new
                    {
                        Alerttype_Id = c.Int(nullable: false),
                        MemberAlert_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Alerttype_Id, t.MemberAlert_Id })
                .ForeignKey("dbo.Alerttypes", t => t.Alerttype_Id, cascadeDelete: true)
                .ForeignKey("dbo.MemberAlerts", t => t.MemberAlert_Id, cascadeDelete: true)
                .Index(t => t.Alerttype_Id)
                .Index(t => t.MemberAlert_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "UserProfileInfo_Id", "dbo.UserProfileInfoes");
            DropForeignKey("dbo.AspNetUserClaims", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.MemberRequests", "Senderid_Id", "dbo.UserProfileInfoes");
            DropForeignKey("dbo.MemberRequests", "Recipientid_Id", "dbo.UserProfileInfoes");
            DropForeignKey("dbo.MemberAlerts", "PUser_Id", "dbo.UserProfileInfoes");
            DropForeignKey("dbo.AlerttypeMemberAlerts", "MemberAlert_Id", "dbo.MemberAlerts");
            DropForeignKey("dbo.AlerttypeMemberAlerts", "Alerttype_Id", "dbo.Alerttypes");
            DropForeignKey("dbo.UserProfileInfoes", "Team_Id", "dbo.MemberGroups");
            DropForeignKey("dbo.MemberGroupmembers", "Vote_Id", "dbo.MemberGroupmembers");
            DropForeignKey("dbo.MemberGroupmembers", "MemberDetails_Id", "dbo.UserProfileInfoes");
            DropForeignKey("dbo.MemberGroupmembers", "Group_Id", "dbo.MemberGroups");
            DropForeignKey("dbo.PositionUserProfileInfoes", "UserProfileInfo_Id", "dbo.UserProfileInfoes");
            DropForeignKey("dbo.PositionUserProfileInfoes", "Position_PositionID", "dbo.Positions");
            DropForeignKey("dbo.UserProfileInfoLanguages", "Language_LanguageID", "dbo.Languages");
            DropForeignKey("dbo.UserProfileInfoLanguages", "UserProfileInfo_Id", "dbo.UserProfileInfoes");
            DropIndex("dbo.AspNetUsers", new[] { "UserProfileInfo_Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "User_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.MemberRequests", new[] { "Senderid_Id" });
            DropIndex("dbo.MemberRequests", new[] { "Recipientid_Id" });
            DropIndex("dbo.MemberAlerts", new[] { "PUser_Id" });
            DropIndex("dbo.AlerttypeMemberAlerts", new[] { "MemberAlert_Id" });
            DropIndex("dbo.AlerttypeMemberAlerts", new[] { "Alerttype_Id" });
            DropIndex("dbo.UserProfileInfoes", new[] { "Team_Id" });
            DropIndex("dbo.MemberGroupmembers", new[] { "Vote_Id" });
            DropIndex("dbo.MemberGroupmembers", new[] { "MemberDetails_Id" });
            DropIndex("dbo.MemberGroupmembers", new[] { "Group_Id" });
            DropIndex("dbo.PositionUserProfileInfoes", new[] { "UserProfileInfo_Id" });
            DropIndex("dbo.PositionUserProfileInfoes", new[] { "Position_PositionID" });
            DropIndex("dbo.UserProfileInfoLanguages", new[] { "Language_LanguageID" });
            DropIndex("dbo.UserProfileInfoLanguages", new[] { "UserProfileInfo_Id" });
            DropTable("dbo.AlerttypeMemberAlerts");
            DropTable("dbo.PositionUserProfileInfoes");
            DropTable("dbo.UserProfileInfoLanguages");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.MemberRequests");
            DropTable("dbo.Alerttypes");
            DropTable("dbo.MemberAlerts");
            DropTable("dbo.MemberGroupmembers");
            DropTable("dbo.MemberGroups");
            DropTable("dbo.Positions");
            DropTable("dbo.UserProfileInfoes");
            DropTable("dbo.Languages");
        }
    }
}
