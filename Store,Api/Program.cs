using Domain.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens.Experimental;
using Persistence;
using Persistence.Data;
using Services;
using Services.Abstrations;
using Services.MappingProfiles;
using Store_Api.ErrorModel;
using Store_Api.Extensions;
using Store_Api.MiddleWare;
using System.Threading.Tasks;

//using AssemblyMapping = Services.MappingProfiles.productProfile;

namespace Store_Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.RegesterAllServices(builder.Configuration);



            var app = builder.Build();

            await app.ConfigureMiddelWares();

            app.Run();
        }
    }
}
