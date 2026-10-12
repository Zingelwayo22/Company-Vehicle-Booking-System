using CompanyVehicleBookingSystem.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CompanyVehicleBookingSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmployeeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ??
                       (_userManager = HttpContext.GetOwinContext()
                           .GetUserManager<ApplicationUserManager>());
            }
        }

        // GET: Employee
        public ActionResult Index()
        {
            var employees = db.Users
                .Include(u => u.Department)
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .ToList();

            return View(employees);
        }

        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(
                db.Departments.OrderBy(d => d.Name),
                "DepartmentId",
                "Name"
            );

            return View();
        }
        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            ApplicationUser model,
            string Email)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.DepartmentId = new SelectList(
                    db.Departments.OrderBy(d => d.Name),
                    "DepartmentId",
                    "Name",
                    model.DepartmentId
                );

                return View(model);
            }

            // Check if employee number already exists
            if (db.Users.Any(u => u.EmployeeNumber == model.EmployeeNumber))
            {
                ModelState.AddModelError(
                    "EmployeeNumber",
                    "This employee number is already registered."
                );

                ViewBag.DepartmentId = new SelectList(
                    db.Departments.OrderBy(d => d.Name),
                    "DepartmentId",
                    "Name",
                    model.DepartmentId
                );

                return View(model);
            }

            // Check if email already exists
            if (db.Users.Any(u => u.Email == Email))
            {
                ModelState.AddModelError(
                    "",
                    "An account with this email address already exists."
                );

                ViewBag.DepartmentId = new SelectList(
                    db.Departments.OrderBy(d => d.Name),
                    "DepartmentId",
                    "Name",
                    model.DepartmentId
                );

                return View(model);
            }

            // Create the employee's Identity account
            var user = new ApplicationUser
            {
                UserName = Email,
                Email = Email,

                EmployeeNumber = model.EmployeeNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DepartmentId = model.DepartmentId,
                Position = model.Position,

                PhoneNumber = model.PhoneNumber,

                DriversLicenceNumber = model.DriversLicenceNumber,
                DriversLicenceExpiry = model.DriversLicenceExpiry,

                IsVehicleBookingAllowed = model.IsVehicleBookingAllowed,
                IsActive = model.IsActive,

                DateAdded = DateTime.Now
            };

            // Generate a temporary password
            string temporaryPassword = "Welcome@123";

            var result = await UserManager.CreateAsync(
                user,
                temporaryPassword
            );

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }

                ViewBag.DepartmentId = new SelectList(
                    db.Departments.OrderBy(d => d.Name),
                    "DepartmentId",
                    "Name",
                    model.DepartmentId
                );

                return View(model);
            }

            // Assign Employee role
            if (!await UserManager.IsInRoleAsync(user.Id, "Employee"))
            {
                var roleResult = await UserManager.AddToRoleAsync(
                    user.Id,
                    "Employee"
                );

                if (!roleResult.Succeeded)
                {
                    ModelState.AddModelError(
                        "",
                        "Employee was created but the Employee role could not be assigned."
                    );

                    return View(model);
                }
            }

            TempData["SuccessMessage"] =
                "Employee created successfully. Temporary password: " +
                temporaryPassword;

            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}