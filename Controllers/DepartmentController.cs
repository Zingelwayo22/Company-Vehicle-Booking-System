using CompanyVehicleBookingSystem.Models;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CompanyVehicleBookingSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DepartmentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            var departments = await db.Departments
                .OrderBy(d => d.Name)
                .ToListAsync();

            return View(departments);
        }

        // GET: Department/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Department/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Department model)
        {
            if (ModelState.IsValid)
            {
                db.Departments.Add(model);
                await db.SaveChangesAsync();

                TempData["Success"] = "Department added successfully.";

                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Department/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(
                    System.Net.HttpStatusCode.BadRequest);
            }

            var department = await db.Departments
                .FirstOrDefaultAsync(d => d.DepartmentId == id);

            if (department == null)
            {
                return HttpNotFound();
            }

            return View(department);
        }


        // GET: Department/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(
                    System.Net.HttpStatusCode.BadRequest);
            }

            var department = await db.Departments
                .FirstOrDefaultAsync(d => d.DepartmentId == id);

            if (department == null)
            {
                return HttpNotFound();
            }

            return View(department);
        }

        // POST: Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var department = await db.Departments
                .FirstOrDefaultAsync(d => d.DepartmentId == id);

            if (department == null)
            {
                return HttpNotFound();
            }

            db.Departments.Remove(department);

            await db.SaveChangesAsync();

            TempData["Success"] = "Department deleted successfully.";

            return RedirectToAction("Index");
        }



        // POST: Department/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Department model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;

                await db.SaveChangesAsync();

                TempData["Success"] = "Department updated successfully.";

                return RedirectToAction("Index");
            }

            return View(model);
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