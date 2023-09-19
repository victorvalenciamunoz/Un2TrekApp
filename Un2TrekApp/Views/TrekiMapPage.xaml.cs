namespace Un2TrekApp.Views;


public partial class TrekiMapPage : ContentPage
{   
    private TrekiMapViewModel viewModel;
    bool isAsking = false;
    public TrekiMapPage(TrekiMapViewModel viewModel)
    {
        InitializeComponent();
        
        BindingContext = viewModel;
        this.viewModel = viewModel;
#if WINDOWS
		// Note that the map control is not supported on Windows.
		// For more details, see https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/map?view=net-maui-7.0
		// For a possible workaround, see https://github.com/CommunityToolkit/Maui/issues/605
		Content = new Label() { Text = "Windows does not have a map control. 😢" };
#endif
    }
    protected async override void OnAppearing()
    {
        if (!isAsking)
        {
            viewModel.Map = this.MapView;
            await viewModel.Init();
            isAsking = false;
        }
        base.OnAppearing();        
    }

    private void MapView_MapClicked(object sender, Microsoft.Maui.Controls.Maps.MapClickedEventArgs e)
    {
        if (e != null)
        {
            viewModel.NewLocationSelected(e.Location);
        }
    }
}
