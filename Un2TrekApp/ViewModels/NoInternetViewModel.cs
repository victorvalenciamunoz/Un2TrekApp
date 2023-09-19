namespace Un2TrekApp.ViewModels
{
    public partial class NoInternetViewModel : BaseViewModel
    {
        [RelayCommand]
        void CloseApp()
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
