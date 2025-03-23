namespace LowlandTech.Vylyr;

public static class MauiProgram
{
    public static async Task<MauiApp> CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder
            .Services
            .AddDbContext<GraphContext>(options =>
            {
                options.UseInMemoryDatabase("VylyrGraph");
            });

        // Add device-specific services used by the LowlandTech.Vylyr.Core project
        builder.Services.AddSingleton<IFormFactor, FormFactor>();
        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddMudServices();
        builder.Services.AddSingleton<IFramework,Framework>();
        builder.Services.AddSingleton<AppVm>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();

        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<GraphContext>();
        await db.Database.EnsureCreatedAsync();
        await db.UseCaseData();

        return app;
    }
}
