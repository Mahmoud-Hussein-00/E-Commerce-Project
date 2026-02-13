using Domain.Contracts;
using Domain.Models;
using Domain.Models.Identity;
using Domain.Models.OrderModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Persistence.Data;
using Persistence.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence
{
    public class DbInitializer : IDbInitializer
    {
        private readonly StoreDbContext _context;
        private readonly StoreIdentityDbContext _IdentityDbContext;
        private readonly UserManager<AppUser> _AppUser;
        private readonly RoleManager<IdentityRole> _RoleManager;

        public DbInitializer(StoreDbContext context, StoreIdentityDbContext identityDbContext,
            UserManager<AppUser> appUser, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _IdentityDbContext = identityDbContext;
            _AppUser = appUser;
            _RoleManager = roleManager;
        }
        public async Task InitializeAsync()
        {
            try
            {

                if (_context.Database.GetPendingMigrations().Any())
                {
                    await _context.Database.MigrateAsync();
                }

                if (!_context.ProductTypes.Any())
                {
                    var TypesData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\types.json");

                    var Types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);

                    if(Types is not null && Types.Any())
                    {
                        await _context.ProductTypes.AddRangeAsync(Types);
                        await _context.SaveChangesAsync();
                    }
                }

                if (!_context.ProductBrands.Any())
                {
                    var BrandData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\brands.json");

                    var Brand = JsonSerializer.Deserialize<List<ProductBrand>>(BrandData);

                    if(Brand is not null && Brand.Any())
                    {
                        await _context.ProductBrands.AddRangeAsync(Brand);
                        await _context.SaveChangesAsync();
                    }
                }

                if (!_context.Products.Any())
                {
                    var ProductData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\products.json");

                    var Product = JsonSerializer.Deserialize<List<Product>>(ProductData);

                    if(Product is not null && Product.Any())
                    {
                        await _context.Products.AddRangeAsync(Product);
                        await _context.SaveChangesAsync();
                    }
                }


                if (!_context.deliveryMethods.Any())
                {
                    var DeliveryData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\delivery.json");

                    var Delivery = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryData);

                    if (Delivery is not null && Delivery.Any())
                    {
                        await _context.deliveryMethods.AddRangeAsync(Delivery);
                        await _context.SaveChangesAsync();
                    }
                }

            }
            catch(Exception) { throw; }
        }

        public async Task InitializeIdentityAsync()
        {
            #region Migrate
            if (_IdentityDbContext.Database.GetPendingMigrations().Any())
            {
                await _IdentityDbContext.Database.MigrateAsync();
            }
            #endregion

            #region Roles

            if (!_RoleManager.Roles.Any())
            {
                await _RoleManager.CreateAsync(new IdentityRole("Admin"));
                await _RoleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            }

            #endregion

            #region Users

            if (!_AppUser.Users.Any())
            {
                var superAdmin = new AppUser
                {
                    DisplayName = "SuperAdmin",
                    Email = "superadmin@gmail.com",
                    UserName = "SuperAdmin",
                    PhoneNumber = "01149884165"
                };

                var admin = new AppUser
                {
                    DisplayName = "Admin",
                    Email = "admin@gmail.com",
                    UserName = "Admin",
                    PhoneNumber = "01149884165"
                };

                await _AppUser.CreateAsync(superAdmin, "P@ssw0rd!");
                await _AppUser.CreateAsync(admin, "P@ssw0rd!");

                await _AppUser.AddToRoleAsync(superAdmin, "SuperAdmin");
                await _AppUser.AddToRoleAsync(admin, "Admin");
            }

            #endregion
        }

    }
}
