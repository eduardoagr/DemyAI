namespace DemyAI.Views;

public partial class StartupPage : ContentPage {

    public StartupPage(StartupPageViewModel startupPageViewModel) {
        InitializeComponent();
        BindingContext = startupPageViewModel;
    }
}