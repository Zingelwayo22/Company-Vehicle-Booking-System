using CompanyVehicleBookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CompanyVehicleBookingSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class VehicleController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Vehicle
        public ActionResult Index()
        {
            return View(db.Vehicles.ToList());
        }

        // GET: Vehicle/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(
                    HttpStatusCode.BadRequest
                );
            }

            Vehicle vehicle = db.Vehicles.Find(id);

            if (vehicle == null)
            {
                return HttpNotFound();
            }

            return View(vehicle);
        }

        // GET: Vehicle/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Vehicle/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "VehicleId,RegistrationNumber,Make,Model,Year,Colour,VIN,InsuranceExpiry,LicenceDiskExpiry,LastServiceDate,NextServiceDate,Status,Notes,DateAdded")]
            Vehicle vehicle,
            HttpPostedFileBase vehicleImage)
        {
            if (ModelState.IsValid)
            {
                // Check whether an image was selected
                if (vehicleImage != null &&
                    vehicleImage.ContentLength > 0)
                {
                    var allowedExtensions = new[]
                    {
                        ".jpg",
                        ".jpeg",
                        ".png",
                        ".webp"
                    };

                    var extension = Path.GetExtension(
                        vehicleImage.FileName
                    ).ToLower();

                    // Validate image type
                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError(
                            "ImagePath",
                            "Please upload a JPG, JPEG, PNG or WebP image."
                        );

                        return View(vehicle);
                    }

                    // Create vehicle image folder
                    var folderPath = Server.MapPath(
                        "~/Content/Images/Vehicles"
                    );

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    // Generate unique filename
                    var fileName =
                        Guid.NewGuid().ToString() + extension;

                    var filePath = Path.Combine(
                        folderPath,
                        fileName
                    );

                    // Save image
                    vehicleImage.SaveAs(filePath);

                    // Save relative path in database
                    vehicle.ImagePath =
                        "~/Content/Images/Vehicles/" + fileName;
                }

                db.Vehicles.Add(vehicle);

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(vehicle);
        }

        // GET: Vehicle/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(
                    HttpStatusCode.BadRequest
                );
            }

            Vehicle vehicle = db.Vehicles.Find(id);

            if (vehicle == null)
            {
                return HttpNotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicle/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "VehicleId,RegistrationNumber,Make,Model,Year,Colour,VIN,InsuranceExpiry,LicenceDiskExpiry,LastServiceDate,NextServiceDate,Status,Notes,DateAdded")]
            Vehicle vehicle,
            HttpPostedFileBase vehicleImage)
        {
            if (ModelState.IsValid)
            {
                var existingVehicle =
                    db.Vehicles.Find(vehicle.VehicleId);

                if (existingVehicle == null)
                {
                    return HttpNotFound();
                }

                // Update vehicle information
                existingVehicle.RegistrationNumber =
                    vehicle.RegistrationNumber;

                existingVehicle.Make =
                    vehicle.Make;

                existingVehicle.Model =
                    vehicle.Model;

                existingVehicle.Year =
                    vehicle.Year;

                existingVehicle.Colour =
                    vehicle.Colour;

                existingVehicle.VIN =
                    vehicle.VIN;

                existingVehicle.InsuranceExpiry =
                    vehicle.InsuranceExpiry;

                existingVehicle.LicenceDiskExpiry =
                    vehicle.LicenceDiskExpiry;

                existingVehicle.LastServiceDate =
                    vehicle.LastServiceDate;

                existingVehicle.NextServiceDate =
                    vehicle.NextServiceDate;

                existingVehicle.Status =
                    vehicle.Status;

                existingVehicle.Notes =
                    vehicle.Notes;

                // Check if Admin selected a new image
                if (vehicleImage != null &&
                    vehicleImage.ContentLength > 0)
                {
                    var allowedExtensions = new[]
                    {
                        ".jpg",
                        ".jpeg",
                        ".png",
                        ".webp"
                    };

                    var extension = Path.GetExtension(
                        vehicleImage.FileName
                    ).ToLower();

                    // Validate image type
                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError(
                            "ImagePath",
                            "Please upload a JPG, JPEG, PNG or WebP image."
                        );

                        return View(vehicle);
                    }

                    // Create vehicle image folder
                    var folderPath = Server.MapPath(
                        "~/Content/Images/Vehicles"
                    );

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    // Delete old image
                    if (!string.IsNullOrEmpty(
                        existingVehicle.ImagePath))
                    {
                        var oldImagePath =
                            Server.MapPath(
                                existingVehicle.ImagePath
                            );

                        if (System.IO.File.Exists(
                            oldImagePath))
                        {
                            System.IO.File.Delete(
                                oldImagePath
                            );
                        }
                    }

                    // Generate new unique filename
                    var fileName =
                        Guid.NewGuid().ToString() + extension;

                    var filePath = Path.Combine(
                        folderPath,
                        fileName
                    );

                    // Save new image
                    vehicleImage.SaveAs(filePath);

                    // Save new image path
                    existingVehicle.ImagePath =
                        "~/Content/Images/Vehicles/" +
                        fileName;
                }

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(vehicle);
        }

        // GET: Vehicle/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(
                    HttpStatusCode.BadRequest
                );
            }

            Vehicle vehicle = db.Vehicles.Find(id);

            if (vehicle == null)
            {
                return HttpNotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vehicle vehicle = db.Vehicles.Find(id);

            if (vehicle == null)
            {
                return HttpNotFound();
            }

            // Delete vehicle image if one exists
            if (!string.IsNullOrEmpty(vehicle.ImagePath))
            {
                var imagePath =
                    Server.MapPath(vehicle.ImagePath);

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            db.Vehicles.Remove(vehicle);

            db.SaveChanges();

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

        // GET: Vehicle/Availability
        [AllowAnonymous]
        public async Task<ActionResult> Availability()
        {
            var vehicles = await db.Vehicles
                .OrderBy(v => v.Make)
                .ThenBy(v => v.Model)
                .ToListAsync();

            return View(vehicles);
        }
    }
}