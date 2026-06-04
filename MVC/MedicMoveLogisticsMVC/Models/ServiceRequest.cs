using System.ComponentModel.DataAnnotations;

namespace prjHealthCareSystem.Models
{
    public class ServiceRequest
    {
        [Key]
        public int Id { get; set; }


        public int ContractId {get;set;}
        public Contract? Contract { get; set; }
        public string Description { get; set; }
        public string CurrencyFrom { get; set; } 
        public decimal Amount { get; set; }
        public decimal Cost { get; set; }
        public string Status { get; set; }
    }
}