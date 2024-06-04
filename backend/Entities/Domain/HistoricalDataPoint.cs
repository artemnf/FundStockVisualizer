namespace FundDataApi.Entities.Domain;

public class HistoricalDataPoint
{
    public DateOnly Date { get; set; }
    
    public Stock Stock { get; set; }

    public decimal Open { get; set; }

    public decimal High { get; set; }

    public decimal Low { get; set; }

    public decimal Close { get; set; }

    public decimal AdjClose { get; set; }

    public decimal Volume { get; set; }
}