using ShopOnline.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.InitializeDatabase();

var app = builder.Build();

app.MigrateDb();

app.Run();