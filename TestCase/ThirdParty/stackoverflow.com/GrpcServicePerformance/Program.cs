using System;

namespace GrpcServicePerformance
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;//Force Task.Run to switch the context to avoid deadlock in the UI thread await call

            AutoCSer.Net.CommandServerConfig commandServerConfig = new AutoCSer.Net.CommandServerConfig { Host = new AutoCSer.Net.HostEndPoint(12907), TaskQueueMaxConcurrent = 16 };
            await using (AutoCSer.Net.CommandListener commandListener = new AutoCSer.Net.CommandListener(commandServerConfig
                , AutoCSer.Net.CommandServerInterfaceControllerCreator.GetCreator<AutoCSer.TestCase.CommandServerPerformance.ITestService>(new AutoCSer.TestCase.CommandServerPerformance.TestService())
                ))
            {
                if (await commandListener.Start())//Start the AutoCSer RPC server
                {
                    gRPC(args);//Start the .NET gRPC server
                }
            }
        }
        private static void gRPC(string[] args)
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