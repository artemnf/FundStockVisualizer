using FundDataApi.Entities.Domain;
using OoplesFinance.YahooFinanceAPI;
using OoplesFinance.YahooFinanceAPI.Enums;

namespace FundDataApi.Services.ExternalFinancialApi;

public class YahooFinanceApi : IExternalFinancialApi
{
    private YahooClient _client;

    public YahooFinanceApi()
    {
        _client = new YahooClient();
    }

    public async Task<IEnumerable<HistoricalDataPoint>> GetHistoricalData(string ticker, DateOnly startDate)
    {
        var data = await _client.GetHistoricalDataAsync(ticker, DataFrequency.Daily, startDate.ToDateTime(TimeOnly.MinValue));

        var dataPoints = data.Select(x => new HistoricalDataPoint
        {
            Date = DateOnly.FromDateTime(x.Date),
            Open = Convert.ToDecimal(x.Open),
            High = Convert.ToDecimal(x.High),
            Low = Convert.ToDecimal(x.Low),
            Close = Convert.ToDecimal(x.Close),
            AdjClose = Convert.ToDecimal(x.AdjClose),
            Volume = Convert.ToDecimal(x.Volume)
        });

        return dataPoints;
    }
}
