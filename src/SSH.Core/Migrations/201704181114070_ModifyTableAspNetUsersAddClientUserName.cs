namespace SSH.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyTableAspNetUsersAddClientUserName : DbMigration
    {
        public override void Up()
        {
            this.AddColumn("dbo.AspNetUsers", "ClientUserName", c => c.String());
        }
        
        public override void Down()
        {
            this.DropColumn("dbo.AspNetUsers", "ClientUserName");
        }
    }
}
