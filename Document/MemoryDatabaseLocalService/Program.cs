using AutoCSer.Document.MemoryDatabaseLocalService.Client;
using AutoCSer.Extensions;

namespace AutoCSer.Document.MemoryDatabaseLocalService
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine($"{nameof(CustomNode.Client.CounterNode)} {await CustomNode.Client.CounterNode.Test()}");

            Console.WriteLine($"{nameof(DictionaryNode)} {await DictionaryNode.Test()}");
            Console.WriteLine($"{nameof(FragmentDictionaryNode)} {await FragmentDictionaryNode.Test()}");
            Console.WriteLine($"{nameof(MessageConsumer)} {await MessageConsumer.Test()}");
            Console.WriteLine($"{nameof(DistributedLockNode)} {await DistributedLockNode.Test()}");
            Console.WriteLine($"{nameof(SortedDictionaryNode)} {await SortedDictionaryNode.Test()}");
            Console.WriteLine($"{nameof(SortedSetNode)} {await SortedSetNode.Test()}");
            Console.WriteLine($"{nameof(SearchTreeDictionaryNode)} {await SearchTreeDictionaryNode.Test()}");
            Console.WriteLine($"{nameof(SearchTreeSetNode)} {await SearchTreeSetNode.Test()}");
            Console.WriteLine($"{nameof(HashSetNode)} {await HashSetNode.Test()}");
            Console.WriteLine($"{nameof(FragmentHashSetNode)} {await FragmentHashSetNode.Test()}");
            Console.WriteLine($"{nameof(IdentityGeneratorNode)} {await IdentityGeneratorNode.Test()}");
            Console.WriteLine($"{nameof(QueueNode)} {await QueueNode.Test()}");
            Console.WriteLine($"{nameof(StackNode)} {await StackNode.Test()}");
            Console.WriteLine($"{nameof(ArrayNode)} {await ArrayNode.Test()}");
            Console.WriteLine($"{nameof(LeftArrayNode)} {await LeftArrayNode.Test()}");
            Console.WriteLine($"{nameof(SortedListNode)} {await SortedListNode.Test()}");
            Console.WriteLine($"{nameof(BitmapNode)} {await BitmapNode.Test()}");

            Console.WriteLine("Press quit to exit.");
            while (Console.ReadLine() != "quit") ;
        }
    }
}
