namespace Un2TrekApp.Services;

public interface IAccountService
{
    Task<string> Authenticate(UserCredentials userCredentials);
    Task<AuthenticationResponse> Register(RegisterRequest userData);    
    Task<UserInfo> DoLogin(UserCredentials userCredentials);
    UserInfo SetUserInfoDataFromAuthResponse(string token);
}