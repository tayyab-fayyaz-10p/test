namespace SSH.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableDevice : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UniqueIdentifier = c.String(),
                        BrandId = c.Int(nullable: false),
                        ModelId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        LastModifiedBy = c.String(),
                        LastModifiedOn = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LOVs", t => t.BrandId)
                .ForeignKey("dbo.LOVs", t => t.ModelId)
                .Index(t => t.BrandId)
                .Index(t => t.ModelId);
        }
        
        public override void Down()
        {
            this.DropForeignKey("dbo.Devices", "ModelId", "dbo.LOVs");
            this.DropForeignKey("dbo.Devices", "BrandId", "dbo.LOVs");
            this.DropIndex("dbo.Devices", new[] { "ModelId" });
            this.DropIndex("dbo.Devices", new[] { "BrandId" });
            this.DropTable("dbo.Devices");
        }
    }
}
