using AutoCSer;
using AutoCSer.CommandService;
using AutoCSer.CommandService.DeployTask;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Diagnostics;
using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.DeployTaskClient
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                Console.WriteLine(@"
1 : AutoCSer.TestCase.NetCoreWeb
T : TestCase
A : AOT TestCase
S : Search TestCase
L : LocalSearch TestCase
M : MemorySearch TestCase

Press quit to exit.");
                switch (Console.ReadLine())
                {
                    case "1": netCoreWeb().NotWait(); break;
                    case "T": testCase().NotWait(); break;
                    case "A": aot().NotWait(); break;
                    case "S": search().NotWait(); break;
                    case "L": localSearch().NotWait(); break;
                    case "M": memorySearch().NotWait(); break;
                    case "quit":
                        return;
                }
            }
            while (true);
        }

        private static async Task getLog(IDeployTaskNodeClientNode node, long identity)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            await using (KeepCallbackResponse<DeployTaskLog> keepCallback = await node.GetLog(identity))
            {
                while (await keepCallback.MoveNext())
                {
                    ResponseResult<DeployTaskLog> log = keepCallback.Current;
                    Console.WriteLine($"{identity} {log.Value.TaskType}[{log.Value.TaskIndex}] {log.Value.OperationState}");
                    if (log.Value.IsFinished) return;
                }
            }
        }
        private static async Task netCoreWeb()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            string clientPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerPath, "TestCase", "NetCoreWeb");
            FileInfo file = new FileInfo(Path.Combine(clientPath, Path.Combine("bin", "Release", "net8.0", "AutoCSer.TestCase.NetCoreWeb.exe")));
            if (!await AutoCSer.Common.FileExists(file))
            {
                Console.WriteLine($"没有找到文件 {file.FullName}");
                return;
            }
            IUploadFileClientSocketEvent uploadClient = await CommandClientSocketEvent.CommandClient.SocketEvent.Wait();
            if (uploadClient == null)
            {
                Console.WriteLine($"文件上传客户端连接失败");
                return;
            }
            CommandClientReturnValue<string> serverWebPath = await uploadClient.UploadFileClient.GetPath("NetCoreWeb");
            if (!serverWebPath.IsSuccess)
            {
                Console.WriteLine($"获取服务端路径失败 {serverWebPath.ReturnType}");
                return;
            }
            CommandClientReturnValue<string> serverFileName = await uploadClient.UploadFileClient.CombinePathArray(new string[] { "bin", "Release", "net8.0", "AutoCSer.TestCase.NetCoreWeb.exe" });
            if (!serverFileName.IsSuccess)
            {
                Console.WriteLine($"获取服务端路径失败 {serverFileName.ReturnType}");
                return;
            }
            CommandClientReturnValue<SynchronousFileInfo> serverFile = await uploadClient.UploadFileClient.GetSwitchProcessPathFileInfo(serverWebPath.Value, serverWebPath.Value + ProcessInfo.DefaultSwitchDirectorySuffixName, serverFileName.Value);
            if (!serverFile.IsSuccess)
            {
                Console.WriteLine($"获取切换进程的上传目录与文件信息失败 {serverFile.ReturnType}");
                return;
            }
            if (serverFile.Value.LastWriteTime >= file.LastWriteTimeUtc)
            {
                Console.WriteLine($"服务端文件最后修改时间 {serverFile.Value.LastWriteTime.ToString("yyyy/MM/dd HH:mm:ss")} 与客户端文件最后修改时间 {file.LastWriteTimeUtc.ToString("yyyy/MM/dd HH:mm:ss")} 冲突");
                return;
            }

            string serverPath = serverFile.Value.FullName ?? serverWebPath.Value; 
            CommandClientReturnValue<string> serverCombinePath = await uploadClient.UploadFileClient.CombinePathArray(new string[] { serverPath, "bin", "Release", "net8.0" });
            if (!serverCombinePath.IsSuccess)
            {
                Console.WriteLine($"获取服务端路径失败 {serverCombinePath.ReturnType}");
                return;
            }

            UploadFileClient uploadHtmlFileClient = new UploadFileClient(uploadClient, clientPath, serverPath, new string[] { ".html", ".js", ".css", ".bmp" }, false, true, false);
            UploadFileStateEnum uploadState = await uploadHtmlFileClient.Upload();
            if (uploadState != UploadFileStateEnum.Success)
            {
                Console.WriteLine($"文件上传失败 {uploadState}");
                return;
            }
            UploadFileClient uploadAssemblyFileClient = new UploadFileClient(uploadClient, Path.Combine(clientPath, "bin", "Release", "net8.0"), serverCombinePath.Value, new string[] { ".exe", ".dll", ".pdb", ".xml", ".json" }, false, true, false);
            uploadState = await uploadAssemblyFileClient.Upload();
            if (uploadState != UploadFileStateEnum.Success)
            {
                Console.WriteLine($"文件上传失败 {uploadState}");
                return;
            }

            ResponseResult<IDeployTaskNodeClientNode> node = await CommandClientSocketEvent.DeployTaskNodeCache.GetSynchronousNode();
            if (!node.IsSuccess)
            {
                Console.WriteLine($"客户端发布节点获取失败 {node.ReturnType}.{node.CallState}");
                return;
            }
            ResponseResult<long> identity = await node.Value.Create();
            if (!identity.IsSuccess)
            {
                Console.WriteLine($"发布任务标识ID获取失败 {identity.ReturnType}.{identity.CallState}");
                return;
            }
            bool isStart = false;
            try
            {
                ResponseResult<OperationStateEnum> state = await node.Value.AppendStepTask(identity.Value, uploadHtmlFileClient);
                if (!state.IsSuccess)
                {
                    Console.WriteLine($"发布任务 {identity.Value} 添加上传完成任务失败 {state.ReturnType}.{state.CallState}");
                    return;
                }
                if (state.Value != OperationStateEnum.Success)
                {
                    Console.WriteLine($"发布任务 {identity.Value} 添加上传完成任务失败 {state.Value}");
                    return;
                }
                state = await node.Value.AppendStepTask(identity.Value, uploadAssemblyFileClient);
                if (!state.IsSuccess)
                {
                    Console.WriteLine($"发布任务 {identity.Value} 添加上传完成任务失败 {state.ReturnType}.{state.CallState}");
                    return;
                }
                if (state.Value != OperationStateEnum.Success)
                {
                    Console.WriteLine($"发布任务 {identity.Value} 添加上传完成任务失败 {state.Value}");
                    return;
                }
                state = await node.Value.AppendStepTask(identity.Value, Path.Combine(serverPath, serverFileName.Value));
                if (!state.IsSuccess)
                {
                    Console.WriteLine($"发布任务 {identity.Value} 添加启动进程任务失败 {state.ReturnType}.{state.CallState}");
                    return;
                }
                if (state.Value != OperationStateEnum.Success)
                {
                    Console.WriteLine($"发布任务 {identity.Value} 添加启动进程任务失败 {state.Value}");
                    return;
                }
                getLog(node.Value, identity.Value).NotWait();
                state = await node.Value.Start(identity.Value);
                if (!state.IsSuccess)
                {
                    Console.WriteLine($"发布任务 {identity.Value} 启动失败 {state.ReturnType}.{state.CallState}");
                    return;
                }
                if (state.Value != OperationStateEnum.Success)
                {
                    Console.WriteLine($"发布任务 {identity.Value} 启动失败 {state.Value}");
                    return;
                }
                isStart = true;
            }
            finally
            {
                if (!isStart) await node.Value.Remove(identity.Value);
            }
        }
        private static async Task testCase()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            try {
                await waitProcess(@"Document\SymmetryService\bin\Release\net8.0\AutoCSer.Document.SymmetryService.exe");
                await waitProcess(@"Document\ServiceDataSerialize\bin\Release\net8.0\AutoCSer.Document.ServiceDataSerialize.exe");
                await waitProcess(@"Document\ServiceThreadStrategy\bin\Release\net8.0\AutoCSer.Document.ServiceThreadStrategy.exe");
                await waitProcess(@"Document\ServiceAuthentication\bin\Release\net8.0\AutoCSer.Document.ServiceAuthentication.exe");
                await waitProcess(@"Document\MemoryDatabaseNode\bin\Release\net8.0\AutoCSer.Document.MemoryDatabaseNode.exe", 2);
                await waitProcess(@"Document\MemoryDatabaseCustomNode\bin\Release\net8.0\AutoCSer.Document.MemoryDatabaseCustomNode.exe", 2);
                await waitProcess(@"Document\MemoryDatabaseLocalService\bin\Release\net8.0\AutoCSer.Document.MemoryDatabaseLocalService.exe", 2);
                await waitProcess(@"Document\ServerRegistry\bin\Release\net8.0\AutoCSer.Document.ServerRegistry.exe", 2);
                await waitProcess(@"Document\ReverseServer\bin\Release\net8.0\AutoCSer.Document.ReverseServer.exe");
                await waitProcess(@"TestCase\TestCase\bin\Release\net8.0\AutoCSer.TestCase.exe");
                await aot();

                await waitProcess(@"TestCase\TimestampVerify\bin\Release\net8.0\AutoCSer.TestCase.TimestampVerify.exe", @"TestCase\TimestampVerify\Client\bin\Release\net8.0\AutoCSer.TestCase.TimestampVerifyClient.exe");
                await waitProcess(@"TestCase\ServerRegistry\bin\Release\net8.0\AutoCSer.TestCase.ServerRegistry.exe", @"TestCase\ServerRegistry\Service\bin\Release\net8.0\AutoCSer.TestCase.ServerRegistryService.exe", @"TestCase\ServerRegistry\Service\Client\bin\Release\net8.0\AutoCSer.TestCase.ServerRegistryServiceClient.exe");
                await waitProcess(@"TestCase\ProcessGuard\bin\Release\net8.0\AutoCSer.TestCase.ProcessGuard.exe", @"TestCase\ProcessGuard\Client\bin\Release\net8.0\AutoCSer.TestCase.ProcessGuardClient.exe");
                await waitProcess(@"TestCase\ProcessGuard\bin\Release\net8.0\AutoCSer.TestCase.ProcessGuard.exe", @"TestCase\ProcessGuard\SwitchProcess\bin\Release\net8.0\AutoCSer.TestCase.ProcessGuardSwitchProcess.exe");
                await waitProcess(@"TestCase\ServerRegistry\bin\Release\net8.0\AutoCSer.TestCase.ServerRegistry.exe", @"TestCase\ReverseLogCollection\bin\Release\net8.0\AutoCSer.TestCase.ReverseLogCollection.exe", @"TestCase\ReverseLogCollection\Client\bin\Release\net8.0\AutoCSer.TestCase.ReverseLogCollectionClient.exe");
                await waitProcess(@"TestCase\ServerRegistry\bin\Release\net8.0\AutoCSer.TestCase.ServerRegistry.exe", @"TestCase\ReverseLogCollection\ReverseService\bin\Release\net8.0\AutoCSer.TestCase.LogCollectionReverseService.exe", @"TestCase\ReverseLogCollection\ReverseClient\bin\Release\net8.0\AutoCSer.TestCase.LogCollectionReverseClient.exe");
                await waitProcess(@"TestCase\FileSynchronous\bin\Release\net8.0\AutoCSer.TestCase.FileSynchronous.exe", @"TestCase\FileSynchronous\Client\bin\Release\net8.0\AutoCSer.TestCase.FileSynchronousClient.exe");
                await waitProcess2(@"TestCase\DiskBlock\bin\Release\net8.0\AutoCSer.TestCase.DiskBlock.exe", @"TestCase\DiskBlock\Client\bin\Release\net8.0\AutoCSer.TestCase.DiskBlockClient.exe", 2);
                await waitProcess(@"TestCase\ProcessGuard\bin\Release\net8.0\AutoCSer.TestCase.ProcessGuard.exe", @"TestCase\InterfaceRealTimeCallMonitor\ExceptionStatistics\bin\Release\net8.0\AutoCSer.TestCase.ExceptionStatistics.exe", @"TestCase\InterfaceRealTimeCallMonitor\bin\Release\net8.0\AutoCSer.TestCase.InterfaceRealTimeCallMonitor.exe", @"TestCase\NetCoreWeb\bin\Release\net8.0\AutoCSer.TestCase.NetCoreWeb.exe");

                await localSearch();
                await localSearch();
                await memorySearch();
                await memorySearch();
                await search();
                await search();

                await waitProcess(@"TestCase\CommandServerPerformance\bin\Release\net8.0\AutoCSer.TestCase.CommandServerPerformance.exe", @"TestCase\CommandServerPerformance\Client\bin\Release\net8.0\AutoCSer.TestCase.CommandClientPerformance.exe");
                await waitProcess(@"TestCase\CommandServerPerformance\bin\Release\net8.0\AutoCSer.TestCase.CommandServerPerformance.exe", @"TestCase\CommandServerPerformance\Client\bin\Release\net8.0\publish\AutoCSer.TestCase.CommandClientPerformance.AOT.exe");
                await waitProcess2(@"TestCase\StreamPersistenceMemoryDatabase\Performance\bin\Release\net8.0\AutoCSer.TestCase.StreamPersistenceMemoryDatabasePerformance.exe", @"C:\AutoCSer2\TestCase\StreamPersistenceMemoryDatabase\PerformanceClient\bin\Release\net8.0\AutoCSer.TestCase.StreamPersistenceMemoryDatabaseClientPerformance.exe", 2);
                await waitProcess(@"TestCase\StreamPersistenceMemoryDatabase\LocalService\bin\Release\net8.0\AutoCSer.TestCase.StreamPersistenceMemoryDatabaseLocalService.exe", 2);
                await waitProcess2(@"TestCase\StreamPersistenceMemoryDatabase\bin\Release\net8.0\AutoCSer.TestCase.StreamPersistenceMemoryDatabase.exe", @"TestCase\StreamPersistenceMemoryDatabase\Client\bin\Release\net8.0\AutoCSer.TestCase.StreamPersistenceMemoryDatabaseClient.exe", 2);

                await waitProcess(@"TestCase\ProcessGuard\bin\Release\net8.0\AutoCSer.TestCase.ProcessGuard.exe", @"TestCase\DeployTask\bin\Release\net8.0\AutoCSer.TestCase.DeployTask.exe");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
        private static Task search()
        {
            return waitProcess(@"TestCase\Search\TrieGraph\bin\Release\net8.0\AutoCSer.TestCase.SearchTrieGraph.exe", @"TestCase\DiskBlock\bin\Release\net8.0\AutoCSer.TestCase.DiskBlock.exe", @"TestCase\Search\DataSource\bin\Release\net8.0\AutoCSer.TestCase.SearchDataSource.exe", @"TestCase\Search\DiskBlockIndex\bin\Release\net8.0\AutoCSer.TestCase.SearchDiskBlockIndex.exe", @"TestCase\Search\WordIdentityBlockIndex\bin\Release\net8.0\AutoCSer.TestCase.SearchWordIdentityBlockIndex.exe", @"TestCase\Search\QueryService\bin\Release\net8.0\AutoCSer.TestCase.SearchQueryService.exe", @"TestCase\Search\bin\Release\net8.0\AutoCSer.TestCase.Search.exe");
        }
        private static Task localSearch()
        {
            return waitProcess(@"TestCase\Search\LocalDataSource\bin\Release\net8.0\AutoCSer.TestCase.LocalSearchDataSource.exe", @"TestCase\Search\LocalQueryService\bin\Release\net8.0\AutoCSer.TestCase.LocalSearchQueryService.exe", @"TestCase\Search\LocalSearchClient\bin\Release\net8.0\AutoCSer.TestCase.LocalSearchClient.exe");
        }
        private static Task memorySearch()
        {
            return waitProcess(@"TestCase\Search\MemorySearchDataSource\bin\Release\net8.0\AutoCSer.TestCase.MemorySearchDataSource.exe", @"TestCase\Search\MemorySearchQueryService\bin\Release\net8.0\AutoCSer.TestCase.MemorySearchQueryService.exe", @"TestCase\Search\MemorySearchClient\bin\Release\net8.0\AutoCSer.TestCase.MemorySearchClient.exe");
        }
        private static async Task aot()
        {
            ProcessArguments publishFile = @"TestCase\TestCase\bin\Release\net8.0\publish\AutoCSer.TestCase.AOT.exe";
            if (!await publishFile.FileExists()) publishFile = @"TestCase\TestCase\bin\Release\net8.0\publish\win-x64\AutoCSer.TestCase.AOT.exe";
            await waitProcess(new ProcessArguments(@"TestCase\TestCase\bin\Release\net8.0\AutoCSer.TestCase.exe", AutoCSer.TestCase.Common.Config.AotClientArgument), publishFile);
        }
        private static async Task waitProcess(ProcessArguments fileName, int count = 1)
        {
            while (--count >= 0)
            {
                Process process = await fileName.Start();
                if (process != null)
                {
                    using (process) await wait(fileName, process);
                }
                else Console.WriteLine("Not Found File");
            }
        }
        private static async Task wait(ProcessArguments fileName, Process process)
        {
            await process.WaitForExitAsync();
            FileInfo file = fileName.GetLogFileInfo();
            if (await AutoCSer.Common.FileExists(file))
            {
                try
                {
                    using (process = await new ProcessInfo(file.FullName).StartAsync()) await process.WaitForExitAsync();
                    await AutoCSer.Common.DeleteFile(file);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }
        }
        private static async Task waitProcess2(ProcessArguments serverFileName, ProcessArguments clientFileName, int count)
        {
            await waitProcess(serverFileName, clientFileName, count);
            await waitProcess(serverFileName, clientFileName, count);
        }
        private static async Task waitProcess(ProcessArguments serverFileName, ProcessArguments clientFileName, int count)
        {
            Process process = await serverFileName.Start();
            if (process != null)
            {
                using (process)
                {
                    await waitProcess(clientFileName, count);
                    await wait(serverFileName, process);
                }
            }
            else Console.WriteLine("Not Found File");
        }
        private static Task waitProcess(params ProcessArguments[] fileNames)
        {
            return waitProcess(fileNames, 0);
        }
        private static async Task waitProcess(ProcessArguments[] fileNames, int index)
        {
            ProcessArguments fileName = fileNames[index];
            Process process = await fileName.Start();
            if (process != null)
            {
                using (process)
                {
                    if(++index != fileNames.Length) await waitProcess(fileNames, index);
                    await wait(fileName, process);
                }
            }
            else Console.WriteLine("Not Found File");
        }
    }
}
