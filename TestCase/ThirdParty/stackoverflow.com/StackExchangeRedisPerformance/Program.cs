using System;

namespace StackExchangeRedisPerformance
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            int maxDegreeOfParallelism = 1 << 13, count = 1 << 22;
            int left = new Random().Next(), success = 0, error = 0;
            AddressData data = new AddressData
            {
                Unit = nameof(AddressData.Unit),
                StreetName = nameof(AddressData.StreetName),
                City = nameof(AddressData.City),
                State = nameof(AddressData.State),
                PostalCode = nameof(AddressData.PostalCode),
                Country = nameof(AddressData.Country)
            };
            await using (StackExchange.Redis.ConnectionMultiplexer connect = StackExchange.Redis.ConnectionMultiplexer.Connect("127.0.0.1:6379"))
            {
                StackExchange.Redis.IDatabase client = connect.GetDatabase(0);

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                await Parallel.ForEachAsync(Enumerable.Range(1, count), new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism }, async (index, c) =>
                {
                    AddressData value = data.Clone();
                    value.StreetNumber = left + index;
                    if (await client.StringSetAsync(value.StreetNumber.ToString(), System.Text.Json.JsonSerializer.Serialize(value))) ++success;
                    else ++error;
                });
                stopwatch.Stop();
                long milliseconds = Math.Max(stopwatch.ElapsedMilliseconds, 1);
                Console.WriteLine($"StackExchange.Redis StringSetAsync API {maxDegreeOfParallelism} Concurrent Completed {count}/{milliseconds}ms = {count / milliseconds}/ms");

                stopwatch.Restart();
                await Parallel.ForEachAsync(Enumerable.Range(1, count), new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism }, async (index, c) =>
                {
                    StackExchange.Redis.RedisValue value = await client.StringGetAsync((left + index).ToString());
                    var data = value.IsNullOrEmpty ? null : System.Text.Json.JsonSerializer.Deserialize<AddressData>(value.ToString());
                    if (data != null) ++success;
                    else ++error;
                });
                stopwatch.Stop();
                milliseconds = Math.Max(stopwatch.ElapsedMilliseconds, 1);
                Console.WriteLine($"StackExchange.Redis StringGetAsync API {maxDegreeOfParallelism} Concurrent Completed {count}/{milliseconds}ms = {count / milliseconds}/ms");

                stopwatch.Restart();
                await Parallel.ForEachAsync(Enumerable.Range(1, count), new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism }, async (index, c) =>
                {
                    if (await client.KeyDeleteAsync((left + index).ToString())) ++success;
                    else ++error;
                });
                stopwatch.Stop();
                milliseconds = Math.Max(stopwatch.ElapsedMilliseconds, 1);
                Console.WriteLine($"StackExchange.Redis KeyDeleteAsync API {maxDegreeOfParallelism} Concurrent Completed {count}/{milliseconds}ms = {count / milliseconds}/ms");
            }
            if (error != 0) Console.WriteLine($"error {error}");
            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }
    }
}
