using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.Entities.Orders;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Services.InSQL
{
    public class SqlOrderService : IOrderService
    {
        private readonly WebStoreDB _db;
        private readonly UserManager<User> _UserManager;
        private readonly ILogger<SqlOrderService> _Logger;

        public SqlOrderService(WebStoreDB db, UserManager<User> UserManager, ILogger<SqlOrderService> Logger)
        {
            _db = db;
            _UserManager = UserManager;
            _Logger = Logger;
        }

        public async Task<IEnumerable<Order>> GetUserOrders(string UserName)
        {
            var orders = await _db.Orders
               .Include(o => o.User)
               .Include(o => o.Items)
               .ThenInclude(o => o.Product)
               .Where(o => o.User.UserName == UserName)
               .ToArrayAsync()
               .ConfigureAwait(false);
            return orders;
        }

        public async Task<Order> GetOrderById(int id)
        {
            var order = await _db.Orders
               .Include(o => o.User)
               .Include(o => o.Items)
               .ThenInclude(o => o.Product)
               .FirstOrDefaultAsync(o => o.Id == id)
               .ConfigureAwait(false);
            return order;
        }

        public async Task<Order> CreateOrder(string UserName, CartViewModel Cart, OrderViewModel OrderModel)
        {
            var user = await _UserManager.FindByNameAsync(UserName).ConfigureAwait(false);

            if (user is null)
                throw new InvalidOperationException($"Пользователь {UserName} не найден");

            using (_Logger.BeginScope("Формирование заказа для {0}", UserName))
            {
                await using var transaction = await _db.Database.BeginTransactionAsync();

                var order = new Order
                {
                    User = user,
                    Address = OrderModel.Address,
                    Phone = OrderModel.Phone,
                    Description = OrderModel.Description,
                };

                var product_ids = Cart.Items.Select(item => item.Product.Id).ToArray();

                var cart_products = await _db.Products
                   .Where(p => product_ids.Contains(p.Id))
                   .ToArrayAsync();

                order.Items = Cart.Items.Join(
                    cart_products,
                    cart_item => cart_item.Product.Id,
                    cart_product => cart_product.Id,
                    (cart_item, cart_product) => new OrderItem
                    {
                        Order = order,
                        Product = cart_product,
                        Price = cart_product.Price, // можно добавить скидку тут!
                        Quantity = cart_item.Quantity,
                    }).ToArray();

                await _db.Orders.AddAsync(order);
                //await _db.Set<OrderItem>().AddRangeAsync(order.Items); // нет необходимости!

                await _db.SaveChangesAsync();

                await transaction.CommitAsync();

                _Logger.LogInformation("Заказ id:{0} успешно сформирован для пользователя {1}", order.Id, UserName);

                return order;
            }
        }
    }
}
