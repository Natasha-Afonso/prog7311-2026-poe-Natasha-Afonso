using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace MedicMoveLogisticsMVC
{
    public class TokenResponse
    {
        public string Token { get; set; } = "";
        public DateTime Expiration { get; set; }
        public string Role { get; set; } = "";
        public string Email { get; set; } = "";

        public int? ClientId { get; set;}
    }
}
