using Microsoft.EntityFrameworkCore;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Users.Data;
using Users.Extensions;
using Users.Profiles;
using Users.Services.ProfileService;
using Users.Services.UsersService;
using AuthorizationService = Users.Services.GrpcServices.AuthorizationService;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("ocelot.json");
builder.Services.AddHealthChecks();
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddDbContextConfigured(builder.Configuration);
builder.Services.AddIdentityConfigured();
builder.Services.AddAutoMapper(options => options.AddProfile<AppMappingProfile>());
builder.Services.AddCors();

builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IProfileService, ProfileService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOcelot();
builder.Services.AddMassTransitConfigured(builder.Configuration);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddGrpc();

var app = builder.Build();
app.UseRouting();
app.UseCors(b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/healthz");
app.MapGrpcService<AuthorizationService>();

// We need this for Ocelot to work correctly;
// Otherwise the middleware chain is in the wrong order.
#pragma warning disable ASP0014
app.UseEndpoints(e => { e.MapControllers(); });
#pragma warning restore ASP0014

if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<AppDbContext>();
    if (context.Database.GetPendingMigrations().Any()) context.Database.Migrate();
}

app.UseWebSockets();
app.UseOcelot().Wait();

app.Run();
