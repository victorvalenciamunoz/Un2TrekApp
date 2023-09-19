namespace Un2TrekApp.Views;

public partial class NoLocationPermissionView : ContentPage
{
	public NoLocationPermissionView()
	{
		InitializeComponent();
        Shell.SetNavBarIsVisible(this, false);
        this.BindingContext = new NoInternetViewModel();
    }
}