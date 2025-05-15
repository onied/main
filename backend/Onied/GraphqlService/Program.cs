using Courses.Data.Models;
using GraphqlService.Quiries;
using GraphqlService.Extensions;
using GraphqlService.Profiles;
using GraphqlService.Types;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddGraphQLServer()
    .AddQueryType<CourseQuery>()
    .AddType<BlockTypeType>()
    .AddPagingArguments()
    .AddProjections()
    .AddFiltering()
    .AddSorting();
builder.Services.AddAutoMapper(options => options.AddProfile<AppMappingProfile>());


var app = builder.Build();

app.MapGraphQL();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
