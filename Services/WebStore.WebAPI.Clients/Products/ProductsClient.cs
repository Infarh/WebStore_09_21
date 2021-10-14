using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using WebStore.Domain;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Products
{
    public class ProductsClient : BaseClient, IProductData
    {
        public ProductsClient(HttpClient Client) : base(Client, "api/products") { }

        public IEnumerable<Section> GetSections()
        {
            var sections = Get<IEnumerable<SectionDTO>>($"{Address}/sections");
            return sections.FromDTO();
        }

        public Section GetSectionById(int Id)
        {
            var section = Get<SectionDTO>($"{Address}/sections/{Id}");
            return section.FromDTO();
        }

        public IEnumerable<Brand> GetBrands()
        {
            var brands = Get<IEnumerable<BrandDTO>>($"{Address}/brands");
            return brands.FromDTO();
        }

        public Brand GetBrandById(int Id)
        {
            var brand = Get<BrandDTO>($"{Address}/brands/{Id}");
            return brand.FromDTO();
        }

        public IEnumerable<Product> GetProducts(ProductFilter Filter = null)
        {
            var response = Post(Address, Filter ?? new());
            var products_dtos = response.Content.ReadFromJsonAsync<IEnumerable<ProductDTO>>().Result;
            return products_dtos.FromDTO();
        }

        public Product GetProductById(int Id)
        {
            var product = Get<ProductDTO>($"{Address}/{Id}");
            return product.FromDTO();
        }
    }
}
