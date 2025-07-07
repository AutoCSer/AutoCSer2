using System;

namespace AutoCSer.Document.ServiceAuthentication.TransferEncoding
{
    /// <summary>
    /// Command server configuration supporting data transmission compression
    /// 支持传输数据压缩的命令服务端配置
    /// </summary>
    internal sealed class CommandServerCompressConfig : AutoCSer.Net.CommandServerConfig
    {
        /// <summary>
        /// Enable the minimum number of bytes for compression when sending data
        /// 发送数据启用压缩最低字节数量
        /// </summary>
        public int MinCompressSize;
        /// <summary>
        /// The default compression level is fast compression. If it is a large amount of data transmission, it is recommended to change the CPU bandwidth to the default compression
        /// 压缩级别默认为快速压缩，如果是大数据量传输建议用 CPU 换带宽修改为默认压缩
        /// </summary>
        public System.IO.Compression.CompressionLevel CompressionLevel = System.IO.Compression.CompressionLevel.Fastest;
        /// <summary>
        /// Send data coding
        /// 发送数据编码
        /// </summary>
        /// <param name="socket">Command server socket
        /// 命令服务套接字</param>
        /// <param name="data">Original data
        /// 原始数据</param>
        /// <param name="dataIndex">Origin of original data
        /// 原始数据起始位置</param>
        /// <param name="dataSize">Number of bytes of original data
        /// 原始数据字节数</param>
        /// <param name="buffer">Output data buffer
        /// 输出数据缓冲区</param>
        /// <param name="outputData">Output data
        /// 输出数据</param>
        /// <param name="outputSeek">Start position of output data
        /// 输出数据起始位置</param>
        /// <param name="outputHeadSize">The output data exceeds the header size
        /// 输出数据多余头部大小</param>
        /// <returns>Whether the sent data is encoded
        /// 发送数据是否编码</returns>
        public override bool TransferEncode(AutoCSer.Net.CommandServerSocket socket, byte[] data, int dataIndex, int dataSize, ref AutoCSer.Memory.ByteArrayBuffer buffer, ref AutoCSer.SubArray<byte> outputData, int outputSeek, int outputHeadSize)
        {
            if(dataSize >= MinCompressSize)
            {
                int length = dataSize + outputSeek;
                using (MemoryStream dataStream = new MemoryStream(length))
                {
                    dataStream.Seek(outputSeek, SeekOrigin.Begin);
                    using (System.IO.Compression.DeflateStream compressStream = new System.IO.Compression.DeflateStream(dataStream, CompressionLevel, true)) compressStream.Write(data, dataIndex, dataSize);
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
        /// Received data decoding
        /// 接收数据解码
        /// </summary>
        /// <param name="socket">Command server socket
        /// 命令服务套接字</param>
        /// <param name="transferData">The encoded data
        /// 编码后的数据</param>
        /// <param name="outputData">Original data buffer waiting to be written
        /// 等待写入的原始数据缓冲区</param>
        /// <returns>Whether the decoding is successful
        /// 是否解码成功</returns>
        public override bool TransferDecode(AutoCSer.Net.CommandServerSocket socket, SubArray<byte> transferData, ref SubArray<byte> outputData)
        {
            using (MemoryStream memoryStream = AutoCSer.Extensions.SubArrayExtension.createMemoryStream(transferData))
            using (System.IO.Compression.DeflateStream compressStream = new System.IO.Compression.DeflateStream(memoryStream, System.IO.Compression.CompressionMode.Decompress, true))
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
        /// Support configuration testing for data transmission compression
        /// 支持传输数据压缩的配置测试
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            AutoCSer.Net.CommandServerConfig commandServerConfig = new CommandServerCompressConfig
            {
                MinCompressSize = 1,
                Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
            };
            await using (AutoCSer.Net.CommandListener commandListener = new AutoCSer.Net.CommandListenerBuilder(0)
                .Append<AutoCSer.CommandService.ITimestampVerifyService>(server => new AutoCSer.CommandService.TimestampVerifyService(server, AutoCSer.TestCase.Common.Config.TimestampVerifyString))
                .Append<ITestService>(new TestService())
                .CreateCommandListener(commandServerConfig))
            {
                if (!await commandListener.Start())
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                return await CommandClientCompressConfig.Test();
            }
        }
    }
}
