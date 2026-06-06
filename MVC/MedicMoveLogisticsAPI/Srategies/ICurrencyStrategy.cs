
namespace MedicMoveLogisticsAPI.Strategies
{
    public interface ICurrencyStrategy
{
    Task<decimal>GetRateAsync();
}
}