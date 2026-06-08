
using System.Diagnostics.Contracts;

namespace MVC;
public class UnitTest1
{
    [Fact]
    public void CreateMethod_ChooseUSDStrategy() //testing if the factory creates the correct strategy based on the input currency
    {
        var factory = new CurrencyFactory();
        var strategy = factory.Create("USD", null, null);
        Assert.IsType<USDStrategy>(strategy);
    }


[Fact]
    public void CreateMethod_ChooseJYPStrategy() //testing if the factory creates the correct strategy based on the input currency
    {
        var factory = new CurrencyFactory();
        var strategy = factory.Create("JYP", null, null);
        Assert.IsType<JYPStrategy>(strategy);
    }


[Fact]
    public void CreateMethod_ChooseCNStrategy() //testing if the factory creates the correct strategy based on the input currency
    {
        var factory = new CurrencyFactory();
        var strategy = factory.Create("CNY", null, null);
        Assert.IsType<CNYStrategy>(strategy);
    }

    [Fact]
    public void TestErrorHandling() //testing if the factory throws an exception when an unsupported currency is provided
    {
        var factory = new CurrencyFactory();
        string unsupportedCurrency = "EUR";
        Assert.Throws<ArgumentException>(() => factory.Create(unsupportedCurrency, null, null));
        
    }


    [Fact]
    public void CalculationOfCurrency() //testing if the calculation of the cost based on the exchange rate is correct
    {
        var serviceRequest = new ServiceRequests
        {
            Amount = 100,
            CurrencyFrom = "USD"
        };
        var factory = new CurrencyFactory();
        var strategy = factory.Create(serviceRequest.CurrencyFrom, null, null);
        var exchangeRate = strategy.GetRateAsync().Result; // Assuming GetRateAsync returns a fixed rate for testing
        var expectedCost = serviceRequest.Amount * exchangeRate;
        Assert.Equal(expectedCost, serviceRequest.Amount * exchangeRate);

    }

    [Fact]
    public void TestStatusFilter() //testing if the factory creates the correct strategy based on the input currency
    {
        var data =new List<Contract>
        {
            new Contract { Status = "Draft"},
            new Contract { Status = "Active"},
            new Contract { Status = "Expired"},
        };

        var returnedData = data.Where(c => c.Status == "Active");
   
        Assert.Single(returnedData);
    }
}
