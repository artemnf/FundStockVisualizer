using System.Collections.Immutable;
using MediatR;

namespace FundDataApi.Services.HistoricalData;

public class GetSp500ConstituentsQuery : IRequest<IImmutableList<string>>;
