namespace CompanyVehicleBookingSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBookingRejectionInformation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "RejectionReason", c => c.String());
            AddColumn("dbo.Bookings", "RejectedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "RejectedDate");
            DropColumn("dbo.Bookings", "RejectionReason");
        }
    }
}
