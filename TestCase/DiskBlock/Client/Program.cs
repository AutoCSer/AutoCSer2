﻿using AutoCSer.CommandService;
using AutoCSer.CommandService.DiskBlock;
using AutoCSer.Net;
using AutoCSer.Threading;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.DiskBlockClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            CommandClientConfig commandClientConfig = new CommandClientCompressConfig
            {
                Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.DiskBlock),
                ControllerCreatorBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client)
            };
            using (CommandClient commandClient = new CommandClient(commandClientConfig))
            {
                IDiskBlockClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client == null)
                {
                    ConsoleWriteQueue.Breakpoint("ERROR");
                    Console.ReadKey();
                    return;
                }

                AutoCSer.CommandService.DiskBlockClient diskBlockClient = new AutoCSer.CommandService.DiskBlockClient(client);
                AutoCSer.FieldEquals.Comparor.IsBreakpoint = true;
                await Task.WhenAll(
                    binaryTest(diskBlockClient)
                    , jsonTest(diskBlockClient)
                    , stringTest(diskBlockClient)
                    , bufferTest(diskBlockClient)
                    );
                Console.WriteLine("Press quit to exit.");
                while (await AutoCSer.Breakpoint.ReadLineDelay() != "quit") ;
            }
#if AOT
            if (AutoCSer.TestCase.DiskBlockClient.AotMethod.Call())
            {
                AutoCSer.CommandService.DiskBlock.AotMethod.Call();
                AutoCSer.CommandService.TimestampVerify.AotMethod.Call();
            }
#endif
        }
        public static bool Breakpoint<T>(ReadResult<T> returnValue, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
        {
            if (returnValue.IsSuccess) return true;
            ConsoleWriteQueue.Breakpoint($"[{returnValue.ReturnType}][{returnValue.BufferState}]{returnValue.ErrorMessage}", callerMemberName, callerFilePath, callerLineNumber);
            return false;
        }
        static async Task bufferTest(AutoCSer.CommandService.DiskBlockClient client)
        {
            byte[] data = new byte[32];
            for (int index = -1; index != data.Length; )
            {
                ++index;
                if (index != 0) data[index - 1] = (byte)index;
                byte[] buffer = new byte[index];
                Buffer.BlockCopy(data, 0, buffer, 0, index);

                CommandClientReturnValue<BlockIndex> blockIndex = await client.WriteAsync(buffer);
                bool isSuccess = ConsoleWriteQueue.Breakpoint(blockIndex);
                if (isSuccess)
                {
                    ReadBufferStateEnum state = blockIndex.Value.GetReadState();
                    switch (state)
                    {
                        case ReadBufferStateEnum.Unknown:
                            if (buffer.Length <= 15)
                            {
                                isSuccess = false;
                                ConsoleWriteQueue.Breakpoint($"*ERROR+{buffer.Length}+ERROR*");
                            }
                            break;
                        case ReadBufferStateEnum.IndexSize:
                            if (buffer.Length > 15)
                            {
                                isSuccess = false;
                                ConsoleWriteQueue.Breakpoint($"*ERROR+{buffer.Length}+ERROR*");
                            }
                            break;
                        default:
                            isSuccess = false;
                            ConsoleWriteQueue.Breakpoint($"*ERROR+{state}+ERROR*");
                            break;
                    }
                }
                if (isSuccess)
                {
                    ReadResult<byte[]> readBuffer = await client.ReadAsync(blockIndex.Value);
                    if (Breakpoint(readBuffer)) AutoCSer.FieldEquals.Comparor.Equals(buffer, readBuffer.Value);
                    if (blockIndex.Value.GetReadState() == ReadBufferStateEnum.Unknown)
                    {
                        CommandClientReturnValue<BlockIndex> newBlockIndex = await client.WriteAsync(buffer);
                        if (ConsoleWriteQueue.Breakpoint(newBlockIndex)) AutoCSer.FieldEquals.Comparor.Equals(blockIndex.Value, newBlockIndex.Value);
                    }
                }
                Console.Write('.');
            }
            Console.WriteLine($"*bufferTest*");
        }
        static async Task stringTest(AutoCSer.CommandService.DiskBlockClient client)
        {
            char[] data = new char[26];
            for (int index = -1; index != data.Length;)
            {
                ++index;
                if (index != 0) data[index - 1] = (char)(index + ('A' - 1));
                string buffer = index == 0 ? string.Empty : new string(data, 0, index);

                CommandClientReturnValue<BlockIndex> blockIndex = await client.WriteStringAsync(buffer);
                bool isSuccess = ConsoleWriteQueue.Breakpoint(blockIndex);
                if (isSuccess)
                {
                    ReadBufferStateEnum state = blockIndex.Value.GetReadState();
                    switch (state)
                    {
                        case ReadBufferStateEnum.Unknown:
                            if (buffer.Length <= 5)
                            {
                                isSuccess = false;
                                ConsoleWriteQueue.Breakpoint($"*ERROR+{buffer.Length}+ERROR*");
                            }
                            break;
                        case ReadBufferStateEnum.IndexSize:
                            if (buffer.Length > 15)
                            {
                                isSuccess = false;
                                ConsoleWriteQueue.Breakpoint($"*ERROR+{buffer.Length}+ERROR*");
                            }
                            break;
                        default:
                            isSuccess = false;
                            ConsoleWriteQueue.Breakpoint($"*ERROR+{state}+ERROR*");
                            break;
                    }
                }
                if (isSuccess)
                {
                    ReadResult<string> readBuffer = await client.ReadStringAsync(blockIndex.Value);
                    if (Breakpoint(readBuffer)) AutoCSer.FieldEquals.Comparor.Equals(buffer, readBuffer.Value);
                    if (blockIndex.Value.GetReadState() == ReadBufferStateEnum.Unknown)
                    {
                        CommandClientReturnValue<BlockIndex> newBlockIndex = await client.WriteStringAsync(buffer);
                        if (ConsoleWriteQueue.Breakpoint(newBlockIndex)) AutoCSer.FieldEquals.Comparor.Equals(blockIndex.Value, newBlockIndex.Value);
                    }
                }
                Console.Write('.');
            }
            Console.WriteLine($"*stringTest*");
        }
        static async Task jsonTest(AutoCSer.CommandService.DiskBlockClient client)
        {
            await jsonTest(client, (object)null);
            await jsonTest(client, new object());
            await jsonTest(client, EmptyArray<object>.Array);
#if AOT
            await jsonTest(client, new IntData { A = 3 });
            char[] data = new char[26];
            for (int index = -1; index != data.Length;)
            {
                ++index;
                if (index != 0) data[index - 1] = (char)(index + ('A' - 1));
                string buffer = index == 0 ? string.Empty : new string(data, 0, index);
                await jsonTest(client, new StringData { A = buffer });
            }
#else
            await jsonTest(client, new { A = 3 });
            char[] data = new char[26];
            for (int index = -1; index != data.Length;)
            {
                ++index;
                if (index != 0) data[index - 1] = (char)(index + ('A' - 1));
                string buffer = index == 0 ? string.Empty : new string(data, 0, index);
                await jsonTest(client, new { A = buffer });
            }
#endif
            Console.WriteLine($"*jsonTest*");
        }
        static async Task jsonTest<T>(AutoCSer.CommandService.DiskBlockClient client, T buffer)
        {
            CommandClientReturnValue<BlockIndex> blockIndex = await client.WriteJsonAsync(buffer);
            bool isSuccess = ConsoleWriteQueue.Breakpoint(blockIndex);
            if (isSuccess)
            {
                ReadBufferStateEnum state = blockIndex.Value.GetReadState();
                switch (state)
                {
                    case ReadBufferStateEnum.Unknown:
                    case ReadBufferStateEnum.IndexSize:
                        break;
                    default:
                        isSuccess = false;
                        ConsoleWriteQueue.Breakpoint($"*ERROR+{state}+ERROR*");
                        break;
                }
            }
            if (isSuccess)
            {
                ReadResult<T> readBuffer = await client.ReadJsonAsync<T>(blockIndex.Value);
                if (Breakpoint(readBuffer)) AutoCSer.FieldEquals.Comparor.Equals(buffer, readBuffer.Value);
                if (blockIndex.Value.GetReadState() == ReadBufferStateEnum.Unknown)
                {
                    CommandClientReturnValue<BlockIndex> newBlockIndex = await client.WriteJsonAsync(buffer);
                    if (ConsoleWriteQueue.Breakpoint(newBlockIndex)) AutoCSer.FieldEquals.Comparor.Equals(blockIndex.Value, newBlockIndex.Value);
                }
            }
            Console.Write('.');
        }
        static async Task binaryTest(AutoCSer.CommandService.DiskBlockClient client)
        {
            await binaryTest(client, (object)null);
            await binaryTest(client, new object());
            await binaryTest(client, EmptyArray<object>.Array);
#if AOT
            await binaryTest(client, new IntData { A = 3 });
            char[] data = new char[26];
            for (int index = -1; index != data.Length;)
            {
                ++index;
                if (index != 0) data[index - 1] = (char)(index + ('A' - 1));
                string buffer = index == 0 ? string.Empty : new string(data, 0, index);
                await binaryTest(client, new StringData { A = buffer });
            }
#else
            await binaryTest(client, new { A = 3 });
            char[] data = new char[26];
            for (int index = -1; index != data.Length;)
            {
                ++index;
                if (index != 0) data[index - 1] = (char)(index + ('A' - 1));
                string buffer = index == 0 ? string.Empty : new string(data, 0, index);
                await binaryTest(client, new { A = buffer });
            }
#endif
            Console.WriteLine($"*binaryTest*");
        }
        static async Task binaryTest<T>(AutoCSer.CommandService.DiskBlockClient client, T buffer)
        {
            CommandClientReturnValue<BlockIndex> blockIndex = await client.WriteBinaryAsync(buffer);
            bool isSuccess = ConsoleWriteQueue.Breakpoint(blockIndex);
            if (isSuccess)
            {
                ReadBufferStateEnum state = blockIndex.Value.GetReadState();
                switch (state)
                {
                    case ReadBufferStateEnum.Unknown:
                    case ReadBufferStateEnum.IndexSize:
                        break;
                    default:
                        isSuccess = false;
                        ConsoleWriteQueue.Breakpoint($"*ERROR+{state}+ERROR*");
                        break;
                }
            }
            if (isSuccess)
            {
                ReadResult<T> readBuffer = await client.ReadBinaryAsync<T>(blockIndex.Value);
                if (Breakpoint(readBuffer)) AutoCSer.FieldEquals.Comparor.Equals(buffer, readBuffer.Value);
                if (blockIndex.Value.GetReadState() == ReadBufferStateEnum.Unknown)
                {
                    CommandClientReturnValue<BlockIndex> newBlockIndex = await client.WriteBinaryAsync(buffer);
                    if (ConsoleWriteQueue.Breakpoint(newBlockIndex)) AutoCSer.FieldEquals.Comparor.Equals(blockIndex.Value, newBlockIndex.Value);
                }
            }
            Console.Write('.');
        }
    }
}
