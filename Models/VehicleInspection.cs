using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyVehicleBookingSystem.Models
{
    public class VehicleInspection
    {
        [Key]
        public int InspectionId { get; set; }

        [Required]
        public int BookingId { get; set; }

        [ForeignKey("BookingId")]
        public virtual Booking Booking { get; set; }

        [Required]
        public int VehicleId { get; set; }

        [ForeignKey("VehicleId")]
        public virtual Vehicle Vehicle { get; set; }

        [Required]
        public string InspectionType { get; set; }

        [Display(Name = "Inspection Date")]
        public DateTime InspectionDate { get; set; }

        [Display(Name = "Mileage")]
        public int? Mileage { get; set; }

        [Display(Name = "Scratches")]
        public string Scratches { get; set; }

        [Display(Name = "Dents")]
        public string Dents { get; set; }

        [Display(Name = "Other Damage")]
        public string OtherDamage { get; set; }

        [Display(Name = "General Condition")]
        public string GeneralCondition { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }

        [Display(Name = "Photo Path")]
        public string PhotoPath { get; set; }

        [Required]
        public string InspectedByUserId { get; set; }

        [ForeignKey("InspectedByUserId")]
        public virtual ApplicationUser InspectedBy { get; set; }
    }
}