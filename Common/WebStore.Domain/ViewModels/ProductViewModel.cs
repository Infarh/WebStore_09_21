﻿namespace WebStore.Domain.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public string Section { get; set; }

        public string Brand { get; set; }
    }
}
