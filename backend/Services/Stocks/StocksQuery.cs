using FundDataApi.Entities.Dtos;
using MediatR;

namespace FundDataApi.Services.Stocks;

public record StocksQuery(int FundId, string? SearchTerm) : IRequest<IEnumerable<Stock>>;
