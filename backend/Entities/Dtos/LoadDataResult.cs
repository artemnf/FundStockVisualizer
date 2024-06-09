namespace FundDataApi.Entities.Dtos;

public record LoadDataResult (DateOnly LatestLoadedDate, IEnumerable<string> FailedSymbols);
