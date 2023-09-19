namespace Un2TrekApp.ViewModels;

public partial class ActivityListViewModel : BaseViewModel
{

    [ObservableProperty]
    public ObservableCollection<Actividad> activityList;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(NotIsBusy))]
    bool isBusy = false;
    private readonly IActivityService activityService;

    public bool NotIsBusy => !IsBusy;
    public ActivityListViewModel(IActivityService activityService)
    {
        this.activityService = activityService;
    }

    [RelayCommand]
    private async void GoToMap(Actividad activity)
    {
        await Shell.Current.GoToAsync(nameof(TrekiMapPage), true, new Dictionary<string, object>
        {
            { "Activity", activity }
        });
    }

    public async Task GetActivityList()
    {
        if (IsBusy) return;
        IsBusy = true;

        if (ActivityList != null && ActivityList.Any())
            ActivityList.Clear();

        try
        {
            ActivityList = new ObservableCollection<Actividad>(await activityService.GetActiveActivityList());

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get ACTIVITY LIST: {ex.Message}");
            await Shell.Current.DisplayAlert("Error", $"Unable to get ACTIVITY LIST {ex.Message}", "Ok");
        }

        IsBusy = false;
    }


}
