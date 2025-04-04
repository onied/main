using Microsoft.EntityFrameworkCore;
using Purchases.Data;
using Purchases.Extensions;
using Purchases.Services.GrpcServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

builder.Services.AddDbContextConfigured();
builder.Services.AddRepositories();
builder.Services.AddServices();

builder.Services.AddAutoMapperConfigured();
builder.Services.AddMassTransitConfigured();
builder.Services.AddHangfireWorker();
builder.Services.AddGrpc();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.AddAuthorizationNegotiate();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHangfireWorker(builder.Configuration);

app.UseHttpsRedirection();

app.UseCorsConfigured();

app.MapGrpcService<PurchasesService>();
app.MapGrpcService<SubscriptionService>();
app.MapControllers();

if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<AppDbContext>();
    if (context.Database.GetPendingMigrations().Any()) context.Database.Migrate();
}

app.Run();
