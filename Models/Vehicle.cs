using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CompanyVehicleBookingSystem.Models
{
    public class Vehicle
    {
        [Key]
        public int VehicleId { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Registration Number")]
        public string RegistrationNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string Make { get; set; }

        [Required]
        [StringLength(50)]
        public string Model { get; set; }

        [Range(1900, 2100)]
        public int Year { get; set; }

        [StringLength(30)]
        public string Colour { get; set; }

        [StringLength(50)]
        [Display(Name = "VIN")]
        public string VIN { get; set; }

        [Display(Name = "Vehicle Image")]
        public string ImagePath { get; set; }

        [Display(Name = "Insurance Expiry")]
        [DataType(DataType.Date)]
        public DateTime? InsuranceExpiry { get; set; }

        [Display(Name = "Licence Disk Expiry")]
        [DataType(DataType.Date)]
        public DateTime? LicenceDiskExpiry { get; set; }

        [Display(Name = "Last Service Date")]
        [DataType(DataType.Date)]
        public DateTime? LastServiceDate { get; set; }

        [Display(Name = "Next Service Date")]
        [DataType(DataType.Date)]
        public DateTime? NextServiceDate { get; set; }

        [Required]
        [StringLength(30)]
        public string Status { get; set; }

        [StringLength(1000)]
        public string Notes { get; set; }

        public DateTime DateAdded { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }

        public virtual ICollection<VehicleInspection> Inspections { get; set; }

        public Vehicle()
        {
            Bookings = new HashSet<Booking>();
            Inspections = new HashSet<VehicleInspection>();
            DateAdded = DateTime.Now;
            Status = "Available";
        }
    }
}