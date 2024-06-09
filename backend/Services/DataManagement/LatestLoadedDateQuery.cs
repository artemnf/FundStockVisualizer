using MediatR;

namespace FundDataApi.Services.DataManagement;

public record LatestLoadedDateQuery : IRequest<DateOnly>;
