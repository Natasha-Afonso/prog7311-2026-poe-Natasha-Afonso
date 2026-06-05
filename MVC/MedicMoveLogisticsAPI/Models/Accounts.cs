using System.ComponentModel.DataAnnotations;
using MedicMoveLogisticsAPI.Models;

namespace MedicMoveLogisticsAPI.Models
{
    //Singh
    public class Accounts
    {
        [Key]
    
        [Display(Name = "ID")]
        public int ID { get; set; }

        
        [Required(ErrorMessage = "Please enter Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Password \"{0}\" must have {2} characters", MinimumLength = 8)]
        [RegularExpression(@"^([a-zA-Z0-9@*#]{8,15})$", ErrorMessage = "Password must contain: Minimum 8 characters, at least 1 UpperCase Alphabet, 1 LowerCase Alphabet, 1 Number, and 1 Special Character")]
        public string Password { get; set; }

    
        public string Role { get; set; } 

       public Client? Client { get; set; }
     
    }
}
/*REFERENCES
 *Singh, P. (2021). Complete Login And Registration System In ASP.NET MVC Application With Database Connection. [online] Available at:< https://www.c-sharpcorner.com/article/simple-login-and-registration-form-in-asp-net-mvc-using-ado-net/ >[Accessed 1 November 2025]
 * 
 */