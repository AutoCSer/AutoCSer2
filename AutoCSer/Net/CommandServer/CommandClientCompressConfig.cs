﻿using AutoCSer.Memory;
using System;
using System.IO.Compression;

namespace AutoCSer.Net
{
    /// <summary>
    /// Command client configuration that supports default compression of transmitted data
    /// 支持传输数据默认压缩的命令客户端配置
    /// </summary>
    public class CommandClientCompressConfig : CommandClientConfig
    {
        /// <summary>
        /// When sending data, enable the minimum number of bytes for compression, with the default being 1KB
        /// 发送数据启用压缩最低字节数量，默认为 1KB
        /// </summary>
        public int MinCompressSize = 1 << 10;
        /// <summary>
        /// The default compression level is fast compression. If it is a large amount of data transmission, it is recommended to change the CPU bandwidth to the default compression
        /// 压缩级别默认为快速压缩，如果是大数据量传输建议用 CPU 换带宽修改为默认压缩
        /// </summary>
        public CompressionLevel CompressionLevel = CompressionLevel.Fastest;
#if NetStandard21
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
        public override bool TransferEncode(CommandClientSocket socket, ReadOnlySpan<byte> data, ref ByteArrayBuffer buffer, ref SubArray<byte> outputData, int outputSeek, int outputHeadSize)
        {
            return data.Length >= MinCompressSize && AutoCSer.Common.Config.Compress(data, ref buffer, ref outputData, outputSeek, outputHeadSize, CompressionLevel);
        }
#else
        /// <summary>
        /// Send data coding
        /// 发送数据编码
        /// </summary>
        /// <param name="socket">Command client socket
        /// 命令客户端套接字</param>
        /// <param name="data">Original data
        /// 原始数据</param>
        /// <param name="dataIndex">Original data start location
        /// 原始数据开始位置</param>
        /// <param name="dataSize">Number of original data bytes
        /// 原始数据字节数量</param>
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
        public override bool TransferEncode(CommandClientSocket socket, byte[] data, int dataIndex, int dataSize, ref ByteArrayBuffer buffer, ref SubArray<byte> outputData, int outputSeek, int outputHeadSize)
        {
            return dataSize >= MinCompressSize && AutoCSer.Common.Config.Compress(data, dataIndex, dataSize, ref buffer, ref outputData, outputSeek, outputHeadSize, CompressionLevel);
        }
#endif
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
        /// When sending data, enable the minimum number of bytes for compression, with the default being 1KB
        /// 发送数据启用压缩最低字节数量，默认为 1KB
        /// </summary>
        public int MinCompressSize = 1 << 10;
        /// <summary>
        /// The default compression level is fast compression. If it is a large amount of data transmission, it is recommended to change the CPU bandwidth to the default compression
        /// 压缩级别默认为快速压缩，如果是大数据量传输建议用 CPU 换带宽修改为默认压缩
        /// </summary>
        public CompressionLevel CompressionLevel = CompressionLevel.Fastest;
#if NetStandard21
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
        public override bool TransferEncode(CommandClientSocket socket, ReadOnlySpan<byte> data, ref ByteArrayBuffer buffer, ref SubArray<byte> outputData, int outputSeek, int outputHeadSize)
        {
            return data.Length >= MinCompressSize && AutoCSer.Common.Config.Compress(data, ref buffer, ref outputData, outputSeek, outputHeadSize, CompressionLevel);
        }
#else
        /// <summary>
        /// Send data coding
        /// 发送数据编码
        /// </summary>
        /// <param name="socket">Command client socket
        /// 命令客户端套接字</param>
        /// <param name="data">Original data
        /// 原始数据</param>
        /// <param name="dataIndex">Original data start location
        /// 原始数据开始位置</param>
        /// <param name="dataSize">Number of original data bytes
        /// 原始数据字节数量</param>
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
        public override bool TransferEncode(CommandClientSocket socket, byte[] data, int dataIndex, int dataSize, ref ByteArrayBuffer buffer, ref SubArray<byte> outputData, int outputSeek, int outputHeadSize)
        {
            return dataSize >= MinCompressSize && AutoCSer.Common.Config.Compress(data, dataIndex, dataSize, ref buffer, ref outputData, outputSeek, outputHeadSize, CompressionLevel);
        }
#endif
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
        public override bool TransferDecode(CommandClientSocket socket, SubArray<byte> transferData, ref SubArray<byte> outputData)
        {
            return AutoCSer.Common.Config.Decompress(ref transferData, ref outputData);
        }
    }
}
