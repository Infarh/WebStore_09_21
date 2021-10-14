using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Domain.DTO;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsApiController : ControllerBase
    {
        private readonly IProductData _ProductData;

        public ProductsApiController(IProductData ProductData) => _ProductData = ProductData;

        [HttpGet("sections")]
        public IActionResult GetSections()
        {
            var sections = _ProductData.GetSections();
            return Ok(sections.ToDTO());
        }

        [HttpGet("sections/{id}")]
        public IActionResult GetSection(int id)
        {
            var section = _ProductData.GetSectionById(id);
            return Ok(section.ToDTO());
        }

        [HttpGet("brands")]
        public IActionResult GetBrands()
        {
            var brands = _ProductData.GetBrands();
            return Ok(brands.ToDTO());
        }

        [HttpGet("brands/{id}")]
        public IActionResult GetBrand(int id)
        {
            var section = _ProductData.GetBrandById(id);
            return Ok(section.ToDTO());
        }

        [HttpPost]
        public IActionResult GetProducts(ProductFilter Filter = null)
        {
            var products = _ProductData.GetProducts(Filter);
            return Ok(products.ToDTO());
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = _ProductData.GetProductById(id);
            if (product is null)
                return NotFound();
            return Ok(product.ToDTO());
        }
    }
}
