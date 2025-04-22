using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using Client.Models;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class ProductController : BaseController
    {
        public ProductController(IConfiguration configuration, HttpClient client) 
            : base(configuration, client)
        {
        }

        [NonAction]
        private bool AddHeader()
        {
            var token = HttpContext.Session.GetString("AuthToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = null;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return true;
            }
            return false;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var options = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var products = await client.GetFromJsonAsync<List<ProductViewModel>>("Product", options);
                return View(products ?? new List<ProductViewModel>());
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error fetching products: {ex.Message}");
                return View(new List<ProductViewModel>());
            }
            catch (System.Text.Json.JsonException jsonEx)
            {
                Console.WriteLine($"Error deserializing products: {jsonEx.Message}");
                return View(new List<ProductViewModel>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            if (HttpContext.Session.GetString("Role") != "User")
                return RedirectToAction("Login", "Account");
        
            if (!AddHeader())
                return RedirectToAction("Login", "Account");
        
            //get user id from token
            var token = HttpContext.Session.GetString("AuthToken");
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
        
            try
            {
                var response = await client.PostAsJsonAsync("Cart", new { ProductId = productId, Quantity = quantity, UserId = userId });
        
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Cart");
                return BadRequest();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding to cart: {ex.Message}");
                return BadRequest();
            }
        }

        public async Task<IActionResult> Cart()
        {
            if (HttpContext.Session.GetString("Role") != "User")
                return RedirectToAction("Login", "Account");

            if (!AddHeader())
                return RedirectToAction("Login", "Account");

            //get user id from token
            var token = HttpContext.Session.GetString("AuthToken");
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

            try
            {
                var response = await client.GetFromJsonAsync<CartItemViewModel[]>("cart/user/" + userId);
                return View(response ?? new CartItemViewModel[0]);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching cart: {ex.Message}");
                return View(new CartItemViewModel[0]);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCart(int cartItemId, int quantity)
        {
            if (HttpContext.Session.GetString("Role") != "User")
                return RedirectToAction("Login", "Account");

            if (!AddHeader())
                return RedirectToAction("Login", "Account");

            try
            {
                var response = await client.PutAsJsonAsync("cart/" + cartItemId, new { Id = cartItemId, Quantity = quantity });

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Cart");
                return BadRequest();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating cart: {ex.Message}");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCartItem(int cartItemId)
        {
            if (HttpContext.Session.GetString("Role") != "User")
                return RedirectToAction("Login", "Account");

            if (!AddHeader())
                return RedirectToAction("Login", "Account");

            try
            {
                var response = await client.DeleteAsync("cart/" + cartItemId);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Cart");
                return BadRequest();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting cart item: {ex.Message}");
                return BadRequest();
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            if (HttpContext.Session.GetString("Role") != "User")
                return RedirectToAction("Login", "Account");

            if (!AddHeader())
                return RedirectToAction("Login", "Account");

            try
            {
                var token = HttpContext.Session.GetString("AuthToken");
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var userId = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
                var response = await client.PostAsync($"Cart/checkout/" + userId, null);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Checkout successful!";
                    return RedirectToAction("Cart");
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking out: {ex.Message}");
                return BadRequest();
            }
        }

    }
}