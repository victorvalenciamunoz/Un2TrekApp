using CommunityToolkit.Maui.Alerts;
using Plugin.ValidationRules;
using Plugin.ValidationRules.Extensions;

namespace Un2TrekApp.ViewModels;

public partial class RegisterViewModel : BaseViewModel
{
    private readonly IAccountService accountService;

    [ObservableProperty]
    bool isBusy = false;

    [ObservableProperty]
    bool isRegistered = false;

    [ObservableProperty]
    public Validatable<string> email;

    [ObservableProperty]
    public Validatable<string> name;

    [ObservableProperty]
    public Validatable<string> lastName;

    [ObservableProperty]
    public Validatable<string> password;

    public RegisterViewModel(IAccountService accountService)
    {
        this.accountService = accountService;
        DefineRules();
        IsRegistered = false;
        IsBusy = false;
    }

    [RelayCommand]
    public async Task Register()
    {
        if (Validate())
        {
            IsBusy = true;
            RegisterRequest userData = new RegisterRequest
            {
                Email = Email.Value,
                Name = Name.Value,
                LastName = LastName.Value,
                Password = Password.Value
            };
            await Register(userData);
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task BackToLogin()
    {
        await Shell.Current.GoToAsync($"//{nameof(AuthPage)}");
    }

    public async Task Register(RegisterRequest userData)
    {
        AuthenticationResponse result = null;
        try
        {
            result = await accountService.Register(userData);
        }
        catch (Exception)
        {
            await Snackbar.Make("Se ha producido un error al conectar con el servidor.", () => { }, "Aceptar", TimeSpan.FromSeconds(5)).Show();
            throw;
        }
        if (result != null)
        {
            IsRegistered = true;
        }
        else
        {
            await Snackbar.Make("No se ha podido realizar el registro. Inténtelo pasados unos minutos.", () => { }, "Aceptar", TimeSpan.FromSeconds(5)).Show();
        }
    }

    private bool Validate()
    {
        return Email.Validate() && Name.Validate() && LastName.Validate() && Password.Validate();
    }

    private void DefineRules()
    {
        Email = Validator.Build<string>()
                            .IsEmail("Indique una dirección de correo válida")
                            .IsRequired("La dirección de correo es requerida");

        Name = Validator.Build<string>()
                            .IsRequired("El nombre es requerido");

        LastName = Validator.Build<string>()
                            .IsRequired("El apellido es requerido");

        Password = Validator.Build<string>()
                            .IsRequired("La contraseña de usuario es requerida");
    }
}
