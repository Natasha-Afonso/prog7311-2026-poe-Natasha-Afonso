using System.ComponentModel.DataAnnotations;

namespace MedicMoveLogisticsMVC.Models
{
    public class ServiceRequests
    {
        [Key]
        public int Id { get; set; }

        public int ContractId {get;set;}
        public Contract? Contract { get; set; }
        public string Description { get; set; }
        public string CurrencyFrom { get; set; } 
        public decimal Amount { get; set; }
        public decimal Cost { get; set; }
        public string PackageSize {get; set;}
        public string Condition {get; set;}

        public string Speed {get; set;}
        public string AssignedVehicleType { get; set;}
        public string Status { get; set; } = "Pending Approval";
    }
}