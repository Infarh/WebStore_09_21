﻿using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Services;

namespace WebStore.Components
{
    public class CartViewComponent : ViewComponent
    {
        private readonly ICartService _CartService;

        public CartViewComponent(ICartService CartService) => _CartService = CartService;

        public IViewComponentResult Invoke()
        {
            ViewBag.Count = _CartService.GetViewModel().ItemsCount;
            return View();
        }
    }
}
