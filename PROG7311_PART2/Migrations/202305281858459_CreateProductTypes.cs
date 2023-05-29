namespace PROG7311_PART2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateProductTypes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductTypes",
                c => new
                    {
                        PTypeID = c.Int(nullable: false, identity: true),
                        PTName = c.String(nullable: false),
                        PTDescription = c.String(nullable: false),
                        PTCategory = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.PTypeID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProductTypes");
        }
    }
}
