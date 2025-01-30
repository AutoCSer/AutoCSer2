using System;

namespace GrpcServicePerformance
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.AddServerHeader = false;
                options.ListenAnyIP(5000, listenOptions =>
                {
                    listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
                });
            });

            builder.Logging.ClearProviders();

            builder.Services.AddGrpc(o => o.IgnoreUnknownServices = true);
            builder.Services.Configure<RouteOptions>(c => c.SuppressCheckForUnhandledSecurityMetadata = true);
            builder.Services.AddSingleton<GrpcServicePerformance.Services.GreeterService>();

            var app = builder.Build();

            app.Lifetime.ApplicationStarted.Register(() => Console.WriteLine("Application started."));
            app.UseMiddleware<LesnyRumcajs.ServiceProvidersMiddleware>();
            app.MapGrpcService<GrpcServicePerformance.Services.GreeterService>();

            app.Run();
        }
    }
}