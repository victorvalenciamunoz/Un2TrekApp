using CommunityToolkit.Maui.Views;

namespace Un2TrekApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(AuthPage), typeof(AuthPage));
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        Routing.RegisterRoute(nameof(ActivityListPage), typeof(ActivityListPage));
        Routing.RegisterRoute(nameof(TrekiMapPage), typeof(TrekiMapPage));

        Routing.RegisterRoute(nameof(NoInternetView), typeof(NoInternetView));

        lblVersion.Text = VersionTracking.Default.CurrentVersion.ToString();
        lblBuild.Text = VersionTracking.Default.CurrentBuild.ToString();
    }
    private async void ButtonIconText_ExitClicked(Controls.ButtonIconClickedEventArgs args)
    {
        var popupResult = await App.Current.MainPage.ShowPopupAsync(new ConfirmationPopup("¿Desea salir de la aplicación?"));
        if (popupResult != null && popupResult is bool result)
        {
            if (result)
            {
                System.Environment.Exit(0);
            }
        }
    }
}
