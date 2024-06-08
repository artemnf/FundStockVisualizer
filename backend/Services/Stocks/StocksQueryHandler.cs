using FundDataApi.Entities.Dtos;
using FundDataApi.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FundDataApi.Services.Stocks;

public class StocksQueryHandler(FundDataDbContext dbContext) : IRequestHandler<StocksQuery, IEnumerable<Stock>>
{
    public async Task<IEnumerable<Stock>> Handle(StocksQuery query, CancellationToken cancellationToken)
    {
        return await dbContext.Set<Entities.Domain.Stock>()
                              .Where(x => x.Fund.Id == query.FundId && (query.SearchTerm == null || x.Ticker.StartsWith(query.SearchTerm)))
                              .Select(x => new Stock(x.Id, x.Ticker))
                              .ToListAsync(cancellationToken);
    }
}

