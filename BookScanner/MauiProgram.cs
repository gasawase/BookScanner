using BookScanner.Services;
using Camera.MAUI;
using Microsoft.Extensions.Logging;

namespace BookScanner
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            SQLitePCL.Batteries_V2.Init();

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCameraView()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton(sp =>
            {
                var databaseService = new DatabaseService();
                // Work around table not properly creating due to async issues
                Task.Run(async () => await databaseService.InitializeDatabaseAsync()).Wait();
                return databaseService;
            });
            builder.Services.AddTransient<MainPage>();

            return builder.Build();
        }
    }
}
