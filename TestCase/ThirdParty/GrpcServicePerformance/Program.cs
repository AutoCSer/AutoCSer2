using GrpcServicePerformance.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace GrpcServicePerformance
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LesnyRumcajs(args);
            //var builder = WebApplication.CreateBuilder(args);
            //builder.Services.AddGrpc();
            //var app = builder.Build();
            //app.MapGrpcService<GreeterService>();
            //app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
            //app.Run();
        }
        /// <summary>
        /// https://github.com/LesnyRumcajs/grpc_bench
        /// </summary>
        /// <param name="args"></param>
        private static void LesnyRumcajs(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.AddServerHeader = false;
                options.ListenAnyIP(5000, listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http2;
                });
            });

            builder.Logging.ClearProviders();

            builder.Services.AddGrpc(o => o.IgnoreUnknownServices = true);
            builder.Services.Configure<RouteOptions>(c => c.SuppressCheckForUnhandledSecurityMetadata = true);
            builder.Services.AddSingleton<GreeterService>();

            var app = builder.Build();

            app.Lifetime.ApplicationStarted.Register(() => Console.WriteLine("Application started."));
            app.UseMiddleware<LesnyRumcajs.ServiceProvidersMiddleware>();
            app.MapGrpcService<GreeterService>();

            app.Run();
        }
    }
}