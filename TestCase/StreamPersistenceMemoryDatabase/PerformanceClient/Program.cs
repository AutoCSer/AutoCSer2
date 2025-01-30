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

                await StringByteArrayFragmentDictionaryNode.Test(data);
                //StringByteArrayFragmentDictionaryNode+Server.SetBinarySerialize 8192 Concurrent Completed 4194304/10151ms = 413/ms
                //StringByteArrayFragmentDictionaryNode+Server.GetBinarySerialize 8192 Concurrent Completed 4194304/7611ms = 551/ms
                //StringByteArrayFragmentDictionaryNode+Server.Remove 8192 Concurrent Completed 4194304/5870ms = 714/ms
                //StringByteArrayFragmentDictionaryNode+Server.SetJsonSerialize 8192 Concurrent Completed 4194304/11368ms = 368/ms
                //StringByteArrayFragmentDictionaryNode+Server.GetJsonSerialize 8192 Concurrent Completed 4194304/9808ms = 427/ms
                //StringByteArrayFragmentDictionaryNode+Server.Remove 8192 Concurrent Completed 4194304/6216ms = 674/ms

                await IntByteArrayFragmentDictionaryNode.Test(data);
                //IntByteArrayFragmentDictionaryNode+Server.SetBinarySerialize 8192 Concurrent Completed 4194304/8037ms = 521/ms
                //IntByteArrayFragmentDictionaryNode+Server.GetBinarySerialize 8192 Concurrent Completed 4194304/5650ms = 742/ms
                //IntByteArrayFragmentDictionaryNode+Server.Remove 8192 Concurrent Completed 4194304/4748ms = 883/ms
                //IntByteArrayFragmentDictionaryNode+Server.SetJsonSerialize 8192 Concurrent Completed 4194304/9611ms = 436/ms
                //IntByteArrayFragmentDictionaryNode+Server.GetJsonSerialize 8192 Concurrent Completed 4194304/8960ms = 468/ms
                //IntByteArrayFragmentDictionaryNode+Server.Remove 8192 Concurrent Completed 4194304/4750ms = 883/ms

                await IntDictionaryNode.Test();

                Console.WriteLine("Press quit to exit.");
                if (Console.ReadLine() == "quit") return;
            }
            while (true);
        }
    }
}
