namespace FinanceHouse.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyTableAspNetUsers : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "DesignationId", "dbo.LOVs");
            DropForeignKey("dbo.AspNetUsers", "ResourceId", "dbo.Resources");
            DropIndex("dbo.AspNetUsers", new[] { "DesignationId" });
            DropIndex("dbo.AspNetUsers", new[] { "ResourceId" });
            AddColumn("dbo.AspNetUsers", "OneTimePin", c => c.String());
            DropColumn("dbo.AspNetUsers", "DesignationId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "DesignationId", c => c.Int());
            DropColumn("dbo.AspNetUsers", "OneTimePin");
            CreateIndex("dbo.AspNetUsers", "ResourceId");
            CreateIndex("dbo.AspNetUsers", "DesignationId");
            AddForeignKey("dbo.AspNetUsers", "ResourceId", "dbo.Resources", "Id");
            AddForeignKey("dbo.AspNetUsers", "DesignationId", "dbo.LOVs", "Id");
        }
    }
}
