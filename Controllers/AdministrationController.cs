using IdentitySystem.Models;
using Microsoft.AspNetCore.Mvc;
using IdentitySystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;


namespace IdentitySystem.Controllers
{
  [Authorize(Roles = "Admin, Moderators")]
  public class AdministrationController(RoleManager<IdentityRole> roleManager,UserManager<AppUser> userManager) : Controller
  {
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly UserManager<AppUser> _userManager = userManager;

    private async Task<bool> CheckRole(string id)
    {
      var role = await _roleManager.FindByIdAsync(id);
      if(role is null)
      {
        ViewBag.ErrorMessage = $"Role with ID = {id} is not found";
        return false;
      }
      else 
        return true;
    }

    [HttpGet]
    public IActionResult CreateRole() => View();
    [HttpPost]
    public async Task<IActionResult> CreateRole(RoleViewModel model)
    {
      if (ModelState.IsValid)
      {
        var role = new IdentityRole()
        {
          Name = model.RoleName
        };
        var result = await _roleManager.CreateAsync(role);
        if (result.Succeeded)
          return RedirectToAction("index", "Home");
        else
          foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);
      }
      return View(model);
    }

    public IActionResult ListRoles()
    {
      var roles = _roleManager.Roles;
      return View(roles);
    }

    [HttpGet]
    public async Task<IActionResult> EditRole(string id)
    {
      // Find the role by its ID
      var role = await _roleManager.FindByIdAsync(id);
      if (role is null)
      {
        ViewBag.ErrorMessage = $"Role with ID = {id} is not found";
        return View("NotFound");
      }

      // Populate the RoleViewModel with the role's data
      var model = new RoleViewModel()
      {
        Id = role.Id,
        RoleName = role.Name
      };

      // Retrieve all the users from the UserManager ==> model.User
      foreach (var user in _userManager.Users)
        if (await _userManager.IsInRoleAsync(user, role.Name))
          model.Users.Add(user.UserName);

      return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> EditRole(RoleViewModel model)
    {
      if (ModelState.IsValid)
      {
        // Find the role by its ID
        var role = await _roleManager.FindByIdAsync(model.Id);
        // if (role is null)
        // {
        //   ViewBag.ErrorMessage = $"Role with ID = {model.Id} is not found";
        //   return View("NotFound");
        // }

        // Update the role's name
        role.Name = model.RoleName;

        // Update the role in the database
        var result = await _roleManager.UpdateAsync(role);

        if (result.Succeeded)
          return RedirectToAction("ListRoles");
        else
          foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);
      }

      return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> RemoveRole(string id)
    {
      var role = await _roleManager.FindByIdAsync(id);
      if (role is null)
      {
        ViewBag.ErrorMessage = $"Role with ID = {id} is not found";
        return View("NotFound");
      }
      var usersInRoles = await _userManager.GetUsersInRoleAsync(role.Name);
      if(usersInRoles.Count is 0)
      {
        var result = await _roleManager.DeleteAsync(role);

        foreach (var error in result.Errors)
          ModelState.AddModelError("", error.Description);
      }

      return RedirectToAction("ListRoles");
    }

    [HttpGet]
    public async Task<IActionResult> EditUserInRole(string roleId)
    {
      ViewBag.roleId = roleId;
      var role = await _roleManager.FindByIdAsync(roleId);
      if(await CheckRole(roleId))
      {
        var model = new List<UserRoleViewModel>();

        foreach (var user in _userManager.Users)
        {
          var userRoleViewModel = new UserRoleViewModel
          {
            UserID = user.Id,
            UserName = user.UserName
          };
          if(await _userManager.IsInRoleAsync(user, role.Name))
          {
            userRoleViewModel.IsSelected = true;
          }
          else
          {
            userRoleViewModel.IsSelected = false;
          }
          model.Add(userRoleViewModel);
        }

        return View(model);
      }
      else
        return View("NotFound");

    }

    [HttpPost]
    public async Task<IActionResult> EditUserInRole(List<UserRoleViewModel> model, string roleId)
    {
      var role = await _roleManager.FindByIdAsync(roleId);
      if (role is null)
      {
        ViewBag.ErrorMessage = $"Role with ID = {roleId} is not found";
        return View("NotFound");
      }
      for (var item = 0; item < model.Count; item++)
      {
        var user = await _userManager.FindByIdAsync(model[item].UserID);
        IdentityResult result = null;
        if (model[item].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
          result = await _userManager.AddToRoleAsync(user, role.Name);
        else if (!model[item].IsSelected && (await _userManager.IsInRoleAsync(user, role.Name)))
          result = await _userManager.RemoveFromRoleAsync(user, role.Name);
      }

      return RedirectToAction("EditRole", new { id = roleId });
    }
  }
}