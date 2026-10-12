namespace CompanyVehicleBookingSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDriversLicenceInformation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "DriversLicenceNumber", c => c.String(maxLength: 50));
            AddColumn("dbo.AspNetUsers", "DriversLicenceExpiry", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "DriversLicenceExpiry");
            DropColumn("dbo.AspNetUsers", "DriversLicenceNumber");
        }
    }
}
