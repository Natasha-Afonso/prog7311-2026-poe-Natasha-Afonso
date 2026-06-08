using System.Text.Json;
using MedicMoveLogisticsMVC.Controllers;

namespace MedicMoveLogisticsMVC.Controllers
{
public interface ICurrencyStrategy
{
    Task<decimal>GetRateAsync();
}
}