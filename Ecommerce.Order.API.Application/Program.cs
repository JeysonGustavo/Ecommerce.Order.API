using Ecommerce.Order.API.Core.Context;
using Ecommerce.Order.API.Core.EventBus.Connection;
using Ecommerce.Order.API.Core.EventBus.Publisher;
using Ecommerce.Order.API.Core.EventBus.Subscriber;
using Ecommerce.Order.API.Core.Infrastructure;
using Ecommerce.Order.API.Core.Listener;
using Ecommerce.Order.API.Core.Manager;
using Ecommerce.Order.API.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region Database
var connection = builder.Configuration.GetConnectionString("EcommerceConn");
builder.Services.AddDbContext<EcommerceDbContext>(opt => opt.UseSqlServer(connection));
#endregion

#region Managers
builder.Services.AddScoped<IOrderManager, OrderManager>();
#endregion

#region DALs
builder.Services.AddScoped<IOrderDAL, OrderDAL>();
#endregion

#region RabbitMQ
builder.Services.AddSingleton<IConnectionProvider>(new ConnectionProvider(builder.Configuration["RabbitMQHost"], int.Parse(builder.Configuration["RabbitMQPort"])));
builder.Services.AddSingleton<ISubscriber, Subscriber>(e => new Subscriber(e.GetService<IConnectionProvider>(), e.GetService<IServiceScopeFactory>(), "product", ExchangeType.Direct));
builder.Services.AddSingleton<IPublisher, Publisher>(e => new Publisher(e.GetService<IConnectionProvider>(), "order_detail", ExchangeType.Direct));
#endregion

#region Listeners
builder.Services.AddHostedService<ProductStockChangedListener>();
#endregion

#region Auto Mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
#endregion

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
