
using DoctorsTower.API.Filters;
using DoctorsTower.Application.Caching;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class CachedFilter : IAsyncActionFilter
{
    private readonly ICacheService _cacheService;

    public CachedFilter(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var cacheKey = GenerateCacheKey(context);

        var cachedData =
            await _cacheService.GetAsync<object>(cacheKey);

        if (cachedData != null)
        {
            context.Result = new OkObjectResult(cachedData);
            return;
        }

        var executedContext = await next();

        if (executedContext.Result is OkObjectResult okResult &&
            okResult.Value != null)
        {
            var attribute = context.ActionDescriptor
                .EndpointMetadata
                .OfType<CachedAttribute>()
                .FirstOrDefault();

            if (attribute != null)
            {
                await _cacheService.SetAsync(
                    cacheKey,
                    okResult.Value,
                    TimeSpan.FromSeconds(attribute.DurationInSeconds));
            }
        }
    }

    private string GenerateCacheKey(ActionExecutingContext context)
    {
        var request = context.HttpContext.Request;

        return $"{request.Path}{request.QueryString}";
    }
}
