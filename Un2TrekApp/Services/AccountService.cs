using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Text;

namespace Un2TrekApp.Services;

public abstract class TokenServiceBase
{
    protected readonly ILocalStorage localStorage;
    protected TokenServiceBase(ILocalStorage localStorage)
    {
        this.localStorage = localStorage;
    }    
    private async Task<AuthenticationResponse> GetTokenFromLocalStorage()
    {
        string serializedUserInfo = await localStorage.GetAsync(App.StorageUserInfoKey);
        var userInfo = JsonConvert.DeserializeObject<UserInfo>(serializedUserInfo);
        if (userInfo != null)
        {
            return userInfo.AuthInfo;
        }

        return null;
    }
    public async Task<string> GetTokenForCall()
    {
        var currentToken = await this.GetTokenFromLocalStorage();
        if (IsAboutExpire(currentToken))
        {
            var renewedToken = await RenewToken();
            if (renewedToken!=null)
            {
                await SetTokenToLocalStorage(renewedToken);
                return renewedToken.JwtSecurityToken;
            }
        }
        return currentToken.JwtSecurityToken;

    }
    private bool IsAboutExpire(AuthenticationResponse token)
    {
        var dateToExpire = token.ExpirationDate;
        var currentDatetime = DateTime.UtcNow;
        var diffOfDates = dateToExpire - currentDatetime;
        
        return (diffOfDates.Minutes < 10);
    }

    private async Task SetTokenToLocalStorage(AuthenticationResponse token)
    {
        string serializedUserInfo = await localStorage.GetAsync(App.StorageUserInfoKey);
        var userInfo = JsonConvert.DeserializeObject<UserInfo>(serializedUserInfo);
        
        userInfo.AuthInfo = token;        
        await localStorage.SetAsync(App.StorageUserInfoKey, JsonConvert.SerializeObject(userInfo));
    }
    
    public JwtSecurityToken DecodedTokenValue(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var decodedValue = handler.ReadJwtToken(token);

        return decodedValue;
    }
    
    public AuthenticationResponse AutenticationResponseFromToken(string token)
    {
        AuthenticationResponse result = new();        
        var decodedValue = DecodedTokenValue(token);    
        result.JwtSecurityToken = token;
        result.ExpirationDate = decodedValue.ValidTo;

        return result;

    }
    private async Task<AuthenticationResponse> RenewToken()
    {
        HttpClient httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri($"{App.UrlBase}Authentication/");

        var token = await this.GetTokenFromLocalStorage();
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.JwtSecurityToken);

        StringBuilder request = new StringBuilder("renew");

        using (HttpResponseMessage response = await httpClient.GetAsync(request.ToString()))
        {
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();                
                return AutenticationResponseFromToken(json);
            }
        }

        return null;
    }
}

public class AccountService : TokenServiceBase, IAccountService
{
    public AccountService(ILocalStorage localStorage) : base(localStorage)
    {

    }
    public async Task<string> Authenticate(UserCredentials userCredentials)
    {
        userCredentials.Email = userCredentials.Email;
        userCredentials.Password = userCredentials.Password;

        HttpClient httpClient = new HttpClient();
        var response = await httpClient.PostAsJsonAsync($"{App.UrlBase}Authentication/login", userCredentials);

        if (response.IsSuccessStatusCode)
        {
            var token = await response.Content.ReadAsStringAsync();
            return token;
        }        
        return string.Empty;
    }

    public async Task<AuthenticationResponse> Register(RegisterRequest userData)
    {
        HttpClient httpClient = new HttpClient();
        var response = await httpClient.PostAsJsonAsync($"{App.UrlBase}Authentication/register", userData);

        if (response.IsSuccessStatusCode)
        {
            var authContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AuthenticationResponse>(authContent);
        }

        return null;
    }

    public UserInfo SetUserInfoDataFromAuthResponse(string token)
    {
        var result = new UserInfo();                
        var handler = new JwtSecurityTokenHandler();
        var decodedValue = DecodedTokenValue(token);

        foreach(var keyValuePair in decodedValue.Payload)
        {
            if (keyValuePair.Key.Contains("userName"))
            {
                result.Email = keyValuePair.Value.ToString();
                result.UserName = keyValuePair.Value.ToString();
            }
            if (keyValuePair.Key.Contains("sub"))
            {
                result.UserId = keyValuePair.Value.ToString();
            }
            if (keyValuePair.Key.Contains("fullName"))
            {
                result.FullName = keyValuePair.Value.ToString();
            }
            if (keyValuePair.Key.Contains("userRoles"))
            {
                result.UserRoles = keyValuePair.Value.ToString();
            }           
        }
        
        result.AuthInfo = new AuthenticationResponse();
        result.AuthInfo.JwtSecurityToken = token;
        result.AuthInfo.ExpirationDate = decodedValue.ValidTo;
        
        return result;
    }

    public async Task<UserInfo> DoLogin(UserCredentials userCredentials)
    {
        var authResponse = await Authenticate(userCredentials);
        if (string.IsNullOrEmpty(authResponse))
        {
            return null;            
        }
        
        return SetUserInfoDataFromAuthResponse(authResponse);
    }
}
