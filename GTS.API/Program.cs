using GTS.Application.CustomerServices;
// CTS Settings
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
    IMaxWashRepository,
    MaxWashRepository>();

builder.Services.AddScoped<
    IMaxWashService,
    MaxWashService>();

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

builder.Services.AddScoped<IWearerRepository, WearerRepository>();

builder.Services.AddScoped<IWearerService, WearerService>();


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