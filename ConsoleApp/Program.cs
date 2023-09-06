using AutoMapper;
using ConsoleApp;
using ConsoleApp.Mapping;
using Infrastructure.EntityFramework;
using Infrastructure.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services.Repositories.Abstractions;
using System.Configuration;

var connection = ConfigurationManager.ConnectionStrings["ConnectionPSQL"].ToString();
HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Start>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository,ProductRepository>();
builder.Services.AddScoped<IShopRepository, ShopRepository>();
builder.Services.AddDbContext<DateBaseContext>(options => options.UseNpgsql(connection));
builder.Services.AddAutoMapper(typeof(UserMappingProfile));
builder.Services.AddAutoMapper(typeof(ShopMappingProfile));
builder.Services.AddAutoMapper(typeof(ProductMappingProfile));

using IHost host = builder.Build();

await host.RunAsync();