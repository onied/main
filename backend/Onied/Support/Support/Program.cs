using Microsoft.EntityFrameworkCore;
using Support.Data;
using Support.Events.Extensions;
using Support.Extensions;
using Support.Hubs;
using Support.Middlewares;
using Support.Services.GrpcServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

builder.Services.AddSingleton<ExceptionMiddleware>();

builder.Services.AddDbContextConfigured();
builder.Services.AddRepositories();

// Add your services inside AddService method
builder.Services.AddServices();
builder.Services.AddEventsServices();

builder.Services.AddAutoMapperConfigured();
builder.Services.AddMassTransitWithHangfireConfigured();

builder.Services.AddAuthorizationConfiguration();
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddGrpc();
builder.Services.AddGrpcClients(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseCorsConfigured();

app.MapControllers();
app.MapHub<ChatHub>("/api/v1/chat/hub");
app.MapGrpcService<UserChatService>();

if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<AppDbContext>();
    if (context.Database.GetPendingMigrations().Any()) context.Database.Migrate();
}

app.Run();
