using FundDataApi.Entities.Domain;

namespace FundDataApi.Services.ExternalFinancialApi;

public interface IExternalFinancialApi
{
    Task<IEnumerable<HistoricalDataPoint>> GetHistoricalData(string ticker, DateOnly startDate);
}
