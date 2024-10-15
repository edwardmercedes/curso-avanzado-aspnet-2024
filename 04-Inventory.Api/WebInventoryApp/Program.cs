using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using System.Globalization;
using WebInventoryApp;
using WebInventoryApp.Services.Inventory;

var builder = WebApplication.CreateBuilder(args);

//Configuracion de recursos
builder.Services.AddLocalization( opt => opt.ResourcesPath = "Ressources" );
builder.Services.AddMvc().AddViewLocalization();


//Configuracion lectura
IConfiguration configuration = builder.Configuration;
builder.Services.Configure<SettingsValue>(configuration);
var servicesProvider = builder.Services.BuildServiceProvider();
var settings = servicesProvider.GetService<IOptions<SettingsValue>>();


// Add services to the container.
builder.Services.AddRazorPages();

//configuracion de servicios
builder.Services.AddHttpClient<IInventoryServices, InventoryServices>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var cultureProviders = new[]
{
    new QueryStringRequestCultureProvider()
};

var supportedCultures = new[] {
    new CultureInfo("en-US"),
    new CultureInfo("es-MX")
};



app.UseHttpsRedirection();

app.UseRequestLocalization(new RequestLocalizationOptions { 
    DefaultRequestCulture = new RequestCulture(settings?.Value.DefaultCulture ?? "es-DO"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,

});

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
