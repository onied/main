using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Users;
using Users.Profiles;
using Users.Services.EmailSender;
using Users.Services.ProfileProducer;
using Users.Services.UserCreatedProducer;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("ocelot.json");
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddDbContext<AppDbContext>(optionsBuilder =>
    optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("UsersDatabase"))
        .UseSnakeCaseNamingConvention());
builder.Services.AddIdentityApiEndpoints<AppUser>().AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddAuthentication(IdentityConstants.BearerScheme);
builder.Services.AddAuthorization();
builder.Services.Configure<IdentityOptions>(options => { options.User.RequireUniqueEmail = true; });
builder.Services.AddAutoMapper(options => options.AddProfile<AppMappingProfile>());
builder.Services.AddCors();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOcelot();
builder.Services.AddMassTransit(x =>
{
    // A Transport
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:Host"], builder.Configuration["RabbitMQ:VHost"], h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Username"]);
            h.Password(builder.Configuration["RabbitMQ:Password"]);
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddScoped<IEmailSender<AppUser>, LoggingEmailSender>();
builder.Services.AddScoped<IUserCreatedProducer, UserCreatedProducer>();
builder.Services.AddScoped<IProfileProducer, ProfileProducer>();

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
