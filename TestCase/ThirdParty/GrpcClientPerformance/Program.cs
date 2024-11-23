using Grpc.Net.Client;

namespace GrpcClientPerformance
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:5000");
            {
                do
                {
                    await AwaiterClient.Test(channel);

                    Console.WriteLine("Press quit to exit.");
                    if (Console.ReadLine() == "quit") return;
                }
                while (true);
            }
        }
    }
}
