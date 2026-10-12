namespace CompanyVehicleBookingSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBookingApprovalInformation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "ApprovedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "ApprovedDate");
        }
    }
}
