using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjHealthCareSystem.Data;



namespace prjHealthCareSystem.Controllers
{
    public class Admin: Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // used to access 
        public Admin(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            //tells the system to look at the wwwroot
        }



        public async Task<IActionResult> Index(int? id)
        {
            //--SESSION CHECK--
            // Retrieve the role from the session
            string role = HttpContext.Session.GetString("AccountRole");



            if (role != "Admin")
            {
                return RedirectToAction("Index", "Accounts");
            }
           //show all requests to the admin
             var requests = await _context.ServiceRequests
               .Include(i => i.Contract)
                  .ThenInclude(i => i.Client)
               .ToListAsync();
          
            return View(requests);

        }

        [HttpPost]
        public async Task<IActionResult> UpdateApprove(int? id)
        {
            var request = await _context.ServiceRequests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            request.Status = "Approved";// Admin has approved the request

            await _context.SaveChangesAsync();

            return RedirectToAction("ServiceRequests"); // show the updated list
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDeny(int? id)
        {
            var request = await _context.ServiceRequests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            request.Status = "Denied";// Admin has denied the request

            await _context.SaveChangesAsync();

            return RedirectToAction("ServiceRequests"); // show the updated list
        }


    }
}
