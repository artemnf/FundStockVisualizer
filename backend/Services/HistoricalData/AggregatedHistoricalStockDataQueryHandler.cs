using FundDataApi.Entities.Dtos;
using MediatR;

namespace FundDataApi.Services.HistoricalData;

public class AggregatedHistoricalStockDataQueryHandler : IRequestHandler<AggregatedHistoricalStockDataQuery, AggregatedHistoricalStockData>
{
    public Task<AggregatedHistoricalStockData> Handle(AggregatedHistoricalStockDataQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
