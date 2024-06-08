using FundDataApi.Entities.Dtos;
using MediatR;

namespace FundDataApi.Services.HistoricalData;

public record HistoricalDataPointsQuery(int StockId) : IRequest<IEnumerable<HistoricalDataPoint>>;
