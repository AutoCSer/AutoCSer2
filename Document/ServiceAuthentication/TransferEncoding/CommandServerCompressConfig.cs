using AutoCSer.CommandService;
using AutoCSer.Net;
using System;
using System.IO.Compression;
using System.Reflection;

namespace AutoCSer.Document.ServiceAuthentication.TransferEncoding
{
    /// <summary>
    /// 支持传输数据压缩的命令服务端配置
    /// </summary>
    internal sealed class CommandServerCompressConfig : AutoCSer.Net.CommandServerConfig
    {
        /// <summary>
        /// 发送数据启用压缩最低字节数量，默认为 0 表示不压缩数据；压缩数据需要消耗一定的 CPU 资源降低带宽使用
        /// </summary>
        public int MinCompressSize;
        /// <summary>
        /// 压缩级别默认为快速压缩，如果是大数据量传输建议用 CPU 换带宽修改为默认压缩
        /// </summary>
        public CompressionLevel CompressionLevel = CompressionLevel.Fastest;
        /// <summary>
        /// 发送数据编码
        /// </summary>
        /// <param name="socket">命令服务套接字</param>
        /// <param name="data">原始数据</param>
        /// <param name="dataIndex">原始数据起始位置</param>
        /// <param name="dataSize">原始数据字节数</param>
        /// <param name="buffer">输出数据缓冲区</param>
        /// <param name="outputData">输出数据</param>
        /// <param name="outputSeek">输出数据起始位置</param>
        /// <param name="outputHeadSize">输出数据多余头部大小</param>
        /// <returns>发送数据是否编码</returns>
        public override bool TransferEncode(AutoCSer.Net.CommandServerSocket socket, byte[] data, int dataIndex, int dataSize, ref AutoCSer.Memory.ByteArrayBuffer buffer, ref AutoCSer.SubArray<byte> outputData, int outputSeek, int outputHeadSize)
        {
            if(dataSize >= MinCompressSize)
            {
                int length = dataSize + outputSeek;
                using (MemoryStream dataStream = new MemoryStream(length))
                {
                    dataStream.Seek(outputSeek, SeekOrigin.Begin);
                    using (DeflateStream compressStream = new DeflateStream(dataStream, CompressionLevel, true)) compressStream.Write(data, dataIndex, dataSize);
                    if (dataStream.Position + outputHeadSize < length)
                    {
                        outputData = new SubArray<byte>(dataStream.GetBuffer(), outputSeek, (int)dataStream.Position - outputSeek);
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 接收数据解码
        /// </summary>
        /// <param name="socket">命令服务套接字</param>
        /// <param name="transferData">编码后的数据</param>
        /// <param name="outputData">等待写入的原始数据缓冲区</param>
        /// <returns>是否解码成功</returns>
        public override bool TransferDecode(AutoCSer.Net.CommandServerSocket socket, SubArray<byte> transferData, ref SubArray<byte> outputData)
        {
            using (MemoryStream memoryStream = AutoCSer.Extensions.SubArrayExtension.createMemoryStream(transferData))
            using (DeflateStream compressStream = new DeflateStream(memoryStream, CompressionMode.Decompress, true))
            {
                byte[] data = outputData.GetArray(out int index, out int count);
                while (count > 0)
                {
                    int readSize = compressStream.Read(data, index, count);
                    if ((count -= readSize) == 0) return true;
                    if (readSize <= 0) return false;
                    index += readSize;
                }
            }
            return false;
        }

        /// <summary>
        /// 支持传输数据压缩的配置测试
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            CommandServerConfig commandServerConfig = new CommandServerCompressConfig
            {
                MinCompressSize = 1 << 10,
                Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
            };
            await using (CommandListener commandListener = new CommandListenerBuilder(0)
                .Append<AutoCSer.CommandService.ITimestampVerifyService>(server => new AutoCSer.CommandService.TimestampVerifyService(server, AutoCSer.TestCase.Common.Config.TimestampVerifyString))
                .Append<ITestService>(new TestService())
                .CreateCommandListener(commandServerConfig))
            {
                if (!await commandListener.Start())
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                return await client();
            }
        }
        /// <summary>
        /// 支持传输数据压缩的配置客户端测试
        /// </summary>
        /// <returns></returns>
        private static async Task<bool> client()
        {
            CommandClientConfig commandClientConfig = new CommandClientCompressConfig
            {
                MinCompressSize = 1 << 10,
                Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
                ControllerCreatorBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                GetSocketEventDelegate = (client) => new TimestampVerify.CommandClientSocketEvent(client, AutoCSer.TestCase.Common.Config.TimestampVerifyString)
            };
            using (CommandClient commandClient = new CommandClient(commandClientConfig))
            {
                TimestampVerify.CommandClientSocketEvent? client = (TimestampVerify.CommandClientSocketEvent?)await commandClient.GetSocketEvent();
                if (client == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                var result = await client.TestService.Add(1, 2);
                if (result.Value != 1 + 2)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            return true;
        }
    }
}
