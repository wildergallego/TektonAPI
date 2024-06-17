using BusinessLayer.Dto;
using BusinessLayer.Filters;
using BusinessLayer.Interfaces;
using BusinessLayer.Repository;
using BusinessLayer.Validators;
using DataLayer;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IProduct, BusinessLayer.Services.ProductService>();
builder.Services.AddScoped<IProductStatus, BusinessLayer.Services.ProductStatusService>();
builder.Services.AddScoped<IProductDiscount, BusinessLayer.Services.ProductDiscountService>();

var urlDiscount = builder.Configuration["UrlDiscountsProduct"] ?? "";
builder.Services.AddHttpClient<IProductDiscount, BusinessLayer.Services.ProductDiscountService>(item => item.BaseAddress = new Uri(urlDiscount));
builder.Services.AddHttpClient<IProduct, BusinessLayer.Services.ProductService>(item => item.BaseAddress = new Uri(urlDiscount));

builder.Services.AddScoped<IProductRepository<Product>, ProductRepository>();
builder.Services.AddScoped<IProductStatusRepository<ProductStatus>, ProductStatusRepository>();

builder.Services.AddDbContext<ProductsDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBLocalConnection"));
});

builder.Services.AddScoped<IValidator<ProductChangeDto>, ProductValidator>();
builder.Services.AddSingleton<ResponseTimeFilter>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLazyCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
