using SkiaSharp.Views.Maui.Controls.Hosting;
using Syncfusion.Maui.Core.Hosting;
using Un2TrekApp.Errors;

namespace Un2TrekApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureSyncfusionCore()
            .UseMauiMaps()
            .UseSkiaSharp()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("materialdesignicons-webfont.ttf", "MaterialDesignIcons");
            })
            .ConfigureEssentials(options =>
             {
                 options.UseVersionTracking();
             }); ;

        builder.Services.AddSingleton<AuthPage>();
        builder.Services.AddSingleton<RegisterPage>();
        builder.Services.AddSingleton<TrekiMapPage>();
        builder.Services.AddTransient<NewsDetailPage>();
        builder.Services.AddSingleton<NewsPage>();
        builder.Services.AddSingleton<ActivityListPage>();

        builder.Services.AddSingleton<AuthViewModel>();
        builder.Services.AddSingleton<RegisterViewModel>();
        builder.Services.AddSingleton<TrekiMapViewModel>();
        builder.Services.AddSingleton<ActivityListViewModel>();
        builder.Services.AddTransient<NewsDetailViewModel>();
        builder.Services.AddSingleton<NewsViewModel>();

        builder.Services.AddTransient<IAccountService, AccountService>();
        builder.Services.AddTransient<ITrekiService, TrekiService>();
        builder.Services.AddTransient<ILocalStorage, LocalStorage>();
        builder.Services.AddTransient<IActivityService, ActivityService>();

        builder.Services.AddSingleton<ErrorManager>();

        builder.ConfigureMauiHandlers(handlers =>
        {
#if ANDROID
            handlers.AddHandler<Microsoft.Maui.Controls.Maps.Map, CustomMapHandler>();
#elif IOS
            handlers.AddHandler<CustomPin, CustomPinHandler>();
#endif
        });

        return builder.Build();
    }
}
