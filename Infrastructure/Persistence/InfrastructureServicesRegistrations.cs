using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Data;
using Persistence.Identity;
using Persistence.Repositories;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public static class InfrastructureServicesRegistrations
    {
        public static IServiceCollection AddInfrastrucreServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<StoreDbContext>(
               options => {
                   //options.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);
                   options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
               });
            services.AddDbContext<StoreIdentityDbContext>( options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
                });
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddSingleton<IConnectionMultiplexer>((servereProvider) =>
            {
                return ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")!);
            });
            services.AddScoped<ICacheRepository , CacheRepository>();

            return services;
        }
    }
}
