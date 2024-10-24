namespace IdentitySystem.ViewModels;

public class UserRoleViewModel
{
  public string UserID { get; set; }
  public string UserName { get; set; }
  // public string RoleID { get; set; } // ViewBag is responsible for storing the role information in the view
  public bool IsSelected { get; set; } 
}