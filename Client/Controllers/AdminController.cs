using System.Net.Http.Headers;
using Client.Models;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class AdminController : BaseController
    {
        public AdminController(IConfiguration configuration, HttpClient client)
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
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Login", "Account");
            if (!AddHeader())
                return RedirectToAction("Login", "Account");
            try
            {
                var products = await client.GetFromJsonAsync<List<ProductViewModel>>("Product");
                return View(products ?? new List<ProductViewModel>());
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Cannot fetch products.";
                return View(new List<ProductViewModel>());
            }
        }

        public IActionResult CreateProduct()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Login", "Account");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductViewModel model)
        {
            AddHeader();
            var response = await client.PostAsJsonAsync("Product", model);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Products");
            return View(model);
        }

        public IActionResult DeleteProduct()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Login", "Account");
            return View();
        }

        // [HttpDelete]
        // public async Task<IActionResult> DeleteProduct(ProductViewModel model)
        // {
        //     
        // }
    }
}