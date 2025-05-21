using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabasePerformance
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            do
            {
                Data.Address data = AutoCSer.RandomObject.Creator<Data.Address>.CreateNotNull();
                AutoCSer.TestCase.Common.ClientPerformance.Left = AutoCSer.Random.Default.Next();

                await StringByteArrayDictionaryNode.Test(data);
                //StringByteArrayDictionaryNode+Server.SetBinarySerialize 8192 Concurrent Completed 4194304/10151ms = 413/ms
                //StringByteArrayDictionaryNode+Server.GetBinarySerialize 8192 Concurrent Completed 4194304/7611ms = 551/ms
                //StringByteArrayDictionaryNode+Server.Remove 8192 Concurrent Completed 4194304/5870ms = 714/ms
                //StringByteArrayDictionaryNode+Server.SetJsonSerialize 8192 Concurrent Completed 4194304/11368ms = 368/ms
                //StringByteArrayDictionaryNode+Server.GetJsonSerialize 8192 Concurrent Completed 4194304/9808ms = 427/ms
                //StringByteArrayDictionaryNode+Server.Remove 8192 Concurrent Completed 4194304/6216ms = 674/ms

                await IntByteArrayDictionaryNode.Test(data);
                //IntByteArrayDictionaryNode+Server.SetBinarySerialize 8192 Concurrent Completed 4194304/8037ms = 521/ms
                //IntByteArrayDictionaryNode+Server.GetBinarySerialize 8192 Concurrent Completed 4194304/5650ms = 742/ms
                //IntByteArrayDictionaryNode+Server.Remove 8192 Concurrent Completed 4194304/4748ms = 883/ms
                //IntByteArrayDictionaryNode+Server.SetJsonSerialize 8192 Concurrent Completed 4194304/9611ms = 436/ms
                //IntByteArrayDictionaryNode+Server.GetJsonSerialize 8192 Concurrent Completed 4194304/8960ms = 468/ms
                //IntByteArrayDictionaryNode+Server.Remove 8192 Concurrent Completed 4194304/4750ms = 883/ms

                await IntDictionaryNode.Test();

                Console.WriteLine("Press quit to exit.");
                if (await AutoCSer.Breakpoint.ReadLineDelay() == "quit") return;
            }
            while (true);
        }
    }
}
