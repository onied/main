using GraphqlService.Extensions;
using GraphqlService.Profiles;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddGraphQl()
    .AddHttpContextAccessor()
    .AddDbContext(builder.Configuration)
    .AddAutoMapper(options => options.AddProfile<AppMappingProfile>());

var app = builder.Build();

app.MapGraphQL();

app.UseHttpsRedirection();

app.Run();
