namespace SSH.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyTableAspNetUsersAddNewColumns : DbMigration
    {
        public override void Up()
        {
            this.AddColumn("dbo.AspNetUsers", "Status", c => c.Int(nullable: false));
            this.AddColumn("dbo.AspNetUsers", "IsAuthorized", c => c.Boolean(nullable: false));
            this.AddColumn("dbo.AspNetUsers", "Entity", c => c.String());
        }
        
        public override void Down()
        {
            this.DropColumn("dbo.AspNetUsers", "Entity");
            this.DropColumn("dbo.AspNetUsers", "IsAuthorized");
            this.DropColumn("dbo.AspNetUsers", "Status");
        }
    }
}
