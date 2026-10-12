using CompanyVehicleBookingSystem.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CompanyVehicleBookingSystem.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Profile
        public async Task<ActionResult> Index()
        {
            var userId = User.Identity.GetUserId();

            var user = await db.Users
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
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