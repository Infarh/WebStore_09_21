using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers.API
{
    [ApiController, Route("api/products")]
    public class ProductsApiController : ControllerBase
    {
        private readonly IProductData _ProductData;

        public ProductsApiController(IProductData ProductData) => _ProductData = ProductData;

        private class ProductInfo
        {
            public int Id { get; set; }

            public string Title { get; set; }

            public decimal Price { get; set; }

            public string Image { get; set; }
        }

        [HttpGet]
        public IActionResult Get()
        {
            var products = _ProductData.GetProducts();

            var infos = products.Products.Select(p => new ProductInfo
            {
                Id = p.Id,
                Title = p.Name,
                Price = p.Price,
                Image = p.ImageUrl
            });

            return Ok(infos);
        }
    }
}
