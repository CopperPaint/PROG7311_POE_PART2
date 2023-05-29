namespace PROG7311_PART2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateFarmers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Farmers",
                c => new
                    {
                        FarmerID = c.Int(nullable: false, identity: true),
                        UserID = c.String(nullable: false),
                        FarName = c.String(nullable: false),
                        FarSurname = c.String(nullable: false),
                        FarAdress = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.FarmerID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Farmers");
        }
    }
}
