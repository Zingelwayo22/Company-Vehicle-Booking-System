using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CompanyVehicleBookingSystem.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string EmployeeNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(100)]
        public string Position { get; set; }

        [Display(Name = "Driver's Licence Number")]
        [StringLength(50)]
        public string DriversLicenceNumber { get; set; }

        [Display(Name = "Driver's Licence Expiry")]
        [DataType(DataType.Date)]
        public DateTime? DriversLicenceExpiry { get; set; }

        public int? DepartmentId { get; set; }

        public virtual Department Department { get; set; }

        public bool IsVehicleBookingAllowed { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateAdded { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<VehicleInspection> VehicleInspections { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VehicleInspection>()
                .HasRequired(i => i.Vehicle)
                .WithMany(v => v.Inspections)
                .HasForeignKey(i => i.VehicleId)
                .WillCascadeOnDelete(false);
        }

    }
}