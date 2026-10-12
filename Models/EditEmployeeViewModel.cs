using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CompanyVehicleBookingSystem.Models
{
    public class EditEmployeeViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Employee Number")]
        [StringLength(50)]
        public string EmployeeNumber { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Position")]
        [StringLength(100)]
        public string Position { get; set; }

        [Display(Name = "Driver's Licence Number")]
        [StringLength(50)]
        public string DriversLicenceNumber { get; set; }

        [Display(Name = "Driver's Licence Expiry")]
        [DataType(DataType.Date)]
        public DateTime? DriversLicenceExpiry { get; set; }

        [Required]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        public IEnumerable<SelectListItem> Departments { get; set; }
    }
}