using Hangfire;
using Hangfire.MemoryStorage;
using Storage.Abstractions;
using Storage.Extensions;
using Storage.Middlewares;
using Storage.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IStorageService, StorageService>();
builder.Services.AddScoped<IRedisRepository, RedisRepository>();

builder.Services.AddSingleton<ExceptionMiddleware>();
builder.Services.AddScoped<TemporaryStorageService>();
builder.Services.AddScoped<ITemporaryStorageCleanerService, TemporaryStorageCleanerService>();
builder.Services.AddScoped<IPermanentStorageTransferService, PermanentStorageTransferService>();

builder.Services
    .AddMinioConfigured()
    .AddRedisConfigured()
    .AddHangfire(builder.Configuration);

builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblyContaining<Program>());

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();
