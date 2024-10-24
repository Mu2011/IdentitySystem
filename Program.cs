using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using IdentitySystem.Data;
using IdentitySystem.Models;

var builder = WebApplication.CreateBuilder(args);
  builder.Services.AddDbContext<IdentitySystemContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("IdentitySystemContext") ?? throw new InvalidOperationException("Connection string 'IdentitySystemContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure cookie policy options
builder.Services.Configure<CookiePolicyOptions>(options =>
{
  options.CheckConsentNeeded = context => true;
  options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
  options.Password.RequireDigit = false; 
  options.Password.RequireUppercase = false;
  options.Password.RequireNonAlphanumeric = false;
  options.Password.RequireLowercase = false; 
}).AddEntityFrameworkStores<IdentitySystemContext>();

builder.Services.AddMvc(config => 
{
  var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
  config.Filters.Add(new AuthorizeFilter(policy));
})/*.SetCompatibilityVersion(CompatibilityVersion.Version_2_2)*/;

// Add authorization policy
// builder.Services.AddAuthorization(options =>
// {
//   options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
// });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
  name: "default",
  pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();