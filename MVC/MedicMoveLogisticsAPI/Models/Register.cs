namespace MedicMoveLogisticsAPI
{
public class Register
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string Role { get; set; } = ""; //Admin, Client,driver
        public int? ClientId { get; set; } // null so it is Optional,it is only for clients
    }
}