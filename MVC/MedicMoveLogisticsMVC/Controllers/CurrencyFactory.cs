namespace prjHealthCareSystem.Controllers
{
   // Refactoring Guru.2014.
    //The CurrencyFactory class is responsible for creating instances of ICurrencyStrategy based on the provided currency code. It uses a switch expression to determine which strategy to instantiate, making it easy to add new currencies in the future by simply adding new cases to the switch statement. This design adheres to the Open/Closed Principle, allowing the system to be open for extension but closed for modification.
    public class CurrencyFactory
    {
        public static ICurrencyStrategy Create(string currency, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {

            /*Newham, J.2017.
             switch statement to determine which strategy to use based on the currency code. Switch
             statements reduce the number of if statements and makes the code more maintainable.*/ 
            return currency switch
            {
                "CNY" => new CNYStrategy(httpClientFactory, configuration),
                "JPY" => new JPYStrategy(httpClientFactory, configuration),
                _ => throw new ArgumentException("Currency not supported")
            };
        }
    }
}

/*References
Refactoring Guru.2014. Factory Method. [online] Available at: <https://refactoring.guru/design-patterns/factory-method.> [Accessed 20 April 2026].
Newham, J.2017. C# Currency Converter: Reduce the amount of if statements. [online]  Available at: <https://stackoverflow.com/questions/44817424/c-sharp-currency-converter-reduce-the-amount-of-if-statements.> [Accessed 20 April 2026].
*/
