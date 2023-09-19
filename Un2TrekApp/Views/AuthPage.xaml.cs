using CommunityToolkit.Maui.Views;

namespace Un2TrekApp.Views;

public partial class AuthPage : ContentPage
{
	public AuthPage(AuthViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
    protected async override void OnAppearing()
	{
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await Shell.Current.GoToAsync(nameof(NoInternetView));
            return;
        }
        var permissions = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
        if (permissions != PermissionStatus.Granted)
        {            
            permissions = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            if (permissions != PermissionStatus.Granted)            
            {                
                await Shell.Current.GoToAsync(nameof(NoLocationPermissionView));
                return;
            }
        }
        base.OnAppearing();
    }    
}
