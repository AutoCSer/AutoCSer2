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
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.DeployTaskClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            do
            {
                Console.WriteLine(@"
0 : AutoCSer.TestCase.ServerRegistry
1 : AutoCSer.TestCase.ExceptionStatistics
2 : InterfaceRealTimeCallMonitor
3 : AutoCSer.TestCase.NetCoreWeb

T : TestCase
A : AOT TestCase
S : Search TestCase
L : LocalSearch TestCase
M : MemorySearch TestCase

Z : AutoCSer2.zip
Press quit to exit.");
                switch (Console.ReadLine())
                {
                    case "0": await deploy("ServerRegistry"); break;
                    case "1": await deploy("InterfaceRealTimeCallMonitor", "ExceptionStatistics"); break;
                    case "2": await deploy("InterfaceRealTimeCallMonitor"); break;
                    case "3": await netCoreWeb(); break;

                    case "T": await testCase(); break;
                    case "A": await aot(); break;
                    case "S": await search(); break;
                    case "L": await localSearch(); break;
                    case "M": await memorySearch(); break;
                    case "Z": await zip(); break;
                    case "quit": return;
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
        private static Task deploy(string type)
        {
            return deployAssembly(type, Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerPath, "TestCase", type, "bin", "Release", "net8.0"));
        }
        private static Task deploy(string testCaseDirectoryName, string type)
        {
            return deployAssembly(type, Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerPath, "TestCase", testCaseDirectoryName, type, "bin", "Release", "net8.0"));
        }
        private static async Task deployAssembly(string type, string clientPath)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            string fileName = AutoCSer.TestCase.Common.JsonFileConfig.Default.IsLinuxServer ? $"AutoCSer.TestCase.{type}.dll" : $"AutoCSer.TestCase.{type}.exe";
            FileInfo file = new FileInfo(Path.Combine(clientPath, fileName));
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
            CommandClientReturnValue<string> serverAssemblyPath = await uploadClient.UploadFileClient.GetPath(type);
            if (!serverAssemblyPath.IsSuccess)
            {
                Console.WriteLine($"获取服务端路径失败 {serverAssemblyPath.ReturnType}");
                return;
            }
            CommandClientReturnValue<string> serverFileName = fileName;
            CommandClientReturnValue<SynchronousFileInfo> serverFile = await uploadClient.UploadFileClient.GetSwitchProcessPathFileInfo(serverAssemblyPath.Value, serverAssemblyPath.Value + ProcessInfo.DefaultSwitchDirectorySuffixName, serverFileName.Value);
            if (!serverFile.IsSuccess)
            {
                Console.WriteLine($"获取切换进程的上传目录与文件信息失败 {serverFile.ReturnType}");
                return;
            }
            if (serverFile.Value.LastWriteTime > file.LastWriteTimeUtc)
            {
                Console.WriteLine($"服务端文件最后修改时间 {serverFile.Value.LastWriteTime.ToString("yyyy/MM/dd HH:mm:ss")} 与客户端文件最后修改时间 {file.LastWriteTimeUtc.ToString("yyyy/MM/dd HH:mm:ss")} 冲突");
                return;
            }

            string serverPath = serverFile.Value.FullName ?? serverAssemblyPath.Value;
            UploadFileClient uploadAssemblyFileClient = new UploadFileClient(uploadClient, clientPath, serverPath, new string[] { ".exe", ".dll", ".pdb", ".xml", ".json" }, false, true, false);
            UploadFileStateEnum uploadState = await uploadAssemblyFileClient.Upload();
            if (uploadState != UploadFileStateEnum.Success)
            {
                Console.WriteLine($"文件上传失败 {uploadState}");
                return;
            }
            serverFileName = await uploadClient.UploadFileClient.CombinePath(serverPath, serverFileName.Value);
            if (!serverFileName.IsSuccess)
            {
                Console.WriteLine($"服务端文件名称获取失败 {serverFileName.ReturnType}");
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
                ResponseResult<OperationStateEnum> state = await node.Value.AppendStepTask(identity.Value, uploadAssemblyFileClient);
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
                state = await node.Value.AppendStepTask(identity.Value, serverFileName.Value);
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
                Console.WriteLine(serverFileName.Value);
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
        private static async Task netCoreWeb()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            string clientPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerPath, "TestCase", "NetCoreWeb");
            string fileName = AutoCSer.TestCase.Common.JsonFileConfig.Default.IsLinuxServer ? "AutoCSer.TestCase.NetCoreWeb.dll" : "AutoCSer.TestCase.NetCoreWeb.exe";
            FileInfo file = new FileInfo(Path.Combine(clientPath, Path.Combine("bin", "Release", "net8.0", fileName)));
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
            CommandClientReturnValue<string> serverFileName = await uploadClient.UploadFileClient.CombinePathArray(new string[] { "bin", "Release", "net8.0", fileName });
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
            if (serverFile.Value.LastWriteTime > file.LastWriteTimeUtc)
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
            serverFileName = await uploadClient.UploadFileClient.CombinePath(serverPath, serverFileName.Value);
            if (!serverFileName.IsSuccess)
            {
                Console.WriteLine($"服务端文件名称获取失败 {serverFileName.ReturnType}");
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
                state = await node.Value.AppendStepTask(identity.Value, serverFileName.Value);
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
                Console.WriteLine(serverFileName.Value);
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
        private static async Task zip()
        {
            FileInfo zipFile = new FileInfo(@"C:\Showjim\AutoCSer2.zip");
            await using (FileStream stream = new FileStream(zipFile.FullName, FileMode.Create))
            using (ZipArchive zipArchive = new ZipArchive(stream, ZipArchiveMode.Create, true))
            {
                DirectoryInfo directory = new DirectoryInfo(@"C:\AutoCSer2");
                string[] githubPaths = new string[] { @"C:\AutoCSer2\Github\", @"C:\AutoCSer2\AtomGit\" };
                foreach (FileInfo file in await AutoCSer.Common.DirectoryGetFiles(directory))
                {
                    await using (Stream entryStream = zipArchive.CreateEntry(file.Name).Open()) await githubFile(file, entryStream, githubPaths);
                }
                foreach (DirectoryInfo nextDircectory in await AutoCSer.Common.GetDirectories(directory))
                {
                    switch (nextDircectory.Name.ToLower())
                    {
                        case "autocser":
                        case ".vs":
                        case "application":
                        case "testcase":
                        case "document":
                            await zipDirectory(zipArchive, nextDircectory.Name + @"\", nextDircectory, githubPaths);
                            break;
                    }
                }
            }
            Console.WriteLine(zipFile.FullName);
        }
        private static async Task githubFile(FileInfo file, Stream entryStream, string[] githubPaths)
        {
            byte[] data = await File.ReadAllBytesAsync(file.FullName);
            switch (file.Extension)
            {
                case ".cs":
                case ".ts":
                //case ".js":
                case ".html":
                    if (data.Length < 3 || data[0] != 0xef || data[1] != 0xbb || data[2] != 0xbf)
                    {
                        Console.WriteLine("文件 " + file.FullName + " 缺少 UTF-8 BOM");
                        string notepad = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.System), "notepad.exe");
                        using (Process process = await AutoCSer.Common.FileExists(notepad) ? Process.Start(notepad, file.FullName) : Process.Start(file.FullName)) await process.WaitForExitAsync();
                        data = await File.ReadAllBytesAsync(file.FullName);
                    }
                    break;
            }
            await entryStream.WriteAsync(data, 0, data.Length);
            foreach (string githubPath in githubPaths)
            {
                FileInfo githubFile = new FileInfo(Path.Combine(githubPath, file.Name));
                bool isChanged = !githubFile.Exists || githubFile.Length != data.Length;
                if (!isChanged)
                {
                    byte[] githubData = await File.ReadAllBytesAsync(githubFile.FullName);
                    isChanged = !data.AsSpan().SequenceEqual(githubData);
                }
                if (isChanged) await File.WriteAllBytesAsync(githubFile.FullName, data);
            }
        }
        private static async Task<string[]> nextGithubPath(DirectoryInfo directory, string[] githubPaths)
        {
            int index = 0;
            string[] nextGithubPaths = new string[githubPaths.Length];
            foreach (string githubPath in githubPaths)
            {
                string path = Path.Combine(githubPath, directory.Name);
                await AutoCSer.Common.TryCreateDirectory(path);
                nextGithubPaths[index++] = path;
            }
            return nextGithubPaths;
        }
        private static async Task zipDirectory(ZipArchive zipArchive, string zipPath, DirectoryInfo directory, string[] githubPaths)
        {
            githubPaths = await nextGithubPath(directory, githubPaths);
            foreach (FileInfo file in await AutoCSer.Common.DirectoryGetFiles(directory))
            {
                string fileName = file.Name;
                bool isDelete = false, isZip = true;
                if (fileName[0] == '%') isDelete = true;
                else
                {
                    switch (file.Extension)
                    {
                        case ".log":
                            if (fileName.StartsWith("AutoCSer", StringComparison.OrdinalIgnoreCase)) isDelete = true;
                            break;
                        case ".json":
                            if (fileName == "launchSettings.json")
                            {
                                if (await File.ReadAllTextAsync(file.FullName, System.Text.Encoding.UTF8) == @"{
  ""profiles"": {
    ""WSL"": {
      ""commandName"": ""WSL2"",
      ""distributionName"": """"
    }
  }
}")
                                {
                                    isDelete = true;
                                }
                            }
                            else if (fileName.EndsWith(".runtimeconfig.dev.json", StringComparison.OrdinalIgnoreCase) || fileName.EndsWith(".runtimeconfig.json", StringComparison.OrdinalIgnoreCase)) isZip = false;
                            break;
                    }
                }
                if (isDelete)
                {
                    file.Attributes = 0;
                    file.Delete();
                }
                else if (isZip)
                {
                    await using (Stream entryStream = zipArchive.CreateEntry(zipPath + fileName).Open()) await githubFile(file, entryStream, githubPaths);
                }
            }
            foreach (DirectoryInfo nextDircectory in await AutoCSer.Common.GetDirectories(directory))
            {
                switch (nextDircectory.Name.ToLower())
                {
                    case "bin":
                    case "obj":
                        break;
                    default: await zipDirectory(zipArchive, zipPath + nextDircectory.Name + @"\", nextDircectory, githubPaths); break;
                }
            }
        }

        private static async Task testCase()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            try
            {
                ProcessArguments gif = new ProcessArguments(@"TestCase\CopyScreenGif\bin\Release\net8.0-windows\publish\AutoCSer.TestCase.CopyScreenGif.AOT.exe");
                bool gifProcess = await gif.Start();
                await waitProcess(@"Document\SymmetryService\bin\Release\net8.0\AutoCSer.Document.SymmetryService.exe");
                await waitProcess(@"Document\ServiceDataSerialize\bin\Release\net8.0\AutoCSer.Document.ServiceDataSerialize.exe");
                await waitProcess(@"Document\ServiceThreadStrategy\bin\Release\net8.0\AutoCSer.Document.ServiceThreadStrategy.exe");
                await waitProcess(@"Document\ServiceAuthentication\bin\Release\net8.0\AutoCSer.Document.ServiceAuthentication.exe");
                await waitProcess(@"Document\MemoryDatabaseNode\bin\Release\net8.0\AutoCSer.Document.MemoryDatabaseNode.exe", 2);
                await waitProcess(@"Document\MemoryDatabaseCustomNode\bin\Release\net8.0\AutoCSer.Document.MemoryDatabaseCustomNode.exe", 2);
                await waitProcess(@"Document\MemoryDatabaseLocalService\bin\Release\net8.0\AutoCSer.Document.MemoryDatabaseLocalService.exe", 2);
                await waitProcess(@"Document\ServerRegistry\bin\Release\net8.0\AutoCSer.Document.ServerRegistry.exe", 2);
                await waitProcess(@"Document\ReverseServer\bin\Release\net8.0\AutoCSer.Document.ReverseServer.exe");
                if (gifProcess) await gif.WaitLogFile();

                await waitProcess(@"TestCase\TestCase\bin\Release\net8.0\AutoCSer.TestCase.exe");
                await aot();

                await waitProcess(@"TestCase\TimestampVerify\bin\Release\net8.0\AutoCSer.TestCase.TimestampVerify.exe", @"TestCase\TimestampVerify\Client\bin\Release\net8.0\AutoCSer.TestCase.TimestampVerifyClient.exe", await getAotPublishFile(@"TestCase\TimestampVerify\Client\bin\Release\net8.0\publish\AutoCSer.TestCase.TimestampVerifyClient.AOT.exe"));
                await waitProcess(@"TestCase\ServerRegistry\bin\Release\net8.0\AutoCSer.TestCase.ServerRegistry.exe", @"TestCase\ServerRegistry\Service\bin\Release\net8.0\AutoCSer.TestCase.ServerRegistryService.exe", @"TestCase\ServerRegistry\Service\Client\bin\Release\net8.0\AutoCSer.TestCase.ServerRegistryServiceClient.exe");
                await waitProcess(@"TestCase\ProcessGuard\bin\Release\net8.0\AutoCSer.TestCase.ProcessGuard.exe", @"TestCase\ProcessGuard\Client\bin\Release\net8.0\AutoCSer.TestCase.ProcessGuardClient.exe");
                await waitProcess(@"TestCase\ProcessGuard\bin\Release\net8.0\AutoCSer.TestCase.ProcessGuard.exe", @"TestCase\ProcessGuard\SwitchProcess\bin\Release\net8.0\AutoCSer.TestCase.ProcessGuardSwitchProcess.exe");
                await waitProcess(@"TestCase\ServerRegistry\bin\Release\net8.0\AutoCSer.TestCase.ServerRegistry.exe", @"TestCase\ReverseLogCollection\bin\Release\net8.0\AutoCSer.TestCase.ReverseLogCollection.exe", @"TestCase\ReverseLogCollection\Client\bin\Release\net8.0\AutoCSer.TestCase.ReverseLogCollectionClient.exe");
                await waitProcess(@"TestCase\ServerRegistry\bin\Release\net8.0\AutoCSer.TestCase.ServerRegistry.exe", @"TestCase\ReverseLogCollection\ReverseService\bin\Release\net8.0\AutoCSer.TestCase.LogCollectionReverseService.exe", @"TestCase\ReverseLogCollection\ReverseClient\bin\Release\net8.0\AutoCSer.TestCase.LogCollectionReverseClient.exe");
                await waitProcess(@"TestCase\FileSynchronous\bin\Release\net8.0\AutoCSer.TestCase.FileSynchronous.exe", await getAotPublishFile(@"TestCase\FileSynchronous\Client\bin\Release\net8.0\publish\AutoCSer.TestCase.FileSynchronousClient.AOT.exe"));
                await waitProcess(@"TestCase\FileSynchronous\bin\Release\net8.0\AutoCSer.TestCase.FileSynchronous.exe", @"TestCase\FileSynchronous\Client\bin\Release\net8.0\AutoCSer.TestCase.FileSynchronousClient.exe");
                await waitProcess2(@"TestCase\DiskBlock\bin\Release\net8.0\AutoCSer.TestCase.DiskBlock.exe", @"TestCase\DiskBlock\Client\bin\Release\net8.0\AutoCSer.TestCase.DiskBlockClient.exe", 2);
                await waitProcess2(@"TestCase\DiskBlock\bin\Release\net8.0\AutoCSer.TestCase.DiskBlock.exe", await getAotPublishFile(@"TestCase\DiskBlock\Client\bin\Release\net8.0\publish\AutoCSer.TestCase.DiskBlockClient.AOT.exe"), 2);
                await waitProcess(@"TestCase\ProcessGuard\bin\Release\net8.0\AutoCSer.TestCase.ProcessGuard.exe", @"TestCase\InterfaceRealTimeCallMonitor\ExceptionStatistics\bin\Release\net8.0\AutoCSer.TestCase.ExceptionStatistics.exe", @"TestCase\InterfaceRealTimeCallMonitor\bin\Release\net8.0\AutoCSer.TestCase.InterfaceRealTimeCallMonitor.exe", @"TestCase\NetCoreWeb\bin\Release\net8.0\AutoCSer.TestCase.NetCoreWeb.exe");

                await localSearch();
                await localSearch();
                await memorySearch();
                await memorySearch();
                await search();
                await search();

                await getAotPublishFile(@"TestCase\SerializePerformance\bin\Release\net8.0\publish\AutoCSer.TestCase.SerializePerformance.AOT.exe");
                await waitProcess(@"TestCase\CommandServerPerformance\bin\Release\net8.0\AutoCSer.TestCase.CommandServerPerformance.exe", @"TestCase\CommandServerPerformance\Client\bin\Release\net8.0\AutoCSer.TestCase.CommandClientPerformance.exe");
                await waitProcess(@"TestCase\CommandServerPerformance\bin\Release\net8.0\AutoCSer.TestCase.CommandServerPerformance.exe", @"TestCase\CommandServerPerformance\Client\bin\Release\net8.0\publish\AutoCSer.TestCase.CommandClientPerformance.AOT.exe");
                await waitProcess2(@"TestCase\StreamPersistenceMemoryDatabase\Performance\bin\Release\net8.0\AutoCSer.TestCase.StreamPersistenceMemoryDatabasePerformance.exe", @"C:\AutoCSer2\TestCase\StreamPersistenceMemoryDatabase\PerformanceClient\bin\Release\net8.0\AutoCSer.TestCase.StreamPersistenceMemoryDatabaseClientPerformance.exe", 2);
                await waitProcess(@"TestCase\StreamPersistenceMemoryDatabase\LocalService\bin\Release\net8.0\AutoCSer.TestCase.StreamPersistenceMemoryDatabaseLocalService.exe", 2);
                await waitProcess(await getAotPublishFile(@"TestCase\StreamPersistenceMemoryDatabase\LocalService\bin\Release\net8.0\publish\AutoCSer.TestCase.StreamPersistenceMemoryDatabaseLocalService.AOT.exe"), 2);
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
            await waitProcess(new ProcessArguments(@"TestCase\TestCase\bin\Release\net8.0\AutoCSer.TestCase.exe", AutoCSer.TestCase.Common.Config.AotClientArgument), await getAotPublishFile(@"TestCase\TestCase\bin\Release\net8.0\publish\AutoCSer.TestCase.AOT.exe"));
        }
        private static async Task waitProcess(ProcessArguments fileName, int count = 1)
        {
            while (--count >= 0)
            {
                if (await fileName.Start()) await fileName.WaitLogFile();
                else Console.WriteLine("Not Found File");
            }
        }
        private static async Task waitProcess2(ProcessArguments serverFileName, ProcessArguments clientFileName, int count)
        {
            await waitProcess(serverFileName, clientFileName, count);
            await waitProcess(serverFileName, clientFileName, count);
        }
        private static async Task waitProcess(ProcessArguments serverFileName, ProcessArguments clientFileName, int count)
        {
            if (await serverFileName.Start())
            {
                await waitProcess(clientFileName, count);
                await serverFileName.WaitLogFile();
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
            if (await fileName.Start())
            {
                if (++index != fileNames.Length) await waitProcess(fileNames, index);
                await fileName.WaitLogFile();
            }
            else Console.WriteLine("Not Found File");
        }
        private static async Task<ProcessArguments> getAotPublishFile(string fileName)
        {
            //C:\AutoCSer2\TestCase\CommandServerPerformance\Client\bin\Release\net8.0\win-x64\publish\AutoCSer.TestCase.CommandClientPerformance.AOT.exe
            ProcessArguments publishFile = fileName, win64PublishFile = fileName.Insert(fileName.LastIndexOf(@"\publish\"), @"\win-x64");
            FileInfo publishFileInfo = publishFile.GetFileInfo(), win64PublishFileInfo = win64PublishFile.GetFileInfo();
            if (await AutoCSer.Common.FileExists(win64PublishFileInfo))
            {
                if (!await AutoCSer.Common.FileExists(publishFileInfo) || win64PublishFileInfo.LastWriteTimeUtc > publishFileInfo.LastWriteTimeUtc) return win64PublishFile;
            }
            return publishFile;
        }
    }
}
