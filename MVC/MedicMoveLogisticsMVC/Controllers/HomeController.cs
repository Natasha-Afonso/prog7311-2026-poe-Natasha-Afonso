using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MedicMoveLogisticsMVC.Models;

namespace MedicMoveLogisticsMVC.Controllers;
public class HomeController : Controller
{

    public IActionResult Index()
    {
         //--SESSION CHECK--
 // Retrieve the Username from the session
 string username = HttpContext.Session.GetString("AccountEmail");


 //If the username is null or empty, it means the user is not logged
 if (string.IsNullOrEmpty(username))
 {
     return RedirectToAction("Index", "Accounts");
 }
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
