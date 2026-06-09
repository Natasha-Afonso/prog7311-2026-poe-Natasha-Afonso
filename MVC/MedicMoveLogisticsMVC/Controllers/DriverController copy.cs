using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicMoveLogisticsMVC.Data;
using MedicMoveLogisticsMVC.Services;



namespace MedicMoveLogisticsMVC.Controllers
{
    public class DriverController : Controller
    {

        private readonly ServiceRequestsService _serviceRequestService;


        // used to access 
        public DriverController(ServiceRequestsService serviceRequestsService)
        {
            _serviceRequestService = serviceRequestsService;


        }


        //Get Driver
        public async Task<IActionResult> Index()
        {
            //--SESSION CHECK--
            // Retrieve the role from the session
            string role = HttpContext.Session.GetString("AccountRole");



            if (role != "Driver")
            {
                return RedirectToAction("Index", "Accounts");
            }
            var token = HttpContext.Session.GetString("JWTToken") ?? "";
            //show all requests to the admin
            var requests = await _serviceRequestService.GetAllServiceRequestsAsync(token);
            var driverRequests = requests.Where(r => r.Status != "Pending Approval").ToList();

            return View(driverRequests);

        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int? id, string nextStatus)
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

                request.Status = nextStatus;// Updates to picked up, on the way, delivered

                await _serviceRequestService.UpdateServiceRequestsAsync(id.Value, request, token);

                return RedirectToAction("Index", "ServiceRequests"); // show the updated list
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to update: {ex.Message}";
                return RedirectToAction("Index", "ServiceRequests");
            }
        }

    }
}