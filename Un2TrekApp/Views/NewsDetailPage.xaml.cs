namespace Un2TrekApp.Views;

public partial class NewsDetailPage : ContentPage
{
	public NewsDetailPage(NewsDetailViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
