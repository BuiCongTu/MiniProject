using Client.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace Client.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IConfiguration configuration, HttpClient client) : base(configuration, client)
        {
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                var result = await client.PostAsJsonAsync("auth/login/", model);
                if (result.IsSuccessStatusCode)
                {
                    var response = JObject.Parse(await result.Content.ReadAsStringAsync());
                    var token = response["token"]?.ToString();
                    if (!string.IsNullOrEmpty(token))
                    {
                        // Lưu token vào Session
                        HttpContext.Session.SetString("AuthToken", token);
                        var role = response["user"]?["role"]?.ToString();
                        var username = model.Username; // Use username from the model

                        // Lưu role vào session
                        HttpContext.Session.SetString("Role", role ?? "User");

                        // Set up claims for cookie authentication
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, username),
                            new Claim(ClaimTypes.Role, role ?? "User")
                        };
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = true, // Persist cookie across sessions
                            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1) // Adjust as needed
                        };

                        // Sign in the user
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity), authProperties);

                        // Redirect based on role
                        if (role == "Admin")
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Product");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid response from server");
                    }
                }
                else if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ModelState.AddModelError("", "401 Invalid username or password");
                }
                else
                {
                    ModelState.AddModelError("", $"Login failed with status code {result.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError("", $"Network error: {ex.Message}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            // Clear session
            HttpContext.Session.Clear();
            // Sign out from cookie authentication
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Product");
        }
    }

    // DTO
    public class LoginResponseViewModel
    {
        public string Token { get; set; }
        public LoginViewModel? User { get; set; }
    }

    public class LoginViewModel
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}