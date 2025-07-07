using System;

namespace AutoCSer.Document.ServiceAuthentication.TransferEncoding
{
    /// <summary>
    /// Command client configuration supporting data transmission compression
    /// 支持传输数据压缩的命令客户端配置
    /// </summary>
    internal sealed class CommandClientCompressConfig : AutoCSer.Net.CommandClientConfig
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
        /// <param name="socket">Command client socket
        /// 命令客户端套接字</param>
        /// <param name="data">Original data
        /// 原始数据</param>
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
        public override bool TransferEncode(AutoCSer.Net.CommandClientSocket socket, ReadOnlySpan<byte> data, ref AutoCSer.Memory.ByteArrayBuffer buffer, ref AutoCSer.SubArray<byte> outputData, int outputSeek, int outputHeadSize)
        {
            if (data.Length >= MinCompressSize)
            {
                int length = data.Length + outputSeek;
                using (MemoryStream dataStream = new MemoryStream(length))
                {
                    dataStream.Seek(outputSeek, SeekOrigin.Begin);
                    using (System.IO.Compression.DeflateStream compressStream = new System.IO.Compression.DeflateStream(dataStream, CompressionLevel, true)) compressStream.Write(data);
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
        /// <param name="socket">Command client socket
        /// 命令客户端套接字</param>
        /// <param name="transferData">The encoded data
        /// 编码后的数据</param>
        /// <param name="outputData">Original data buffer waiting to be written
        /// 等待写入的原始数据缓冲区</param>
        /// <returns>Whether the decoding is successful
        /// 是否解码成功</returns>
        public override bool TransferDecode(AutoCSer.Net.CommandClientSocket socket, SubArray<byte> transferData, ref SubArray<byte> outputData)
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
        /// Client singleton
        /// 客户端单例
        /// </summary>
        public static readonly AutoCSer.Net.CommandClientSocketEventCache<TimestampVerify.CommandClientSocketEvent> CommandClient = new AutoCSer.Net.CommandClientSocketEventCache<TimestampVerify.CommandClientSocketEvent>(new CommandClientCompressConfig
        {
            MinCompressSize = 1,
            Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
            ControllerCreatorBindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public,
            GetSocketEventDelegate = (client) => new TimestampVerify.CommandClientSocketEvent(client, AutoCSer.TestCase.Common.Config.TimestampVerifyString)
        });
        /// <summary>
        /// Configuration client testing that supports data transmission compression
        /// 支持传输数据压缩的配置客户端测试
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            TimestampVerify.CommandClientSocketEvent? client = await CommandClient.SocketEvent.Wait();
            if (client == null)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            var result = await client.TestService.Add(1, 2);
            if (result.Value != 1 + 2)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
