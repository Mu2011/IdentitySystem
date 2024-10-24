using System.ComponentModel.DataAnnotations;

namespace IdentitySystem.ViewModels;

public class RoleViewModel
{
  public string? Id { get; set; }

  [Required]
  [Display(Name = "Role")]
  public string RoleName { get; set; }

  // [Display(Name = "Users")]
  public List<string> Users { get; set; } = new List<string>();
}