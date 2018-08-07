namespace SSH.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyTableAspNetUsersAddDeviceColumn : DbMigration
    {
        public override void Up()
        {
            this.AddColumn("dbo.AspNetUsers", "DeviceId", c => c.Int());
            this.CreateIndex("dbo.AspNetUsers", "DeviceId");
            this.AddForeignKey("dbo.AspNetUsers", "DeviceId", "dbo.Devices", "Id");
        }
        
        public override void Down()
        {
            this.DropForeignKey("dbo.AspNetUsers", "DeviceId", "dbo.Devices");
            this.DropIndex("dbo.AspNetUsers", new[] { "DeviceId" });
            this.DropColumn("dbo.AspNetUsers", "DeviceId");
        }
    }
}
