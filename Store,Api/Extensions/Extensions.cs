using Domain.Contracts;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Persistence.Identity;
using Services;
using Services.MappingProfiles;
using Shared;
using Store_Api.ErrorModel;
using Store_Api.MiddleWare;
using System.Text;

namespace Store_Api.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection RegesterAllServices(this IServiceCollection services ,IConfiguration configuration)
        {


            services.AddBuilderServices();
            services.AddSwaggerServices();
 
            services.AddInfrastrucreServices(configuration);

            services.AddApplicationsServices(configuration);

            services.AddTransient<ProductUrlResolver>();

            services.ConfigureServises();

            services.AddIdentityServices();

            services.ConfigureJwtSecices(configuration);

            services.AddCors(config =>
            {
                config.AddPolicy("MyPolicy", options =>
                {
                    options.AllowAnyHeader();
                    options.AllowAnyMethod();
                    options.WithOrigins("");
                });
            });

            return services;
        }

        private static IServiceCollection AddBuilderServices(this IServiceCollection services)
        {
            services.AddControllers();

            return services;
        }

        private static IServiceCollection ConfigureJwtSecices(this IServiceCollection services, IConfiguration configuration)
        {
            var JwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,

                    ValidIssuer = JwtOptions.issuer,
                    ValidAudience = JwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SecretKey)),
                };
            });
            return services;
        }

        private static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<StoreIdentityDbContext>();
            return services;
        }

        private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddSwaggerGen();
            services.AddEndpointsApiExplorer();

            return services;
        }


        private static IServiceCollection ConfigureServises(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(Config =>
            {
                Config.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(m => m.Value.Errors.Any())
                    .Select(m => new ErrorModel.ValidationError()
                    {
                        Field = m.Key,
                        Errors = m.Value.Errors.Select(e => e.ErrorMessage)
                    });
                    var response = new ValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(response);

                };
            });

            return services;
        }


        public static async Task<WebApplication> ConfigureMiddelWares(this WebApplication app)
        {
            #region seeding

            using var Scop = app.Services.CreateScope();
            var dbInitialzer = Scop.ServiceProvider.GetRequiredService<IDbInitializer>();
            await dbInitialzer.InitializeAsync();
            await dbInitialzer.InitializeIdentityAsync();

            #endregion

            app.UseGlobalErrorHandling();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseStaticFiles();

            app.MapControllers();

            return app;
        }

        private static WebApplication UseGlobalErrorHandling(this WebApplication app)
        {
            app.UseMiddleware<GlobalErrorHandlingMiddleWare>();

            return app;
        }

        

    }
}
