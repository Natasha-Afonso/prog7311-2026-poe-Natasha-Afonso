using System.Text;
using System.Text.Json;
using MedicMoveLogisticsMVC.Models;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace MedicMoveLogisticsMVC.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://api-service:8080/api/Accounts";

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> RegisterAsync(RegisterView register)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(register);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{BaseUrl}/register", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception("The registration request failed at the service level. Please try again later.", ex);
            }
        }

        public async Task<TokenResponse?> LoginAsync(LoginView login)
        {
            try
            {
                
            var jsonPayload = JsonSerializer.Serialize(login);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{BaseUrl}/login", content);
           if (!response.IsSuccessStatusCode)
            {
                return null; // Login failed
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var parsingOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<TokenResponse>(responseContent, parsingOptions);
        }
        catch (Exception ex)
            {
                throw new Exception("The login request failed at the service level. Please try again later.", ex);
            }
    }
    }
}