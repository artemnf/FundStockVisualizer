using FundDataApi.Data;
using FundDataApi.Entities.Domain;
using MediatR;

namespace FundDataApi.Services.DataManagement;

public class LatestLoadedDateQueryHandler(FundDataDbContext dbContext) : IRequestHandler<LatestLoadedDateQuery, DateOnly>
{
    public async Task<DateOnly> Handle(LatestLoadedDateQuery request, CancellationToken cancellationToken)
    {
        return dbContext.Set<HistoricalDataPoint>()
                        .Max(x => x.Date);
    }
}
