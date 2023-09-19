namespace Un2TrekApp.Models;

public class UserInfo
{
    public string UserId { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Pass { get; set; }
    public string UserRoles { get; set; }
    public string FullName { get; set; }
    public AuthenticationResponse AuthInfo { get; set; }
}
