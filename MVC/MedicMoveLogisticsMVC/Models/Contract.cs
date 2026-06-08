using System.ComponentModel.DataAnnotations;

namespace MedicMoveLogisticsMVC.Models{

public class Contract
{
    public int Id { get; set; }
    
        [Display(Name ="Contract Name")]
        public string ContractName {get; set; }
    public int ClientId { get; set; }
    public Client? Client { get; set; }

    [DataType(DataType.Date)] public DateTime StartDate { get; set; }

    [DataType(DataType.Date)] //This forces the data type to be a date 
    public DateTime EndDate { get; set; }

    public string Status { get; set; } = "Draft"; //Default it will be drafted
    public string ServiceLevel { get; set; } = "Basic"; //Default it will be basic
    public string? SignedConsentPath { get; set; }


}
}
    
