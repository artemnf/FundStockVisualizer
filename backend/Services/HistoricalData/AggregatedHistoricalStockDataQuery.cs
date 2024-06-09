using FundDataApi.Entities.Dtos;
using MediatR;

namespace FundDataApi.Services.HistoricalData;

public record AggregatedHistoricalStockDataQuery(int StockId, int Years = 1) : IRequest<AggregatedHistoricalStockData>;


