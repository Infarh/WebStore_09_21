using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Services.Data
{
    public class WebStoreDbInitializer
    {
        private readonly WebStoreDB _db;
        private readonly UserManager<User> _UserManager;
        private readonly RoleManager<Role> _RoleManager;
        private readonly ILogger<WebStoreDbInitializer> _Logger;

        public WebStoreDbInitializer(
            WebStoreDB db,
            UserManager<User> UserManager,
            RoleManager<Role> RoleManager,
            ILogger<WebStoreDbInitializer> Logger)
        {
            _db = db;
            _UserManager = UserManager;
            _RoleManager = RoleManager;
            _Logger = Logger;
        }

        public async Task InitializeAsync()
        {
            _Logger.LogInformation("Запуск инициализации БД");

            //var db_deleted = await _db.Database.EnsureDeletedAsync();

            //var dv_created = await _db.Database.EnsureCreatedAsync();

            if (_db.Database.ProviderName.EndsWith(".InMemory"))
                await _db.Database.EnsureCreatedAsync();
            else
            {
                var pending_migrations = await _db.Database.GetPendingMigrationsAsync();
                var applied_migrations = await _db.Database.GetAppliedMigrationsAsync();

                if (pending_migrations.Any())
                {
                    _Logger.LogInformation("Применение миграций: {0}", string.Join(",", pending_migrations));
                    await _db.Database.MigrateAsync();
                }
            }

            try
            {
                await InitializeProductsAsync();
            }
            catch (Exception e)
            {
                _Logger.LogError(e, "Ошибка инициализации каталога товаров");
                throw;
            }

            try
            {
                await InitializeIdentityAsync();
            }
            catch (Exception e)
            {
                _Logger.LogError(e, "Ошибка инициализации системы Identity");
                throw;
            }
        }

        private async Task InitializeProductsAsync()
        {
            var timer = Stopwatch.StartNew();
            if (_db.Sections.Any())
            {
                _Logger.LogInformation("Инициализация БД информацией о товарах не требуется");
                return;
            }

            var sections_pool = TestData.Sections.ToDictionary(section => section.Id);
            var brands_pool = TestData.Brands.ToDictionary(brand => brand.Id);

            foreach (var child_section in TestData.Sections.Where(s => s.ParentId is not null))
                child_section.Parent = sections_pool[(int)child_section.ParentId!];

            foreach (var product in TestData.Products)
            {
                product.Section = sections_pool[product.SectionId];
                if (product.BrandId is { } brand_id)
                    product.Brand = brands_pool[brand_id];

                product.Id = 0;
                product.SectionId = 0;
                product.BrandId = null;
            }

            foreach (var section in TestData.Sections)
            {
                section.Id = 0;
                section.ParentId = null;
            }

            foreach (var brand in TestData.Brands)
                brand.Id = 0;


            _Logger.LogInformation("Запись товаров...");
            await using (await _db.Database.BeginTransactionAsync())
            {
                _db.Sections.AddRange(TestData.Sections);
                _db.Brands.AddRange(TestData.Brands);
                _db.Products.AddRange(TestData.Products);

                await _db.SaveChangesAsync();
                await _db.Database.CommitTransactionAsync();
            }
            _Logger.LogInformation("Запись товаров выполнена успешно за {0} мс", timer.Elapsed.TotalMilliseconds);
        }

        private async Task InitializeIdentityAsync()
        {
            _Logger.LogInformation("Инициализация системы Identity");
            var timer = Stopwatch.StartNew();

            //if (!await _RoleManager.RoleExistsAsync(Role.Administrators))
            //    await _RoleManager.CreateAsync(new Role { Name = Role.Administrators });

            async Task CheckRole(string RoleName)
            {
                if (await _RoleManager.RoleExistsAsync(RoleName))
                    _Logger.LogInformation("Роль {0} существует", RoleName);
                else
                {
                    _Logger.LogInformation("Роль {0} не существует", RoleName);
                    await _RoleManager.CreateAsync(new Role { Name = RoleName });
                    _Logger.LogInformation("Роль {0} успешно создана", RoleName);
                }
            }

            await CheckRole(Role.Administrators);
            await CheckRole(Role.Users);

            if (await _UserManager.FindByNameAsync(User.Administrator) is null)
            {
                _Logger.LogInformation("Пользователь {0} не существует", User.Administrator);

                var admin = new User
                {
                    UserName = User.Administrator,
                };

                var creation_result = await _UserManager.CreateAsync(admin, User.DefaultAdminPassword);
                if (creation_result.Succeeded)
                {
                    _Logger.LogInformation("Пользователь {0} успешно создан", User.Administrator);

                    await _UserManager.AddToRoleAsync(admin, Role.Administrators);

                    _Logger.LogInformation("Пользователю {0} успешно добавлена роль {1}", 
                        User.Administrator, Role.Administrators);
                }
                else
                {
                    var errors = creation_result.Errors.Select(err => err.Description).ToArray();
                    _Logger.LogError("Учётная запись администратора не создана! Ошибки: {0}",
                        string.Join(", ", errors));

                    throw new InvalidOperationException($"Невозможно создать Администратора {string.Join(", ", errors)}");
                }

                _Logger.LogInformation("Данные системы Identity успешно добавлены в БД за {0}мс", timer.Elapsed.TotalMilliseconds);
            }
        }
    }
}
