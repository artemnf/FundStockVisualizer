using System.Collections.Immutable;
using MediatR;

namespace FundDataApi.Services.HistoricalData;

public class Sp500ConstituentsQuery : IRequest<IImmutableList<string>>;
