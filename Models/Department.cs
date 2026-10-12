using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CompanyVehicleBookingSystem.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public virtual ICollection<ApplicationUser> Employees { get; set; }

        public Department()
        {
            Employees = new HashSet<ApplicationUser>();
        }
    }
}