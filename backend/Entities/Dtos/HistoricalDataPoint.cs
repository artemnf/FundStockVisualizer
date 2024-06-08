namespace FundDataApi.Entities.Dtos;

public record HistoricalDataPoint(long MillisecondsUnixEpoch, decimal Price);
