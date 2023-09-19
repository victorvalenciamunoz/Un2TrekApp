using System.ComponentModel.DataAnnotations;

namespace Un2TrekApp.Models;

public class UserCredentials
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }


    [Required]
    [MinLength(5)]
    public string Password { get; set; }
}
