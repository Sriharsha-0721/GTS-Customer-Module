using GTS.Application.CustomerServices;
using GTS.Application.Interfaces;
using GTS.Application.Interfaces.CustomerRepositories;
using GTS.Application.Interfaces.CustomerServices;
using GTS.Application.Interfaces.WearerRepositories;
using GTS.Application.Interfaces.WearerServices;
using GTS.Application.Services;
using GTS.Domain.Interfaces;
using GTS.Infrastructure.CustomerRepositories;
using GTS.Infrastructure.Repositories;
using GTS.Infrastructure.WearerRepositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//
// Customer
//
builder.Services.AddScoped<
    ICustomerRepository,
    CustomerRepository>();

builder.Services.AddScoped<
    ICustomerService,
    CustomerService>();

//
// CTS Settings
//
builder.Services.AddScoped<
    ICTSSettingRepository,
    CTSSettingRepository>();

builder.Services.AddScoped<
    ICTSSettingService,
    CTSSettingService>();

builder.Services.AddScoped<
    ICustomerLineCommentsRepository,
    CustomerLineCommentsRepository>();

builder.Services.AddScoped<
    ICustomerLineCommentsService,
    CustomerLineCommentsService>();

//
// Wearer
//
builder.Services.AddScoped<
    IWearerRepository,
    WearerRepository>();

builder.Services.AddScoped<
    IWearerService,
    WearerService>();

//
// Special Lines
//
builder.Services.AddScoped<
    ISpecialLinesService,
    SpecialLinesService>();

//
// Max Wash
//
builder.Services.AddScoped<
    IMaxWashRepository,
    MaxWashRepository>();

builder.Services.AddScoped<
    IMaxWashService,
    MaxWashService>();

//
// Billing
//
builder.Services.AddScoped<
    IBillingRepository,
    BillingRepository>();

builder.Services.AddScoped<
    IBillingService,
    BillingService>();

//
// Packout
//
builder.Services.AddScoped<
    IPackoutRepository,
    PackoutRepository>();

builder.Services.AddScoped<
    IPackoutService,
    PackoutService>();

builder.Services.AddScoped<IWashRepository, WashRepository>();
builder.Services.AddScoped<IWashService, WashService>();

builder.Services.AddScoped<ISpecialLinesRepository, SpecialLinesRepository>();

// Soil Station
builder.Services.AddScoped<
    ISoilStationRepository,
    SoilStationRepository>();

builder.Services.AddScoped<ISoilStationService, SoilStationService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();