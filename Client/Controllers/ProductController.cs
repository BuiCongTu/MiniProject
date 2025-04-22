using System.Net.Http.Headers;
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
            var options = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var products = await client.GetFromJsonAsync<List<ProductViewModel>>("Product", options);
            return View(products ?? new List<ProductViewModel>());
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            if (HttpContext.Session.GetString("Role") != "User")
                return RedirectToAction("Login", "Account");

            if (!AddHeader())
                return RedirectToAction("Login", "Account");

            try
            {
                var response = await client.PostAsJsonAsync($"CartItem", new { ProductId = productId, Quantity = quantity });

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

            try
            {
                var response = await client.GetFromJsonAsync<CartItemViewModel[]>("cart");
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
                var response = await client.PutAsJsonAsync($"cart/{cartItemId}", new { Quantity = quantity });

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
                var response = await client.DeleteAsync($"cart/{cartItemId}");

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
                var response = await client.PostAsync("orders/checkout", null);

                if (response.IsSuccessStatusCode)
                {
                    var order = await response.Content.ReadFromJsonAsync<OrderViewModel>();
                    ViewBag.Message = $"Payment successful, Order Id: {order.Id}, Tổng tiền: {order.TotalAmount}";
                    return View("OrderConfirmation", order);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during checkout: {ex.Message}");
                return BadRequest();
            }
        }
    }
}