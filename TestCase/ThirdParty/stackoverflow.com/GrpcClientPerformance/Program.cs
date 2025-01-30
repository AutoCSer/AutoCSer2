using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GrpcClientPerformance
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            int maxDegreeOfParallelism = 1 << 13, count = 1 << 20;
            int left = new Random().Next(), success = 0, error = 0;
            using (var channel = Grpc.Net.Client.GrpcChannel.ForAddress("http://localhost:5000"))
            {
                Greeter.GreeterClient client = new Greeter.GreeterClient(channel);
                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                await Parallel.ForEachAsync(Enumerable.Range(1, count), new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism }, async (index, c) =>
                {
                    var reply = await client.AddAsync(new AddRequest { Left = left, Right = index });
                    if (reply != null) ++success;
                    else ++error;
                });
                stopwatch.Stop();
                long milliseconds = Math.Max(stopwatch.ElapsedMilliseconds, 1);
                Console.WriteLine($".NET gRPC Add API {maxDegreeOfParallelism} Concurrent Completed {count}/{milliseconds}ms = {count / milliseconds}/ms");
            }
            if (error != 0) Console.WriteLine($"error {error}");
            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }
    }
}
