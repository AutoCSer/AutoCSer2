using AutoCSer.Memory;
using System;
using System.Threading;

namespace AutoCSer.Net
{
    /// <summary>
    /// 原始套接字监听数据缓冲区
    /// </summary>
    public sealed class RawSocketBuffer : AutoCSer.Threading.Link<RawSocketBuffer>, IDisposable
    {
        /// <summary>
        /// 数据缓冲区计数
        /// </summary>
        private BufferCount bufferCount;
        /// <summary>
        /// 数据
        /// </summary>
        private SubArray<byte> buffer;
        /// <summary>
        /// 字节数
        /// </summary>
        public int Length { get { return buffer.Length; } }
        /// <summary>
        /// IPv4 数据包
        /// </summary>
        public Packet.Ip Ip
        {
            get { return new Packet.Ip(ref buffer); }
        }
        /// <summary>
        /// IPv6 数据包
        /// </summary>
        public Packet.Ip6 Ip6
        {
            get { return new Packet.Ip6(ref buffer); }
        }
        /// <summary>
        /// 原始套接字监听数据缓冲区
        /// </summary>
        /// <param name="bufferCount">数据缓冲区计数</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">字节数量</param>
        internal RawSocketBuffer(BufferCount bufferCount, int index, int count)
        {
            this.bufferCount = bufferCount;
            buffer.Set(bufferCount.Buffer.Buffer.Buffer, index, count);
            ++bufferCount.Count;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            BufferCount bufferCount = Interlocked.Exchange(ref this.bufferCount, null);
            if (bufferCount != null)
            {
                bufferCount.Free();
                buffer.SetEmpty();
            }
        }
    }
}
