using Microsoft.AspNetCore.Authentication.Cookies; // Required for CookieAuthenticationDefaults
using System; // Required for TimeSpan
using Microsoft.AspNetCore.Builder; // Required for DefaultFilesOptions
using System.Collections.Generic; // Required for List<string>

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure Authentication services
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Path to your login page
        options.LogoutPath = "/Account/Logout"; // Path to your logout page
        options.AccessDeniedPath = "/Account/AccessDenied"; // Optional: for unauthorized access
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Set cookie expiration
        options.SlidingExpiration = true; // Renew cookie if half of its lifetime has passed
    });

// Add Distributed Memory Cache for session state (TempData uses session by default)
builder.Services.AddDistributedMemoryCache();

// Add Session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session idle timeout
    options.Cookie.HttpOnly = true; // Make the session cookie HTTP-only
    options.Cookie.IsEssential = true; // Make the session cookie essential for the app to function
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS

// IMPORTANT: Configure DefaultFiles to serve home.html as the default document
// This must come BEFORE UseStaticFiles()
app.UseDefaultFiles(new DefaultFilesOptions
{
    DefaultFileNames = new List<string> { "index.html", "home.html" } // Add home.html to the list of default files
});
app.UseStaticFiles(); // Enable serving static files (CSS, JS, images) from wwwroot

app.UseRouting(); // Enable routing

// IMPORTANT: Authentication middleware MUST come BEFORE Authorization middleware
app.UseAuthentication(); // Enables authentication features
app.UseAuthorization();  // Enables authorization features (e.g., [Authorize] attribute)

app.UseSession(); // Enable session middleware (MUST be after UseRouting and before MapControllerRoute)

// This default route will now primarily handle MVC controller actions
// that are not covered by static files (like /Account/Login or /Invoice/Create)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // Default to a Home controller if no static file is found

app.Run();
