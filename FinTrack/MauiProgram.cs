using CommunityToolkit.Maui;
using FinTrack.Helper;
using FinTrack.IViews;
using FinTrack.Mvvm.ViewModels;
using FinTrack.Mvvm.Views;
using FinTrack.Mvvm.Views.MobileViews;
using FinTrack.Services;
using FinTrack.Services.IServices;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Syncfusion.Maui.Core.Hosting;
using System.Diagnostics;

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
            //builder.Services.AddHttpClient();
            builder.Configuration.AddConfiguration(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build());
            var baseApiUrl = builder.Configuration.GetValue<string>("BaseAPIUrl");
            builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(baseApiUrl) });


            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddAuthorizationCore();
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

            return builder
            .RegisterAppServices(baseApiUrl)
            .RegisterViewModels()
            .RegisterViews()
            .Build();
        }
        public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder builder, string baseApiUrl)
        {
            builder.Services.AddScoped<IBudgetApiService, BudgetApiService>();
            builder.Services.AddScoped<IGoalApiService, GoalApiService>();
            builder.Services.AddScoped<IRecordApiService, RecordApiService>();
            builder.Services.AddScoped<ITransactionApiService, TransactionApiService>();

            builder.Services.AddSingleton<IAccountService, AccountService>();
            builder.Services.AddSingleton<AuthenticationStateProvider, AuthStateProvider>();
            builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
            builder.Services.AddSingleton<IPreferences>(sp => Preferences.Default);
            builder.Services.AddSingleton<IMenuHandler, MenuHandler>();
            return builder;
        }
        public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<OverviewViewModel>();
            builder.Services.AddSingleton<BudgetsViewModel>();
            builder.Services.AddSingleton<GoalsViewModel>();
            builder.Services.AddSingleton<RecordsViewModel>();
            builder.Services.AddSingleton<TransactionsViewModel>();
            return builder;
        }

        public static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
        { 
            builder.Services.AddSingleton<OverviewPage>();


            builder.Services.AddTransient<BudgetsPage>();
            builder.Services.AddTransient<GoalsPage>();
            builder.Services.AddTransient<RecordsPage>();
            builder.Services.AddTransient<TransactionsPage>();
            //builder.Services.AddTransient<SettingsPage>();
            //builder.Services.AddTransient<AccountsPage>();
            return builder;
        }
    }

 }
