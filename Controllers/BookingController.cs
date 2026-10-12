using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CompanyVehicleBookingSystem.Models;
using Microsoft.AspNet.Identity;

namespace CompanyVehicleBookingSystem.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Booking
        public async Task<ActionResult> Index()
        {
            var userId = User.Identity.GetUserId();

            var bookings = await db.Bookings
                .Include(b => b.Vehicle)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.StartDateTime)
                .ToListAsync();

            return View(bookings);
        }

        // GET: Booking/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(
                    System.Net.HttpStatusCode.BadRequest);
            }

            var userId = User.Identity.GetUserId();

            var booking = await db.Bookings
                .Include(b => b.Vehicle)
                .Include(b => b.Inspections)
                .FirstOrDefaultAsync(b =>
                    b.BookingId == id &&
                    b.UserId == userId);

            if (booking == null)
            {
                return HttpNotFound();
            }

            return View(booking);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AdminIndex()
        {
            var bookings = await db.Bookings
                .Include(b => b.User)
                .Include(b => b.Vehicle)
                .OrderBy(b => b.Status == "Pending" ? 0 : 1)
                .ThenByDescending(b => b.DateCreated)
                .ToListAsync();

            return View(bookings);
        }

        // GET: Booking/Dashboard
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Dashboard()
        {
            var dashboard = new AdminDashboardViewModel
            {
                TotalBookings = await db.Bookings.CountAsync(),

                PendingBookings = await db.Bookings
                    .CountAsync(b => b.Status == "Pending"),

                ApprovedBookings = await db.Bookings
                    .CountAsync(b => b.Status == "Approved"),

                RejectedBookings = await db.Bookings
                    .CountAsync(b => b.Status == "Rejected"),

                CancelledBookings = await db.Bookings
                    .CountAsync(b => b.Status == "Cancelled"),


                CompletedBookings = await db.Bookings
                    .CountAsync(b => b.Status == "Completed"),

                TotalVehicles = await db.Vehicles.CountAsync(),

                AvailableVehicles = await db.Vehicles
                    .CountAsync(v => v.Status == "Available"),

                UnavailableVehicles = await db.Vehicles
                    .CountAsync(v => v.Status != "Available"),

                RecentBookings = await db.Bookings
                    .Include(b => b.User)
                    .Include(b => b.Vehicle)
                    .OrderByDescending(b => b.DateCreated)
                    .Take(10)
                    .ToListAsync()
            };

            return View(dashboard);
        }

        // GET: Booking/Create
        public async Task<ActionResult> Create()
        {
            var userId = User.Identity.GetUserId();

            var user = await db.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return HttpNotFound();
            }

            if (!user.IsActive)
            {
                return new HttpStatusCodeResult(
                    403,
                    "Your account is inactive."
                );
            }

            if (!user.IsVehicleBookingAllowed)
            {
                return new HttpStatusCodeResult(
                    403,
                    "You are not permitted to book company vehicles."
                );
            }

            var vehicles = await db.Vehicles
                .Where(v => v.Status == "Available")
                .OrderBy(v => v.Make)
                .ThenBy(v => v.Model)
                .ToListAsync();

            ViewBag.VehicleId = new SelectList(
                vehicles,
                "VehicleId",
                "RegistrationNumber"
            );

            return View();
        }

        // POST: Booking/CheckAvailability
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CheckAvailability(
            DateTime startDateTime,
            DateTime endDateTime)
        {
            var userId = User.Identity.GetUserId();

            var user = await db.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return HttpNotFound();
            }

            // Check whether employee is active
            if (!user.IsActive)
            {
                return new HttpStatusCodeResult(
                    403,
                    "Your account is inactive."
                );
            }

            // Check whether employee is allowed to book vehicles
            if (!user.IsVehicleBookingAllowed)
            {
                return new HttpStatusCodeResult(
                    403,
                    "You are not permitted to book company vehicles."
                );
            }

            // Validate dates
            if (endDateTime <= startDateTime)
            {
                TempData["Error"] =
                    "The end date and time must be after the start date and time.";

                return RedirectToAction("Create");
            }

            // Booking cannot start in the past
            if (startDateTime < DateTime.Now)
            {
                TempData["Error"] =
                    "The booking cannot start in the past.";

                return RedirectToAction("Create");
            }

            // Check whether employee already has a booking
            bool employeeHasBooking = await db.Bookings.AnyAsync(b =>
                b.UserId == userId &&
                (b.Status == "Pending" || b.Status == "Approved") &&
                startDateTime < b.EndDateTime &&
                endDateTime > b.StartDateTime
            );

            if (employeeHasBooking)
            {
                TempData["Error"] =
                    "You already have a booking during this period.";

                return RedirectToAction("Create");
            }

            // Find vehicles that are currently active/available
            var vehicles = await db.Vehicles
                .Where(v => v.Status == "Available")
                .OrderBy(v => v.Make)
                .ThenBy(v => v.Model)
                .ToListAsync();

            // Find vehicles that already have a pending or approved booking
            var bookedVehicleIds = await db.Bookings
                .Where(b =>
                    (b.Status == "Pending" || b.Status == "Approved") &&
                    startDateTime < b.EndDateTime &&
                    endDateTime > b.StartDateTime
                )
                .Select(b => b.VehicleId)
                .Distinct()
                .ToListAsync();

            // Remove vehicles that are already booked/requested
            var availableVehicles = vehicles
                .Where(v => !bookedVehicleIds.Contains(v.VehicleId))
                .ToList();

            ViewBag.StartDateTime = startDateTime;
            ViewBag.EndDateTime = endDateTime;

            return View("AvailableVehicles", availableVehicles);
        }

        // POST: Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Booking model)
        {
            System.Diagnostics.Debug.WriteLine("BOOKING POST WAS CALLED");
            var userId = User.Identity.GetUserId();

            var user = await db.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return HttpNotFound();
            }

            // Check employee status
            if (!user.IsActive)
            {
                return new HttpStatusCodeResult(
                    403,
                    "Your account is inactive."
                );
            }

            // Check booking permission
            if (!user.IsVehicleBookingAllowed)
            {
                return new HttpStatusCodeResult(
                    403,
                    "You are not permitted to book company vehicles."
                );
            }

            // Validate dates
            if (model.EndDateTime <= model.StartDateTime)
            {
                ModelState.AddModelError(
                    "EndDateTime",
                    "The end date and time must be after the start date and time."
                );
            }

            // Make sure the booking isn't in the past
            if (model.StartDateTime < DateTime.Now)
            {
                ModelState.AddModelError(
                    "StartDateTime",
                    "The booking cannot start in the past."
                );
            }

            // Check if employee already has an overlapping booking
            bool employeeHasBooking = await db.Bookings.AnyAsync(b =>
                b.UserId == userId &&
                (b.Status == "Pending" || b.Status == "Approved") &&
                model.StartDateTime < b.EndDateTime &&
                model.EndDateTime > b.StartDateTime
            );

            if (employeeHasBooking)
            {
                ModelState.AddModelError(
                    "",
                    "You already have a booking during this period."
                );
            }

            // Check vehicle exists
            var vehicle = await db.Vehicles
                .FirstOrDefaultAsync(v => v.VehicleId == model.VehicleId);

            if (vehicle == null)
            {
                ModelState.AddModelError(
                    "VehicleId",
                    "The selected vehicle could not be found."
                );
            }
            else
            {
                // Check vehicle status
                if (vehicle.Status != "Available")
                {
                    ModelState.AddModelError(
                        "VehicleId",
                        "The selected vehicle is not currently available."
                    );
                }

                // Check vehicle booking conflict
                bool vehicleHasBooking = await db.Bookings.AnyAsync(b =>
                    b.VehicleId == model.VehicleId &&
                    (b.Status == "Pending" || b.Status == "Approved") &&
                    model.StartDateTime < b.EndDateTime &&
                    model.EndDateTime > b.StartDateTime
                );

                if (vehicleHasBooking)
                {
                    ModelState.AddModelError(
                        "VehicleId",
                        "This vehicle is already requested or booked during the selected period."
                    );
                }
            }

            if (!ModelState.IsValid)
            {
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
                    System.Diagnostics.Debug.WriteLine("BOOKING ERROR: " + error);
                }

                var vehicles = await db.Vehicles
                    .Where(v => v.Status == "Available")
                    .OrderBy(v => v.Make)
                    .ThenBy(v => v.Model)
                    .ToListAsync();

                ViewBag.VehicleId = new SelectList(
                    vehicles,
                    "VehicleId",
                    "RegistrationNumber",
                    model.VehicleId
                );

                return View(model);
            }

            model.UserId = userId;
            model.Status = "Pending";
            model.DateCreated = DateTime.Now;

            db.Bookings.Add(model);

            await db.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Your vehicle booking request has been submitted and is awaiting Admin approval.";

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

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Review(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(
                    System.Net.HttpStatusCode.BadRequest);
            }

            var booking = await db.Bookings
                .Include(b => b.User)
                .Include(b => b.Vehicle)
                .Include(b => b.Inspections)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
            {
                return HttpNotFound();
            }

            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Approve(int id)
        {
            var booking = await db.Bookings
                .Include(b => b.Vehicle)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
            {
                return HttpNotFound();
            }

            if (booking.Status != "Pending")
            {
                TempData["Error"] =
                    "This booking has already been processed.";

                return RedirectToAction("AdminIndex");
            }

            // Final availability check
            var conflictingBooking = await db.Bookings
                .AnyAsync(b =>
                    b.BookingId != booking.BookingId &&
                    b.VehicleId == booking.VehicleId &&
                    b.Status == "Approved" &&
                    booking.StartDateTime < b.EndDateTime &&
                    booking.EndDateTime > b.StartDateTime
                );

            if (conflictingBooking)
            {
                TempData["Error"] =
                    "This vehicle has already been approved for another booking during this period.";

                return RedirectToAction("AdminIndex");
            }

            // Approve the booking
            booking.Status = "Approved";

            // Record which administrator approved it
            booking.ApprovedByUserId = User.Identity.GetUserId();

            // Record when it was approved
            booking.ApprovedDate = DateTime.Now;

            await db.SaveChangesAsync();

            TempData["Success"] =
                "Booking approved successfully.";

            return RedirectToAction("AdminIndex");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Reject(int id, string rejectionReason)
        {
            var booking = await db.Bookings
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
            {
                return HttpNotFound();
            }

            if (booking.Status != "Pending")
            {
                TempData["Error"] =
                    "This booking has already been processed.";

                return RedirectToAction("AdminIndex");
            }

            if (string.IsNullOrWhiteSpace(rejectionReason))
            {
                TempData["Error"] =
                    "You must provide a reason for rejecting this booking.";

                return RedirectToAction(
                    "Review",
                    new { id = booking.BookingId });
            }

            booking.Status = "Rejected";

            booking.RejectionReason = rejectionReason.Trim();

            booking.RejectedDate = DateTime.Now;

            await db.SaveChangesAsync();

            TempData["Success"] =
                "Booking rejected successfully.";

            return RedirectToAction("AdminIndex");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Complete(int id)
        {
            var booking = await db.Bookings
                .Include(b => b.Vehicle)
                .Include(b => b.Inspections)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
            {
                return HttpNotFound();
            }

            if (booking.Status != "Approved")
            {
                TempData["Error"] =
                    "Only approved bookings can be completed.";

                return RedirectToAction(
                    "Review",
                    new { id = booking.BookingId }
                );
            }

            // Make sure an After inspection has been completed
            bool hasAfterInspection = booking.Inspections
                .Any(i => i.InspectionType == "After");

            if (!hasAfterInspection)
            {
                TempData["Error"] =
                    "An After Vehicle Inspection must be completed before the booking can be marked as completed.";

                return RedirectToAction(
                    "Review",
                    new { id = booking.BookingId }
                );
            }

            booking.Status = "Completed";
            booking.DecisionDate = DateTime.Now;

            await db.SaveChangesAsync();

            TempData["Success"] =
                "Booking completed successfully.";

            return RedirectToAction("AdminIndex");
        }

        // POST: Booking/Cancel
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Cancel(int id, string cancellationReason)
        {
            var userId = User.Identity.GetUserId();

            var booking = await db.Bookings
                .FirstOrDefaultAsync(b =>
                    b.BookingId == id &&
                    b.UserId == userId);

            if (booking == null)
            {
                return HttpNotFound();
            }

            // Employees cannot cancel completed bookings.
            if (booking.Status == "Completed")
            {
                TempData["Error"] =
                    "A completed booking cannot be cancelled.";

                return RedirectToAction("Index");
            }

            // A booking that has already been cancelled cannot be cancelled again.
            if (booking.Status == "Cancelled")
            {
                TempData["Error"] =
                    "This booking has already been cancelled.";

                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(cancellationReason))
            {
                TempData["Error"] =
                    "Please provide a cancellation reason.";

                return RedirectToAction("Index");
            }

            booking.Status = "Cancelled";
            booking.CancellationReason = cancellationReason;
            booking.CancelledDate = DateTime.Now;
            booking.CancelledByUserId = userId;

            await db.SaveChangesAsync();

            TempData["Success"] =
                "Booking cancelled successfully.";

            return RedirectToAction("Index");
        }

    }
}