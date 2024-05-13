namespace Purchases.Extensions;

public static class CorsConfigurationExtension
{
    public static IApplicationBuilder UseCorsConfigured(this IApplicationBuilder app)
    {
        return app.UseCors(
            b => b.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
    }
}
