using FundDataApi.Data;
using FundDataApi.Entities.Domain;
using FundDataApi.Services.DataManagement;
using FundDataApi.Services.ExternalFinancialApi;
using FundDataApi.Services.HistoricalData;
using FundDataApi.Services.Stocks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FundDataDbContext>(opts => opts.UseSqlite("Data Source=database.dat"));
builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(LoadHistoricalDataCommand).Assembly));

builder.Services.AddTransient<IExternalFinancialApi, YahooFinanceApi>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();


app.MapGet("/stocks/{id}/historical-prices", async (int id, IMediator mediator, CancellationToken cancellationToken) =>
{
    return await mediator.Send(new HistoricalDataPointsQuery(StockId: id), cancellationToken);
})
.WithName("Historical Prices")
.WithTags("Stocks")
.WithOpenApi();

app.MapGet("/stocks/{id}/aggregated-data", async (int id, [FromQuery] int years, IMediator mediator, CancellationToken cancellationToken) =>
{
    return await mediator.Send(new AggregatedHistoricalStockDataQuery(StockId: id, years), cancellationToken);
})
.WithName("Aggregated Data")
.WithTags("Stocks")
.WithOpenApi();

app.MapGet("/funds/{id}/stocks", async (int id, [FromQuery] string? searchTerm, IMediator mediator, CancellationToken cancellationToken) =>
{
    return await mediator.Send(new StocksQuery(FundId:id, searchTerm), cancellationToken);
})
.WithName("Fund Stocks")
.WithTags("Funds")
.WithOpenApi();

app.MapGet("/data/latest-loaded", async(IMediator mediator, CancellationToken cancellationToken) => 
{
    return await mediator.Send(new LatestLoadedDateQuery(), cancellationToken);
})
.WithName("Latest Loaded Date")
.WithTags("Data Management")
.WithOpenApi();

app.MapPost("/data/load", async(IMediator mediator, CancellationToken cancellationToken) => {
    return await mediator.Send(new LoadHistoricalDataCommand());
})
.WithName("Load Data")
.WithTags("Data Management")
.WithOpenApi();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<FundDataDbContext>();
    dbContext.Database.EnsureCreated(); // TODO: this might be ok for Sqlite but normally should be done via migration during deployment
}

app.Run();
