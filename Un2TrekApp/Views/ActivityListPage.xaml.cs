namespace Un2TrekApp.Views;

public partial class ActivityListPage : ContentPage
{
	ActivityListViewModel ViewModel;
	public ActivityListPage(ActivityListViewModel viewModel)
	{
		InitializeComponent();

        BindingContext = ViewModel = viewModel;
    }
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
	{
        base.OnNavigatedTo(args);

        await ViewModel.GetActivityList();
    }
}