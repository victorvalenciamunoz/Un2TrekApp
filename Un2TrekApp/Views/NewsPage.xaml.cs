namespace Un2TrekApp.Views;

public partial class NewsPage : ContentPage
{
	NewsViewModel ViewModel;

	public NewsPage(NewsViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = ViewModel = viewModel;
	}

	protected override async void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);

		await ViewModel.LoadDataAsync();
	}
}
