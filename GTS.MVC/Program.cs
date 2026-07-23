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
using GTS.MVC.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IWearerRepository, WearerRepository>();

builder.Services.AddScoped<IWearerService, WearerService>();
builder.Services.AddHttpClient<CTSSettingsController>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7161/");
});

var app = builder.Build();
    
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Main}/{action=Index}/{id?}" );

app.Run();