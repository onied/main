using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.EntityFrameworkCore;
using Purchases.Data;
using Purchases.Extensions;
using Purchases.Profiles;
using Purchases.Services;
using Purchases.Services.Abstractions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

builder.Services.AddDbContextConfigured();
builder.Services.AddRepositories();

builder.Services.AddAutoMapper(options => options.AddProfile<AppMappingProfile>());

builder.Services.AddHangfireWorker();

builder.Services.AddControllers();

builder.Services.AddMassTransitConfigured();

builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
    .AddNegotiate();

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>(
    x => ActivatorUtilities.CreateInstance<JwtTokenService>(x, x.GetService<IConfiguration>()!["JwtSecretKey"]!));
builder.Services.AddScoped<IPurchaseTokenService, PurchaseTokenService>();
builder.Services.AddScoped<IPurchaseManagementService, PurchaseManagementService>();
builder.Services.AddScoped<ISubscriptionManagementService, SubscriptionManagementService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHangfireWorker(builder.Configuration);

app.UseHttpsRedirection();

app.UseCors(b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapControllers();

if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<AppDbContext>();
    if (context.Database.GetPendingMigrations().Any()) context.Database.Migrate();
}

app.Run();
