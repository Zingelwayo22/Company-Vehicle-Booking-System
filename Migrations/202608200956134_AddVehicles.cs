namespace CompanyVehicleBookingSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVehicles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        VehicleId = c.Int(nullable: false, identity: true),
                        RegistrationNumber = c.String(nullable: false, maxLength: 20),
                        Make = c.String(nullable: false, maxLength: 50),
                        Model = c.String(nullable: false, maxLength: 50),
                        Year = c.Int(nullable: false),
                        Colour = c.String(maxLength: 30),
                        FuelType = c.String(maxLength: 30),
                        NumberOfSeats = c.Int(nullable: false),
                        Mileage = c.Int(nullable: false),
                        InsuranceExpiry = c.DateTime(),
                        LicenceDiscExpiry = c.DateTime(),
                        LastServiceDate = c.DateTime(),
                        NextServiceDate = c.DateTime(),
                        Status = c.String(nullable: false, maxLength: 30),
                        Notes = c.String(maxLength: 500),
                        ImagePath = c.String(maxLength: 500),
                        DateAdded = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.VehicleId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Vehicles");
        }
    }
}
