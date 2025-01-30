using System;

namespace GarnetPerformance
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Garnet.server.GarnetServerOptions options = new Garnet.server.GarnetServerOptions
            {
                Address = "127.0.0.1",
                Port = 6379,
                LogDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nameof(GarnetPerformance), nameof(Garnet.server.GarnetServerOptions.LogDir)),
                CheckpointDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nameof(GarnetPerformance), nameof(Garnet.server.GarnetServerOptions.CheckpointDir)),
                EnableAOF = true,
                Recover = true,
                EnableStorageTier = true,
            };
            using (Garnet.GarnetServer server = new Garnet.GarnetServer(options))
            {
                server.Start();

                Console.WriteLine("Press quit to exit.");
                while (Console.ReadLine() != "quit") ;
            }
        }
    }
}
