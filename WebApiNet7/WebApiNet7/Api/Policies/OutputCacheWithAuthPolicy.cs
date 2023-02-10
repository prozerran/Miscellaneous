using Microsoft.AspNetCore.OutputCaching;

// Ref:
// https://learn.microsoft.com/en-us/aspnet/core/performance/caching/output?view=aspnetcore-7.0
// https://stackoverflow.com/questions/74805606/outputcache-attribute-not-working-on-net-7-api-endpoint

namespace WebApiNet7.Api.Policies
{
    public class OutputCacheWithAuthPolicy : IOutputCachePolicy
    {
        public static readonly OutputCacheWithAuthPolicy Instance = new();
        private OutputCacheWithAuthPolicy() { }

        ValueTask IOutputCachePolicy.CacheRequestAsync(OutputCacheContext context, CancellationToken cancellationToken)
        {
            var attemptOutputCaching = AttemptOutputCaching(context);
            context.EnableOutputCaching = true;
            context.AllowCacheLookup = attemptOutputCaching;
            context.AllowCacheStorage = attemptOutputCaching;
            context.AllowLocking = true;

            // Vary by any query by default
            context.CacheVaryByRules.QueryKeys = "*";
            return ValueTask.CompletedTask;
        }
        private static bool AttemptOutputCaching(OutputCacheContext context)
        {
            // Check if the current request fulfills the requirements to be cached
            var request = context.HttpContext.Request;

            // Methods allowed to be cached
            if (!HttpMethods.IsGet(request.Method) &&
                //!HttpMethods.IsPost(request.Method) &&
                //!HttpMethods.IsPut(request.Method) &&
                //!HttpMethods.IsDelete(request.Method) &&
                !HttpMethods.IsHead(request.Method))
            {
                return false;
            }

            // we comment out below code to cache authorization response.
            // Verify existence of authorization headers
            //if (!StringValues.IsNullOrEmpty(request.Headers.Authorization) ||
            //    request.HttpContext.User?.Identity?.IsAuthenticated == true)
            //{
            //    return false;
            //}
            return true;
        }
        public ValueTask ServeFromCacheAsync(OutputCacheContext context, CancellationToken cancellation) => ValueTask.CompletedTask;
        public ValueTask ServeResponseAsync(OutputCacheContext context, CancellationToken cancellation) => ValueTask.CompletedTask;

    }
}
