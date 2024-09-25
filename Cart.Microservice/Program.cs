using System.Net;
using Cart.Domain.Consumers;
using Cart.Domain.Hub;
using Cart.Domain.SetDiscountsUsed;
using Cart.Microservice.Hub;
using Cart.Microservice.Services;
using MassTransit;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Sol.Caching.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel((context, serverOptions) =>
{
    // Listen on port 5002 for HTTP/2 (gRPC)
    serverOptions.Listen(IPAddress.Any, 5002, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });

    // Listen on port 5003 for HTTP/1.1 (SignalR)
    serverOptions.Listen(IPAddress.Any, 5004, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
    });
});

builder.Services.AddSignalR();
// Add services to the container.
builder.Services.AddGrpc();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(typeof(Cart.Domain.Entities.Cart).Assembly);
});


var rabbitMqSettings = builder.Configuration.GetSection("RabbitMQ");

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<AddDiscountConsumer>(); // Register the AddDiscountConsumer
    x.AddConsumer<CartUpdatedConsumer>(); // Register the CartUpdatedConsumer

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitMqSettings["Host"], "/", h =>
        {
            h.Username(rabbitMqSettings["Username"]);
            h.Password(rabbitMqSettings["Password"]);
        });
        
        cfg.ReceiveEndpoint("default", e =>
        {
            e.ConfigureConsumer<AddDiscountConsumer>(context);
            e.ConfigureConsumer<CartUpdatedConsumer>(context);
        });
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRedisCaching(builder.Configuration);
builder.Services.AddSingleton<IConnectionMapping,ConnectionMapping>();
builder.Services.AddScoped<SetDiscountsUsedMqCommand>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("ClientPermission", policy =>
    {
        policy.AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("http://web-sol:5003")
            .AllowCredentials();
    });
});


var app = builder.Build();
app.MapHub<CartHub>("/cartHub");
app.MapGrpcService<OrderService>();
app.UseAuthorization();



app.Run();