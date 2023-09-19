namespace Un2TrekApp.ViewModels;

[QueryProperty(nameof(Item), "Item")]
public partial class NewsDetailViewModel : BaseViewModel
{
	[ObservableProperty]
	SampleItem item;
}
