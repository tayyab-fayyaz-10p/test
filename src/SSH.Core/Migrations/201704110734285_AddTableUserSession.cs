namespace SSH.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableUserSession : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserSessions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        DeviceId = c.String(),
                        Token = c.String(),
                        DeviceType = c.String(),
                        PushToken = c.String(),
                        UniqueIdentifier = c.String(),
                        AppVersion = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
        }
        
        public override void Down()
        {
            this.DropForeignKey("dbo.UserSessions", "UserId", "dbo.AspNetUsers");
            this.DropIndex("dbo.UserSessions", new[] { "UserId" });
            this.DropTable("dbo.UserSessions");
        }
    }
}
