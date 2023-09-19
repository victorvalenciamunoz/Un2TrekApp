namespace Un2TrekApp.Views;

public partial class NoInternetView : ContentPage
{
	public NoInternetView()
	{
		InitializeComponent();
        Shell.SetNavBarIsVisible(this, false);
        this.BindingContext = new NoInternetViewModel();
    }
}