using DoctorsTower.API.Filters;
using DoctorsTower.Application.Caching;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class CachedFilter : IAsyncActionFilter 
{ private readonly ICacheService _cacheService;
    public CachedFilter(ICacheService cacheService) 
    { _cacheService = cacheService; }
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    { // 1. Create Cache Key
      var cacheKey = GenerateCacheKey(context); 
        // 2. Check Cache
        var cachedData = await _cacheService.GetAsync<object>(cacheKey);
        // 3. If data exists in Cache
        if (cachedData != null) { context.Result = new OkObjectResult(cachedData); return; }
        // 4. Execute Controller Action
        var executedContext = await next(); 
        // 5. Get Controller Resul
        if (executedContext.Result is OkObjectResult okResult) 
        { var attribute = context.ActionDescriptor.EndpointMetadata .OfType<CachedAttribute>() .FirstOrDefault();
            // 6. Save Result in Cache
            if (attribute != null && okResult.Value != null)
            { await _cacheService.SetAsync( cacheKey, okResult.Value, TimeSpan.FromSeconds( attribute.DurationInSeconds)); } } }
    private string GenerateCacheKey( ActionExecutingContext context)
    { var request = context.HttpContext.Request; return $"{request.Path}{request.QueryString}";
    }
}
