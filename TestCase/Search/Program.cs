using AutoCSer.CommandService.Search.DiskBlockIndex;
using AutoCSer.CommandService.Search.StaticTrieGraph;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Net;
using AutoCSer.TestCase.SearchDataSource;
using AutoCSer.TestCase.SearchQueryService;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AutoCSer.TestCase.Search
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            DataSourceCommandClientSocketEvent dataSourceClient = (DataSourceCommandClientSocketEvent)await DataSourceCommandClientSocketEvent.CommandClient.Client.GetSocketEvent();
            if (dataSourceClient == null)
            {
                AutoCSer.ConsoleWriteQueue.Breakpoint("dataSourceClient is null");
            }
            else
            {
                await query(dataSourceClient, new UserQueryParameter { Name = "AutoCSer", Remark = "现在", Gender = GenderEnum.Male });
                await query(dataSourceClient, new UserQueryParameter { Gender = GenderEnum.Male });
                await query(dataSourceClient, new UserQueryParameter { Order = UserOrderEnum.IdDesc });
                await query(dataSourceClient, new UserQueryParameter { Order = UserOrderEnum.LoginTimeDesc });
            }
            Console.WriteLine("Press quit to exit.");
            while (Console.ReadLine() != "quit") ;
        }
        private static async Task query(DataSourceCommandClientSocketEvent dataSourceClient, UserQueryParameter queryParameter)
        {
            Console.WriteLine(AutoCSer.JsonSerializer.Serialize(queryParameter));
            CommandClientReturnValue<PageResult<User>> users = await dataSourceClient.UserClient.GetPage(queryParameter);
            if (!users.IsSuccess)
            {
                AutoCSer.ConsoleWriteQueue.Breakpoint(users.ReturnType);
                return;
            }
            if (!users.Value.IsSuccess)
            {
                AutoCSer.ConsoleWriteQueue.Breakpoint($"{users.Value.ReturnType} {users.Value.CallState}");
                return;
            }
            Console.WriteLine($"{users.Value.Values.Length} {nameof(users.Value.PageIndex)}[{users.Value.PageIndex}] {nameof(users.Value.PageSize)}[{users.Value.PageSize}] {nameof(users.Value.TotalCount)}[{users.Value.TotalCount}]");
            foreach (User user in users.Value.Values) Console.WriteLine(AutoCSer.JsonSerializer.Serialize(user));
            Console.WriteLine();
        }
    }
}
