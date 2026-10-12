using CompanyVehicleBookingSystem.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CompanyVehicleBookingSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserManagementController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ??
                    HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public async Task<ActionResult> Index()
        {
            var users = await db.Users
                .Include(u => u.Department)
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToListAsync();

            return View(users);
        }

        public async Task<ActionResult> Create()
        {
            var model = new CreateEmployeeViewModel
            {
                IsActive = true,
                IsVehicleBookingAllowed = true,
                Departments = await db.Departments
                    .OrderBy(d => d.Name)
                    .Select(d => new SelectListItem
                    {
                        Value = d.DepartmentId.ToString(),
                        Text = d.Name
                    })
                    .ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateEmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Departments = await db.Departments
                    .OrderBy(d => d.Name)
                    .Select(d => new SelectListItem
                    {
                        Value = d.DepartmentId.ToString(),
                        Text = d.Name
                    })
                    .ToListAsync();

                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,

                EmployeeNumber = model.EmployeeNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Position = model.Position,

                DriversLicenceNumber = model.DriversLicenceNumber,
                DriversLicenceExpiry = model.DriversLicenceExpiry,

                DepartmentId = model.DepartmentId,

                IsActive = model.IsActive,
                IsVehicleBookingAllowed = model.IsVehicleBookingAllowed,

                DateAdded = DateTime.Now
            };

            var result = await UserManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                TempData["Success"] = "Employee created successfully.";

                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }

            model.Departments = await db.Departments
                .OrderBy(d => d.Name)
                .Select(d => new SelectListItem
                {
                    Value = d.DepartmentId.ToString(),
                    Text = d.Name
                })
                .ToListAsync();

            return View(model);
        }

        // GET: UserManagement/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(
                    System.Net.HttpStatusCode.BadRequest);
            }

            var user = await db.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return HttpNotFound();
            }

            var model = new EditEmployeeViewModel
            {
                Id = user.Id,
                EmployeeNumber = user.EmployeeNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Position = user.Position,
                DriversLicenceNumber = user.DriversLicenceNumber,
                DriversLicenceExpiry = user.DriversLicenceExpiry,
                DepartmentId = user.DepartmentId ?? 0
            };

            model.Departments = await db.Departments
                .OrderBy(d => d.Name)
                .Select(d => new SelectListItem
                {
                    Value = d.DepartmentId.ToString(),
                    Text = d.Name
                })
                .ToListAsync();

            return View(model);
        }

        // POST: UserManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditEmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Departments = await db.Departments
                    .OrderBy(d => d.Name)
                    .Select(d => new SelectListItem
                    {
                        Value = d.DepartmentId.ToString(),
                        Text = d.Name
                    })
                    .ToListAsync();

                return View(model);
            }

            var user = await db.Users
                .FirstOrDefaultAsync(u => u.Id == model.Id);

            if (user == null)
            {
                return HttpNotFound();
            }

            user.EmployeeNumber = model.EmployeeNumber;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UserName = model.Email;
            user.Position = model.Position;
            user.DriversLicenceNumber = model.DriversLicenceNumber;
            user.DriversLicenceExpiry = model.DriversLicenceExpiry;
            user.DepartmentId = model.DepartmentId;

            await db.SaveChangesAsync();

            TempData["Success"] = "Employee information updated successfully.";

            return RedirectToAction("Index");
        }


        // GET: UserManagement/Manage/5
        public async Task<ActionResult> Manage(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(
                    System.Net.HttpStatusCode.BadRequest);
            }

            var user = await db.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdatePermissions(
    string id,
    bool IsActive,
    bool IsVehicleBookingAllowed)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(
                    System.Net.HttpStatusCode.BadRequest);
            }

            var user = await db.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return HttpNotFound();
            }

            user.IsActive = IsActive;
            user.IsVehicleBookingAllowed = IsVehicleBookingAllowed;

            await db.SaveChangesAsync();

            TempData["Success"] =
                "Employee permissions updated successfully.";

            return RedirectToAction("Index");
        }

        // GET: UserManagement/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(
                    System.Net.HttpStatusCode.BadRequest);
            }

            var user = await db.Users
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return HttpNotFound();
            }

            var bookings = await db.Bookings
                .Include(b => b.Vehicle)
                .Where(b => b.UserId == id)
                .OrderByDescending(b => b.DateCreated)
                .ToListAsync();

            ViewBag.Bookings = bookings;

            return View(user);
        }

        // GET: UserManagement/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(
                    System.Net.HttpStatusCode.BadRequest);
            }

            var user = await db.Users
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return HttpNotFound();
            }

            var hasBookings = await db.Bookings
                .AnyAsync(b => b.UserId == id);

            ViewBag.HasBookings = hasBookings;

            return View(user);
        }


        // POST: UserManagement/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(
                    System.Net.HttpStatusCode.BadRequest);
            }

            var user = await db.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return HttpNotFound();
            }

            var hasBookings = await db.Bookings
                .AnyAsync(b => b.UserId == id);

            if (hasBookings)
            {
                TempData["Error"] =
                    "This employee cannot be deleted because they have existing booking records. Deactivate the account instead.";

                return RedirectToAction("Index");
            }

            var result = await UserManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }

                return View("Delete", user);
            }

            TempData["Success"] = "Employee deleted successfully.";

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