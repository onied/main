using Courses;
using Courses.Extensions;
using Courses.Profiles;
using Courses.Services;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(optionsBuilder =>
    optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("CoursesDatabase"))
        .UseSnakeCaseNamingConvention());
builder.Services.AddAutoMapper(options => options.AddProfile<AppMappingProfile>());
builder.Services.AddCors();
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
    .AddNegotiate();

builder.Services.AddMassTransitConfigured();

builder.Services.AddScoped<ICourseManagementService, CourseManagementService>();
builder.Services.AddScoped<ICheckTasksService, CheckTasksService>();
builder.Services.AddScoped<IUserTaskPointsRepository, UserTaskPointsRepository>();
builder.Services.AddScoped<IUpdateTasksBlockService, UpdateTasksBlockService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IBlockRepository, BlockRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUserCourseInfoRepository, UserCourseInfoRepository>();
builder.Services.AddScoped<ICheckTaskManagementService, CheckTaskManagementService>();
builder.Services.AddScoped<IBlockCompletedInfoRepository, BlockCompletedInfoRepository>();
builder.Services.AddScoped<IModuleRepository, ModuleRepository>();
builder.Services.AddScoped<IManualReviewTaskUserAnswerRepository, ManualReviewTaskUserAnswerRepository>();
builder.Services.AddScoped<IManualReviewService, ManualReviewService>();

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

app.Run();
