using CommunityToolkit.Maui;
using FinTrack.IViews;
using FinTrack.Mvvm.Views;
using FinTrack.Mvvm.Views.MobileViews;
using FinTrack.Services;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;

namespace FinTrack
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureSyncfusionCore()
                // Initialize the .NET MAUI Community Toolkit by adding the below line of code
                .UseMauiCommunityToolkit()
                // After initializing the .NET MAUI Community Toolkit, optionally add additional fonts
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("fontello.ttf", "Icons");
                })
                .UseViewServices();
          
            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

#if ANDROID || IOS
            builder.Services.AddSingleton<IStartPage, MOverviewPage>();
            builder.Services.AddTransient<ISecondPage, MRecordsPage>();
#else
            builder.Services.AddSingleton<IStartPage, OverviewPage>();
            builder.Services.AddTransient<ISecondPage, RecordsPage>();
#endif

            return builder.Build();
        }
    }
}
