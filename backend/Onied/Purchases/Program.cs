using Purchases.Abstractions;
using Purchases.Data;
using Purchases.Extensions;
using Purchases.Profiles;
using Purchases.Services;

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

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>(
    x => ActivatorUtilities.CreateInstance<JwtTokenService>(x, builder.Configuration["JwtSecretKey"]!));

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
