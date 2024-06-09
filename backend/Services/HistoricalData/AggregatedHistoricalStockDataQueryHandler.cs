using System.Runtime.CompilerServices;
using FundDataApi.Data;
using FundDataApi.Entities.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FundDataApi.Services.HistoricalData;

public class AggregatedHistoricalStockDataQueryHandler(FundDataDbContext dbContext) : IRequestHandler<AggregatedHistoricalStockDataQuery, AggregatedHistoricalStockData>
{
    public async Task<AggregatedHistoricalStockData> Handle(AggregatedHistoricalStockDataQuery query, CancellationToken cancellationToken)
    {
        var sqlQuery = @$"
            WITH TargetDate as
            (
                SELECT hdp.StockId, hdp.Date, s.Ticker 
                FROM HistoricalDataPoint hdp
                INNER JOIN Stock s on s.Id = hdp.StockId
                WHERE StockId={query.StockId} order by date desc limit 1
            )
            
            SELECT StockId,
                   Ticker as Symbol,
                   Date,
                   (
                        SELECT max(High)
                        FROM HistoricalDataPoint hdp
                        WHERE hdp.stockid = tDate.StockId and hdp.date BETWEEN date(tDate.date, '-1 years') and date
                    )  as MaxPrice,
                    (
                        SELECT min(Low)
                        FROM HistoricalDataPoint hdp
                        WHERE hdp.stockid = tDate.StockId and hdp.date BETWEEN date(tDate.date, '-{query.Years} years') and date
                    )  as MinPrice,
                    (
                        SELECT avg(AdjClose)
                        FROM HistoricalDataPoint hdp
                        WHERE hdp.stockid = tDate.StockId and hdp.date BETWEEN date(tDate.date, '-{query.Years} years') and date
                    )  as AvgPrice,
                    (
                        SELECT avg(Volume)
                        FROM HistoricalDataPoint hdp
                        WHERE hdp.stockid = tDate.StockId and hdp.date BETWEEN date(tDate.date, '-{query.Years} years') and date
                    )  as AvgVolume

            FROM TargetDate tDate
        ";

        var result = await dbContext.Database.SqlQuery<AggregatedHistoricalStockData>(FormattableStringFactory.Create(sqlQuery)).SingleAsync(); // cannot use 'real' FormattableString here for parameter sanitizing preventing the intended usage

        return result;
    }
}
