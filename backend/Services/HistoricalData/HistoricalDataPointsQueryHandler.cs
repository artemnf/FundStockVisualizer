using FundDataApi.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HistoricalDataPoint = Entities.Dtos.HistoricalDataPoint;

namespace FundDataApi.Services.HistoricalData;

public class HistoricalDataPointsQueryHandler(FundDataDbContext dbContext) : IRequestHandler<HistoricalDataPointsQuery, IEnumerable<HistoricalDataPoint>>
{
    public async Task<IEnumerable<HistoricalDataPoint>> Handle(HistoricalDataPointsQuery query, CancellationToken cancellationToken)
    {
        var startDate = DateOnly.FromDateTime(DateTime.Today).AddYears(-5);
        var ticker = query.Symbol.ToUpperInvariant();
        return (await dbContext.Set<Entities.Domain.HistoricalDataPoint>()
                              .AsNoTracking()
                              .Where(x => x.Stock.Ticker == ticker && x.Date >= startDate)
                              .OrderBy(x => x.Date)
                              .Select(x => new {
                                x.Date,
                                Price = x.Close
                              })
                              .ToListAsync())
                              .Select(x => new HistoricalDataPoint{
                                Ticks = Convert.ToInt64((x.Date.ToDateTime(TimeOnly.MinValue) - DateTime.UnixEpoch).TotalMilliseconds),
                                Price = x.Price
                              });
    }
}
