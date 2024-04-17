using Microsoft.AspNetCore.Authentication.Negotiate;
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

builder.Services.AddControllers();

builder.Services.AddMassTransitConfigured();

builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
    .AddNegotiate();

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>(
    x => ActivatorUtilities.CreateInstance<JwtTokenService>(x, x.GetService<IConfiguration>()!["JwtSecretKey"]!));
builder.Services.AddScoped<IPurchaseTokenService, PurchaseTokenService>();
builder.Services.AddScoped<IPurchaseManagementService, PurchaseManagementService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapControllers();
app.Run();
