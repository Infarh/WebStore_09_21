﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Components
{
    //[ViewComponent(Name = "BrandsView")]
    public class BrandsViewComponent : ViewComponent
    {
        private readonly IProductData _ProductData;

        public BrandsViewComponent(IProductData ProductData) => _ProductData = ProductData;

        //public async Task<IViewComponentResult> InvokeAsync() => View();

        public IViewComponentResult Invoke() => View(GetBrands());

        private IEnumerable<BrandViewModel> GetBrands() =>
            _ProductData.GetBrands()
               .OrderBy(b => b.Order)
               .Select(b => new BrandViewModel
                {
                   Id = b.Id,
                   Name = b.Name,
                });
    }
}
