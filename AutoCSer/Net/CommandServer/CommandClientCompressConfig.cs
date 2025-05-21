using AutoCSer.Memory;
using System;
using System.IO.Compression;

namespace AutoCSer.Net
{
    /// <summary>
    /// 支持传输数据默认压缩的命令客户端配置
    /// </summary>
    public class CommandClientCompressConfig : CommandClientConfig
    {
        /// <summary>
        /// 发送数据启用压缩最低字节数量，默认为 1KB
        /// </summary>
        public int MinCompressSize = 1 << 10;
        /// <summary>
        /// 压缩级别默认为快速压缩，如果是大数据量传输建议用 CPU 换带宽修改为默认压缩
        /// </summary>
        public CompressionLevel CompressionLevel = CompressionLevel.Fastest;
#if NetStandard21
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
        public override bool TransferEncode(CommandClientSocket socket, ReadOnlySpan<byte> data, ref ByteArrayBuffer buffer, ref SubArray<byte> outputData, int outputSeek, int outputHeadSize)
        {
            return data.Length >= MinCompressSize && AutoCSer.Common.Config.Compress(data, ref buffer, ref outputData, outputSeek, outputHeadSize, CompressionLevel);
        }
#else
        /// <summary>
        /// 发送数据编码
        /// </summary>
        /// <param name="socket">命令客户端套接字</param>
        /// <param name="data">原始数据</param>
        /// <param name="dataIndex">原始数据开始位置</param>
        /// <param name="dataSize">原始数据字节数量</param>
        /// <param name="buffer">输出数据缓冲区</param>
        /// <param name="outputData">输出数据</param>
        /// <param name="outputSeek">输出数据起始位置</param>
        /// <param name="outputHeadSize">输出数据多余头部大小</param>
        /// <returns>发送数据是否编码</returns>
        public override bool TransferEncode(CommandClientSocket socket, byte[] data, int dataIndex, int dataSize, ref ByteArrayBuffer buffer, ref SubArray<byte> outputData, int outputSeek, int outputHeadSize)
        {
            return dataSize >= MinCompressSize && AutoCSer.Common.Config.Compress(data, dataIndex, dataSize, ref buffer, ref outputData, outputSeek, outputHeadSize, CompressionLevel);
        }
#endif
        /// <summary>
        /// 接收数据解码
        /// </summary>
        /// <param name="socket">命令客户端套接字</param>
        /// <param name="transferData">编码后的数据</param>
        /// <param name="outputData">等待写入的原始数据缓冲区</param>
        /// <returns>是否解码成功</returns>
        public override bool TransferDecode(CommandClientSocket socket, SubArray<byte> transferData, ref SubArray<byte> outputData)
        {
            return AutoCSer.Common.Config.Decompress(ref transferData, ref outputData);
        }
    }
    /// <summary>
    /// Command client configuration
    /// 支持传输数据默认压缩的命令客户端配置
    /// </summary>
    /// <typeparam name="T">Primary controller interface type
    /// 主控制器接口类型</typeparam>
    public class CommandClientCompressConfig<T> : AutoCSer.Net.CommandClientConfig<T>
        where T : class
    {
        /// <summary>
        /// 发送数据启用压缩最低字节数量，默认为 1KB
        /// </summary>
        public int MinCompressSize = 1 << 10;
        /// <summary>
        /// 压缩级别默认为快速压缩，如果是大数据量传输建议用 CPU 换带宽修改为默认压缩
        /// </summary>
        public CompressionLevel CompressionLevel = CompressionLevel.Fastest;
#if NetStandard21
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
        public override bool TransferEncode(CommandClientSocket socket, ReadOnlySpan<byte> data, ref ByteArrayBuffer buffer, ref SubArray<byte> outputData, int outputSeek, int outputHeadSize)
        {
            return data.Length >= MinCompressSize && AutoCSer.Common.Config.Compress(data, ref buffer, ref outputData, outputSeek, outputHeadSize, CompressionLevel);
        }
#else
        /// <summary>
        /// 发送数据编码
        /// </summary>
        /// <param name="socket">命令客户端套接字</param>
        /// <param name="data">原始数据</param>
        /// <param name="dataIndex">原始数据开始位置</param>
        /// <param name="dataSize">原始数据字节数量</param>
        /// <param name="buffer">输出数据缓冲区</param>
        /// <param name="outputData">输出数据</param>
        /// <param name="outputSeek">输出数据起始位置</param>
        /// <param name="outputHeadSize">输出数据多余头部大小</param>
        /// <returns>发送数据是否编码</returns>
        public override bool TransferEncode(CommandClientSocket socket, byte[] data, int dataIndex, int dataSize, ref ByteArrayBuffer buffer, ref SubArray<byte> outputData, int outputSeek, int outputHeadSize)
        {
            return dataSize >= MinCompressSize && AutoCSer.Common.Config.Compress(data, dataIndex, dataSize, ref buffer, ref outputData, outputSeek, outputHeadSize, CompressionLevel);
        }
#endif
        /// <summary>
        /// 接收数据解码
        /// </summary>
        /// <param name="socket">命令客户端套接字</param>
        /// <param name="transferData">编码后的数据</param>
        /// <param name="outputData">等待写入的原始数据缓冲区</param>
        /// <returns>是否解码成功</returns>
        public override bool TransferDecode(CommandClientSocket socket, SubArray<byte> transferData, ref SubArray<byte> outputData)
        {
            return AutoCSer.Common.Config.Decompress(ref transferData, ref outputData);
        }
    }
}
