﻿using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _CartService;

        public CartController(ICartService CartService) => _CartService = CartService;

        public IActionResult Index() => View(new CartOrderViewModel { Cart = _CartService.GetViewModel() });

        public IActionResult Add(int Id)
        {
            _CartService.Add(Id);
            return RedirectToAction("Index", "Cart");
        }

        public IActionResult Decrement(int Id)
        {
            _CartService.Decrement(Id);
            return RedirectToAction("Index", "Cart");
        }

        public IActionResult Remove(int Id)
        {
            _CartService.Remove(Id);
            return RedirectToAction("Index", "Cart");
        }

        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut(OrderViewModel OrderModel, [FromServices] IOrderService OrderService)
        {
            if (!ModelState.IsValid)
                return View(nameof(Index), new CartOrderViewModel
                {
                    Cart = _CartService.GetViewModel(),
                    Order = OrderModel,
                });

            var order = await OrderService.CreateOrder(
                User.Identity!.Name,
                _CartService.GetViewModel(),
                OrderModel);

            _CartService.Clear();

            return RedirectToAction(nameof(OrderConfirmed), new { order.Id });
        }

        public IActionResult OrderConfirmed(int Id)
        {
            ViewBag.OrderId = Id;
            return View();
        }

        #region WebAPI

        public IActionResult GetCartView() => ViewComponent("Cart");

        public IActionResult AddAPI(int Id)
        {
            _CartService.Add(Id);
            return Json(new { Id, message = $"Товар с Id:{Id} добавлен в корзину" });
        }

        public IActionResult DecrementAPI(int Id)
        {
            _CartService.Decrement(Id);
            return Ok(new { Id, message = $"Товар с Id:{Id} - уменьшено количество на 1" });
        }

        public IActionResult RemoveAPI(int Id)
        {
            _CartService.Remove(Id);
            return Ok();
        }

        #endregion
    }
}
