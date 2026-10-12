using CompanyVehicleBookingSystem.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CompanyVehicleBookingSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class VehicleInspectionController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: VehicleInspection/Manage
        public async Task<ActionResult> Manage()
        {
            var bookings = await db.Bookings
                .Include(b => b.User)
                .Include(b => b.Vehicle)
                .Include(b => b.Inspections)
                .OrderByDescending(b => b.StartDateTime)
                .ToListAsync();

            return View(bookings);
        }
        // GET: VehicleInspection
        public async Task<ActionResult> Index(int? bookingId)
        {
            if (bookingId == null)
            {
                return new HttpStatusCodeResult(
                    System.Net.HttpStatusCode.BadRequest);
            }

            var booking = await db.Bookings
                .Include(b => b.User)
                .Include(b => b.Vehicle)
                .Include(b => b.Inspections)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);

            if (booking == null)
            {
                return HttpNotFound();
            }

            return View(booking);
        }

        // GET: VehicleInspection/Create
        public async Task<ActionResult> Create(int? bookingId, string type)
        {
            if (bookingId == null)
            {
                return new HttpStatusCodeResult(
                    System.Net.HttpStatusCode.BadRequest);
            }

            if (type != "Before" && type != "After")
            {
                return new HttpStatusCodeResult(
                    System.Net.HttpStatusCode.BadRequest,
                    "Invalid inspection type.");
            }

            var booking = await db.Bookings
                .Include(b => b.User)
                .Include(b => b.Vehicle)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);

            if (booking == null)
            {
                return HttpNotFound();
            }

            var inspection = new VehicleInspection
            {
                BookingId = booking.BookingId,
                VehicleId = booking.VehicleId,
                InspectionType = type,
                InspectionDate = DateTime.Now,
                InspectedByUserId = User.Identity.GetUserId()
            };

            return View(inspection);
        }

        // POST: VehicleInspection/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(VehicleInspection model)
        {
            // Get the currently logged-in administrator
            var userId = User.Identity.GetUserId();

            // Get the booking
            var booking = await db.Bookings
                .FirstOrDefaultAsync(b => b.BookingId == model.BookingId);

            if (booking == null)
            {
                return HttpNotFound();
            }

            // Set values that are controlled by the system
            model.VehicleId = booking.VehicleId;
            model.InspectedByUserId = userId;
            model.InspectionDate = DateTime.Now;

            // Validate inspection type
            if (model.InspectionType != "Before" &&
                model.InspectionType != "After")
            {
                ModelState.AddModelError(
                    "InspectionType",
                    "Invalid inspection type."
                );
            }

            // Remove system-generated fields from validation
            ModelState.Remove("InspectedByUserId");
            ModelState.Remove("InspectionDate");
            ModelState.Remove("VehicleId");

            if (!ModelState.IsValid)
            {
                // Debug errors
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors.Select(error =>
                        x.Key + ": " +
                        (string.IsNullOrEmpty(error.ErrorMessage)
                            ? error.Exception?.Message
                            : error.ErrorMessage)))
                    .ToList();

                foreach (var error in errors)
                {
                    System.Diagnostics.Debug.WriteLine(
                        "INSPECTION ERROR: " + error
                    );
                }

                return View(model);
            }

            // Make sure the inspection belongs to the booking vehicle
            model.VehicleId = booking.VehicleId;

            db.VehicleInspections.Add(model);

            await db.SaveChangesAsync();

            TempData["Success"] =
                model.InspectionType +
                " vehicle inspection recorded successfully.";

            return RedirectToAction(
                "Index",
                new { bookingId = model.BookingId }
            );
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