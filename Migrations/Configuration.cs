namespace CompanyVehicleBookingSystem.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CompanyVehicleBookingSystem.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "CompanyVehicleBookingSystem.Models.ApplicationDbContext";
        }

        protected override void Seed(CompanyVehicleBookingSystem.Models.ApplicationDbContext context)
        {
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                context.Roles.Add(new IdentityRole("Admin"));
            }

            if (!context.Roles.Any(r => r.Name == "Employee"))
            {
                context.Roles.Add(new IdentityRole("Employee"));
            }

            context.SaveChanges();
        }
    }
}
