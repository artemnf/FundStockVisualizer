namespace FundDataApi.Entities.Domain;

public class Fund
{
    public int Id { get; private set; }
    public string Symbol { get; set; }
    public string Name { get; set; }

    public ICollection<Stock> Stocks { get; private set; } = [];
}