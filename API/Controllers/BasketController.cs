using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly StoreContext _context;
        public BasketController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetBasket")]
        public async Task<ActionResult<BasketDto>> GetBasket()
        {
            var basket = await RetriveBasket();

            if (basket == null) return NotFound();

            return MapBasketToDto(basket);
        }



        [HttpPost]
        public async Task<ActionResult<BasketDto>> AddItemToBasket(int productId, int quantity)
        {
            // GetBasket --> If Null Create Basket
            var basket = await RetriveBasket();
            if (basket == null) basket = CreateBasket();
            // Get Product By ProductId
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return NotFound();
            // Add Item
            basket.AddItem(product, quantity);
            // Save Changes
            var result = await _context.SaveChangesAsync() > 0;

            if (result) return CreatedAtRoute("GetBasket", MapBasketToDto(basket));

            return BadRequest(new ProblemDetails
            {
                Title = "Problem saving item to basket"
            });
        }



        [HttpDelete]
        public async Task<ActionResult> DeleteBasket(int productId, int quantity)
        {
            // GetBasket 
            var basket = await RetriveBasket();
            if (basket == null) return NotFound();
            // Remove Item
            basket.RemoveItem(productId, quantity);
            // Save Changes
            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails
            {
                Title = "Problem delete item to basket"
            });
        }

        private async Task<Basket> RetriveBasket()
        {
            return await _context.Baskets
                .Include(i => i.Items)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(x => x.BuyerId == Request.Cookies["buyerId"]);
        }

        private Basket CreateBasket()
        {
            var buyerId = Guid.NewGuid().ToString();
            var cookieOption = new CookieOptions
            {
                IsEssential = true,
                Expires = DateTimeOffset.Now.AddDays(30)
            };
            Response.Cookies.Append("buyerId", buyerId, cookieOption);
            var basket = new Basket { BuyerId = buyerId };

            _context.Baskets.Add(basket);

            return basket;
        }

        private BasketDto MapBasketToDto(Basket basket)
        {
            return new BasketDto
            {
                Id = basket.Id,
                BuyerId = basket.BuyerId,
                Items = basket.Items.Select(item => new BasketItemDto
                {
                    ProductId = item.ProductId,
                    Name = item.Product.Name,
                    Price = item.Product.Price,
                    Brand = item.Product.Brand,
                    PictureUrl = item.Product.PictureUrl,
                    Type = item.Product.Type,
                    Quantity = item.Quantity
                }).ToList()
            };
        }
    }
}