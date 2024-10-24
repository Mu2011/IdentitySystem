using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using IdentitySystem.Helpers;

namespace IdentitySystem.ViewModels;

public class RegisterViewModel
{
  [Required]
  [EmailAddress]
  [Display(Name = "Email")]
  [ValidateDomainName("iteshare.com")]//, ErrorMessage: "domain name must be iteshare.com")]
  [Remote("IsEmailExist", "Account")]
  public string Email { get; set; }

  [Required]
  [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.")]
  [DataType(DataType.Password)]
  [Display(Name = "Password")]
  public string Password { get; set; }

  [DataType(DataType.Password)]
  [Display(Name = "Confirm password")]
  [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
  public string ConfirmPassword { get; set; }

  [Required]
  public string Country { get; set; }
}
