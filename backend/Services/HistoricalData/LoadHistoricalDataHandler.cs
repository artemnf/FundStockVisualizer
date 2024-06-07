using System.Collections.Immutable;
using FundDataApi.Data;
using FundDataApi.Entities.Domain;
using FundDataApi.Services.ExternalFinancialApi;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FundDataApi.Services.HistoricalData;

public class LoadHistoricalDataHandler(FundDataDbContext dbContext,
                                       IExternalFinancialApi externalFinancialApi,
                                       IMediator mediator)
    : IRequestHandler<LoadHistoricalDataCommand>
{
    private const string FundSymbol = "SPY";

    public async Task Handle(LoadHistoricalDataCommand request, CancellationToken cancellationToken)
    {
        var sp500Symbols = await mediator.Send(new GetSp500ConstituentsQuery(), cancellationToken);

        var fund = await dbContext.Funds
                                  .Include(x => x.Stocks)
                                  .SingleOrDefaultAsync(x => x.Symbol == FundSymbol);

        if (fund == null)
        {
            fund = new Fund
            {
                Symbol = FundSymbol,
                Name = "SPDR S&P 500 ETF Trust"
            };
            dbContext.Funds.Add(fund);
        }

        var stocksToCreate = sp500Symbols.Except(fund.Stocks.Select(x => x.Ticker.ToUpper()))
                                         .Select(x => new Stock
                                         {
                                            Ticker = x,
                                            Fund = fund
                                         })
                                         .ToImmutableList();

        stocksToCreate.ForEach(x => fund.Stocks.Add(x));

        var stocksToDelete = fund.Stocks.ExceptBy(sp500Symbols, x => x.Ticker.ToUpper())
                                        .ToImmutableList();

        stocksToDelete.ForEach(x => fund.Stocks.Remove(x));

        await dbContext.Set<HistoricalDataPoint>().ExecuteDeleteAsync(); // TODO: might be a better sync solution than wipe out all data points and reload

        var stocksWithFailedRetrival = new List<Stock>();
        foreach (var stock in fund.Stocks)
        {
            var dataPoints = Enumerable.Empty<HistoricalDataPoint>();
            try
            {
                dataPoints = await externalFinancialApi.GetHistoricalData(stock.Ticker, DateOnly.FromDateTime(DateTime.Now).AddYears(-5));
            }
            catch (Exception e)
            {
                stocksWithFailedRetrival.Add(stock);
            }
            
            foreach (var dataPoint in dataPoints)
            {
                stock.HistoricalDataPoints.Add(dataPoint);
            }
        }

        await dbContext.SaveChangesAsync();
    }
}