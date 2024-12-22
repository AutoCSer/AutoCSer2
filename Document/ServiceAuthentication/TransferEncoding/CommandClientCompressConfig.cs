using AutoCSer.Memory;
using AutoCSer.Net;
using System;
using System.IO.Compression;

namespace AutoCSer.Document.ServiceAuthentication.TransferEncoding
{
    /// <summary>
    /// 支持传输数据压缩的命令客户端配置
    /// </summary>
    internal sealed class CommandClientCompressConfig : AutoCSer.Net.CommandClientConfig
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
        /// <param name="socket">命令客户端套接字</param>
        /// <param name="data">原始数据</param>
        /// <param name="buffer">输出数据缓冲区</param>
        /// <param name="outputData">输出数据</param>
        /// <param name="outputSeek">输出数据起始位置</param>
        /// <param name="outputHeadSize">输出数据多余头部大小</param>
        /// <returns>发送数据是否编码</returns>
        public override bool TransferEncode(AutoCSer.Net.CommandClientSocket socket, ReadOnlySpan<byte> data, ref AutoCSer.Memory.ByteArrayBuffer buffer, ref AutoCSer.SubArray<byte> outputData, int outputSeek, int outputHeadSize)
        {
            if (data.Length >= MinCompressSize)
            {
                int length = data.Length + outputSeek;
                using (MemoryStream dataStream = new MemoryStream(length))
                {
                    dataStream.Seek(outputSeek, SeekOrigin.Begin);
                    using (DeflateStream compressStream = new DeflateStream(dataStream, CompressionLevel, true)) compressStream.Write(data);
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
        /// <param name="socket">命令客户端套接字</param>
        /// <param name="transferData">编码后的数据</param>
        /// <param name="outputData">等待写入的原始数据缓冲区</param>
        /// <returns>是否解码成功</returns>
        public override bool TransferDecode(AutoCSer.Net.CommandClientSocket socket, SubArray<byte> transferData, ref SubArray<byte> outputData)
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
    }
}
