using GTS.Application.CustomerServices;
using GTS.Application.Interfaces;
using GTS.Application.Interfaces.CustomerRepositories;
using GTS.Application.Interfaces.CustomerServices;
using GTS.Application.Interfaces.WearerRepositories;
using GTS.Application.Interfaces.WearerServices;
using GTS.Application.Services;

using GTS.Infrastructure.CustomerRepositories;
using GTS.Infrastructure.Repositories;
using GTS.Infrastructure.WearerRepositories;

using GTS.MVC.Gateways;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// MVC
// ============================================================

builder.Services
    .AddControllersWithViews()
    .ConfigureApplicationPartManager(manager =>
    {
        // GTS.API is referenced by this practice solution.
        // Remove its controllers from MVC discovery so that
        // SoilStationController exists only in GTS.MVC.
        var apiAssembly = AppDomain.CurrentDomain
            .GetAssemblies()
            .FirstOrDefault(a =>
                a.GetName().Name == "GTS.API");

        if (apiAssembly != null)
        {
            var apiPart = manager.ApplicationParts
                .FirstOrDefault(part =>
                    part.Name == apiAssembly.GetName().Name);

            if (apiPart != null)
            {
                manager.ApplicationParts.Remove(apiPart);
            }
        }
    });


// ============================================================
// CUSTOMER
// ============================================================

builder.Services.AddScoped<
    ICustomerRepository,
    CustomerRepository>();

builder.Services.AddScoped<
    ICustomerService,
    CustomerService>();


// ============================================================
// WEARER
// ============================================================

builder.Services.AddScoped<
    IWearerRepository,
    WearerRepository>();

builder.Services.AddScoped<
    IWearerService,
    WearerService>();


// ============================================================
// SOIL STATION
// ============================================================

// MVC Web Service
builder.Services.AddScoped<
    ISoilStationWebService,
    SoilStationWebService>();

// MVC Gateway
builder.Services.AddScoped<
    ISoilStationGateway,
    SoilStationGateway>();


// ============================================================
// HTTP CLIENT → GTS.API
// ============================================================



// ============================================================
// BUILD
// ============================================================

var app = builder.Build();


// ============================================================
// HTTP PIPELINE
// ============================================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


// ============================================================
// MVC ROUTING
// ============================================================

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Main}/{action=Index}/{id?}");


// ============================================================
// RUN
// ============================================================

app.Run();