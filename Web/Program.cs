using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

using Grpc.Net.Client;
using Microsoft.AspNetCore.DataProtection;
using Web;
using Web.Components;
using Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel((context, serverOptions) =>
{
    serverOptions.Listen(IPAddress.Any, 5003);
});
// Configure data protection to persist keys to a specific directory
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "keys")))
    //.UseEphemeralDataProtectionProvider()
    .SetApplicationName("TestSol");
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


builder.Services.AddScoped<IDiscountService,DiscountService>();
builder.Services.AddScoped<IOrderService,OrderService>();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

await app.RunAsync();