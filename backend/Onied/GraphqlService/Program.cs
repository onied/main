using GraphqlService.Quiries;
using GraphqlService.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddGraphQLServer()
    .AddQueryType<CourseQuery>()
    .AddPagingArguments()
    .AddProjections()
    .AddFiltering()
    .AddSorting();


var app = builder.Build();

app.MapGraphQL();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
