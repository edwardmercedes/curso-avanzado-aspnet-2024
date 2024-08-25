using _04_Inventory.Api;
using _04_Inventory.Api.Infrastruture;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Configuracion de base de datos Entt
IConfiguration configuration = builder.Configuration;
builder.Services.Configure<SettingsValue>(configuration);
var serviceProvider = builder.Services.BuildServiceProvider();
var settings = serviceProvider.GetService<IOptions<SettingsValue>>();

//var connectionString = configuration.GetValue<string>("ConnectionString"); forma directa de coneccion

builder.Services.AddDbContext<DBContext>(options =>
{
    options.UseOracle(settings.Value.ConnectionString, op => op.UseOracleSQLCompatibility(OracleSQLCompatibility.DatabaseVersion19));
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
