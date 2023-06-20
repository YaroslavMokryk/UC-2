using Microsoft.AspNetCore.Diagnostics;
using Stripe;
using System.Net;
using UC_2.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Set Stripe API secrets
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

builder.Services
    .AddScoped<BalanceService>()
    .AddScoped<BalanceTransactionService>()
    .AddScoped<IStripeApiService, StripeApiService>();

var app = builder.Build();

app.UseHttpsRedirection();

// Add error handling
app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";
        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
        if (contextFeature != null)
        {
            await context.Response.WriteAsync($"Internal Server Error: {context.Response.StatusCode}, {context.Response.Body}");
        }
    });
});

app.UseAuthorization();

app.MapControllers();

app.Run();
