using MedicMoveLogisticsMVC;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using MedicMoveLogisticsMVC.Models;

namespace MedicMoveLogisticsMVC.Services
{
    public class ClientService
    {
        private readonly HttpClient _httpClient;

        public ClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        private void AttachJwtHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
        

        public async Task<List<Client>> GetAllClientsAsync(string token)
        {
            AttachJwtHeader(token);
            List<Client> clients = new List<Client>();
            try
            {
                var response = await _httpClient.GetAsync("http://api-service:8080/api/Client");
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Sorry, the request failed with status code: {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    throw new Exception("Sorry, the response content is empty.");
                }

                return JsonConvert.DeserializeObject<List<Client>>(content) ?? new List<Client>();
            }
            catch (HttpRequestException ex)
            {
                // Log the exception or handle it as needed
                throw new Exception("Sorry, there was an error while fetching data from the API.", ex);
            }
            catch (System.Text.Json.JsonException ex)
            {
                // Log the exception or handle it as needed
                throw new Exception("Sorry, there was an error deserializing the response from the API.", ex);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new Exception("Sorry, an unexpected error occurred while fetching clients.", ex);
            }
        }

        public async Task<Client> GetClientByIdAsync(int id, string token)
        {
            try
            {
                AttachJwtHeader(token);
                Client client = null;
                var response = await _httpClient.GetAsync($"http://api-service:8080/api/Client/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Sorry, the request failed with status code: {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    throw new Exception("Sorry, the response content is empty.");
                }

                return JsonConvert.DeserializeObject<Client>(content) ?? new Client();
            }
            catch (HttpRequestException ex)
            {
                // Log the exception or handle it as needed
                throw new Exception("Sorry, there was an error while fetching data from the API.", ex);
            }
            catch (System.Text.Json.JsonException ex)
            {
                // Log the exception or handle it as needed
                throw new Exception("Sorry, there was an error deserializing the response from the API.", ex);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new Exception("Sorry, an unexpected error occurred while fetching the client.", ex);
            }
        }

        public async Task CreateClientAsync(Client client, string token)
        {
            try
            {
                AttachJwtHeader(token);
                var response = await _httpClient.PostAsJsonAsync("http://api-service:8080/api/Client", client);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Sorry, the request failed with status code: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                // Log the exception or handle it as needed
                throw new Exception("Sorry, there was an error while sending data to the API.", ex);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new Exception("Sorry, an unexpected error occurred while creating the client.", ex);
            }
        }

        public async Task UpdateClientAsync(int id, Client client, string token)
        {
            try
            {
                AttachJwtHeader(token);
                var response = await _httpClient.PutAsJsonAsync($"http://api-service:8080/api/Client/{id}", client);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Sorry, the request failed with status code: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                // Log the exception or handle it as needed
                throw new Exception("Sorry, there was an error while sending data to the API.", ex);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new Exception("Sorry, an unexpected error occurred while updating the client.", ex);
            }
        }

        public async Task DeleteClientAsync(int id, string token)
        {
            try
            {
                AttachJwtHeader(token);
                var response = await _httpClient.DeleteAsync($"http://api-service:8080/api/Client/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Sorry, the request failed with status code: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                // Log the exception or handle it as needed
                throw new Exception("Sorry, there was an error while sending data to the API.", ex);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new Exception("Sorry, an unexpected error occurred while deleting the client.", ex);
            }
        }
    }
}