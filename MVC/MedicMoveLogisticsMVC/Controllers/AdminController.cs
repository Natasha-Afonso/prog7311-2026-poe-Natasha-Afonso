using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicMoveLogisticsMVC.Data;
using MedicMoveLogisticsMVC.Services;



namespace MedicMoveLogisticsMVC.Controllers
{
    public class AdminController : Controller
    {

        private readonly ServiceRequestsService _serviceRequestService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // used to access 
        public AdminController(ServiceRequestsService serviceRequestsService, IWebHostEnvironment webHostEnvironment)
        {
            _serviceRequestService = serviceRequestsService;

            _webHostEnvironment = webHostEnvironment;
            //tells the system to look at the wwwroot
        }



        public async Task<IActionResult> Index()
        {
            //--SESSION CHECK--
            // Retrieve the role from the session
            string role = HttpContext.Session.GetString("AccountRole");



            if (role != "Admin")
            {
                return RedirectToAction("Index", "Accounts");
            }
            var token = HttpContext.Session.GetString("JWTToken") ?? "";
            //show all requests to the admin
            var requests = await _serviceRequestService.GetAllServiceRequestsAsync(token);


            return View(requests);

        }

        [HttpPost]
        public async Task<IActionResult> UpdateApprove(int? id)
        {
            if (id == null) return NotFound();
            var token = HttpContext.Session.GetString("JWTToken") ?? "";
            try
            {
                var request = await _serviceRequestService.GetServiceRequestsByIdAsync(id.Value, token);
                if (request == null)
                {
                    return NotFound();
                }

                request.Status = "Approved";// Admin has approved the request

                await _serviceRequestService.UpdateServiceRequestsAsync(id.Value, request, token);

                return RedirectToAction("Index", "ServiceRequests"); // show the updated list
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to approve: {ex.Message}";
                return RedirectToAction("Index", "ServiceRequests");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDeny(int? id)
        {
                if (id == null) return NotFound();
            var token = HttpContext.Session.GetString("JWTToken") ?? "";
            try
            {
                var request = await _serviceRequestService.GetServiceRequestsByIdAsync(id.Value, token);
                if (request == null)
                {
                    return NotFound();
                }

                request.Status = "Denied";// Admin has not approved the request

                await _serviceRequestService.UpdateServiceRequestsAsync(id.Value, request, token);

                return RedirectToAction(nameof(Index)); // show the updated list
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to deny: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
