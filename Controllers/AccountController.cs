using Microsoft.AspNetCore.Authorization;
using IdentitySystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IdentitySystem.Models;

namespace IdentitySystem.Controllers
{
  // [Route("Account")]
  [AllowAnonymous]
  public class AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ILogger<AccountController> logger) : Controller
  {
    private readonly SignInManager<AppUser> _signInManager = signInManager;
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly ILogger<AccountController> _logger = logger;

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel userModel)
    {
      if (ModelState.IsValid)
      {
        // 1: Copy Data from RegisterViewModel to AppUser
        var user = new AppUser
        {
          UserName = userModel.Email,
          Email = userModel.Email,
          Country = userModel.Country
        };
        // 2: store the user in DB : UserManager class
        var result = await _userManager.CreateAsync(user, userModel.Password);

        // 3: Process ? Succeed Or Fail
        if (result.Succeeded)
        {
          // 3: Sign In the User
          _logger.LogInformation("User created a new account with password.");
          await _signInManager.SignInAsync(user, isPersistent: false);
          return RedirectToAction("Index", "Home");
        }
        // 4: on case of any error in registrations
        foreach (var error in result.Errors)
          ModelState.AddModelError(string.Empty, error.Description);
      }
      return View(userModel);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
      await _signInManager.SignOutAsync();
      return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginModel, string returnUrl = null)
    { 
      if (ModelState.IsValid)
      {
        var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, loginModel.RememberMe, lockoutOnFailure: false);

        if (result.Succeeded)
          return !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl) ?
                  Redirect(returnUrl): RedirectToAction("Index", "Home");
        else
          ModelState.AddModelError(string.Empty, "Invalid login attempt.");
      }
      return View(loginModel);
    }
    
    [AcceptVerbs("POST", "GET")]
    public async Task<IActionResult>IsEmailExist(string email)
    {
      var user = await _userManager.FindByEmailAsync(email);
      return Json(user is null ? true : $"The email {email} is already in use"); // Return true if email does not exist, false otherwise
      // return Json(user is null);
    }
  }
}