﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebStore.Domain;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private const string __PageSizeConfig = "CatalogPageSize";

        private readonly IProductData _ProductData;
        private readonly IConfiguration _Configuration;

        public CatalogController(IProductData ProductData, IConfiguration Configuration)
        {
            _ProductData = ProductData;
            _Configuration = Configuration;
        }

        public IActionResult Index(int? BrandId, int? SectionId, int Page = 1, int? PageSize = null)
        {
            var page_size = PageSize
                ?? (int.TryParse(_Configuration[__PageSizeConfig], out var value) ? value : null);

            var filter = new ProductFilter
            {
                BrandId = BrandId,
                SectionId = SectionId,
                Page = Page,
                PageSize = page_size,
            };

            var (products, total_count) = _ProductData.GetProducts(filter);

            var view_model = new CatalogViewModel
            {
                BrandId = BrandId,
                SectionId = SectionId,
                Products = products.OrderBy(p => p.Order).ToView(),
                PageViewModel = new()
                {
                    Page = Page,
                    PageSize = page_size ?? 0,
                    TotalItems = total_count,
                }
            };

            return View(view_model);
        }

        public IActionResult Details(int Id)
        {
            var product = _ProductData.GetProductById(Id);

            if (product is null)
                return NotFound();

            return View(product.ToView());
        }

        public IActionResult GetProductsView(int? BrandId, int? SectionId, int Page = 1, int? PageSize = null)
        {
            var products = GetProducts(BrandId, SectionId, Page, PageSize);
            return PartialView("Partial/_Products", products);
        }

        private IEnumerable<ProductViewModel> GetProducts(int? BrandId, int? SectionId, int Page, int? PageSize)
        {
            var products = _ProductData.GetProducts(new()
            {
                BrandId = BrandId,
                SectionId = SectionId,
                Page = Page,
                PageSize = PageSize ?? _Configuration.GetValue(__PageSizeConfig, 6)
            });
            return products.Products.OrderBy(p => p.Order).ToView();
        }
    }
}
