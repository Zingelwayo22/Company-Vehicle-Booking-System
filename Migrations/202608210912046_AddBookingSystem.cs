namespace CompanyVehicleBookingSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBookingSystem : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bookings", "ApprovedByUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Bookings", new[] { "ApprovedByUserId" });
            AddColumn("dbo.Bookings", "VehicleSelectionReason", c => c.String(nullable: false, maxLength: 500));
            AddColumn("dbo.Bookings", "AdminComments", c => c.String(maxLength: 500));
            AddColumn("dbo.Bookings", "DateCreated", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Bookings", "ApprovedByUserId", c => c.String());
            DropColumn("dbo.Bookings", "VehicleJustification");
            DropColumn("dbo.Bookings", "Destination");
            DropColumn("dbo.Bookings", "AdditionalNotes");
            DropColumn("dbo.Bookings", "AdminDecisionReason");
            DropColumn("dbo.Bookings", "DateRequested");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bookings", "DateRequested", c => c.DateTime(nullable: false));
            AddColumn("dbo.Bookings", "AdminDecisionReason", c => c.String(maxLength: 500));
            AddColumn("dbo.Bookings", "AdditionalNotes", c => c.String(maxLength: 1000));
            AddColumn("dbo.Bookings", "Destination", c => c.String(nullable: false, maxLength: 250));
            AddColumn("dbo.Bookings", "VehicleJustification", c => c.String(nullable: false, maxLength: 500));
            AlterColumn("dbo.Bookings", "ApprovedByUserId", c => c.String(maxLength: 128));
            DropColumn("dbo.Bookings", "DateCreated");
            DropColumn("dbo.Bookings", "AdminComments");
            DropColumn("dbo.Bookings", "VehicleSelectionReason");
            CreateIndex("dbo.Bookings", "ApprovedByUserId");
            AddForeignKey("dbo.Bookings", "ApprovedByUserId", "dbo.AspNetUsers", "Id");
        }
    }
}
