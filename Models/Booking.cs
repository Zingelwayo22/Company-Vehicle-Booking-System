using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CompanyVehicleBookingSystem.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        public string UserId { get; set; }

        [Required]
        public int VehicleId { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Reason for Vehicle")]
        public string ReasonForVehicle { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Reason for Choosing This Vehicle")]
        public string VehicleSelectionReason { get; set; }

        [Required]
        [Display(Name = "Start Date and Time")]
        public DateTime StartDateTime { get; set; }

        [Required]
        [Display(Name = "End Date and Time")]
        public DateTime EndDateTime { get; set; }

        [Required]
        [StringLength(30)]
        public string Status { get; set; }

        [StringLength(500)]
        [Display(Name = "Admin Comments")]
        public string AdminComments { get; set; }

        public string ApprovedByUserId { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public DateTime? DecisionDate { get; set; }

        public string RejectionReason { get; set; }

        public DateTime? RejectedDate { get; set; }

        public string CancellationReason { get; set; }

        public DateTime? CancelledDate { get; set; }

        public string CancelledByUserId { get; set; }
        public DateTime DateCreated { get; set; }


        public virtual ApplicationUser User { get; set; }

        public virtual Vehicle Vehicle { get; set; }

        public virtual ICollection<VehicleInspection> Inspections { get; set; }

        public Booking()
        {
            Status = "Pending";
            DateCreated = DateTime.Now;
            Inspections = new HashSet<VehicleInspection>();
        }
    }
}