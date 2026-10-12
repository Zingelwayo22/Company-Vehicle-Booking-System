namespace CompanyVehicleBookingSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVehicle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vehicles", "VIN", c => c.String(maxLength: 50));
            AddColumn("dbo.Vehicles", "LicenceDiskExpiry", c => c.DateTime());
            AlterColumn("dbo.Vehicles", "Notes", c => c.String(maxLength: 1000));
            AlterColumn("dbo.Vehicles", "ImagePath", c => c.String());
            DropColumn("dbo.Vehicles", "FuelType");
            DropColumn("dbo.Vehicles", "NumberOfSeats");
            DropColumn("dbo.Vehicles", "Mileage");
            DropColumn("dbo.Vehicles", "LicenceDiscExpiry");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vehicles", "LicenceDiscExpiry", c => c.DateTime());
            AddColumn("dbo.Vehicles", "Mileage", c => c.Int(nullable: false));
            AddColumn("dbo.Vehicles", "NumberOfSeats", c => c.Int(nullable: false));
            AddColumn("dbo.Vehicles", "FuelType", c => c.String(maxLength: 30));
            AlterColumn("dbo.Vehicles", "ImagePath", c => c.String(maxLength: 500));
            AlterColumn("dbo.Vehicles", "Notes", c => c.String(maxLength: 500));
            DropColumn("dbo.Vehicles", "LicenceDiskExpiry");
            DropColumn("dbo.Vehicles", "VIN");
        }
    }
}
