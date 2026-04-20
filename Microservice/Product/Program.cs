using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using ProductService.Grpc;
using ProductService.Models;
using ProductService.Repository.CategoryRepo;
using ProductService.Repository.CategoryRepo.Implementation;
using ProductService.Repository.ProductRepo;
using ProductService.Repository.ProductRepo.Implementation;
using ProductService.Services.CategoryService;
using ProductService.Services.CategoryService.Implementation;
using ProductService.Services.ProductService;
using ProductService.Services.ProductService.Implementation;
using System.Runtime.InteropServices;
using ProductServiceImplementation = ProductService.Services.ProductService.Implementation.ProductService;

var builder = WebApplication.CreateBuilder(args);

var grpcPort = builder.Configuration.GetValue<int?>("Grpc:Port");

builder.WebHost.ConfigureKestrel(options =>
{
    if (grpcPort.HasValue && grpcPort.Value > 0)
    {
        options.ListenAnyIP(grpcPort.Value, listenOptions =>
        {
            listenOptions.Protocols = HttpProtocols.Http2;
        });
    }
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddGrpc();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductService, ProductServiceImplementation>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapGrpcService<ProductLookupService>();
app.MapControllers();

app.Run();
