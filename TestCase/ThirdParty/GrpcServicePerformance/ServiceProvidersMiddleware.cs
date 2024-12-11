using Microsoft.AspNetCore.Http.Features;

namespace GrpcServicePerformance.LesnyRumcajs
{
    public class ServiceProvidersMiddleware
    {
        private readonly ServiceProvidersFeature _serviceProvidersFeature;
        private readonly RequestDelegate _next;

        public ServiceProvidersMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _serviceProvidersFeature = new ServiceProvidersFeature(serviceProvider);
            _next = next;
        }

        public Task InvokeAsync(HttpContext context)
        {
            // Configure request to use application services to avoid creating a request scope
            context.Features.Set<IServiceProvidersFeature>(_serviceProvidersFeature);
            return _next(context);
        }

        private class ServiceProvidersFeature : IServiceProvidersFeature
        {
            public ServiceProvidersFeature(IServiceProvider requestServices)
            {
                RequestServices = requestServices;
            }

            public IServiceProvider RequestServices { get; set; }
        }
    }
}
