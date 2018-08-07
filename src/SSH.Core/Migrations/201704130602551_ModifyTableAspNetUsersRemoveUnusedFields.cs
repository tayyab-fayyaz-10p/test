namespace SSH.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyTableAspNetUsersRemoveUnusedFields : DbMigration
    {
        public override void Up()
        {
            this.DropColumn("dbo.AspNetUsers", "Education");
            this.DropColumn("dbo.AspNetUsers", "Address");
            this.DropColumn("dbo.AspNetUsers", "ResourceId");
            this.DropColumn("dbo.AspNetUsers", "RegionId");
            this.DropColumn("dbo.AspNetUsers", "DistributionId");
            this.DropColumn("dbo.AspNetUsers", "CanEditAttendance");
        }
        
        public override void Down()
        {
            this.AddColumn("dbo.AspNetUsers", "CanEditAttendance", c => c.Boolean(nullable: false));
            this.AddColumn("dbo.AspNetUsers", "DistributionId", c => c.Int());
            this.AddColumn("dbo.AspNetUsers", "RegionId", c => c.Int());
            this.AddColumn("dbo.AspNetUsers", "ResourceId", c => c.Int());
            this.AddColumn("dbo.AspNetUsers", "Address", c => c.String());
            this.AddColumn("dbo.AspNetUsers", "Education", c => c.String());
        }
    }
}
