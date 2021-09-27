using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using WebStore.DAL.Context;

namespace WebStore.Data
{
    public class WebStoreDbInitializer
    {
        private readonly WebStoreDB _db;

        public WebStoreDbInitializer(WebStoreDB db) => _db = db;

        public async Task InitializeAsync()
        {
            //var db_deleted = await _db.Database.EnsureDeletedAsync();

            //var dv_created = await _db.Database.EnsureCreatedAsync();

            var pending_migrations = await _db.Database.GetPendingMigrationsAsync();
            var applied_migrations = await _db.Database.GetAppliedMigrationsAsync();

            if (pending_migrations.Any())
                await _db.Database.MigrateAsync();

            await InitializeProductsAsync();
        }

        private async Task InitializeProductsAsync()
        {
            await using (await _db.Database.BeginTransactionAsync())
            {
                _db.Sections.AddRange(TestData.Sections);

                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Sections] ON");
                await _db.SaveChangesAsync();
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Sections] OFF");
                await _db.Database.CommitTransactionAsync();
            }

            await using (await _db.Database.BeginTransactionAsync())
            {
                _db.Brands.AddRange(TestData.Brands);

                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Brands] ON");
                await _db.SaveChangesAsync();
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Brands] OFF");
                await _db.Database.CommitTransactionAsync();
            }

            await using (await _db.Database.BeginTransactionAsync())
            {
                _db.Products.AddRange(TestData.Products);

                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Products] ON");
                await _db.SaveChangesAsync();
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Products] OFF");
                await _db.Database.CommitTransactionAsync();
            }
        }
    }
}
