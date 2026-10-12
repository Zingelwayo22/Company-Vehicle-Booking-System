namespace CompanyVehicleBookingSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBookings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        BookingId = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        VehicleId = c.Int(nullable: false),
                        ReasonForVehicle = c.String(nullable: false, maxLength: 500),
                        VehicleJustification = c.String(nullable: false, maxLength: 500),
                        Destination = c.String(nullable: false, maxLength: 250),
                        StartDateTime = c.DateTime(nullable: false),
                        EndDateTime = c.DateTime(nullable: false),
                        AdditionalNotes = c.String(maxLength: 1000),
                        Status = c.String(nullable: false, maxLength: 30),
                        AdminDecisionReason = c.String(maxLength: 500),
                        ApprovedByUserId = c.String(maxLength: 128),
                        DecisionDate = c.DateTime(),
                        DateRequested = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.BookingId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApprovedByUserId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Vehicles", t => t.VehicleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.VehicleId)
                .Index(t => t.ApprovedByUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "VehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.Bookings", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Bookings", "ApprovedByUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Bookings", new[] { "ApprovedByUserId" });
            DropIndex("dbo.Bookings", new[] { "VehicleId" });
            DropIndex("dbo.Bookings", new[] { "UserId" });
            DropTable("dbo.Bookings");
        }
    }
}
