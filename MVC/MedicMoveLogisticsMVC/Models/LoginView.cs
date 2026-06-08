using System.ComponentModel.DataAnnotations;
using MedicMoveLogisticsMVC.Models;

namespace MedicMoveLogisticsMVC.Models
{
    //Singh
    public class LoginView
    {
        [Required(ErrorMessage = "Please enter Email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
               public string Password { get; set; } 


    }
}
/*REFERENCES
 *Singh, P. (2021). Complete Login And Registration System In ASP.NET MVC Application With Database Connection. [online] Available at:< https://www.c-sharpcorner.com/article/simple-login-and-registration-form-in-asp-net-mvc-using-ado-net/ >[Accessed 1 November 2025]
 * 
 */