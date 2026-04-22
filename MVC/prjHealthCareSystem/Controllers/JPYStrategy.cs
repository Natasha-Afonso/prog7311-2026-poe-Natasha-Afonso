using System.Text.Json;
using prjHealthCareSystem.Controllers;

namespace prjHealthCareSystem.Controllers
{
    public class JPYStrategy : ICurrencyStrategy
    {

        //Use HttpClient to call the OpenExchangeRates API.
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public JPYStrategy(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

//Wrap the API call in a try-catch. If the API fails (e.g., no internet or invalid key), the method must return a fallback rate of 16.50.
public async Task<decimal> GetRateAsync()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var apiBaseUrl = _configuration["FunctionApi:BaseUrl"]+"JPY";

                var response = await httpClient.GetAsync(apiBaseUrl);
                if (response.IsSuccessStatusCode)
                {
                    using var stream= await response.Content.ReadAsStreamAsync();
                    var exchangeRates = await JsonSerializer.DeserializeAsync<ExchangeRates>(stream);

                    return exchangeRates.Rates["ZAR"];
                }
            }
            catch
            {
                return 16.50m; // Fallback rate
            }
            return 16.50m; // Fallback rate if API call fails
        }
    }  
}