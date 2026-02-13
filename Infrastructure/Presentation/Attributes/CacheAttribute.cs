using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Attributes
{
    public class CacheAttribute(int duration) : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var CacheService = context.HttpContext.RequestServices.
                GetRequiredService<IServiceManger>().CasheService;

            var CacheKey = GenerateCacheKey(context.HttpContext.Request);

            var Result = await CacheService.GetCasheValueAsync(CacheKey);

            if (!string.IsNullOrEmpty(Result))
            {
                context.Result = new ContentResult()
                {
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK,
                    Content = Result
                };
                return;
            }

            var ContextResult = await next.Invoke(); 

            if(ContextResult.Result is OkObjectResult okObject) 
            {
                CacheService.SetCasheValueAsync(CacheKey, okObject.Value,
                    TimeSpan.FromSeconds(duration));
            }

        }

        private string GenerateCacheKey(HttpRequest request)
        {
            var Key = new StringBuilder();
            Key.Append(request.Path);
            foreach (var item in request.Query.OrderBy(b => b.Key))
            {
                Key.Append($"|{item.Key}-{item.Value}");
            }

            return Key.ToString();
        }

    }
}
