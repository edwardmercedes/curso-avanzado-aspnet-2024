using _04_Inventory.Api;
using _04_Inventory.Api.Infrastruture;
using _04_Inventory.Api.Mapper;
using _04_Inventory.Api.Middlewares;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;
using NLog.Web;

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
    //options.UseOracle(settings.Value.ConnectionString, op => op.UseOracleSQLCompatibility(OracleSQLCompatibility.DatabaseVersion19));
    options.UseOracle(settings.Value.ConnectionString, op => op.UseOracleSQLCompatibility("11"));
});

//automaper
var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new AutoMapping());
});
IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);


//nlog
var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
ConfigureLogger(builder.Logging);


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

app.UseMiddleware<ExceptionHandLingMiddleware>();

app.MapControllers();

app.Run();


void ConfigureLogger(ILoggingBuilder loggingBuilder) {
    loggingBuilder.ClearProviders();
    loggingBuilder.SetMinimumLevel(LogLevel.Trace);
    loggingBuilder.AddNLog();
    
    NLog.LogManager.Setup().LoadConfigurationFromAppSettings();
}