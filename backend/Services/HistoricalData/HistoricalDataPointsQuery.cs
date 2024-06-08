using Entities.Dtos;
using MediatR;

namespace FundDataApi.Services.HistoricalData;

public class HistoricalDataPointsQuery : IRequest<IEnumerable<HistoricalDataPoint>>
{
    public string Symbol {get; set;}
}
