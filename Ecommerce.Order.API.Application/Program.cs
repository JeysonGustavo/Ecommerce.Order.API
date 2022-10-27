using Ecommerce.Order.API.Core.Context;
using Ecommerce.Order.API.Core.Infrastructure;
using Ecommerce.Order.API.Core.Manager;
using Ecommerce.Order.API.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connection = builder.Configuration.GetConnectionString("EcommerceConn");
builder.Services.AddDbContext<EcommerceDbContext>(opt => opt.UseSqlServer(connection));


builder.Services.AddScoped<IOrderManager, OrderManager>();
builder.Services.AddScoped<IOrderDAL, OrderDAL>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
