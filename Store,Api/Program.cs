using Store_Api.Extensions;

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
