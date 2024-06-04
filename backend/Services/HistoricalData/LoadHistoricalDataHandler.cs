using FundDataApi.Data;
using FundDataApi.Services.ExternalFinancialApi;
using MediatR;

namespace FundDataApi.Services.HistoricalData;

public class LoadHistoricalDataHandler(FundDataDbContext fundDataDbContext, IExternalFinancialApi externalFinancialApi)
    : IRequestHandler<LoadHistoricalDataCommand>
{
    public async Task Handle(LoadHistoricalDataCommand request, CancellationToken cancellationToken)
    {
        //var dataPoints = await externalFinancialApi.GetHistoricalDataWeekly()
    }
}