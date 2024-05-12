using Purchases.Data;
using Purchases.Extensions;

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

app.MapControllers();
app.Run();
