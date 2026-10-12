namespace CompanyVehicleBookingSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixBookingModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bookings", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Bookings", new[] { "UserId" });
            AlterColumn("dbo.Bookings", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Bookings", "UserId");
            AddForeignKey("dbo.Bookings", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Bookings", new[] { "UserId" });
            AlterColumn("dbo.Bookings", "UserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Bookings", "UserId");
            AddForeignKey("dbo.Bookings", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
