﻿global using CommunityToolkit.Maui;
global using CommunityToolkit.Maui.Core;
global using CommunityToolkit.Mvvm.ComponentModel;
global using CommunityToolkit.Mvvm.Input;

global using DemyAI.Interfaces;
global using DemyAI.Models;
global using DemyAI.Services;
global using DemyAI.ViewModels;
global using DemyAI.Views;

global using Firebase.Database;

global using Microsoft.Extensions.Logging;

global using Syncfusion.Maui.Core.Hosting;

global using System.Text.Json;

namespace DemyAI {
    public static class MauiProgram {
        public static MauiApp CreateMauiApp() {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts => {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<IAppService, AppService>();
            builder.Services.AddTransient(typeof(IDataService<>), typeof(DataService<>));
            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddSingleton<AppShellViewModel>();
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddTransient<LoginPageViewModel>();
            builder.Services.AddSingleton<HomePage>();
            builder.Services.AddTransient<HomePageViewModel>();

            
            return builder.Build();
        }
    }
}