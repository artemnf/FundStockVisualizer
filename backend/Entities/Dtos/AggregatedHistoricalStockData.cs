namespace FundDataApi.Entities.Dtos;

public record AggregatedHistoricalStockData (decimal MinPrice, decimal MaxPrice, decimal AvgPrice, decimal AvgVolume, string Symbol);
