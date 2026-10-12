namespace CompanyVehicleBookingSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVehicleInspections : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VehicleInspections",
                c => new
                {
                    InspectionId = c.Int(nullable: false, identity: true),
                    BookingId = c.Int(nullable: false),
                    VehicleId = c.Int(nullable: false),
                    InspectionType = c.String(nullable: false),
                    InspectionDate = c.DateTime(nullable: false),
                    Mileage = c.Int(),
                    Scratches = c.String(),
                    Dents = c.String(),
                    OtherDamage = c.String(),
                    GeneralCondition = c.String(),
                    Notes = c.String(),
                    PhotoPath = c.String(),
                    InspectedByUserId = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => t.InspectionId)
                .ForeignKey("dbo.Bookings", t => t.BookingId, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.InspectedByUserId, cascadeDelete: false)
                .ForeignKey("dbo.Vehicles", t => t.VehicleId, cascadeDelete: false)
                .Index(t => t.BookingId)
                .Index(t => t.VehicleId)
                .Index(t => t.InspectedByUserId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.VehicleInspections", "VehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.VehicleInspections", "InspectedByUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.VehicleInspections", "BookingId", "dbo.Bookings");
            DropIndex("dbo.VehicleInspections", new[] { "InspectedByUserId" });
            DropIndex("dbo.VehicleInspections", new[] { "VehicleId" });
            DropIndex("dbo.VehicleInspections", new[] { "BookingId" });
            DropTable("dbo.VehicleInspections");
        }
    }
}
