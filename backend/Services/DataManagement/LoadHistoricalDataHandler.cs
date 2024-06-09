using System.Collections.Immutable;
using FundDataApi.Data;
using FundDataApi.Entities.Domain;
using FundDataApi.Entities.Dtos;
using FundDataApi.Services.ExternalFinancialApi;
using FundDataApi.Services.HistoricalData;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HistoricalDataPoint = FundDataApi.Entities.Domain.HistoricalDataPoint;
using Stock = FundDataApi.Entities.Domain.Stock;

namespace FundDataApi.Services.DataManagement;

public class LoadHistoricalDataHandler(FundDataDbContext dbContext,
                                       IExternalFinancialApi externalFinancialApi,
                                       IMediator mediator)
    : IRequestHandler<LoadHistoricalDataCommand, LoadDataResult>
{
    private const string FundSymbol = "SPY";

    public async Task<LoadDataResult> Handle(LoadHistoricalDataCommand request, CancellationToken cancellationToken)
    {
        var sp500Symbols = await mediator.Send(new Sp500ConstituentsQuery(), cancellationToken);

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

        //await dbContext.Set<HistoricalDataPoint>().ExecuteDeleteAsync(); // TODO: might be a better sync solution than wipe out all data points and reload

        var latestLoadedDates = await dbContext
                .Set<HistoricalDataPoint>()
                .GroupBy(x => x.Stock.Id)
                .ToDictionaryAsync(x => x.Key, g => g.Max(x => x.Date), cancellationToken);

        var stocksWithFailedRetrival = new List<Stock>();
        foreach (var stock in fund.Stocks)
        {
            var dataPoints = Enumerable.Empty<HistoricalDataPoint>();
            var startDate = latestLoadedDates.ContainsKey(stock.Id)
                ? latestLoadedDates[stock.Id].AddDays(1)
                : DateOnly.FromDateTime(DateTime.Now).AddYears(-5);

            try
            {
                dataPoints = await externalFinancialApi.GetHistoricalData(stock.Ticker, startDate);
            }
            catch (Exception e)
            {
                stocksWithFailedRetrival.Add(stock);
            }

            foreach (var dataPoint in dataPoints.Where(x => x.Date > startDate)) // need to filter out already loaded dates as external api returns last avaialable date event if is before start date
            {
                stock.HistoricalDataPoints.Add(dataPoint);
            }
        }

        await dbContext.SaveChangesAsync();

        var latestLoadedDate = await mediator.Send(new LatestLoadedDateQuery());

        return new LoadDataResult(latestLoadedDate, stocksWithFailedRetrival.Select(x => x.Ticker));
    }
}