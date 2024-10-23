using AutoCSer.Memory;
using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net
{
    /// <summary>
    /// 原始套接字监听（仅支持 Windows）
    /// </summary>
    [System.Runtime.Versioning.SupportedOSPlatform(AutoCSer.SupportedOSPlatformName.Windows)]
    public sealed class RawSocketListener : IDisposable
    {
        /// <summary>
        /// 默认获取的数据包的字节数(默认为以太网)
        /// </summary>
        private const int defaultBufferSize = 1500;

        /// <summary>
        /// 缓冲区池
        /// </summary>
        private readonly ByteArrayPool bufferPool;
        /// <summary>
        /// 监听套接字
        /// </summary>
        private Socket socket;
        /// <summary>
        /// 接收数据异步回调
        /// </summary>
        private readonly EventHandler<SocketAsyncEventArgs> onReceiveAsyncCallback;
        /// <summary>
        /// 异步套接字操作
        /// </summary>
        private readonly SocketAsyncEventArgs async;
        /// <summary>
        /// 监听地址
        /// </summary>
        private readonly IPEndPoint ipEndPoint;
        /// <summary>
        /// 原始套接字监听数据缓冲区处理队列线程
        /// </summary>
        private readonly RawSocketQueue queue;
        /// <summary>
        /// 数据包字节数
        /// </summary>
        private readonly int packetSize;
        /// <summary>
        /// 缓冲区最大可用索引
        /// </summary>
        private readonly int maxBufferIndex;
        /// <summary>
        /// 数据缓冲区计数
        /// </summary>
        private BufferCount buffer;
        /// <summary>
        /// 缓冲区起始位置
        /// </summary>
        private int bufferIndex;
        /// <summary>
        /// 缓冲区结束位置
        /// </summary>
        private int bufferEndIndex;
        /// <summary>
        /// 是否处理错误状态
        /// </summary>
        public bool IsError { get; private set; }
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        private bool isDisposed;
        /// <summary>
        /// 原始套接字监听
        /// </summary>
        /// <param name="ipAddress">监听地址，无线网卡不支持 IPAddress.Any</param>
        /// <param name="onPacket">数据包处理委托</param>
        /// <param name="bufferPool">接收缓冲区池，默认为 128KB</param>
        /// <param name="packetSize">数据包字节数，默认为 1500</param>
        public RawSocketListener(IPAddress ipAddress, Action<RawSocketBuffer> onPacket, ByteArrayPool bufferPool = null, int packetSize = defaultBufferSize)
        {
            if (onPacket == null) throw new ArgumentNullException();
            this.bufferPool = bufferPool ?? ByteArrayPool.GetPool(BufferSizeBitsEnum.Kilobyte128);
            if (packetSize <= 0) packetSize = defaultBufferSize;
            else if (packetSize > bufferPool.Size) packetSize = bufferPool.Size;
            this.packetSize = packetSize;
            maxBufferIndex = bufferPool.Size - packetSize;
            ipEndPoint = new IPEndPoint(ipAddress, 0);
            onReceiveAsyncCallback = onReceive;
            async = new SocketAsyncEventArgs();
            async.SocketFlags = SocketFlags.None;
            async.DisconnectReuseSocket = false;
            async.UserToken = this;
            async.Completed += onReceiveAsyncCallback;
            queue = new RawSocketQueue(onPacket);
            AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(start);
        }

        /// <summary>
        /// 关闭套接字
        /// </summary>
        public void Dispose()
        {
            isDisposed = true;
            closeSocket();
        }
        /// <summary>
        /// 关闭套接字
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void closeSocket()
        {
            Socket socket = this.socket;
            if (socket != null)
            {
                this.socket = null;
                try
                {
                    if (socket.Connected) socket.Shutdown(SocketShutdown.Both);
                }
                catch { }
                finally { socket.Dispose(); }
            }
        }
        /// <summary>
        /// 开始监听
        /// </summary>
        private void start()
        {
            while (!isDisposed)
            {
                try
                {
                    closeSocket();
                    IPAddress ipAddress = ipEndPoint.Address;
                    //在发送时必须提供完整的IP标头，所接收的数据报在返回时会保持其IP标头和选项不变。
                    socket = new Socket(ipAddress.AddressFamily, SocketType.Raw, ipAddress.AddressFamily == AddressFamily.InterNetworkV6 ? ProtocolType.IPv6 : ProtocolType.IP);
                    socket.Blocking = false;
                    socket.Bind(ipEndPoint);
                    socket.SetSocketOption(ipAddress.AddressFamily == AddressFamily.InterNetworkV6 ? SocketOptionLevel.IPv6 : SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);
                    byte[] optionIn = new byte[] { 1, 0, 0, 0 };
                    socket.IOControl(IOControlCode.ReceiveAll, optionIn, null);
                    if (receive())
                    {
                        IsError = false;
                        return;
                    }
                    IsError = true;
                    System.Threading.Thread.Sleep(1000);
                }
                catch (Exception error)
                {
                    if (!IsError) AutoCSer.LogHelper.ExceptionIgnoreException(error, "监听初始化失败，可能需要管理员权限。", LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
                }
                IsError = true;
            }
            if (buffer != null)
            {
                buffer.Free();
                buffer = null;
            }
            using (async) async.Completed -= onReceiveAsyncCallback;
            queue.Dispose();
        }
        /// <summary>
        /// 继续接收数据
        /// </summary>
        /// <returns>是否接收成功</returns>
        private bool receive()
        {
            do
            {
                if (buffer == null)
                {
                    buffer = new BufferCount(bufferPool);
                    bufferIndex = buffer.Buffer.StartIndex;
                    bufferEndIndex = bufferIndex + bufferPool.Size;
                    async.SetBuffer(buffer.Buffer.Buffer.Buffer, bufferIndex, bufferPool.Size);
                }
                else async.SetBuffer(bufferIndex, bufferEndIndex - bufferIndex);
                if (socket.ReceiveAsync(async)) return true;
                if (async.SocketError != SocketError.Success) return false;
                onReceive(async.BytesTransferred);
            }
            while (true);
        }
        /// <summary>
        /// 数据接收完成后的回调委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="async">异步回调参数</param>
        private void onReceive(object sender, SocketAsyncEventArgs async)
        {
            try
            {
                if (async.SocketError == SocketError.Success)
                {
                    onReceive(async.BytesTransferred);
                    if (receive()) return;
                }
            }
            catch (Exception error)
            {
                AutoCSer.LogHelper.ExceptionIgnoreException(error, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
            }
            start();
        }
        /// <summary>
        /// 数据接收完成后的回调委托
        /// </summary>
        /// <param name="count">接收数据数量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void onReceive(int count)
        {
            if (count != 0)
            {
                queue.Add(new RawSocketBuffer(buffer, bufferIndex, count));
                bufferIndex += (count + 3) & (int.MaxValue - 3);
                if (bufferIndex - buffer.Buffer.StartIndex > maxBufferIndex) buffer = null;
            }
        }
    }
}
