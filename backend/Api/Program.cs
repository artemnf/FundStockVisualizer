using FundDataApi.Data;
using FundDataApi.Entities.Domain;
using FundDataApi.Services.ExternalFinancialApi;
using FundDataApi.Services.HistoricalData;
using FundDataApi.Services.Stocks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FundDataDbContext>(opts => opts.UseSqlite("Data Source=database.dat"));
builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(LoadHistoricalDataCommand).Assembly));

builder.Services.AddTransient<IExternalFinancialApi, YahooFinanceApi>();
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(cfg => cfg.AllowAnyOrigin()); //TODO: remove allow all CORS


app.MapGet("/stocks/{id}/historical-prices", async (int id, IMediator mediator, CancellationToken cancellationToken) =>
{
    var dataPoints = await mediator.Send(new HistoricalDataPointsQuery(StockId: id), cancellationToken);

    return dataPoints;
})
.WithName("Historical Prices")
.WithOpenApi();

app.MapGet("/funds/{id}/stocks", async (int id, [FromQuery] string? searchTerm, IMediator mediator, CancellationToken cancellationToken) =>
{
    var stocks = await  mediator.Send(new StocksQuery(FundId:id, searchTerm), cancellationToken);

    return stocks;
})
.WithName("Fund Stocks")
.WithTags("Funds")
.WithOpenApi();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<FundDataDbContext>();
    dbContext.Database.EnsureCreated(); // TODO: this might be ok for Sqlite but normally should be done via migration during deployment
}

app.Run();
