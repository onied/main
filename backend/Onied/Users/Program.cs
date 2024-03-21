using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Users;
using Users.Extensions.WebApplicationExtensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(optionsBuilder =>
    optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("UsersDatabase"))
        .UseSnakeCaseNamingConvention());
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<AppUser>().AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddCors();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.ResolveConflictingActions(apiDescriptions =>
    {
        const int bestOrder = int.MaxValue;
        ApiDescription? bestDescr = null;
        foreach (var currDescr in apiDescriptions)
        {
            var currOrder = currDescr.ActionDescriptor.AttributeRouteInfo?.Order ?? 0;
            if (currOrder < bestOrder) bestDescr = currDescr;
        }

        return bestDescr;
    });
});


var app = builder.Build();
app.UseCors(b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapGroup("/api/v1/").MapCustomIdentityApi();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi().RequireAuthorization();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
