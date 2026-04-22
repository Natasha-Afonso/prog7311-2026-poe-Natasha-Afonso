namespace prjHealthCareSystem.Controllers
{
    public class CurrencyFactory
    {
        public static ICurrencyStrategy Create(string currency, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            return currency switch
            {
                "USD" => new USDStrategy(httpClientFactory, configuration),
                "CNY" => new CNYStrategy(httpClientFactory, configuration),
                "JPY" => new JPYStrategy(httpClientFactory, configuration),
                _ => throw new ArgumentException("Currency not supported")
            };
        }
    }
}