using System.Collections.Generic;

namespace CompanyVehicleBookingSystem.Models
{
    public class AdminDashboardViewModel
    {
        public int TotalBookings { get; set; }

        public int PendingBookings { get; set; }

        public int ApprovedBookings { get; set; }

        public int RejectedBookings { get; set; }

        public int CompletedBookings { get; set; }

        public int TotalVehicles { get; set; }

        public int AvailableVehicles { get; set; }

        public int UnavailableVehicles { get; set; }

        public List<Booking> RecentBookings { get; set; }

        public int CancelledBookings { get; set; }
    }
}