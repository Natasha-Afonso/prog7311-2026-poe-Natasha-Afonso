using System.Text.Json;
using prjHealthCareSystem.Controllers;

namespace prjHealthCareSystem.Controllers
{
public interface ICurrencyStrategy
{
    Task<decimal>GetRateAsync();
}
}