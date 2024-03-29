﻿using System.Collections.Generic;
using WebStore.Domain;
using WebStore.Domain.Entities;

namespace WebStore.Interfaces.Services
{
    public interface IProductData
    {
        IEnumerable<Section> GetSections();

        Section GetSectionById(int Id);

        IEnumerable<Brand> GetBrands();

        Brand GetBrandById(int Id);

        ProductsPage GetProducts(ProductFilter Filter = null);

        Product GetProductById(int Id);
    }
}
