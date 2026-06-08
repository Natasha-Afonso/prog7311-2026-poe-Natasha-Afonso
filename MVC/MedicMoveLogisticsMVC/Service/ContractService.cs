using MedicMoveLogisticsMVC;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using MedicMoveLogisticsMVC.Models;

namespace MedicMoveLogisticsMVC.Services
{
    public class ContractService
    {
        private readonly HttpClient _httpClient;

        public ContractService(HttpClient httpClient)
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

        public async Task<List<Contract>> GetAllContractsAsync(string token)
        {
            try
            {
                AttachJwtHeader(token);
                var response = await _httpClient.GetAsync("http://api-service:8080/api/Contract");
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Sorry, the request failed with status code: {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    throw new Exception("Sorry, the response content is empty.");
                }

                return JsonConvert.DeserializeObject<List<Contract>>(content) ?? new List<Contract>();
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

        public async Task<Contract> GetContractByIdAsync(int id, string token)
        {
            try
            {
                 AttachJwtHeader(token);
                var response = await _httpClient.GetAsync($"http://api-service:8080/api/Contract/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Sorry, the request failed with status code: {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    throw new Exception("Sorry, the response content is empty.");
                }

                return JsonConvert.DeserializeObject<Contract>(content) ?? new Contract();
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

        public async Task CreateContractAsync(Contract contract, string token)
        {
            try
            {
                AttachJwtHeader(token);
                var response = await _httpClient.PostAsJsonAsync("http://api-service:8080/api/Contract", contract);
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

        public async Task UpdateContractAsync(int id, Contract contract, string token)
        {
            try
            {
                AttachJwtHeader(token);
                var response = await _httpClient.PutAsJsonAsync($"http://api-service:8080/api/Contract/{id}", contract);
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

        public async Task DeleteContractAsync(int id, string token)
        {
            try
            {
                AttachJwtHeader(token);
                var response = await _httpClient.DeleteAsync($"http://api-service:8080/api/Contract/{id}");
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