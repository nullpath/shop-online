using ShopOnline.Api.Endpoints;
using ShopOnline.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.InitializeDb();

var app = builder.Build();

app.MigrateDb();
app.MapEndpoints();

app.Run();