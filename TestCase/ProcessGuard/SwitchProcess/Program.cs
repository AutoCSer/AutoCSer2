using AutoCSer.CommandService;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Threading;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.ProcessGuardSwitchProcess
{
    class Program : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ProcessGuardSwitchProcess
    {
        static Task Main(string[] args)
        {
            Program program = new Program(args);
            if (!program.isStart)
            {
                program.Start().NotWait();
                Console.WriteLine("Press quit to exit.");
                while (Console.ReadLine() != "quit") ;
                return program.exit();
            }
            return AutoCSer.Common.CompletedTask;
        }

        private Program(string[] args) : base(args) { }
        /// <summary>
        /// 获取进程守护节点客户端
        /// </summary>
        protected override StreamPersistenceMemoryDatabaseClientNodeCache<IProcessGuardNodeClientNode> getProcessGuardClient
        {
            get { return CommandClientSocketEvent.ProcessGuardNodeCache; }
        }
    }
}
