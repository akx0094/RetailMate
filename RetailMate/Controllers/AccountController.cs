using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies; // Required for CookieAuthenticationDefaults
using System.Threading.Tasks;
using System.Collections.Generic; // Required for List<Claim>
using System; // Required for DateTime

namespace RetailMate.Controllers // Adjust namespace to match your project
{
    public class AccountController : Controller
    {
        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            // Clear ModelState on GET request to ensure a clean form
            ModelState.Clear();
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken] // Important for security
        public async Task<IActionResult> Login(string username, string password, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            // --- MOCK AUTHENTICATION LOGIC ---
            // In a real application, you would validate credentials against a database
            // using ASP.NET Core Identity or a custom membership system.
            if (username == "admin" && password == "password") // Example credentials
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "Administrator") // Example role
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, // Keep the user logged in across browser sessions
                    IssuedUtc = DateTime.UtcNow,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30) // Session expires in 30 minutes
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                // Redirect to the original URL or the Invoice Create page
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Create", "Invoice"); // Redirect to Invoice Generator
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(); // Return to login page with error
            }
        }

        // GET: /Account/Logout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // Redirect to the home page of your static website, which is not protected
            return RedirectToAction("Index", "Home"); // Assuming HomeController Index is public or serves static files
        }
    }
}
