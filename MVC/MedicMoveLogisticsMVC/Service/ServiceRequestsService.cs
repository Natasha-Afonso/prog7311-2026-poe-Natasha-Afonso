using MedicMoveLogisticsMVC;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using MedicMoveLogisticsMVC.Models;

namespace MedicMoveLogisticsMVC.Services
{
    public class ServiceRequestsService
    {
        private readonly HttpClient _httpClient;

        public ServiceRequestsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        private void AttachJwtHeader(string token)
        {
            if(!string.IsNullOrEmpty(token))
            {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }

        public async Task<List<ServiceRequests>> GetAllServiceRequestsAsync(string token)
        {
            try
            {
                AttachJwtHeader(token);
                var response = await _httpClient.GetAsync("http://api-service:8080/api/ServiceRequests");
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Sorry, the request failed with status code: {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    throw new Exception("Sorry, the response content is empty.");
                }

                return JsonConvert.DeserializeObject<List<ServiceRequests>>(content) ?? new List<ServiceRequests>();
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
                throw new Exception("Sorry, an unexpected error occurred while fetching contracts.", ex);
            }
        }

        public async Task<ServiceRequests> GetServiceRequestsByIdAsync(int id,string token)
        {
            try
            {
                AttachJwtHeader(token);
                var response = await _httpClient.GetAsync($"http://api-service:8080/api/ServiceRequests/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Sorry, the request failed with status code: {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    throw new Exception("Sorry, the response content is empty.");
                }

                return JsonConvert.DeserializeObject<ServiceRequests>(content) ?? new ServiceRequests();
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
                throw new Exception("Sorry, an unexpected error occurred while fetching the contract.", ex);
            }
        }

        public async Task CreateServiceRequestsAsync(ServiceRequests serviceRequests, string token)
        {
            try
            {
                AttachJwtHeader(token);
                var response = await _httpClient.PostAsJsonAsync("http://api-service:8080/api/ServiceRequests", serviceRequests);
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
                throw new Exception("Sorry, an unexpected error occurred while creating the contract.", ex);
            }
        }

        public async Task UpdateServiceRequestsAsync(int id, ServiceRequests serviceRequests,string token)
        {
            try
            {
                AttachJwtHeader(token);
                var response = await _httpClient.PutAsJsonAsync($"http://api-service:8080/api/ServiceRequests/{id}", serviceRequests);
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
                throw new Exception("Sorry, an unexpected error occurred while updating the contract.", ex);
            }
        }

        public async Task DeleteServiceRequestsAsync(int id, string token)
        {
            try
            {
                AttachJwtHeader(token);
                var response = await _httpClient.DeleteAsync($"http://api-service:8080/api/ServiceRequests/{id}");
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
                throw new Exception("Sorry, an unexpected error occurred while deleting the contract.", ex);
            }
        }
    }
}