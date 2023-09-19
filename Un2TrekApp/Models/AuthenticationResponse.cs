namespace Un2TrekApp.Models;

public class AuthenticationResponse
{
    public string JwtSecurityToken { get; set; }
    public DateTime ExpirationDate { get; set; }
}
