using ClickHouse.Client.ADO;
using Courses.Data;
using Courses.Data.Abstractions;
using Courses.Extensions;
using Courses.Profiles;
using Courses.Services.BackgroundServices;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

// Added context database
builder.Services.AddDbContext(builder.Configuration);

// Added profiles for mapper
builder.Services.AddAutoMapper(options => options.AddProfile<AppMappingProfile>());

builder.Services.AddCors();
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();

builder.Services.AddMassTransitConfigured();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.AddGrpcClients(builder.Configuration);

builder.Services.AddScoped<ClickHouseConnection>(_ =>
    new ClickHouseConnection(builder.Configuration.GetConnectionString("StatsDatabase")));

// Added business logic services
builder.Services.AddServices();

// Added all repositories
builder.Services.AddRepositories();

// Added converters
builder.Services.AddConverters();

builder.Services.AddHostedService<StatsSenderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<AppDbContext>();
    if (context.Database.GetPendingMigrations().Any())
        context.Database.Migrate();
}

using (var statsScope = app.Services.CreateScope())
{
    var services = statsScope.ServiceProvider;
    var statsRepository = services.GetRequiredService<IStatsRepository>();
    await statsRepository.CreateTablesIfNotExistsAsync();
}


app.Run();
