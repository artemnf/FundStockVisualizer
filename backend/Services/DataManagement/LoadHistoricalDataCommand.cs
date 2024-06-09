using FundDataApi.Entities.Dtos;
using MediatR;

namespace FundDataApi.Services.DataManagement;

public class LoadHistoricalDataCommand() : IRequest<LoadDataResult>
{
}