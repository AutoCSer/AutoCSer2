using AutoCSer.Extensions;
using System;

namespace GrpcClientPerformance
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;//Force Task.Run to switch the context to avoid deadlock in the UI thread await call

            long milliseconds;
            int maxDegreeOfParallelism = 1 << 13, count = 1 << 21;
            int left = new Random().Next(), success = 0, error = 0;
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            using (var channel = Grpc.Net.Client.GrpcChannel.ForAddress("http://localhost:5000"))
            {
                Greeter.GreeterClient client = new Greeter.GreeterClient(channel);
                stopwatch.Start();
                await Parallel.ForEachAsync(Enumerable.Range(1, count), new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism }, async (index, c) =>
                {
                    var reply = await client.AddAsync(new AddRequest { Left = left, Right = index });
                    if (reply != null) ++success;
                    else ++error;
                });
                stopwatch.Stop();
                milliseconds = Math.Max(stopwatch.ElapsedMilliseconds, 1);
                Console.WriteLine($".NET gRPC AddAsync API {maxDegreeOfParallelism} Concurrent Completed {count}/{milliseconds}ms = {count / milliseconds}/ms");
            }
            if (error != 0) Console.WriteLine($"error {error}");


            count = 1 << 26;
            success = error = 0;
            stopwatch.Restart();
            await Parallel.ForEachAsync(Enumerable.Range(1, count), new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism }, async (index, c) =>
            {
                ++success;//Empty logic test for comparison description Parallel.ForEachAsync is not suitable for testing AutoCSer because Parallel.ForEachAsync itself becomes a bottleneck below the TPS level of this test.
            });
            stopwatch.Stop();
            milliseconds = Math.Max(stopwatch.ElapsedMilliseconds, 1);
            Console.WriteLine($"Empty Task {maxDegreeOfParallelism} Concurrent Completed {count}/{milliseconds}ms = {count / milliseconds}/ms");


            AutoCSer.Net.CommandClientConfig<AutoCSer.TestCase.CommandClientPerformance.ITestClient> commandClientConfig = new AutoCSer.Net.CommandClientConfig<AutoCSer.TestCase.CommandClientPerformance.ITestClient> { Host = new AutoCSer.Net.HostEndPoint(12907), CheckSeconds = 0 };
            using (AutoCSer.Net.CommandClient commandClient = new AutoCSer.Net.CommandClient(commandClientConfig
                , AutoCSer.Net.CommandClientInterfaceControllerCreator.GetCreator<AutoCSer.TestCase.CommandClientPerformance.ITestClient, AutoCSer.TestCase.CommandServerPerformance.ITestService>()))
            {
                AutoCSer.Net.CommandClientSocketEvent<AutoCSer.TestCase.CommandClientPerformance.ITestClient> client = ((AutoCSer.Net.CommandClientSocketEvent<AutoCSer.TestCase.CommandClientPerformance.ITestClient>?)await commandClient.GetSocketEvent()).notNull();

                success = error = 0;
                stopwatch.Restart();
                await Parallel.ForEachAsync(Enumerable.Range(1, count), new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism }, async (index, c) =>
                {
                    if ((await client.InterfaceController.Task(left, index)).IsSuccess) ++success;
                    else ++error;
                });
                stopwatch.Stop();
                milliseconds = Math.Max(stopwatch.ElapsedMilliseconds, 1);
                Console.WriteLine($"AutoCSer ITestService.Task API {maxDegreeOfParallelism} Concurrent Completed {count}/{milliseconds}ms = {count / milliseconds}/ms");
                if (error != 0) Console.WriteLine($"error {error}");

                success = error = 0;
                stopwatch.Restart();
                await Parallel.ForEachAsync(Enumerable.Range(1, count), new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism }, async (index, c) =>
                {
                    if ((await client.InterfaceController.SynchronousCallTask(left, index)).IsSuccess) ++success;
                    else ++error;
                });
                stopwatch.Stop();
                milliseconds = Math.Max(stopwatch.ElapsedMilliseconds, 1);
                Console.WriteLine($"AutoCSer ITestService.SynchronousCallTask API {maxDegreeOfParallelism} Concurrent Completed {count}/{milliseconds}ms = {count / milliseconds}/ms");
                if (error != 0) Console.WriteLine($"error {error}");

                success = error = 0;
                stopwatch.Restart();
                await Parallel.ForEachAsync(Enumerable.Range(1, count), new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism }, async (index, c) =>
                {
                    if ((await client.InterfaceController.Queue(left, index)).IsSuccess) ++success;
                    else ++error;
                });
                stopwatch.Stop();
                milliseconds = Math.Max(stopwatch.ElapsedMilliseconds, 1);
                Console.WriteLine($"AutoCSer ITestService.Queue API {maxDegreeOfParallelism} Concurrent Completed {count}/{milliseconds}ms = {count / milliseconds}/ms");
                if (error != 0) Console.WriteLine($"error {error}");

                success = error = 0;
                stopwatch.Restart();
                await Parallel.ForEachAsync(Enumerable.Range(1, count), new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism }, async (index, c) =>
                {
                    if ((await client.InterfaceController.Callback(left, index)).IsSuccess) ++success;
                    else ++error;
                });
                stopwatch.Stop();
                milliseconds = Math.Max(stopwatch.ElapsedMilliseconds, 1);
                Console.WriteLine($"AutoCSer ITestService.Callback API {maxDegreeOfParallelism} Concurrent Completed {count}/{milliseconds}ms = {count / milliseconds}/ms");
                if (error != 0) Console.WriteLine($"error {error}");

                success = error = 0;
                stopwatch.Restart();
                await Parallel.ForEachAsync(Enumerable.Range(1, count), new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism }, async (index, c) =>
                {
                    if ((await client.InterfaceController.Synchronous(left, index)).IsSuccess) ++success;
                    else ++error;
                });
                stopwatch.Stop();
                milliseconds = Math.Max(stopwatch.ElapsedMilliseconds, 1);
                Console.WriteLine($"AutoCSer ITestService.Synchronous API {maxDegreeOfParallelism} Concurrent Completed {count}/{milliseconds}ms = {count / milliseconds}/ms");
                if (error != 0) Console.WriteLine($"error {error}");

                success = error = 0;
                stopwatch.Restart();
                await Parallel.ForEachAsync(Enumerable.Range(1, count), new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism }, async (index, c) =>
                {
                    if ((await client.InterfaceController.TaskQueue(left, index)).IsSuccess) ++success;
                    else ++error;
                });
                stopwatch.Stop();
                milliseconds = Math.Max(stopwatch.ElapsedMilliseconds, 1);
                Console.WriteLine($"AutoCSer ITestService.TaskQueue API {maxDegreeOfParallelism} Concurrent Completed {count}/{milliseconds}ms = {count / milliseconds}/ms");
                if (error != 0) Console.WriteLine($"error {error}");
            }

            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }
    }
}
