namespace SSH.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyTableAspNetUsersRemoveNICField : DbMigration
    {
        public override void Up()
        {
            this.DropColumn("dbo.AspNetUsers", "NIC");
        }
        
        public override void Down()
        {
            this.AddColumn("dbo.AspNetUsers", "NIC", c => c.String());
        }
    }
}
