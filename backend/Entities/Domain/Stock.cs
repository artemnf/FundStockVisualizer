namespace FundDataApi.Entities.Domain;

public class Stock
{
    public int Id { get; private set; }

    public string Ticker { get; set; }

    public Fund Fund { get; set; }

    public ICollection<HistoricalDataPoint> HistoricalDataPoints { get; private set; } = [];
}