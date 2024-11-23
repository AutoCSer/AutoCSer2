using AutoCSer.CommandService.FileSynchronous;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.FileSynchronousClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            CommandClientConfig commandClientConfig = new CommandClientConfig
            {
                Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.FileSynchronous),
                ControllerCreatorBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client)
            };
            using (CommandClient commandClient = new CommandClient(commandClientConfig))
            {
                IPullFileClientSocketEvent pullClient = (IPullFileClientSocketEvent)await commandClient.GetSocketEvent();
                if (pullClient == null)
                {
                    ConsoleWriteQueue.Breakpoint("ERROR");
                    Console.ReadKey();
                    return;
                }

                if (AutoCSer.TestCase.Common.Config.AutoCSerPath != null)
                {
                    PullFileClient pullFileClient = new PullFileClient(pullClient);
                    string serverPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerPath, AutoCSer.Common.NamePrefix);
                    string clientPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryPath, nameof(AutoCSer.CommandService.FileSynchronous), nameof(PullFileClient), AutoCSer.Common.NamePrefix);
                    PullFileStateEnum pullState = await pullFileClient.PullDirectory(serverPath, clientPath);
                    if (pullState == PullFileStateEnum.Success) await compare(serverPath, clientPath);
                    else AutoCSer.ConsoleWriteQueue.Breakpoint($"拉取文件失败 {pullState}");

                    IUploadFileClientSocketEvent uploadClient = (IUploadFileClientSocketEvent)commandClient.SocketEvent;
                    serverPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryPath, nameof(AutoCSer.CommandService.FileSynchronous), nameof(UploadFileClient), AutoCSer.Common.NamePrefix);
                    UploadFileClient uploadFileClient = new UploadFileClient(uploadClient, clientPath, serverPath);
                    UploadFileStateEnum uploadState = await uploadFileClient.Upload();
                    if (uploadState == UploadFileStateEnum.Success) await compare(clientPath, serverPath);
                    else AutoCSer.ConsoleWriteQueue.Breakpoint($"拉取上传失败 {uploadState}");
                }
                else AutoCSer.ConsoleWriteQueue.Breakpoint("没有找到目标目录");
                Console.WriteLine("Press quit to exit.");
                while (Console.ReadLine() != "quit") ;
            }
        }
        private static async Task compare(string leftPath, string rightPath)
        {
            DirectoryInfo leftDirectory = new DirectoryInfo(leftPath), rightDirectory = new DirectoryInfo(rightPath);
            if (await AutoCSer.Common.DirectoryExists(rightDirectory)) await compare(leftDirectory, rightDirectory);
            else ConsoleWriteQueue.Breakpoint($"没有找到文件比较路径 {rightDirectory.FullName}");
        }
        private static readonly byte[] leftBuffer = new byte[1 << 20];
        private static readonly byte[] rightBuffer = new byte[1 << 20];
        private static readonly Dictionary<HashString, FileInfo> emptyFiles = DictionaryCreator.CreateHashString<FileInfo>(0);
        private static readonly Dictionary<HashString, DirectoryInfo> emptyDirectorys = DictionaryCreator.CreateHashString<DirectoryInfo>(0);
        private static async Task compare(DirectoryInfo leftDirectory, DirectoryInfo rightDirectory)
        {
            Dictionary<HashString, FileInfo> rightFiles = emptyFiles;
            FileInfo[] files = await AutoCSer.Common.DirectoryGetFiles(rightDirectory);
            if (files.Length != 0)
            {
                rightFiles = DictionaryCreator.CreateHashString<FileInfo>(files.Length);
                foreach (FileInfo file in files) rightFiles.Add(file.Name, file);
            }
            foreach(FileInfo file in await AutoCSer.Common.DirectoryGetFiles(leftDirectory))
            {
                if (rightFiles.Remove(file.Name, out FileInfo rightFile))
                {
                    if (file.Length == rightFile.Length)
                    {
                        if (file.LastWriteTimeUtc == rightFile.LastWriteTimeUtc)
                        {
                            long unreadSize = file.Length;
                            if (unreadSize != 0)
                            {
                                await using (FileStream leftStream = await AutoCSer.Common.CreateFileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.None, 4 << 10, FileOptions.SequentialScan))
                                await using (FileStream rightStream = await AutoCSer.Common.CreateFileStream(rightFile.FullName, FileMode.Open, FileAccess.Read, FileShare.None, 4 << 10, FileOptions.SequentialScan))
                                {
                                    do
                                    {
                                        int readSize = await leftStream.ReadAsync(leftBuffer, 0, leftBuffer.Length);
                                        await rightStream.ReadAsync(rightBuffer, 0, rightBuffer.Length);
                                        SubArray<byte> leftData = new SubArray<byte>(leftBuffer, 0, readSize), rightData = new SubArray<byte>(rightBuffer, 0, readSize);
                                        if (!AutoCSer.Common.SequenceEqual(ref leftData, ref rightData))
                                        {
                                            ConsoleWriteQueue.Breakpoint($"文件 {file.FullName} 对比 {rightFile.FullName} 数据不匹配 {file.Length - unreadSize}[{readSize}]");
                                            break;
                                        }
                                        unreadSize -= readSize;
                                    }
                                    while (unreadSize > 0);
                                }
                            }
                        }
                        else ConsoleWriteQueue.Breakpoint($"文件 {file.FullName} 对比 {rightFile.FullName} 修改时间不匹配 {file.LastWriteTimeUtc} <> {rightFile.LastWriteTimeUtc}");
                    }
                    else ConsoleWriteQueue.Breakpoint($"文件 {file.FullName} 对比 {rightFile.FullName} 长度不匹配 {file.Length} <> {rightFile.Length}");
                }
                else ConsoleWriteQueue.Breakpoint($"没有找到对比目标文件 {Path.Combine(rightDirectory.FullName, file.Name)}");
            }
            foreach (FileInfo file in rightFiles.Values)
            {
                ConsoleWriteQueue.Breakpoint($"没有找到对比原文件 {Path.Combine(leftDirectory.FullName, file.Name)}");
            }

            Dictionary<HashString, DirectoryInfo> rightDictionarys = emptyDirectorys;
            DirectoryInfo[] directorys = await AutoCSer.Common.GetDirectories(rightDirectory);
            if (directorys.Length != 0)
            {
                rightDictionarys = DictionaryCreator.CreateHashString<DirectoryInfo>(directorys.Length);
                foreach (DirectoryInfo directory in directorys) rightDictionarys.Add(directory.Name, directory);
            }
            foreach (DirectoryInfo directory in await AutoCSer.Common.GetDirectories(leftDirectory))
            {
                if (rightDictionarys.Remove(directory.Name, out DirectoryInfo right))
                {
                    await compare(directory, right);
                }
                else ConsoleWriteQueue.Breakpoint($"没有找到对比目标目录 {Path.Combine(rightDirectory.FullName, directory.Name)}");
            }
            foreach (DirectoryInfo directory in rightDictionarys.Values)
            {
                ConsoleWriteQueue.Breakpoint($"没有找到对比原目录 {Path.Combine(leftDirectory.FullName, directory.Name)}");
            }
        }
    }
}
