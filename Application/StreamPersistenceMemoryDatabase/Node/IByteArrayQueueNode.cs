﻿using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 队列节点接口（先进先出）
    /// </summary>
    [ServerNode(MethodIndexEnumType = typeof(ByteArrayQueueNodeMethodEnum), IsAutoMethodIndex = false)]
    public interface IByteArrayQueueNode
    {
        /// <summary>
        /// 快照添加数据
        /// </summary>
        /// <param name="value"></param>
        [ServerMethod(IsClientCall = false, IsSnapshotMethod = true)]
        void SnapshotAdd(byte[] value);
        /// <summary>
        /// 获取队列数据数量
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int Count();
        /// <summary>
        /// 清除所有数据
        /// </summary>
        void Clear();
        /// <summary>
        /// 将数据添加到队列
        /// </summary>
        /// <param name="value"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void Enqueue(ServerByteArray value);
        /// <summary>
        /// 从队列中弹出一个数据 持久化参数检查
        /// </summary>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        ValueResult<ValueResult<byte[]>> TryDequeueBeforePersistence();
        /// <summary>
        /// 从队列中弹出一个数据
        /// </summary>
        /// <returns>没有可弹出数据则返回无数据</returns>
#if NetStandard21
        ValueResult<byte[]?> TryDequeue();
#else
        ValueResult<byte[]> TryDequeue();
#endif
        /// <summary>
        /// 从队列中弹出一个数据 持久化参数检查
        /// </summary>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        ValueResult<ResponseParameter> TryDequeueResponseParameterBeforePersistence();
        /// <summary>
        /// 从队列中弹出一个数据
        /// </summary>
        /// <returns>没有可弹出数据则返回无数据</returns>
        ResponseParameter TryDequeueResponseParameter();
        /// <summary>
        /// 获取队列中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <returns>没有可弹出数据则返回无数据</returns>
        [ServerMethod(IsPersistence = false)]
#if NetStandard21
        ValueResult<byte[]?> TryPeek();
#else
        ValueResult<byte[]> TryPeek();
#endif
        /// <summary>
        /// 获取队列中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <returns>没有可弹出数据则返回无数据</returns>
        [ServerMethod(IsPersistence = false)]
        ResponseParameter TryPeekResponseParameter();
    }
}