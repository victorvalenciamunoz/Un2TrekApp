namespace Un2TrekApp.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(RegisterViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}