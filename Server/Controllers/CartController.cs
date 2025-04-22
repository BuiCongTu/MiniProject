using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs;
using Server.Services;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;

        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<CartDTO>>> GetCartByUser(string userId)
        {
            var cartItems = await cartService.GetCartByUser(userId);
            return Ok(cartItems);
        }

        [HttpPost]
        public async Task<ActionResult<CartDTO>> AddToCart(CartDTO cartDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var addedItem = await cartService.AddToCart(cartDto);
            return CreatedAtAction(nameof(GetCartByUser), new { userId = addedItem.UserId }, addedItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCartItem(int id, CartDTO cartDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != cartDto.Id)
            {
                return BadRequest("ID mismatch");
            }
            var updatedItem = await cartService.UpdateCart(cartDto);
            if (updatedItem == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveCartItem(int id)
        {
            var result = await cartService.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
        
        //payment
        [HttpPost("checkout/{userId}")]
        public async Task<IActionResult> Checkout(string userId)
        {
            await cartService.Payment(userId);
            return NoContent();
        }
    }
}