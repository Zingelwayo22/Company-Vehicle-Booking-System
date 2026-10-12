namespace CompanyVehicleBookingSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmployeeInformation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "EmployeeNumber", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.AspNetUsers", "Position", c => c.String(maxLength: 100));
            AddColumn("dbo.AspNetUsers", "Department", c => c.String(maxLength: 100));
            AddColumn("dbo.AspNetUsers", "IsVehicleBookingAllowed", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "DateAdded", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "DateAdded");
            DropColumn("dbo.AspNetUsers", "IsActive");
            DropColumn("dbo.AspNetUsers", "IsVehicleBookingAllowed");
            DropColumn("dbo.AspNetUsers", "Department");
            DropColumn("dbo.AspNetUsers", "Position");
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "FirstName");
            DropColumn("dbo.AspNetUsers", "EmployeeNumber");
        }
    }
}
