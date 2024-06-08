using FundDataApi.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HistoricalDataPoint = FundDataApi.Entities.Dtos.HistoricalDataPoint;

namespace FundDataApi.Services.HistoricalData;

public class HistoricalDataPointsQueryHandler(FundDataDbContext dbContext) : IRequestHandler<HistoricalDataPointsQuery, IEnumerable<HistoricalDataPoint>>
{
    public async Task<IEnumerable<HistoricalDataPoint>> Handle(HistoricalDataPointsQuery query, CancellationToken cancellationToken)
    {
        var startDate = DateOnly.FromDateTime(DateTime.Today).AddYears(-5);
        return (await dbContext.Set<Entities.Domain.HistoricalDataPoint>()
                              .AsNoTracking()
                              .Where(x => x.Stock.Id == query.StockId && x.Date >= startDate)
                              .OrderBy(x => x.Date)
                              .Select(x => new {
                                x.Date,
                                Price = x.AdjClose
                              })
                              .ToListAsync())
                              .Select(x => new HistoricalDataPoint(
                                MillisecondsUnixEpoch: Convert.ToInt64((x.Date.ToDateTime(TimeOnly.MinValue) - DateTime.UnixEpoch).TotalMilliseconds),
                                Price: x.Price
                              ));
    }
}
