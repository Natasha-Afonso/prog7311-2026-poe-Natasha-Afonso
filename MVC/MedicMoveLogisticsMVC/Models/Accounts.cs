using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace MedicMoveLogisticsMVC.Models
{
    public class Accounts
    {
        public int Id { get; set; }
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string Role {get; set;}
        public int? ClientId { get; set; }
        [NotMapped]
        public string? ClientName { get; set; }
    }
}