using GTSCustomerAPI.Application.Interfaces;
using GTSCustomerAPI.Application.Services;
using GTSCustomerAPI.Domain.Interfaces;
using GTSCustomerAPI.Infrastructure.Data;
using GTSCustomerAPI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlServer(builder.Configuration
        .GetConnectionString("DefaultConnection")));

// Customer
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

// CTS Settings
builder.Services.AddScoped<ICTSSettingsRepository, CTSSettingsRepository>();
builder.Services.AddScoped<ICTSSettingsService, CTSSettingsService>();

// Customer Admin
builder.Services.AddScoped<ICustomerAdminRepository, CustomerAdminRepository>();
builder.Services.AddScoped<ICustomerAdminService, CustomerAdminService>();

// Wearer
builder.Services.AddScoped<IWearerRepository, WearerRepository>();
builder.Services.AddScoped<IWearerService, WearerService>();

builder.Services.AddScoped<ICustomerProfileRepository, CustomerProfileRepository>();
builder.Services.AddScoped<ICustomerProfileService, CustomerProfileService>();


// Billing
builder.Services.AddScoped<
    IBillingRepository,
    BillingRepository>();

builder.Services.AddScoped<
    IBillingService,
    BillingService>();

// Packout
builder.Services.AddScoped<
    IPackoutRepository,
    PackoutRepository>();

builder.Services.AddScoped<
    IPackoutService,
    PackoutService>();

// Wash
builder.Services.AddScoped<IWashRepository, WashRepository>();
builder.Services.AddScoped<IWashService, WashService>();

// Dosage
builder.Services.AddScoped<
    IDosageRepository,
    DosageRepository>();

builder.Services.AddScoped<
    IDosageService,
    DosageService>();



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "GTS Customer API",
            Version = "v1"
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint(
            "/swagger/v1/swagger.json",
            "GTS Customer API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
