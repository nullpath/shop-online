using ShopOnline.Api.Endpoints;
using ShopOnline.Api.Extensions;
using ShopOnline.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();
builder.InitializeDb();
builder.Services.AddInfrastructureServices();

var app = builder.Build();

app.MigrateDb();
app.MapEndpoints();

app.Run();