using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.CommandClientPerformance
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            do
            {
                //测试顺序是应用场景优先，不是性能优先
                await AwaiterClientPerformance.Test(); // await 异步客户端是推荐的常规模式
                Console.WriteLine();
                await CallbackClientPerformance.Test(); //回调客户端是最佳性能模式，但是会造成程序逻辑割裂不利于可读性
                Console.WriteLine();
                await SynchronousCllientPerformance.Test(); //非特殊场景不应该使用同步客户端模式，并发场景会产生大量线程上下文切换问题

                Console.WriteLine("Press quit to exit.");
                if (await AutoCSer.Breakpoint.ReadLineDelay() == "quit") break;
            }
            while (true);
#if AOT
            AutoCSer.TestCase.CommandClientPerformance.AotMethod.Call();
#endif
        }
    }
}
