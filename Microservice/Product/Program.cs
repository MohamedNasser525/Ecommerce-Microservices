using Microsoft.EntityFrameworkCore;
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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
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

app.MapControllers();

app.Run();
