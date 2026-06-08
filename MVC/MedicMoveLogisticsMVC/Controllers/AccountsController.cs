using Microsoft.AspNetCore.Mvc;
using prjHealthCareSystem;
using prjHealthCareSystem.Data;

namespace prjHealthCareSystem.Controllers
{
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountsController(ApplicationDbContext context)
        {
            _context = context;
        }


public IActionResult Index()
        {
            return View("Index");
        }



        //(Troelsen, A. and Japikse, P. 2021)

        [HttpPost]
        public IActionResult Login(string email, string password)
        {

            var valid = _context.Accounts.FirstOrDefault(a => a.Email == email);

            //check for email
            if (valid == null)
            {
                ViewBag.Error = "Invalid username or password";
                return View("Index");
            }
            if (valid.Password != password)
            {
                ViewBag.Error = "Invalid username or password";
                return View("Index");
            }


            //---SESSION CREATION
            //If credentionals are valid, create a session variable
            //to store the username
            //This marks the user as "loggen in"
            HttpContext.Session.SetString("AccountEmail", valid.Email);
            HttpContext.Session.SetString("AccountRole", valid.Role);
         
            HttpContext.Session.SetInt32("AccountID", valid.ID);

            if (valid.Role == "Admin")
            {
              
                //Redirect HR
                return RedirectToAction("Index", "Admin");
            }
           
            else
            {
                //Redirect Lecturers
                return RedirectToAction("Index", "Client");
            }
        
        }






        //GET: /Account/Logout
        //This action logs the user out

        public IActionResult Logout()
        {
            //-- SESSSION DESTRUCTION ---
            // Clear the session to the user out
            HttpContext.Session.Clear();


            TempData["SuccessMessage"] = $"You have been logged out!";
            //Redirect the user back to the login  page
            return RedirectToAction("Index");
        }





    }
}
/*References
 * Troelsen, A. and Japikse, P. 2021.Pro C# 9 with .NET 5:Foundational Principles and Practices in Programming. 10th ed.Apress.*/