using Hangfire;
using Hangfire.MemoryStorage;
using Minio;
using Storage.Abstractions;
using Storage.Middlewares;
using Storage.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IStorageService, StorageService>();

builder.Services.AddSingleton<ExceptionMiddleware>();

var minioConfiguration = builder.Configuration.GetSection("MinIO");
builder.Services.AddMinio(configureClient => configureClient
    .WithEndpoint(minioConfiguration["Endpoint"], Convert.ToInt32(minioConfiguration["Port"]))
    .WithCredentials(minioConfiguration["AccessKey"], minioConfiguration["SecretKey"])
    .WithSSL(false)
    .Build());

builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblyContaining<Program>());

RecurringJob.AddOrUpdate<ITemporaryStorageCleanerService>(
    typeof(ITemporaryStorageCleanerService).FullName,
    x => x.CleanTemporaryStorage(),
    builder.Configuration.GetSection("Hangfire")["CronTemporaryStorageCleaner"],
    new RecurringJobOptions
    {
        TimeZone = TimeZoneInfo.Utc
    });

builder.Services.AddHangfire(x => x.UseMemoryStorage());

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
