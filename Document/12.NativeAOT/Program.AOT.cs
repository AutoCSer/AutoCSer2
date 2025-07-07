using AutoCSer.Document.NativeAOT.DataSerialize;
using AutoCSer.Document.NativeAOT.MemoryDatabaseLocalService;
using AutoCSer.Document.NativeAOT.Service;
using AutoCSer.Extensions;

namespace AutoCSer.Document.NativeAOT
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            Console.WriteLine($"{nameof(BinaryProperty)} {BinaryProperty.TestCase()}");
            Console.WriteLine($"{nameof(BinaryJsonMix)} {BinaryJsonMix.TestCase()}");
            Console.WriteLine($"{nameof(JsonProperty)} {JsonProperty.TestCase()}");
            Console.WriteLine($"{nameof(XmlProperty)} {XmlProperty.TestCase()}");

            Console.WriteLine($"{nameof(StringDictionaryNode)} {await StringDictionaryNode.TestCase()}");

            var client = await CommandClientSocketEvent.CommandClient.SocketEvent.Wait();
            if (client != null)
            {
                var result = await client.ServiceController.Add(1, 2);
                if (result.Value == 3) Console.WriteLine($"{nameof(IServiceController)} {result.Value == 3}");
                else Console.WriteLine($"{nameof(IServiceController)} {result.ReturnType}");
            }
            else Console.WriteLine($"{nameof(IServiceController)} is null");

            Console.WriteLine("Completed");
            Console.ReadKey();

            //Important: Reflection activation and triggering of AOT compilation must be explicitly called
            //重要：反射激活与触发 AOT 编译，必须显式调用
            AutoCSer.Document.NativeAOT.AotMethod.Call();
        }
    }
}
