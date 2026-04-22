using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prjHealthCareSystem.Models
{
    public class Client
    {
        [Key]

        public int Id {get;set;}
        public string Name {get; set;}
        public string ContactDetails {get; set;} 
        public string Region { get; set; } //Where they come from, dankie :)
      public int AccountId { get; set; }//link to accounts table
    public Accounts? Account { get; set; }
        public List<Contract> Contracts { get; set; } = new();
    }
}

