using Microsoft.AspNetCore.Identity;

namespace IdentitySystem.Models;

public class AppUser : IdentityUser
{
  public string Country { get; set; }
}