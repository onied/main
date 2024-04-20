using Notifications.Data;
using Notifications.Extensions;
using Notifications.Hubs;
using Notifications.Profiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddDbContextConfigured();
builder.Services.AddRepositories();
builder.Services.AddAutoMapper(options => options.AddProfile<AppMappingProfile>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(_ => true)
    .AllowCredentials());

app.MapControllers();
app.MapHub<NotificationsHub>("api/v1/notifications/hub");

app.Run();
