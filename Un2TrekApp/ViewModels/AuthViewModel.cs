using CommunityToolkit.Maui.Alerts;
using Newtonsoft.Json;
using Plugin.Fingerprint;
using Plugin.ValidationRules;
using Plugin.ValidationRules.Extensions;
using Un2TrekApp.Helpers;

namespace Un2TrekApp.ViewModels;

public partial class AuthViewModel : BaseViewModel
{
    private readonly IAccountService accountService;
    private readonly ILocalStorage localStorage;
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(OpenFingerprintCommand))]
    [NotifyCanExecuteChangedFor(nameof(DoLoginCommand))]
    [NotifyPropertyChangedFor(nameof(NotIsBusy))]
    bool isBusy = true;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DoLoginCommand))]
    public Validatable<string> userName;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DoLoginCommand))]
    public Validatable<string> userPassword;

    [ObservableProperty]
    public bool isBiometricAccessAllowed;

    public bool NotIsBusy => !IsBusy;
    public AuthViewModel(IAccountService accountService, ILocalStorage localStorage)
    {
        IsBusy = false;
        this.accountService = accountService;
        this.localStorage = localStorage;

        DefineLoginRules();
        MainThread.BeginInvokeOnMainThread(async () => await CheckUserSession());
    }

    [RelayCommand]
    public async Task DoLogin()
    {

        if (Validate())
        {
            IsBusy = true;
            UserCredentials userCredentials = new UserCredentials();
            userCredentials.Email = UserName.Value;
            userCredentials.Password = UserPassword.Value;
            await DoLogin(userCredentials);
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task Register()
    {
        await Shell.Current.GoToAsync($"{nameof(RegisterPage)}");
    }

    [RelayCommand]
    public async Task OpenFingerprint()
    {   
        var request = new Plugin.Fingerprint.Abstractions.AuthenticationRequestConfiguration("Un2Trek", "Iniciar sesión con biometría");
        var result = await CrossFingerprint.Current.AuthenticateAsync(request);
        if (result.Authenticated)
        {
            if (App.UserInfo != null)
            {
                IsBusy = true;
                await DoLogin(new UserCredentials { Email = App.UserInfo.UserName, Password = App.UserInfo.Pass });
                IsBusy = false;
                return;
            }
        }        
    }

    private async Task DoLogin(UserCredentials userCredentials)
    {
        UserInfo result = null;
        try
        {
            result = await accountService.DoLogin(userCredentials);
            if (result == null)
            {
                await Snackbar.Make("No se han podido validar las credenciales.", () => { }, "Aceptar", TimeSpan.FromSeconds(5)).Show();                
            }            
        }
        catch (Exception)
        {
            await Snackbar.Make("Se ha producido un error al conectar con el servidor.", () => { }, "Aceptar", TimeSpan.FromSeconds(5)).Show();
            throw;
        }
        if (result != null)
        {
            result.Pass = userCredentials.Password;
            await PerfomLoginSuccess(result);
        }
    }
    private async Task PerfomLoginSuccess(UserInfo user)
    {
        App.UserInfo = user;
        FlyoutHelper.AddFlyoutMenusDetails();
        await localStorage.SetAsync(App.StorageUserInfoKey, JsonConvert.SerializeObject(user));        
    }
    private bool Validate()
    {
        //TODO: Lo quito para poder hacer las pruebas mas rapido
        return true;
        //return UserName.Validate() && UserPassword.Validate();
    }
    private void DefineLoginRules()
    {
        UserName = Validator.Build<string>()
                            .IsRequired("El nombre de usuario es requerido");

        UserPassword = Validator.Build<string>()
                            .IsRequired("La contraseña de usuario es requerida");
    }
    private async Task CheckUserSession()
    {
        IsBiometricAccessAllowed = false;
        var userData = await SecureStorage.Default.GetAsync(App.StorageUserInfoKey);
        if (userData != null)
        {
            try
            {
                var userSession = JsonConvert.DeserializeObject<UserInfo>(userData);
                UserName.Value = userSession.UserName;
                App.UserInfo = userSession;
                IsBiometricAccessAllowed = true;
                await OpenFingerprint();
            }
            catch (Exception ex)
            {
                //Si no se ha podido deserializar el valor, reiniciamos los datos en el almacenamiento
                SecureStorage.Default.RemoveAll();
                return;
            }
        }
    }
}
