using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstrations;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public static class ApplicationsServicesRegistrations
    {
       public static IServiceCollection AddApplicationsServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddAutoMapper(config =>
            {
                config.AddMaps(typeof(Services.AssemblyReference).Assembly);
            });
            services.AddScoped<IServiceManger, ServiceManger>();
            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));

            return services;
        }
    }
}
