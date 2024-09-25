using System.Net;
using Discounts.Domain.AddDiscountToCart;
using Discounts.Domain.Consumers;
using Discounts.Domain.Database;
using Discounts.Domain.Database.Utils;
using Discounts.Service.Services;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel((context, serverOptions) =>
{
    serverOptions.Listen(IPAddress.Any, 5001);
});

// Add services to the container.
builder.Services.AddGrpc();


var config = new ServerConfig();
builder.Configuration.Bind(config);


builder.Services.AddSingleton<IDiscountsDbContext>(new DiscountsDbContext(config.MongoDB));
        

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(typeof(Discounts.Domain.Entities.Discount).Assembly);
});
var rabbitMqSettings = builder.Configuration.GetSection("RabbitMQ");


builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<SetDiscountUsedConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitMqSettings["Host"], "/", h =>
        {
            h.Username(rabbitMqSettings["Username"]);
            h.Password(rabbitMqSettings["Password"]);
        });

        cfg.ReceiveEndpoint("set-discount-used", e => { e.ConfigureConsumer<SetDiscountUsedConsumer>(context); });
    });
});


builder.Services.AddScoped<AddDiscountToCartMqCommand>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<CreateDiscountService>();
app.MapGrpcService<ValidateDiscountService>();
app.MapGrpcService<GetAllDiscountsService>();
app.MapGrpcService<AddToCartDiscountService>();


app.Run();