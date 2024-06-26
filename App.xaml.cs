﻿using Syncfusion.Licensing;

namespace DemyAI;
public partial class App : Application {

    public App(IConnectivity connectivity, AppShell shell, NoInternetPage noInternetPage) {

        SyncfusionLicenseProvider.RegisterLicense(Constants.LICENSE);

        InitializeComponent();

        if (connectivity.NetworkAccess is not NetworkAccess.Internet) {
            MainPage = noInternetPage;
        } else {
            MainPage = shell;
        }

        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("BorderlessEntry", (handler, view) => {
            if (view is BorderlessEntry) {
#if ANDROID
                handler.PlatformView.Background = null;
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);

#elif WINDOWS
                handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
#elif IOS || MACCATALYST
                handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
                handler.PlatformView.Layer.BorderWidth = 0;
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;

#endif
            }
        });

        Microsoft.Maui.Handlers.SearchBarHandler.Mapper.AppendToMapping("BorderlessSearchBar", (handler, view) => {
            if (view is BorderlessSearchBar) {
#if ANDROID
                Android.Widget.LinearLayout? linearLayout = handler.PlatformView.GetChildAt(0) as Android.Widget.LinearLayout;
                linearLayout = linearLayout?.GetChildAt(2) as Android.Widget.LinearLayout;
                linearLayout = linearLayout?.GetChildAt(1) as Android.Widget.LinearLayout;
                linearLayout!.Background = null;

#elif WINDOWS
                handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
#elif IOS || MACCATALYST

                handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
                handler.PlatformView.SearchBarStyle = UIKit.UISearchBarStyle.Minimal;
#endif
            }
        });
    }
}
