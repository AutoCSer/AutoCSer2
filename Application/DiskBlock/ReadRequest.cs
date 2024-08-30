using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 读取数据请求信息
    /// </summary>
    public sealed class ReadRequest : AutoCSer.Threading.Link<ReadRequest>
    {
        /// <summary>
        /// 读取数据回调
        /// </summary>
        internal readonly BlockCallback BlockCallback;
        /// <summary>
        /// 读取数据回调委托
        /// </summary>
        private readonly KeyValue<CommandServerCallback<ReadBuffer>, int> callback;
        /// <summary>
        /// 读取数据回调委托
        /// </summary>
        private LeftArray<KeyValue<CommandServerCallback<ReadBuffer>, int>> callbacks;
        /// <summary>
        /// 读取起始位置
        /// </summary>
        internal readonly long Index;
        /// <summary>
        /// 读取数据缓冲区
        /// </summary>
        internal ReadBuffer Buffer;
        /// <summary>
        /// 读取数据请求信息
        /// </summary>
        /// <param name="blockManager"></param>
        /// <param name="index"></param>
        /// <param name="callback"></param>
        internal ReadRequest(Block blockManager, ref BlockIndex index, ref CommandServerCallback<ReadBuffer> callback)
        {
            this.callback.Set(callback, index.Size);
            BlockCallback = new BlockCallback(BlockCallbackTypeEnum.Read, blockManager, this);
            Index = index.Index;
            callback = null;
        }
        /// <summary>
        /// 添加读取数据回调委托
        /// </summary>
        /// <param name="size"></param>
        /// <param name="callback"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void AppendCallback(int size, ref CommandServerCallback<ReadBuffer> callback)
        {
            callbacks.Add(new KeyValue<CommandServerCallback<ReadBuffer>, int>(callback, size));
            callback = null;
        }
        /// <summary>
        /// 错误取消写入操作
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void CancelCallback(ReadBuffer buffer)
        {
            callback.Key.Callback(buffer);
        }
        /// <summary>
        /// 读取数据回调
        /// </summary>
        internal void Callback()
        {
            if (Buffer.State == ReadBufferStateEnum.Success)
            {
                int size = Buffer.Buffer.Length;
                ReadBuffer errorBuffer = new ReadBuffer(ReadBufferStateEnum.Size);
                callback.Key.Callback(callback.Value == size ? Buffer : errorBuffer);
                foreach (KeyValue<CommandServerCallback<ReadBuffer>, int> callback in callbacks) callback.Key.Callback(callback.Value == size ? Buffer : errorBuffer);
            }
            else
            {
                callback.Key.Callback(Buffer);
                foreach (KeyValue<CommandServerCallback<ReadBuffer>, int> callback in callbacks) callback.Key.Callback(Buffer);
            }
        }
        /// <summary>
        /// 获取最大字节数
        /// </summary>
        /// <returns></returns>
        internal int GetMaxSize()
        {
            int size = callback.Value;
            foreach (KeyValue<CommandServerCallback<ReadBuffer>, int> callback in callbacks)
            {
                if (callback.Value > size) size = callback.Value;
            }
            return size;
        }
        /// <summary>
        /// 判断读取字节数是否存在
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        internal bool CheckSize(int size)
        {
            if (callback.Value == size) return true;
            foreach (KeyValue<CommandServerCallback<ReadBuffer>, int> callback in callbacks)
            {
                if (callback.Value == size) return true;
            }
            return false;
        }
    }
}
