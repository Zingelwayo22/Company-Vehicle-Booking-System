namespace CompanyVehicleBookingSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBookingCancellation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "CancellationReason", c => c.String());
            AddColumn("dbo.Bookings", "CancelledDate", c => c.DateTime());
            AddColumn("dbo.Bookings", "CancelledByUserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "CancelledByUserId");
            DropColumn("dbo.Bookings", "CancelledDate");
            DropColumn("dbo.Bookings", "CancellationReason");
        }
    }
}
