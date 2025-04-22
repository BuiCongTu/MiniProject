// using Client.Models;
// using Microsoft.AspNetCore.Mvc;
// using Newtonsoft.Json.Linq;
//
// namespace Client.Controllers
// {
//     public class ReAccountController : BaseController
//     {
//
//         public ReAccountController(IConfiguration configuration, HttpClient client) : base(configuration, client)
//         {
//         }
//
//         public IActionResult Login()
//         {
//             return View();
//         }
//
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Login(LoginViewModel model)
//         {
//             if (!ModelState.IsValid)
//             {
//                 return View();
//             }
//
//             try
//             {
//                 var result = await client.PostAsJsonAsync("auth/login/", model);
//                 if (result.IsSuccessStatusCode)
//                 {
//                     var response = JObject.Parse(result.Content.ReadAsStringAsync().Result);
//                     var token = response["token"].ToString();
//                     if (token != null)
//                     {
//                         // Lưu token vào Session
//                         HttpContext.Session.SetString("AuthToken", token);
//                         if (response["user"]["role"]?.ToString() == "Admin")
//                         {                        
//                             HttpContext.Session.SetString("Role", "Admin");
//                             return RedirectToAction("Index", "Admin");
//                         }
//                         else
//                         {
//                             HttpContext.Session.SetString("Role", "User");
//                             return RedirectToAction("Index", "Product");
//                         }
//                     }
//                     else
//                     {
//                         ModelState.AddModelError("", "Invalid response from server");
//                     }
//                 }
//                 else if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
//                 {
//                     ModelState.AddModelError("", "401 Invalid username or password");
//                 }
//                 else
//                 {
//                     ModelState.AddModelError("", $"Login failed with status code {result.StatusCode}");
//                 }
//             }
//             catch (HttpRequestException ex)
//             {
//                 ModelState.AddModelError("", $"Network error: {ex.Message}");
//             }
//             catch (Exception ex)
//             {
//                 ModelState.AddModelError("", $"An error occurred: {ex.Message}");
//             }
//             return View();
//         }
//
//         public IActionResult Logout()
//         {
//             HttpContext.Session.Clear();
//             return RedirectToAction("Index", "Product");
//         }
//     }
//     //DTO
//     public class LoginResponseViewModel
//     {
//         public string Token { get; set; }
//         public LoginViewModel? User { get; set; }
//     }
//
//     public class LoginViewModel
//     {
//         public string? Username { get; set; }
//         public string? Password { get; set; }
//     }
// }