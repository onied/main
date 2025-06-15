using ClickHouse.Client.ADO;
using Courses.Data.Abstractions;
using Courses.Data.Repositories;
using GraphqlService.Extensions;
using GraphqlService.Profiles;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddGraphQl()
    .AddHttpContextAccessor()
    .AddDbContext(builder.Configuration)
    .AddAutoMapper(options => options.AddProfile<AppMappingProfile>())
    .AddScoped<ClickHouseConnection>(_ =>
        new ClickHouseConnection(builder.Configuration.GetConnectionString("StatsDatabase")))
    .AddScoped<IStatsRepository, StatsRepository>();


var app = builder.Build();

app.MapGraphQL();

app.UseHttpsRedirection();

using (var statsScope = app.Services.CreateScope())
{
    var services = statsScope.ServiceProvider;
    var statsRepository = services.GetRequiredService<IStatsRepository>();
    await statsRepository.CreateTablesIfNotExistsAsync();
}

app.Run();
