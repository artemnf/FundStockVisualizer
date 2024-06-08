using FundDataApi.Entities.Dtos;
using MediatR;

namespace FundDataApi.Services.HistoricalData;

public record AggregatedHistoricalStockDataQuery(int StockId) : IRequest<AggregatedHistoricalStockData>;


