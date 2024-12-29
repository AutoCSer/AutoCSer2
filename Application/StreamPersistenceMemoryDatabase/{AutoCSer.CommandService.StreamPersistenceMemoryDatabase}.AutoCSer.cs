//本文件由程序自动生成，请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 数组节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IArrayNode<>))]
        public partial interface IArrayNodeClientNode<T>
        {
            /// <summary>
            /// 清除指定位置数据
            /// </summary>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">清除数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Clear(int startIndex, int count);
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter ClearArray();
            /// <summary>
            /// 用数据填充数组指定位置
            /// </summary>
            /// <param name="value"></param>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">填充数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Fill(T value, int startIndex, int count);
            /// <summary>
            /// 用数据填充整个数组
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter FillArray(T value);
            /// <summary>
            /// 获取数组长度
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> GetLength();
            /// <summary>
            /// 根据索引位置获取数据
            /// </summary>
            /// <param name="index">索引位置</param>
            /// <returns>超出索引返回则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>> GetValue(int index);
            /// <summary>
            /// 根据索引位置设置数据并返回设置之前的数据
            /// </summary>
            /// <param name="index">索引位置</param>
            /// <param name="value">数据</param>
            /// <returns>设置之前的数据，超出索引返回则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>> GetValueSet(int index, T value);
            /// <summary>
            /// 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">查找匹配数据数量</param>
            /// <returns>失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> IndexOf(T value, int startIndex, int count);
            /// <summary>
            /// 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns>失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> IndexOfArray(T value);
            /// <summary>
            /// 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <param name="startIndex">最后一个匹配位置（起始位置）</param>
            /// <param name="count">查找匹配数据数量</param>
            /// <returns>失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> LastIndexOf(T value, int startIndex, int count);
            /// <summary>
            /// 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns>失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> LastIndexOfArray(T value);
            /// <summary>
            /// 反转指定位置数组数据
            /// </summary>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">反转数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Reverse(int startIndex, int count);
            /// <summary>
            /// 反转整个数组数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter ReverseArray();
            /// <summary>
            /// 根据索引位置设置数据
            /// </summary>
            /// <param name="index">索引位置</param>
            /// <param name="value">数据</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> SetValue(int index, T value);
            /// <summary>
            /// 排序指定位置数组数据
            /// </summary>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">排序数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Sort(int startIndex, int count);
            /// <summary>
            /// 数组排序
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter SortArray();
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 位图节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IBitmapNode))]
        public partial interface IBitmapNodeClientNode
        {
            /// <summary>
            /// 清除位状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>是否设置成功，失败表示索引超出范围</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> ClearBit(uint index);
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter ClearMap();
            /// <summary>
            /// 读取位状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>非 0 表示二进制位为已设置状态，索引超出则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<int>> GetBit(uint index);
            /// <summary>
            /// 清除位状态并返回设置之前的状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>清除操作之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<int>> GetBitClearBit(uint index);
            /// <summary>
            /// 状态取反并返回操作之前的状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>取反操作之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<int>> GetBitInvertBit(uint index);
            /// <summary>
            /// 设置位状态并返回设置之前的状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>设置之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<int>> GetBitSetBit(uint index);
            /// <summary>
            /// 获取二进制位数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<uint> GetCapacity();
            /// <summary>
            /// 状态取反
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>是否设置成功，失败表示索引超出范围</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> InvertBit(uint index);
            /// <summary>
            /// 设置位状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>是否设置成功，失败表示索引超出范围</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> SetBit(uint index);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 字典节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IByteArrayDictionaryNode<>))]
        public partial interface IByteArrayDictionaryNodeClientNode<KT>
        {
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> ContainsKey(KT key);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> Count();
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns>被删除数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<byte[]>> GetRemove(KT key);
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns>被删除数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter> GetRemoveResponseParameter(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter returnValue, KT key);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="key"></param>
            /// <returns>是否删除成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Remove(KT key);
            /// <summary>
            /// 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            /// <param name="capacity">新容器初始化大小</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Renew(int capacity);
            /// <summary>
            /// 强制设置数据，如果关键字已存在则覆盖
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否设置成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Set(KT key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value);
            /// <summary>
            /// 尝试添加数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> TryAdd(KT key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter> TryGetResponseParameter(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter returnValue, KT key);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<byte[]>> TryGetValue(KT key);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 256 基分片字典 节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IByteArrayFragmentDictionaryNode<>))]
        public partial interface IByteArrayFragmentDictionaryNodeClientNode<KT>
        {
            /// <summary>
            /// 清除数据（保留分片数组）
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 清除分片数组（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter ClearArray();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> ContainsKey(KT key);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> Count();
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns>被删除数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<byte[]>> GetRemove(KT key);
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns>被删除数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter> GetRemoveResponseParameter(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter returnValue, KT key);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="key"></param>
            /// <returns>是否存在关键字</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Remove(KT key);
            /// <summary>
            /// 强制设置数据，如果关键字已存在则覆盖
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否设置成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Set(KT key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value);
            /// <summary>
            /// 如果关键字不存在则添加数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> TryAdd(KT key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter> TryGetResponseParameter(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter returnValue, KT key);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<byte[]>> TryGetValue(KT key);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 队列节点接口（先进先出） 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IByteArrayQueueNode))]
        public partial interface IByteArrayQueueNodeClientNode
        {
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 获取队列数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> Count();
            /// <summary>
            /// 将数据添加到队列
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Enqueue(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value);
            /// <summary>
            /// 从队列中弹出一个数据
            /// </summary>
            /// <returns>没有可弹出数据则返回无数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<byte[]>> TryDequeue();
            /// <summary>
            /// 从队列中弹出一个数据
            /// </summary>
            /// <returns>没有可弹出数据则返回无数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter> TryDequeueResponseParameter(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter returnValue);
            /// <summary>
            /// 获取队列中下一个弹出数据（不弹出数据仅查看）
            /// </summary>
            /// <returns>没有可弹出数据则返回无数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<byte[]>> TryPeek();
            /// <summary>
            /// 获取队列中下一个弹出数据（不弹出数据仅查看）
            /// </summary>
            /// <returns>没有可弹出数据则返回无数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter> TryPeekResponseParameter(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter returnValue);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 栈节点（后进先出） 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IByteArrayStackNode))]
        public partial interface IByteArrayStackNodeClientNode
        {
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> Count();
            /// <summary>
            /// 将数据添加到栈
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Push(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value);
            /// <summary>
            /// 获取栈中下一个弹出数据（不弹出数据仅查看）
            /// </summary>
            /// <returns>没有可弹出数据则返回无数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<byte[]>> TryPeek();
            /// <summary>
            /// 获取栈中下一个弹出数据（不弹出数据仅查看）
            /// </summary>
            /// <returns>没有可弹出数据则返回无数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter> TryPeekResponseParameter(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter returnValue);
            /// <summary>
            /// 从栈中弹出一个数据
            /// </summary>
            /// <returns>没有可弹出数据则返回无数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<byte[]>> TryPop();
            /// <summary>
            /// 从栈中弹出一个数据
            /// </summary>
            /// <returns>没有可弹出数据则返回无数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter> TryPopResponseParameter(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter returnValue);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 字典节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IDictionaryNode<,>))]
        public partial interface IDictionaryNodeClientNode<KT,VT>
        {
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> ContainsKey(KT key);
            /// <summary>
            /// 判断数据是否存在，时间复杂度 O(n) 不建议调用（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> ContainsValue(VT value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> Count();
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns>被删除数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>> GetRemove(KT key);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="key"></param>
            /// <returns>是否删除成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Remove(KT key);
            /// <summary>
            /// 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            /// <param name="capacity">新容器初始化大小</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Renew(int capacity);
            /// <summary>
            /// 强制设置数据，如果关键字已存在则覆盖
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否设置成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Set(KT key, VT value);
            /// <summary>
            /// 尝试添加数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> TryAdd(KT key, VT value);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>> TryGetValue(KT key);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 分布式锁节点 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IDistributedLockNode<>))]
        public partial interface IDistributedLockNodeClientNode<T>
        {
            /// <summary>
            /// 申请锁
            /// </summary>
            /// <param name="key">锁关键字</param>
            /// <param name="timeoutSeconds">超时秒数</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<long> Enter(T key, ushort timeoutSeconds);
            /// <summary>
            /// 释放锁
            /// </summary>
            /// <param name="key">锁关键字</param>
            /// <param name="identity">锁操作标识</param>
            AutoCSer.Net.SendOnlyCommand Release(T key, long identity);
            /// <summary>
            /// 尝试申请锁
            /// </summary>
            /// <param name="key">锁关键字</param>
            /// <param name="timeoutSeconds">超时秒数</param>
            /// <returns>失败返回 0</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<long> TryEnter(T key, ushort timeoutSeconds);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 256 基分片字典 节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IFragmentDictionaryNode<,>))]
        public partial interface IFragmentDictionaryNodeClientNode<KT,VT>
        {
            /// <summary>
            /// 清除数据（保留分片数组）
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 清除分片数组（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter ClearArray();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> ContainsKey(KT key);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> Count();
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>> GetRemove(KT key);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="key"></param>
            /// <returns>是否存在关键字</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Remove(KT key);
            /// <summary>
            /// 强制设置数据，如果关键字已存在则覆盖
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否设置成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Set(KT key, VT value);
            /// <summary>
            /// 如果关键字不存在则添加数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> TryAdd(KT key, VT value);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>> TryGetValue(KT key);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 256 基分片 哈希表 节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IFragmentHashSetNode<>))]
        public partial interface IFragmentHashSetNodeClientNode<T>
        {
            /// <summary>
            /// 如果关键字不存在则添加数据
            /// </summary>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Add(T value);
            /// <summary>
            /// 清除数据（保留分片数组）
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 清除分片数组（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter ClearArray();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Contains(T value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> Count();
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="value"></param>
            /// <returns>是否存在关键字</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Remove(T value);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 字典节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IHashBytesDictionaryNode))]
        public partial interface IHashBytesDictionaryNodeClientNode
        {
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> ContainsKey(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> Count();
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns>被删除数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<byte[]>> GetRemove(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key);
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns>被删除数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter> GetRemoveResponseParameter(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter returnValue, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="key"></param>
            /// <returns>是否删除成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Remove(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key);
            /// <summary>
            /// 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            /// <param name="capacity">新容器初始化大小</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Renew(int capacity);
            /// <summary>
            /// 强制设置数据，如果关键字已存在则覆盖
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否设置成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Set(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value);
            /// <summary>
            /// 尝试添加数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> TryAdd(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter> TryGetResponseParameter(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter returnValue, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<byte[]>> TryGetValue(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 256 基分片 HashBytes 字典 节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IHashBytesFragmentDictionaryNode))]
        public partial interface IHashBytesFragmentDictionaryNodeClientNode
        {
            /// <summary>
            /// 清除数据（保留分片数组）
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 清除分片数组（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter ClearArray();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> ContainsKey(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> Count();
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns>被删除数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<byte[]>> GetRemove(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key);
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns>被删除数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter> GetRemoveResponseParameter(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter returnValue, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="key"></param>
            /// <returns>是否存在关键字</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Remove(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key);
            /// <summary>
            /// 强制设置数据，如果关键字已存在则覆盖
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否设置成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Set(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value);
            /// <summary>
            /// 如果关键字不存在则添加数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> TryAdd(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter> TryGetResponseParameter(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter returnValue, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<byte[]>> TryGetValue(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 哈希表节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IHashSetNode<>))]
        public partial interface IHashSetNodeClientNode<T>
        {
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Add(T value);
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Contains(T value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> Count();
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="value"></param>
            /// <returns>是否删除成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Remove(T value);
            /// <summary>
            /// 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Renew();
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 64 位自增ID 节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IIdentityGeneratorNode))]
        public partial interface IIdentityGeneratorNodeClientNode
        {
            /// <summary>
            /// 获取下一个自增ID
            /// </summary>
            /// <returns>下一个自增ID，失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<long> Next();
            /// <summary>
            /// 获取自增 ID 分段
            /// </summary>
            /// <param name="count">获取数量</param>
            /// <returns>自增 ID 分段</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityFragment> NextFragment(int count);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 数组节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ILeftArrayNode<>))]
        public partial interface ILeftArrayNodeClientNode<T>
        {
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="value">数据</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Add(T value);
            /// <summary>
            /// 清除指定位置数据
            /// </summary>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">清除数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Clear(int startIndex, int count);
            /// <summary>
            /// 清除所有数据并将数据有效长度设置为 0
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter ClearLength();
            /// <summary>
            /// 用数据填充数组指定位置
            /// </summary>
            /// <param name="value"></param>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">填充数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Fill(T value, int startIndex, int count);
            /// <summary>
            /// 用数据填充整个数组
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter FillArray(T value);
            /// <summary>
            /// 获取数组容器初大小
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> GetCapacity();
            /// <summary>
            /// 获取容器空闲数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> GetFreeCount();
            /// <summary>
            /// 获取有效数组长度
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> GetLength();
            /// <summary>
            /// 移除最后一个数据并返回该数据
            /// </summary>
            /// <returns>没有可移除数据则无数据返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>> GetTryPopValue();
            /// <summary>
            /// 根据索引位置获取数据
            /// </summary>
            /// <param name="index">索引位置</param>
            /// <returns>超出索引返回则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>> GetValue(int index);
            /// <summary>
            /// 移除指定索引位置数据并返回被移除的数据
            /// </summary>
            /// <param name="index">数据位置</param>
            /// <returns>超出索引范围则无数据返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>> GetValueRemoveAt(int index);
            /// <summary>
            /// 移除指定索引位置数据，将最后一个数据移动到该指定位置，并返回被移除的数据
            /// </summary>
            /// <param name="index"></param>
            /// <returns>超出索引范围则无数据返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>> GetValueRemoveToEnd(int index);
            /// <summary>
            /// 根据索引位置设置数据并返回设置之前的数据
            /// </summary>
            /// <param name="index">索引位置</param>
            /// <param name="value">数据</param>
            /// <returns>设置之前的数据，超出索引返回则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>> GetValueSet(int index, T value);
            /// <summary>
            /// 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">查找匹配数据数量</param>
            /// <returns>失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> IndexOf(T value, int startIndex, int count);
            /// <summary>
            /// 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns>失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> IndexOfArray(T value);
            /// <summary>
            /// 插入数据
            /// </summary>
            /// <param name="index">插入位置</param>
            /// <param name="value">数据</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Insert(int index, T value);
            /// <summary>
            /// 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <param name="startIndex">最后一个匹配位置（起始位置）</param>
            /// <param name="count">查找匹配数据数量</param>
            /// <returns>失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> LastIndexOf(T value, int startIndex, int count);
            /// <summary>
            /// 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns>失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> LastIndexOfArray(T value);
            /// <summary>
            /// 移除第一个匹配数据（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value">数据</param>
            /// <returns>是否存在移除数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Remove(T value);
            /// <summary>
            /// 移除指定索引位置数据
            /// </summary>
            /// <param name="index">数据位置</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> RemoveAt(int index);
            /// <summary>
            /// 移除指定索引位置数据并将最后一个数据移动到该指定位置
            /// </summary>
            /// <param name="index"></param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> RemoveToEnd(int index);
            /// <summary>
            /// 反转指定位置数组数据
            /// </summary>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">反转数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Reverse(int startIndex, int count);
            /// <summary>
            /// 反转整个数组数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter ReverseArray();
            /// <summary>
            /// 置空并释放数组
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter SetEmpty();
            /// <summary>
            /// 根据索引位置设置数据
            /// </summary>
            /// <param name="index">索引位置</param>
            /// <param name="value">数据</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> SetValue(int index, T value);
            /// <summary>
            /// 排序指定位置数组数据
            /// </summary>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">排序数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Sort(int startIndex, int count);
            /// <summary>
            /// 数组排序
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter SortArray();
            /// <summary>
            /// 当有空闲位置时添加数据
            /// </summary>
            /// <param name="value"></param>
            /// <returns>如果数组已满则添加失败并返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> TryAdd(T value);
            /// <summary>
            /// 移除最后一个数据
            /// </summary>
            /// <returns>是否存在可移除数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> TryPop();
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 消息处理节点 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IMessageNode<>))]
        public partial interface IMessageNodeClientNode<T>
        {
            /// <summary>
            /// 生产者添加新消息
            /// </summary>
            /// <param name="message"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter AppendMessage(T message);
            /// <summary>
            /// 清除所有消息
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 清除所有失败消息
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter ClearFailed();
            /// <summary>
            /// 消息完成处理
            /// </summary>
            /// <param name="identity"></param>
            AutoCSer.Net.SendOnlyCommand Completed(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity identity);
            /// <summary>
            /// 消息失败处理
            /// </summary>
            /// <param name="identity"></param>
            AutoCSer.Net.SendOnlyCommand Failed(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity identity);
            /// <summary>
            /// 获取消费者回调数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> GetCallbackCount();
            /// <summary>
            /// 获取未处理完成消息数量（不包括失败消息）
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> GetCount();
            /// <summary>
            /// 获取失败消息数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> GetFailedCount();
            /// <summary>
            /// 
            /// </summary>
            /// <param name="maxCount"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackResponse<T>> GetMessage(int maxCount);
            /// <summary>
            /// 获取未完成处理超时消息数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> GetTimeoutCount();
            /// <summary>
            /// 获取未处理完成消息数量（包括失败消息）
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> GetTotalCount();
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 队列节点接口（先进先出） 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IQueueNode<>))]
        public partial interface IQueueNodeClientNode<T>
        {
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 判断队列中是否存在匹配数据（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value">匹配数据</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Contains(T value);
            /// <summary>
            /// 获取队列数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> Count();
            /// <summary>
            /// 将数据添加到队列
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Enqueue(T value);
            /// <summary>
            /// 从队列中弹出一个数据
            /// </summary>
            /// <returns>没有可弹出数据则返回无数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>> TryDequeue();
            /// <summary>
            /// 获取队列中下一个弹出数据（不弹出数据仅查看）
            /// </summary>
            /// <returns>没有可弹出数据则返回无数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>> TryPeek();
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 二叉搜索树节点 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISearchTreeDictionaryNode<,>))]
        public partial interface ISearchTreeDictionaryNodeClientNode<KT,VT>
        {
            /// <summary>
            /// 清除数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 判断是否包含关键字
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>是否包含关键字</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> ContainsKey(KT key);
            /// <summary>
            /// 获取节点数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> Count();
            /// <summary>
            /// 根据关键字比它小的节点数量
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>节点数量，失败返回 -1</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> CountLess(KT key);
            /// <summary>
            /// 根据关键字比它大的节点数量
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>节点数量，失败返回 -1</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> CountThan(KT key);
            /// <summary>
            /// 获取树高度，时间复杂度 O(n)
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> GetHeight();
            /// <summary>
            /// 根据关键字删除节点
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>被删除数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>> GetRemove(KT key);
            /// <summary>
            /// 获取范围数据集合
            /// </summary>
            /// <param name="skipCount">跳过记录数</param>
            /// <param name="getCount">获取记录数</param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackResponse<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> GetValues(int skipCount, byte getCount);
            /// <summary>
            /// 根据关键字获取一个匹配节点位置
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>一个匹配节点位置,失败返回-1</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> IndexOf(KT key);
            /// <summary>
            /// 根据关键字删除节点
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>是否存在关键字</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Remove(KT key);
            /// <summary>
            /// 设置数据
            /// </summary>
            /// <param name="key">关键字</param>
            /// <param name="value">数据</param>
            /// <returns>是否添加了关键字</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Set(KT key, VT value);
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="key">关键字</param>
            /// <param name="value">数据</param>
            /// <returns>是否添加了数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> TryAdd(KT key, VT value);
            /// <summary>
            /// 获取第一个关键字数据
            /// </summary>
            /// <returns>第一个关键字数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<KT>> TryGetFirstKey();
            /// <summary>
            /// 获取第一组数据
            /// </summary>
            /// <returns>第一组数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<AutoCSer.KeyValue<KT,VT>>> TryGetFirstKeyValue();
            /// <summary>
            /// 获取第一个数据
            /// </summary>
            /// <returns>第一个数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>> TryGetFirstValue();
            /// <summary>
            /// 根据节点位置获取数据
            /// </summary>
            /// <param name="index">节点位置</param>
            /// <returns>数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<AutoCSer.KeyValue<KT,VT>>> TryGetKeyValueByIndex(int index);
            /// <summary>
            /// 获取最后一个关键字数据
            /// </summary>
            /// <returns>最后一个关键字数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<KT>> TryGetLastKey();
            /// <summary>
            /// 获取最后一组数据
            /// </summary>
            /// <returns>最后一组数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<AutoCSer.KeyValue<KT,VT>>> TryGetLastKeyValue();
            /// <summary>
            /// 获取最后一个数据
            /// </summary>
            /// <returns>最后一个数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>> TryGetLastValue();
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>目标数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>> TryGetValue(KT key);
            /// <summary>
            /// 根据节点位置获取数据
            /// </summary>
            /// <param name="index">节点位置</param>
            /// <returns>数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>> TryGetValueByIndex(int index);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 二叉搜索树集合节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISearchTreeSetNode<>))]
        public partial interface ISearchTreeSetNodeClientNode<T>
        {
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="value">关键字</param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Add(T value);
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="value">关键字</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Contains(T value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> Count();
            /// <summary>
            /// 根据关键字比它小的节点数量
            /// </summary>
            /// <param name="value">关键字</param>
            /// <returns>节点数量，失败返回 -1</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> CountLess(T value);
            /// <summary>
            /// 根据关键字比它大的节点数量
            /// </summary>
            /// <param name="value">关键字</param>
            /// <returns>节点数量，失败返回 -1</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> CountThan(T value);
            /// <summary>
            /// 根据节点位置获取数据
            /// </summary>
            /// <param name="index">节点位置</param>
            /// <returns>数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>> GetByIndex(int index);
            /// <summary>
            /// 获取第一个数据
            /// </summary>
            /// <returns>没有数据时返回无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>> GetFrist();
            /// <summary>
            /// 获取最后一个数据
            /// </summary>
            /// <returns>没有数据时返回无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>> GetLast();
            /// <summary>
            /// 根据关键字获取一个匹配节点位置
            /// </summary>
            /// <param name="value">关键字</param>
            /// <returns>一个匹配节点位置,失败返回-1</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> IndexOf(T value);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="value">关键字</param>
            /// <returns>是否删除成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Remove(T value);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 服务基础操作接口方法映射枚举 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNode))]
        public partial interface IServiceNodeClientNode
        {
            /// <summary>
            /// 创建数组节点 ArrayNode{T}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="length">数组长度</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateArrayNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int length);
            /// <summary>
            /// 创建位图节点 BitmapNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="capacity">二进制位数量</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateBitmapNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, uint capacity);
            /// <summary>
            /// 创建字典节点 ByteArrayDictionaryNode{KT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateByteArrayDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity);
            /// <summary>
            /// 创建字典节点 ByteArrayFragmentDictionaryNode{KT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">节点信息</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateByteArrayFragmentDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType);
            /// <summary>
            /// 创建队列节点（先进先出） ByteArrayQueueNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateByteArrayQueueNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int capacity);
            /// <summary>
            /// 创建栈节点（后进先出） ByteArrayStackNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateByteArrayStackNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int capacity);
            /// <summary>
            /// 创建字典节点 DictionaryNode{KT,VT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="valueType">数据类型</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType, int capacity);
            /// <summary>
            /// 创建分布式锁节点 DistributedLockNode{KT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateDistributedLockNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType);
            /// <summary>
            /// 创建字典节点 FragmentDictionaryNode{KT,VT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="valueType">数据类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateFragmentDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType);
            /// <summary>
            /// 创建 256 基分片哈希表节点 FragmentHashSetNode{KT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateFragmentHashSetNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType);
            /// <summary>
            /// 创建字典节点 HashBytesDictionaryNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateHashBytesDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int capacity);
            /// <summary>
            /// 创建字典节点 HashBytesFragmentDictionaryNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateHashBytesFragmentDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
            /// <summary>
            /// 创建哈希表节点 HashSetNode{KT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateHashSetNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType);
            /// <summary>
            /// 创建 64 位自增ID 节点 IdentityGeneratorNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="identity">起始分配 ID</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateIdentityGeneratorNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, long identity);
            /// <summary>
            /// 创建数组节点 LeftArrayNode{T}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateLeftArrayNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity);
            /// <summary>
            /// 创建消息处理节点 MessageNode{T}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="messageType">消息数据类型</param>
            /// <param name="arraySize">正在处理消息数组大小</param>
            /// <param name="timeoutSeconds">消息处理超时秒数</param>
            /// <param name="checkTimeoutSeconds">消息超时检查间隔秒数</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateMessageNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType messageType, int arraySize, int timeoutSeconds, int checkTimeoutSeconds);
            /// <summary>
            /// 创建队列节点（先进先出） QueueNode{T}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateQueueNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity);
            /// <summary>
            /// 创建二叉搜索树节点 SearchTreeDictionaryNode{KT,VT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="valueType">数据类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateSearchTreeDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType);
            /// <summary>
            /// 创建二叉搜索树集合节点 SearchTreeSetNode{KT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateSearchTreeSetNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType);
            /// <summary>
            /// 创建消息处理节点 MessageNode{ServerByteArrayMessage}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="arraySize">正在处理消息数组大小</param>
            /// <param name="timeoutSeconds">消息处理超时秒数</param>
            /// <param name="checkTimeoutSeconds">消息超时检查间隔秒数</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateServerByteArrayMessageNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds);
            /// <summary>
            /// 创建排序字典节点 SortedDictionaryNode{KT,VT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="valueType">数据类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateSortedDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType);
            /// <summary>
            /// 创建排序列表节点 SortedListNode{KT,VT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="valueType">数据类型</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateSortedListNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType, int capacity);
            /// <summary>
            /// 创建排序集合节点 SortedSetNode{KT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateSortedSetNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType);
            /// <summary>
            /// 创建栈节点（后进先出） StackNode{T}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateStackNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity);
            /// <summary>
            /// 删除节点
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <returns>是否成功删除节点，否则表示没有找到节点</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> RemoveNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 排序字典节点 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISortedDictionaryNode<,>))]
        public partial interface ISortedDictionaryNodeClientNode<KT,VT>
        {
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> ContainsKey(KT key);
            /// <summary>
            /// 判断数据是否存在，时间复杂度 O(n) 不建议调用（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> ContainsValue(VT value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> Count();
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>> GetRemove(KT key);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="key"></param>
            /// <returns>是否删除成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Remove(KT key);
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> TryAdd(KT key, VT value);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>> TryGetValue(KT key);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 排序列表节点 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISortedListNode<,>))]
        public partial interface ISortedListNodeClientNode<KT,VT>
        {
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> ContainsKey(KT key);
            /// <summary>
            /// 判断数据是否存在，时间复杂度 O(n) 不建议调用（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> ContainsValue(VT value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> Count();
            /// <summary>
            /// 获取容器大小
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> GetCapacity();
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>> GetRemove(KT key);
            /// <summary>
            /// 获取关键字排序位置
            /// </summary>
            /// <param name="key"></param>
            /// <returns>负数表示没有找到关键字</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> IndexOfKey(KT key);
            /// <summary>
            /// 获取第一个匹配数据排序位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns>负数表示没有找到匹配数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> IndexOfValue(VT value);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="key"></param>
            /// <returns>是否删除成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Remove(KT key);
            /// <summary>
            /// 删除指定排序索引位置数据
            /// </summary>
            /// <param name="index"></param>
            /// <returns>索引超出范围返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> RemoveAt(int index);
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> TryAdd(KT key, VT value);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>> TryGetValue(KT key);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 排序集合节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISortedSetNode<>))]
        public partial interface ISortedSetNodeClientNode<T>
        {
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Add(T value);
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Contains(T value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> Count();
            /// <summary>
            /// 获取最大值
            /// </summary>
            /// <returns>没有数据时返回无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>> GetMax();
            /// <summary>
            /// 获取最小值
            /// </summary>
            /// <returns>没有数据时返回无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>> GetMin();
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="value"></param>
            /// <returns>是否删除成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Remove(T value);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 栈节点（后进先出） 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IStackNode<>))]
        public partial interface IStackNodeClientNode<T>
        {
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 判断是否存在匹配数据（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value">匹配数据</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Contains(T value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> Count();
            /// <summary>
            /// 将数据添加到栈
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Push(T value);
            /// <summary>
            /// 获取栈中下一个弹出数据（不弹出数据仅查看）
            /// </summary>
            /// <returns>没有可弹出数据则返回无数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>> TryPeek();
            /// <summary>
            /// 从栈中弹出一个数据
            /// </summary>
            /// <returns>没有可弹出数据则返回无数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>> TryPop();
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 数组节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IArrayNode<>))]
        public partial interface IArrayNodeLocalClientNode<T>
        {
            /// <summary>
            /// 清除指定位置数据
            /// </summary>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">清除数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Clear(int startIndex, int count);
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> ClearArray();
            /// <summary>
            /// 用数据填充数组指定位置
            /// </summary>
            /// <param name="value"></param>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">填充数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Fill(T value, int startIndex, int count);
            /// <summary>
            /// 用数据填充整个数组
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> FillArray(T value);
            /// <summary>
            /// 获取数组长度
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> GetLength();
            /// <summary>
            /// 根据索引位置获取数据
            /// </summary>
            /// <param name="index">索引位置</param>
            /// <returns>超出索引返回则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetValue(int index);
            /// <summary>
            /// 根据索引位置设置数据并返回设置之前的数据
            /// </summary>
            /// <param name="index">索引位置</param>
            /// <param name="value">数据</param>
            /// <returns>设置之前的数据，超出索引返回则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetValueSet(int index, T value);
            /// <summary>
            /// 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">查找匹配数据数量</param>
            /// <returns>失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> IndexOf(T value, int startIndex, int count);
            /// <summary>
            /// 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns>失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> IndexOfArray(T value);
            /// <summary>
            /// 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <param name="startIndex">最后一个匹配位置（起始位置）</param>
            /// <param name="count">查找匹配数据数量</param>
            /// <returns>失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> LastIndexOf(T value, int startIndex, int count);
            /// <summary>
            /// 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns>失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> LastIndexOfArray(T value);
            /// <summary>
            /// 反转指定位置数组数据
            /// </summary>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">反转数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Reverse(int startIndex, int count);
            /// <summary>
            /// 反转整个数组数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> ReverseArray();
            /// <summary>
            /// 根据索引位置设置数据
            /// </summary>
            /// <param name="index">索引位置</param>
            /// <param name="value">数据</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> SetValue(int index, T value);
            /// <summary>
            /// 排序指定位置数组数据
            /// </summary>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">排序数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Sort(int startIndex, int count);
            /// <summary>
            /// 数组排序
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> SortArray();
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 位图节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IBitmapNode))]
        public partial interface IBitmapNodeLocalClientNode
        {
            /// <summary>
            /// 清除位状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>是否设置成功，失败表示索引超出范围</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> ClearBit(uint index);
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> ClearMap();
            /// <summary>
            /// 读取位状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>非 0 表示二进制位为已设置状态，索引超出则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<int>>> GetBit(uint index);
            /// <summary>
            /// 清除位状态并返回设置之前的状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>清除操作之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<int>>> GetBitClearBit(uint index);
            /// <summary>
            /// 状态取反并返回操作之前的状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>取反操作之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<int>>> GetBitInvertBit(uint index);
            /// <summary>
            /// 设置位状态并返回设置之前的状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>设置之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<int>>> GetBitSetBit(uint index);
            /// <summary>
            /// 获取二进制位数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<uint>> GetCapacity();
            /// <summary>
            /// 状态取反
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>是否设置成功，失败表示索引超出范围</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> InvertBit(uint index);
            /// <summary>
            /// 设置位状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>是否设置成功，失败表示索引超出范围</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> SetBit(uint index);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 字典节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IDictionaryNode<,>))]
        public partial interface IDictionaryNodeLocalClientNode<KT,VT>
        {
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Clear();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> ContainsKey(KT key);
            /// <summary>
            /// 判断数据是否存在，时间复杂度 O(n) 不建议调用（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> ContainsValue(VT value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> Count();
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns>被删除数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> GetRemove(KT key);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="key"></param>
            /// <returns>是否删除成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Remove(KT key);
            /// <summary>
            /// 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            /// <param name="capacity">新容器初始化大小</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Renew(int capacity);
            /// <summary>
            /// 强制设置数据，如果关键字已存在则覆盖
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否设置成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Set(KT key, VT value);
            /// <summary>
            /// 尝试添加数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> TryAdd(KT key, VT value);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> TryGetValue(KT key);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 分布式锁节点 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IDistributedLockNode<>))]
        public partial interface IDistributedLockNodeLocalClientNode<T>
        {
            /// <summary>
            /// 申请锁
            /// </summary>
            /// <param name="key">锁关键字</param>
            /// <param name="timeoutSeconds">超时秒数</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<long>> Enter(T key, ushort timeoutSeconds);
            /// <summary>
            /// 释放锁
            /// </summary>
            /// <param name="key">锁关键字</param>
            /// <param name="identity">锁操作标识</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter Release(T key, long identity);
            /// <summary>
            /// 尝试申请锁
            /// </summary>
            /// <param name="key">锁关键字</param>
            /// <param name="timeoutSeconds">超时秒数</param>
            /// <returns>失败返回 0</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<long>> TryEnter(T key, ushort timeoutSeconds);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 256 基分片字典 节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IFragmentDictionaryNode<,>))]
        public partial interface IFragmentDictionaryNodeLocalClientNode<KT,VT>
        {
            /// <summary>
            /// 清除数据（保留分片数组）
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Clear();
            /// <summary>
            /// 清除分片数组（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> ClearArray();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> ContainsKey(KT key);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> Count();
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> GetRemove(KT key);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="key"></param>
            /// <returns>是否存在关键字</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Remove(KT key);
            /// <summary>
            /// 强制设置数据，如果关键字已存在则覆盖
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否设置成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Set(KT key, VT value);
            /// <summary>
            /// 如果关键字不存在则添加数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> TryAdd(KT key, VT value);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> TryGetValue(KT key);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 256 基分片 哈希表 节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IFragmentHashSetNode<>))]
        public partial interface IFragmentHashSetNodeLocalClientNode<T>
        {
            /// <summary>
            /// 如果关键字不存在则添加数据
            /// </summary>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Add(T value);
            /// <summary>
            /// 清除数据（保留分片数组）
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Clear();
            /// <summary>
            /// 清除分片数组（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> ClearArray();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Contains(T value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> Count();
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="value"></param>
            /// <returns>是否存在关键字</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Remove(T value);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 哈希表节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IHashSetNode<>))]
        public partial interface IHashSetNodeLocalClientNode<T>
        {
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Add(T value);
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Clear();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Contains(T value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> Count();
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="value"></param>
            /// <returns>是否删除成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Remove(T value);
            /// <summary>
            /// 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Renew();
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 64 位自增ID 节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IIdentityGeneratorNode))]
        public partial interface IIdentityGeneratorNodeLocalClientNode
        {
            /// <summary>
            /// 获取下一个自增ID
            /// </summary>
            /// <returns>下一个自增ID，失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<long>> Next();
            /// <summary>
            /// 获取自增 ID 分段
            /// </summary>
            /// <param name="count">获取数量</param>
            /// <returns>自增 ID 分段</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityFragment>> NextFragment(int count);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 数组节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ILeftArrayNode<>))]
        public partial interface ILeftArrayNodeLocalClientNode<T>
        {
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="value">数据</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Add(T value);
            /// <summary>
            /// 清除指定位置数据
            /// </summary>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">清除数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Clear(int startIndex, int count);
            /// <summary>
            /// 清除所有数据并将数据有效长度设置为 0
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> ClearLength();
            /// <summary>
            /// 用数据填充数组指定位置
            /// </summary>
            /// <param name="value"></param>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">填充数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Fill(T value, int startIndex, int count);
            /// <summary>
            /// 用数据填充整个数组
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> FillArray(T value);
            /// <summary>
            /// 获取数组容器初大小
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> GetCapacity();
            /// <summary>
            /// 获取容器空闲数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> GetFreeCount();
            /// <summary>
            /// 获取有效数组长度
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> GetLength();
            /// <summary>
            /// 移除最后一个数据并返回该数据
            /// </summary>
            /// <returns>没有可移除数据则无数据返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetTryPopValue();
            /// <summary>
            /// 根据索引位置获取数据
            /// </summary>
            /// <param name="index">索引位置</param>
            /// <returns>超出索引返回则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetValue(int index);
            /// <summary>
            /// 移除指定索引位置数据并返回被移除的数据
            /// </summary>
            /// <param name="index">数据位置</param>
            /// <returns>超出索引范围则无数据返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetValueRemoveAt(int index);
            /// <summary>
            /// 移除指定索引位置数据，将最后一个数据移动到该指定位置，并返回被移除的数据
            /// </summary>
            /// <param name="index"></param>
            /// <returns>超出索引范围则无数据返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetValueRemoveToEnd(int index);
            /// <summary>
            /// 根据索引位置设置数据并返回设置之前的数据
            /// </summary>
            /// <param name="index">索引位置</param>
            /// <param name="value">数据</param>
            /// <returns>设置之前的数据，超出索引返回则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetValueSet(int index, T value);
            /// <summary>
            /// 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">查找匹配数据数量</param>
            /// <returns>失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> IndexOf(T value, int startIndex, int count);
            /// <summary>
            /// 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns>失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> IndexOfArray(T value);
            /// <summary>
            /// 插入数据
            /// </summary>
            /// <param name="index">插入位置</param>
            /// <param name="value">数据</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Insert(int index, T value);
            /// <summary>
            /// 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <param name="startIndex">最后一个匹配位置（起始位置）</param>
            /// <param name="count">查找匹配数据数量</param>
            /// <returns>失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> LastIndexOf(T value, int startIndex, int count);
            /// <summary>
            /// 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns>失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> LastIndexOfArray(T value);
            /// <summary>
            /// 移除第一个匹配数据（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value">数据</param>
            /// <returns>是否存在移除数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Remove(T value);
            /// <summary>
            /// 移除指定索引位置数据
            /// </summary>
            /// <param name="index">数据位置</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> RemoveAt(int index);
            /// <summary>
            /// 移除指定索引位置数据并将最后一个数据移动到该指定位置
            /// </summary>
            /// <param name="index"></param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> RemoveToEnd(int index);
            /// <summary>
            /// 反转指定位置数组数据
            /// </summary>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">反转数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Reverse(int startIndex, int count);
            /// <summary>
            /// 反转整个数组数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> ReverseArray();
            /// <summary>
            /// 置空并释放数组
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> SetEmpty();
            /// <summary>
            /// 根据索引位置设置数据
            /// </summary>
            /// <param name="index">索引位置</param>
            /// <param name="value">数据</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> SetValue(int index, T value);
            /// <summary>
            /// 排序指定位置数组数据
            /// </summary>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">排序数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Sort(int startIndex, int count);
            /// <summary>
            /// 数组排序
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> SortArray();
            /// <summary>
            /// 当有空闲位置时添加数据
            /// </summary>
            /// <param name="value"></param>
            /// <returns>如果数组已满则添加失败并返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> TryAdd(T value);
            /// <summary>
            /// 移除最后一个数据
            /// </summary>
            /// <returns>是否存在可移除数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> TryPop();
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 消息处理节点 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IMessageNode<>))]
        public partial interface IMessageNodeLocalClientNode<T>
        {
            /// <summary>
            /// 生产者添加新消息
            /// </summary>
            /// <param name="message"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> AppendMessage(T message);
            /// <summary>
            /// 清除所有消息
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Clear();
            /// <summary>
            /// 清除所有失败消息
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> ClearFailed();
            /// <summary>
            /// 消息完成处理
            /// </summary>
            /// <param name="identity"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter Completed(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity identity);
            /// <summary>
            /// 消息失败处理
            /// </summary>
            /// <param name="identity"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter Failed(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity identity);
            /// <summary>
            /// 获取消费者回调数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> GetCallbackCount();
            /// <summary>
            /// 获取未处理完成消息数量（不包括失败消息）
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> GetCount();
            /// <summary>
            /// 获取失败消息数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> GetFailedCount();
            /// <summary>
            /// 
            /// </summary>
            /// <param name="maxCount"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackResponse<T>> GetMessage(int maxCount);
            /// <summary>
            /// 获取未完成处理超时消息数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> GetTimeoutCount();
            /// <summary>
            /// 获取未处理完成消息数量（包括失败消息）
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> GetTotalCount();
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 队列节点接口（先进先出） 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IQueueNode<>))]
        public partial interface IQueueNodeLocalClientNode<T>
        {
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Clear();
            /// <summary>
            /// 判断队列中是否存在匹配数据（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value">匹配数据</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Contains(T value);
            /// <summary>
            /// 获取队列数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> Count();
            /// <summary>
            /// 将数据添加到队列
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Enqueue(T value);
            /// <summary>
            /// 从队列中弹出一个数据
            /// </summary>
            /// <returns>没有可弹出数据则返回无数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> TryDequeue();
            /// <summary>
            /// 获取队列中下一个弹出数据（不弹出数据仅查看）
            /// </summary>
            /// <returns>没有可弹出数据则返回无数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> TryPeek();
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 二叉搜索树节点 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISearchTreeDictionaryNode<,>))]
        public partial interface ISearchTreeDictionaryNodeLocalClientNode<KT,VT>
        {
            /// <summary>
            /// 清除数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Clear();
            /// <summary>
            /// 判断是否包含关键字
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>是否包含关键字</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> ContainsKey(KT key);
            /// <summary>
            /// 获取节点数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> Count();
            /// <summary>
            /// 根据关键字比它小的节点数量
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>节点数量，失败返回 -1</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> CountLess(KT key);
            /// <summary>
            /// 根据关键字比它大的节点数量
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>节点数量，失败返回 -1</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> CountThan(KT key);
            /// <summary>
            /// 获取树高度，时间复杂度 O(n)
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> GetHeight();
            /// <summary>
            /// 根据关键字删除节点
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>被删除数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> GetRemove(KT key);
            /// <summary>
            /// 获取范围数据集合
            /// </summary>
            /// <param name="skipCount">跳过记录数</param>
            /// <param name="getCount">获取记录数</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackResponse<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> GetValues(int skipCount, byte getCount);
            /// <summary>
            /// 根据关键字获取一个匹配节点位置
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>一个匹配节点位置,失败返回-1</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> IndexOf(KT key);
            /// <summary>
            /// 根据关键字删除节点
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>是否存在关键字</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Remove(KT key);
            /// <summary>
            /// 设置数据
            /// </summary>
            /// <param name="key">关键字</param>
            /// <param name="value">数据</param>
            /// <returns>是否添加了关键字</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Set(KT key, VT value);
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="key">关键字</param>
            /// <param name="value">数据</param>
            /// <returns>是否添加了数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> TryAdd(KT key, VT value);
            /// <summary>
            /// 获取第一个关键字数据
            /// </summary>
            /// <returns>第一个关键字数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<KT>>> TryGetFirstKey();
            /// <summary>
            /// 获取第一组数据
            /// </summary>
            /// <returns>第一组数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<AutoCSer.KeyValue<KT,VT>>>> TryGetFirstKeyValue();
            /// <summary>
            /// 获取第一个数据
            /// </summary>
            /// <returns>第一个数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> TryGetFirstValue();
            /// <summary>
            /// 根据节点位置获取数据
            /// </summary>
            /// <param name="index">节点位置</param>
            /// <returns>数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<AutoCSer.KeyValue<KT,VT>>>> TryGetKeyValueByIndex(int index);
            /// <summary>
            /// 获取最后一个关键字数据
            /// </summary>
            /// <returns>最后一个关键字数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<KT>>> TryGetLastKey();
            /// <summary>
            /// 获取最后一组数据
            /// </summary>
            /// <returns>最后一组数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<AutoCSer.KeyValue<KT,VT>>>> TryGetLastKeyValue();
            /// <summary>
            /// 获取最后一个数据
            /// </summary>
            /// <returns>最后一个数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> TryGetLastValue();
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>目标数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> TryGetValue(KT key);
            /// <summary>
            /// 根据节点位置获取数据
            /// </summary>
            /// <param name="index">节点位置</param>
            /// <returns>数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> TryGetValueByIndex(int index);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 二叉搜索树集合节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISearchTreeSetNode<>))]
        public partial interface ISearchTreeSetNodeLocalClientNode<T>
        {
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="value">关键字</param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Add(T value);
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Clear();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="value">关键字</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Contains(T value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> Count();
            /// <summary>
            /// 根据关键字比它小的节点数量
            /// </summary>
            /// <param name="value">关键字</param>
            /// <returns>节点数量，失败返回 -1</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> CountLess(T value);
            /// <summary>
            /// 根据关键字比它大的节点数量
            /// </summary>
            /// <param name="value">关键字</param>
            /// <returns>节点数量，失败返回 -1</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> CountThan(T value);
            /// <summary>
            /// 根据节点位置获取数据
            /// </summary>
            /// <param name="index">节点位置</param>
            /// <returns>数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetByIndex(int index);
            /// <summary>
            /// 获取第一个数据
            /// </summary>
            /// <returns>没有数据时返回无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetFrist();
            /// <summary>
            /// 获取最后一个数据
            /// </summary>
            /// <returns>没有数据时返回无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetLast();
            /// <summary>
            /// 根据关键字获取一个匹配节点位置
            /// </summary>
            /// <param name="value">关键字</param>
            /// <returns>一个匹配节点位置,失败返回-1</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> IndexOf(T value);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="value">关键字</param>
            /// <returns>是否删除成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Remove(T value);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 服务基础操作接口方法映射枚举 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNode))]
        public partial interface IServiceNodeLocalClientNode
        {
            /// <summary>
            /// 创建数组节点 ArrayNode{T}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="length">数组长度</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateArrayNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int length);
            /// <summary>
            /// 创建位图节点 BitmapNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="capacity">二进制位数量</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateBitmapNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, uint capacity);
            /// <summary>
            /// 创建字典节点 ByteArrayDictionaryNode{KT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateByteArrayDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity);
            /// <summary>
            /// 创建字典节点 ByteArrayFragmentDictionaryNode{KT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">节点信息</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateByteArrayFragmentDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType);
            /// <summary>
            /// 创建队列节点（先进先出） ByteArrayQueueNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateByteArrayQueueNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int capacity);
            /// <summary>
            /// 创建栈节点（后进先出） ByteArrayStackNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateByteArrayStackNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int capacity);
            /// <summary>
            /// 创建字典节点 DictionaryNode{KT,VT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="valueType">数据类型</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType, int capacity);
            /// <summary>
            /// 创建分布式锁节点 DistributedLockNode{KT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateDistributedLockNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType);
            /// <summary>
            /// 创建字典节点 FragmentDictionaryNode{KT,VT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="valueType">数据类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateFragmentDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType);
            /// <summary>
            /// 创建 256 基分片哈希表节点 FragmentHashSetNode{KT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateFragmentHashSetNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType);
            /// <summary>
            /// 创建字典节点 HashBytesDictionaryNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateHashBytesDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int capacity);
            /// <summary>
            /// 创建字典节点 HashBytesFragmentDictionaryNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateHashBytesFragmentDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
            /// <summary>
            /// 创建哈希表节点 HashSetNode{KT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateHashSetNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType);
            /// <summary>
            /// 创建 64 位自增ID 节点 IdentityGeneratorNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="identity">起始分配 ID</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateIdentityGeneratorNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, long identity);
            /// <summary>
            /// 创建数组节点 LeftArrayNode{T}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateLeftArrayNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity);
            /// <summary>
            /// 创建消息处理节点 MessageNode{T}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="messageType">消息数据类型</param>
            /// <param name="arraySize">正在处理消息数组大小</param>
            /// <param name="timeoutSeconds">消息处理超时秒数</param>
            /// <param name="checkTimeoutSeconds">消息超时检查间隔秒数</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateMessageNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType messageType, int arraySize, int timeoutSeconds, int checkTimeoutSeconds);
            /// <summary>
            /// 创建队列节点（先进先出） QueueNode{T}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateQueueNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity);
            /// <summary>
            /// 创建二叉搜索树节点 SearchTreeDictionaryNode{KT,VT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="valueType">数据类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateSearchTreeDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType);
            /// <summary>
            /// 创建二叉搜索树集合节点 SearchTreeSetNode{KT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateSearchTreeSetNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType);
            /// <summary>
            /// 创建消息处理节点 MessageNode{ServerByteArrayMessage}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="arraySize">正在处理消息数组大小</param>
            /// <param name="timeoutSeconds">消息处理超时秒数</param>
            /// <param name="checkTimeoutSeconds">消息超时检查间隔秒数</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateServerByteArrayMessageNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds);
            /// <summary>
            /// 创建排序字典节点 SortedDictionaryNode{KT,VT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="valueType">数据类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateSortedDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType);
            /// <summary>
            /// 创建排序列表节点 SortedListNode{KT,VT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="valueType">数据类型</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateSortedListNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType, int capacity);
            /// <summary>
            /// 创建排序集合节点 SortedSetNode{KT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateSortedSetNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType);
            /// <summary>
            /// 创建栈节点（后进先出） StackNode{T}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateStackNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity);
            /// <summary>
            /// 删除节点
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <returns>是否成功删除节点，否则表示没有找到节点</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> RemoveNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 排序字典节点 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISortedDictionaryNode<,>))]
        public partial interface ISortedDictionaryNodeLocalClientNode<KT,VT>
        {
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Clear();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> ContainsKey(KT key);
            /// <summary>
            /// 判断数据是否存在，时间复杂度 O(n) 不建议调用（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> ContainsValue(VT value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> Count();
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> GetRemove(KT key);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="key"></param>
            /// <returns>是否删除成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Remove(KT key);
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> TryAdd(KT key, VT value);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> TryGetValue(KT key);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 排序列表节点 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISortedListNode<,>))]
        public partial interface ISortedListNodeLocalClientNode<KT,VT>
        {
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Clear();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> ContainsKey(KT key);
            /// <summary>
            /// 判断数据是否存在，时间复杂度 O(n) 不建议调用（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> ContainsValue(VT value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> Count();
            /// <summary>
            /// 获取容器大小
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> GetCapacity();
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> GetRemove(KT key);
            /// <summary>
            /// 获取关键字排序位置
            /// </summary>
            /// <param name="key"></param>
            /// <returns>负数表示没有找到关键字</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> IndexOfKey(KT key);
            /// <summary>
            /// 获取第一个匹配数据排序位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns>负数表示没有找到匹配数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> IndexOfValue(VT value);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="key"></param>
            /// <returns>是否删除成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Remove(KT key);
            /// <summary>
            /// 删除指定排序索引位置数据
            /// </summary>
            /// <param name="index"></param>
            /// <returns>索引超出范围返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> RemoveAt(int index);
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> TryAdd(KT key, VT value);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> TryGetValue(KT key);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 排序集合节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISortedSetNode<>))]
        public partial interface ISortedSetNodeLocalClientNode<T>
        {
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Add(T value);
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Clear();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Contains(T value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> Count();
            /// <summary>
            /// 获取最大值
            /// </summary>
            /// <returns>没有数据时返回无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetMax();
            /// <summary>
            /// 获取最小值
            /// </summary>
            /// <returns>没有数据时返回无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetMin();
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="value"></param>
            /// <returns>是否删除成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Remove(T value);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 栈节点（后进先出） 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IStackNode<>))]
        public partial interface IStackNodeLocalClientNode<T>
        {
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Clear();
            /// <summary>
            /// 判断是否存在匹配数据（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value">匹配数据</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Contains(T value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> Count();
            /// <summary>
            /// 将数据添加到栈
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Push(T value);
            /// <summary>
            /// 获取栈中下一个弹出数据（不弹出数据仅查看）
            /// </summary>
            /// <returns>没有可弹出数据则返回无数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> TryPeek();
            /// <summary>
            /// 从栈中弹出一个数据
            /// </summary>
            /// <returns>没有可弹出数据则返回无数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> TryPop();
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 数组节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IArrayNodeMethodEnum))]
        public partial interface IArrayNode<T> { }
        /// <summary>
        /// 数组节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IArrayNodeMethodEnum
        {
            /// <summary>
            /// [0] 清除指定位置数据
            /// int startIndex 起始位置
            /// int count 清除数据数量
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            Clear = 0,
            /// <summary>
            /// [1] 清除所有数据
            /// </summary>
            ClearArray = 1,
            /// <summary>
            /// [2] 用数据填充数组指定位置
            /// T value 
            /// int startIndex 起始位置
            /// int count 填充数据数量
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            Fill = 2,
            /// <summary>
            /// [3] 用数据填充整个数组
            /// T value 
            /// </summary>
            FillArray = 3,
            /// <summary>
            /// [4] 获取数组长度
            /// 返回值 int 
            /// </summary>
            GetLength = 4,
            /// <summary>
            /// [5] 根据索引位置获取数据
            /// int index 索引位置
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 超出索引返回则无返回值
            /// </summary>
            GetValue = 5,
            /// <summary>
            /// [6] 根据索引位置设置数据并返回设置之前的数据
            /// int index 索引位置
            /// T value 数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 设置之前的数据，超出索引返回则无返回值
            /// </summary>
            GetValueSet = 6,
            /// <summary>
            /// [7] 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// T value 
            /// int startIndex 起始位置
            /// int count 查找匹配数据数量
            /// 返回值 int 失败返回负数
            /// </summary>
            IndexOf = 7,
            /// <summary>
            /// [8] 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// T value 
            /// 返回值 int 失败返回负数
            /// </summary>
            IndexOfArray = 8,
            /// <summary>
            /// [9] 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// T value 
            /// int startIndex 最后一个匹配位置（起始位置）
            /// int count 查找匹配数据数量
            /// 返回值 int 失败返回负数
            /// </summary>
            LastIndexOf = 9,
            /// <summary>
            /// [10] 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// T value 
            /// 返回值 int 失败返回负数
            /// </summary>
            LastIndexOfArray = 10,
            /// <summary>
            /// [11] 反转指定位置数组数据
            /// int startIndex 起始位置
            /// int count 反转数据数量
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            Reverse = 11,
            /// <summary>
            /// [12] 反转整个数组数据
            /// </summary>
            ReverseArray = 12,
            /// <summary>
            /// [13] 根据索引位置设置数据
            /// int index 索引位置
            /// T value 数据
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            SetValue = 13,
            /// <summary>
            /// [14] 
            /// AutoCSer.KeyValue{int,T} value 
            /// </summary>
            SnapshotSet = 14,
            /// <summary>
            /// [15] 排序指定位置数组数据
            /// int startIndex 起始位置
            /// int count 排序数据数量
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            Sort = 15,
            /// <summary>
            /// [16] 数组排序
            /// </summary>
            SortArray = 16,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 位图节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IBitmapNodeMethodEnum))]
        public partial interface IBitmapNode { }
        /// <summary>
        /// 位图节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IBitmapNodeMethodEnum
        {
            /// <summary>
            /// [0] 清除位状态
            /// uint index 位索引
            /// 返回值 bool 是否设置成功，失败表示索引超出范围
            /// </summary>
            ClearBit = 0,
            /// <summary>
            /// [1] 清除所有数据
            /// </summary>
            ClearMap = 1,
            /// <summary>
            /// [2] 读取位状态
            /// uint index 位索引
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 非 0 表示二进制位为已设置状态，索引超出则无返回值
            /// </summary>
            GetBit = 2,
            /// <summary>
            /// [3] 清除位状态并返回设置之前的状态
            /// uint index 位索引
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 清除操作之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值
            /// </summary>
            GetBitClearBit = 3,
            /// <summary>
            /// [4] 状态取反并返回操作之前的状态
            /// uint index 位索引
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 取反操作之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值
            /// </summary>
            GetBitInvertBit = 4,
            /// <summary>
            /// [5] 设置位状态并返回设置之前的状态
            /// uint index 位索引
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 设置之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值
            /// </summary>
            GetBitSetBit = 5,
            /// <summary>
            /// [6] 获取二进制位数量
            /// 返回值 uint 
            /// </summary>
            GetCapacity = 6,
            /// <summary>
            /// [7] 状态取反
            /// uint index 位索引
            /// 返回值 bool 是否设置成功，失败表示索引超出范围
            /// </summary>
            InvertBit = 7,
            /// <summary>
            /// [8] 设置位状态
            /// uint index 位索引
            /// 返回值 bool 是否设置成功，失败表示索引超出范围
            /// </summary>
            SetBit = 8,
            /// <summary>
            /// [9] 快照添加数据
            /// byte[] map 
            /// </summary>
            SnapshotSet = 9,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 字典节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IByteArrayDictionaryNodeMethodEnum))]
        public partial interface IByteArrayDictionaryNode<KT> { }
        /// <summary>
        /// 字典节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IByteArrayDictionaryNodeMethodEnum
        {
            /// <summary>
            /// [0] 清除所有数据
            /// </summary>
            Clear = 0,
            /// <summary>
            /// [1] 判断关键字是否存在
            /// KT key 
            /// 返回值 bool 
            /// </summary>
            ContainsKey = 1,
            /// <summary>
            /// [2] 获取数据数量
            /// 返回值 int 
            /// </summary>
            Count = 2,
            /// <summary>
            /// [3] 删除关键字并返回被删除数据
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{byte[]} 被删除数据
            /// </summary>
            GetRemove = 3,
            /// <summary>
            /// [4] 删除关键字并返回被删除数据
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter 被删除数据
            /// </summary>
            GetRemoveResponseParameter = 4,
            /// <summary>
            /// [5] 删除关键字
            /// KT key 
            /// 返回值 bool 是否删除成功
            /// </summary>
            Remove = 5,
            /// <summary>
            /// [6] 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// int capacity 新容器初始化大小
            /// </summary>
            Renew = 6,
            /// <summary>
            /// [7] 强制设置数据，如果关键字已存在则覆盖
            /// KT key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value 
            /// 返回值 bool 是否设置成功
            /// </summary>
            Set = 7,
            /// <summary>
            /// [8] 
            /// AutoCSer.KeyValue{KT,byte[]} value 
            /// </summary>
            SnapshotAdd = 8,
            /// <summary>
            /// [9] 尝试添加数据
            /// KT key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value 
            /// 返回值 bool 是否添加成功，否则表示关键字已经存在
            /// </summary>
            TryAdd = 9,
            /// <summary>
            /// [10] 根据关键字获取数据
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter 
            /// </summary>
            TryGetResponseParameter = 10,
            /// <summary>
            /// [11] 根据关键字获取数据
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{byte[]} 
            /// </summary>
            TryGetValue = 11,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 256 基分片字典 节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IByteArrayFragmentDictionaryNodeMethodEnum))]
        public partial interface IByteArrayFragmentDictionaryNode<KT> { }
        /// <summary>
        /// 256 基分片字典 节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IByteArrayFragmentDictionaryNodeMethodEnum
        {
            /// <summary>
            /// [0] 清除数据（保留分片数组）
            /// </summary>
            Clear = 0,
            /// <summary>
            /// [1] 清除分片数组（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            ClearArray = 1,
            /// <summary>
            /// [2] 判断关键字是否存在
            /// KT key 
            /// 返回值 bool 
            /// </summary>
            ContainsKey = 2,
            /// <summary>
            /// [3] 获取数据数量
            /// 返回值 int 
            /// </summary>
            Count = 3,
            /// <summary>
            /// [4] 删除关键字并返回被删除数据
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{byte[]} 被删除数据
            /// </summary>
            GetRemove = 4,
            /// <summary>
            /// [5] 删除关键字并返回被删除数据
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter 被删除数据
            /// </summary>
            GetRemoveResponseParameter = 5,
            /// <summary>
            /// [6] 删除关键字
            /// KT key 
            /// 返回值 bool 是否存在关键字
            /// </summary>
            Remove = 6,
            /// <summary>
            /// [7] 强制设置数据，如果关键字已存在则覆盖
            /// KT key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value 
            /// 返回值 bool 是否设置成功
            /// </summary>
            Set = 7,
            /// <summary>
            /// [8] 
            /// AutoCSer.KeyValue{KT,byte[]} value 
            /// </summary>
            SnapshotAdd = 8,
            /// <summary>
            /// [9] 如果关键字不存在则添加数据
            /// KT key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value 
            /// 返回值 bool 是否添加成功，否则表示关键字已经存在
            /// </summary>
            TryAdd = 9,
            /// <summary>
            /// [10] 根据关键字获取数据
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter 
            /// </summary>
            TryGetResponseParameter = 10,
            /// <summary>
            /// [11] 根据关键字获取数据
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{byte[]} 
            /// </summary>
            TryGetValue = 11,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 队列节点接口（先进先出）
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IByteArrayQueueNodeMethodEnum))]
        public partial interface IByteArrayQueueNode { }
        /// <summary>
        /// 队列节点接口（先进先出） 节点方法序号映射枚举类型
        /// </summary>
        public enum IByteArrayQueueNodeMethodEnum
        {
            /// <summary>
            /// [0] 清除所有数据
            /// </summary>
            Clear = 0,
            /// <summary>
            /// [1] 获取队列数据数量
            /// 返回值 int 
            /// </summary>
            Count = 1,
            /// <summary>
            /// [2] 将数据添加到队列
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value 
            /// </summary>
            Enqueue = 2,
            /// <summary>
            /// [3] 快照添加数据
            /// byte[] value 
            /// </summary>
            SnapshotAdd = 3,
            /// <summary>
            /// [4] 从队列中弹出一个数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{byte[]} 没有可弹出数据则返回无数据
            /// </summary>
            TryDequeue = 4,
            /// <summary>
            /// [5] 从队列中弹出一个数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter 没有可弹出数据则返回无数据
            /// </summary>
            TryDequeueResponseParameter = 5,
            /// <summary>
            /// [6] 获取队列中下一个弹出数据（不弹出数据仅查看）
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{byte[]} 没有可弹出数据则返回无数据
            /// </summary>
            TryPeek = 6,
            /// <summary>
            /// [7] 获取队列中下一个弹出数据（不弹出数据仅查看）
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter 没有可弹出数据则返回无数据
            /// </summary>
            TryPeekResponseParameter = 7,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 栈节点（后进先出）
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IByteArrayStackNodeMethodEnum))]
        public partial interface IByteArrayStackNode { }
        /// <summary>
        /// 栈节点（后进先出） 节点方法序号映射枚举类型
        /// </summary>
        public enum IByteArrayStackNodeMethodEnum
        {
            /// <summary>
            /// [0] 清除所有数据
            /// </summary>
            Clear = 0,
            /// <summary>
            /// [1] 获取数据数量
            /// 返回值 int 
            /// </summary>
            Count = 1,
            /// <summary>
            /// [2] 将数据添加到栈
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value 
            /// </summary>
            Push = 2,
            /// <summary>
            /// [3] 快照添加数据
            /// byte[] value 
            /// </summary>
            SnapshotAdd = 3,
            /// <summary>
            /// [4] 获取栈中下一个弹出数据（不弹出数据仅查看）
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{byte[]} 没有可弹出数据则返回无数据
            /// </summary>
            TryPeek = 4,
            /// <summary>
            /// [5] 获取栈中下一个弹出数据（不弹出数据仅查看）
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter 没有可弹出数据则返回无数据
            /// </summary>
            TryPeekResponseParameter = 5,
            /// <summary>
            /// [6] 从栈中弹出一个数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{byte[]} 没有可弹出数据则返回无数据
            /// </summary>
            TryPop = 6,
            /// <summary>
            /// [7] 从栈中弹出一个数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter 没有可弹出数据则返回无数据
            /// </summary>
            TryPopResponseParameter = 7,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 字典节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IDictionaryNodeMethodEnum))]
        public partial interface IDictionaryNode<KT,VT> { }
        /// <summary>
        /// 字典节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IDictionaryNodeMethodEnum
        {
            /// <summary>
            /// [0] 清除所有数据
            /// </summary>
            Clear = 0,
            /// <summary>
            /// [1] 判断关键字是否存在
            /// KT key 
            /// 返回值 bool 
            /// </summary>
            ContainsKey = 1,
            /// <summary>
            /// [2] 判断数据是否存在，时间复杂度 O(n) 不建议调用（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// VT value 
            /// 返回值 bool 
            /// </summary>
            ContainsValue = 2,
            /// <summary>
            /// [3] 获取数据数量
            /// 返回值 int 
            /// </summary>
            Count = 3,
            /// <summary>
            /// [4] 删除关键字并返回被删除数据
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT} 被删除数据
            /// </summary>
            GetRemove = 4,
            /// <summary>
            /// [5] 删除关键字
            /// KT key 
            /// 返回值 bool 是否删除成功
            /// </summary>
            Remove = 5,
            /// <summary>
            /// [6] 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// int capacity 新容器初始化大小
            /// </summary>
            Renew = 6,
            /// <summary>
            /// [7] 强制设置数据，如果关键字已存在则覆盖
            /// KT key 
            /// VT value 
            /// 返回值 bool 是否设置成功
            /// </summary>
            Set = 7,
            /// <summary>
            /// [8] 
            /// AutoCSer.KeyValue{KT,VT} value 
            /// </summary>
            SnapshotAdd = 8,
            /// <summary>
            /// [9] 尝试添加数据
            /// KT key 
            /// VT value 
            /// 返回值 bool 是否添加成功，否则表示关键字已经存在
            /// </summary>
            TryAdd = 9,
            /// <summary>
            /// [10] 根据关键字获取数据
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT} 
            /// </summary>
            TryGetValue = 10,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 分布式锁节点
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IDistributedLockNodeMethodEnum))]
        public partial interface IDistributedLockNode<T> { }
        /// <summary>
        /// 分布式锁节点 节点方法序号映射枚举类型
        /// </summary>
        public enum IDistributedLockNodeMethodEnum
        {
            /// <summary>
            /// [0] 申请锁
            /// T key 锁关键字
            /// ushort timeoutSeconds 超时秒数
            /// 返回值 long 
            /// </summary>
            Enter = 0,
            /// <summary>
            /// [1] 释放锁
            /// T key 锁关键字
            /// long identity 锁操作标识
            /// </summary>
            Release = 1,
            /// <summary>
            /// [2] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.DistributedLockIdentity{T} value 
            /// </summary>
            SnapshotSet = 2,
            /// <summary>
            /// [3] 尝试申请锁
            /// T key 锁关键字
            /// ushort timeoutSeconds 超时秒数
            /// 返回值 long 失败返回 0
            /// </summary>
            TryEnter = 3,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 256 基分片字典 节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IFragmentDictionaryNodeMethodEnum))]
        public partial interface IFragmentDictionaryNode<KT,VT> { }
        /// <summary>
        /// 256 基分片字典 节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IFragmentDictionaryNodeMethodEnum
        {
            /// <summary>
            /// [0] 清除数据（保留分片数组）
            /// </summary>
            Clear = 0,
            /// <summary>
            /// [1] 清除分片数组（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            ClearArray = 1,
            /// <summary>
            /// [2] 判断关键字是否存在
            /// KT key 
            /// 返回值 bool 
            /// </summary>
            ContainsKey = 2,
            /// <summary>
            /// [3] 获取数据数量
            /// 返回值 int 
            /// </summary>
            Count = 3,
            /// <summary>
            /// [4] 删除关键字并返回被删除数据
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT} 
            /// </summary>
            GetRemove = 4,
            /// <summary>
            /// [5] 删除关键字
            /// KT key 
            /// 返回值 bool 是否存在关键字
            /// </summary>
            Remove = 5,
            /// <summary>
            /// [6] 强制设置数据，如果关键字已存在则覆盖
            /// KT key 
            /// VT value 
            /// 返回值 bool 是否设置成功
            /// </summary>
            Set = 6,
            /// <summary>
            /// [7] 
            /// AutoCSer.KeyValue{KT,VT} value 
            /// </summary>
            SnapshotAdd = 7,
            /// <summary>
            /// [8] 如果关键字不存在则添加数据
            /// KT key 
            /// VT value 
            /// 返回值 bool 是否添加成功，否则表示关键字已经存在
            /// </summary>
            TryAdd = 8,
            /// <summary>
            /// [9] 根据关键字获取数据
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT} 
            /// </summary>
            TryGetValue = 9,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 256 基分片 哈希表 节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IFragmentHashSetNodeMethodEnum))]
        public partial interface IFragmentHashSetNode<T> { }
        /// <summary>
        /// 256 基分片 哈希表 节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IFragmentHashSetNodeMethodEnum
        {
            /// <summary>
            /// [0] 如果关键字不存在则添加数据
            /// T value 
            /// 返回值 bool 是否添加成功，否则表示关键字已经存在
            /// </summary>
            Add = 0,
            /// <summary>
            /// [1] 清除数据（保留分片数组）
            /// </summary>
            Clear = 1,
            /// <summary>
            /// [2] 清除分片数组（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            ClearArray = 2,
            /// <summary>
            /// [3] 判断关键字是否存在
            /// T value 
            /// 返回值 bool 
            /// </summary>
            Contains = 3,
            /// <summary>
            /// [4] 获取数据数量
            /// 返回值 int 
            /// </summary>
            Count = 4,
            /// <summary>
            /// [5] 删除关键字
            /// T value 
            /// 返回值 bool 是否存在关键字
            /// </summary>
            Remove = 5,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 字典节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IHashBytesDictionaryNodeMethodEnum))]
        public partial interface IHashBytesDictionaryNode { }
        /// <summary>
        /// 字典节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IHashBytesDictionaryNodeMethodEnum
        {
            /// <summary>
            /// [0] 清除所有数据
            /// </summary>
            Clear = 0,
            /// <summary>
            /// [1] 判断关键字是否存在
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key 
            /// 返回值 bool 
            /// </summary>
            ContainsKey = 1,
            /// <summary>
            /// [2] 获取数据数量
            /// 返回值 int 
            /// </summary>
            Count = 2,
            /// <summary>
            /// [3] 删除关键字并返回被删除数据
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{byte[]} 被删除数据
            /// </summary>
            GetRemove = 3,
            /// <summary>
            /// [4] 删除关键字并返回被删除数据
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter 被删除数据
            /// </summary>
            GetRemoveResponseParameter = 4,
            /// <summary>
            /// [5] 删除关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key 
            /// 返回值 bool 是否删除成功
            /// </summary>
            Remove = 5,
            /// <summary>
            /// [6] 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// int capacity 新容器初始化大小
            /// </summary>
            Renew = 6,
            /// <summary>
            /// [7] 强制设置数据，如果关键字已存在则覆盖
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value 
            /// 返回值 bool 是否设置成功
            /// </summary>
            Set = 7,
            /// <summary>
            /// [8] 快照添加数据
            /// AutoCSer.KeyValue{byte[],byte[]} value 
            /// </summary>
            SnapshotAdd = 8,
            /// <summary>
            /// [9] 尝试添加数据
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value 
            /// 返回值 bool 是否添加成功，否则表示关键字已经存在
            /// </summary>
            TryAdd = 9,
            /// <summary>
            /// [10] 根据关键字获取数据
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter 
            /// </summary>
            TryGetResponseParameter = 10,
            /// <summary>
            /// [11] 根据关键字获取数据
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{byte[]} 
            /// </summary>
            TryGetValue = 11,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 256 基分片 HashBytes 字典 节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IHashBytesFragmentDictionaryNodeMethodEnum))]
        public partial interface IHashBytesFragmentDictionaryNode { }
        /// <summary>
        /// 256 基分片 HashBytes 字典 节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IHashBytesFragmentDictionaryNodeMethodEnum
        {
            /// <summary>
            /// [0] 清除数据（保留分片数组）
            /// </summary>
            Clear = 0,
            /// <summary>
            /// [1] 清除分片数组（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            ClearArray = 1,
            /// <summary>
            /// [2] 判断关键字是否存在
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key 
            /// 返回值 bool 
            /// </summary>
            ContainsKey = 2,
            /// <summary>
            /// [3] 获取数据数量
            /// 返回值 int 
            /// </summary>
            Count = 3,
            /// <summary>
            /// [4] 删除关键字并返回被删除数据
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{byte[]} 被删除数据
            /// </summary>
            GetRemove = 4,
            /// <summary>
            /// [5] 删除关键字并返回被删除数据
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter 被删除数据
            /// </summary>
            GetRemoveResponseParameter = 5,
            /// <summary>
            /// [6] 删除关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key 
            /// 返回值 bool 是否存在关键字
            /// </summary>
            Remove = 6,
            /// <summary>
            /// [7] 强制设置数据，如果关键字已存在则覆盖
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value 
            /// 返回值 bool 是否设置成功
            /// </summary>
            Set = 7,
            /// <summary>
            /// [8] 快照添加数据
            /// AutoCSer.KeyValue{byte[],byte[]} value 
            /// </summary>
            SnapshotAdd = 8,
            /// <summary>
            /// [9] 如果关键字不存在则添加数据
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value 
            /// 返回值 bool 是否添加成功，否则表示关键字已经存在
            /// </summary>
            TryAdd = 9,
            /// <summary>
            /// [10] 根据关键字获取数据
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter 
            /// </summary>
            TryGetResponseParameter = 10,
            /// <summary>
            /// [11] 根据关键字获取数据
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{byte[]} 
            /// </summary>
            TryGetValue = 11,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 哈希表节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IHashSetNodeMethodEnum))]
        public partial interface IHashSetNode<T> { }
        /// <summary>
        /// 哈希表节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IHashSetNodeMethodEnum
        {
            /// <summary>
            /// [0] 添加数据
            /// T value 
            /// 返回值 bool 是否添加成功，否则表示关键字已经存在
            /// </summary>
            Add = 0,
            /// <summary>
            /// [1] 清除所有数据
            /// </summary>
            Clear = 1,
            /// <summary>
            /// [2] 判断关键字是否存在
            /// T value 
            /// 返回值 bool 
            /// </summary>
            Contains = 2,
            /// <summary>
            /// [3] 获取数据数量
            /// 返回值 int 
            /// </summary>
            Count = 3,
            /// <summary>
            /// [4] 删除关键字
            /// T value 
            /// 返回值 bool 是否删除成功
            /// </summary>
            Remove = 4,
            /// <summary>
            /// [5] 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            Renew = 5,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 64 位自增ID 节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IIdentityGeneratorNodeMethodEnum))]
        public partial interface IIdentityGeneratorNode { }
        /// <summary>
        /// 64 位自增ID 节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IIdentityGeneratorNodeMethodEnum
        {
            /// <summary>
            /// [0] 获取下一个自增ID
            /// 返回值 long 下一个自增ID，失败返回负数
            /// </summary>
            Next = 0,
            /// <summary>
            /// [1] 获取自增 ID 分段
            /// int count 获取数量
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityFragment 自增 ID 分段
            /// </summary>
            NextFragment = 1,
            /// <summary>
            /// [2] 快照添加数据
            /// long identity 
            /// </summary>
            SnapshotSet = 2,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 数组节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(ILeftArrayNodeMethodEnum))]
        public partial interface ILeftArrayNode<T> { }
        /// <summary>
        /// 数组节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum ILeftArrayNodeMethodEnum
        {
            /// <summary>
            /// [0] 添加数据
            /// T value 数据
            /// </summary>
            Add = 0,
            /// <summary>
            /// [1] 清除指定位置数据
            /// int startIndex 起始位置
            /// int count 清除数据数量
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            Clear = 1,
            /// <summary>
            /// [2] 清除所有数据并将数据有效长度设置为 0
            /// </summary>
            ClearLength = 2,
            /// <summary>
            /// [3] 用数据填充数组指定位置
            /// T value 
            /// int startIndex 起始位置
            /// int count 填充数据数量
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            Fill = 3,
            /// <summary>
            /// [4] 用数据填充整个数组
            /// T value 
            /// </summary>
            FillArray = 4,
            /// <summary>
            /// [5] 获取数组容器初大小
            /// 返回值 int 
            /// </summary>
            GetCapacity = 5,
            /// <summary>
            /// [6] 获取容器空闲数量
            /// 返回值 int 
            /// </summary>
            GetFreeCount = 6,
            /// <summary>
            /// [7] 获取有效数组长度
            /// 返回值 int 
            /// </summary>
            GetLength = 7,
            /// <summary>
            /// [8] 移除最后一个数据并返回该数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 没有可移除数据则无数据返回
            /// </summary>
            GetTryPopValue = 8,
            /// <summary>
            /// [9] 根据索引位置获取数据
            /// int index 索引位置
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 超出索引返回则无返回值
            /// </summary>
            GetValue = 9,
            /// <summary>
            /// [10] 移除指定索引位置数据并返回被移除的数据
            /// int index 数据位置
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 超出索引范围则无数据返回
            /// </summary>
            GetValueRemoveAt = 10,
            /// <summary>
            /// [11] 移除指定索引位置数据，将最后一个数据移动到该指定位置，并返回被移除的数据
            /// int index 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 超出索引范围则无数据返回
            /// </summary>
            GetValueRemoveToEnd = 11,
            /// <summary>
            /// [12] 根据索引位置设置数据并返回设置之前的数据
            /// int index 索引位置
            /// T value 数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 设置之前的数据，超出索引返回则无返回值
            /// </summary>
            GetValueSet = 12,
            /// <summary>
            /// [13] 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// T value 
            /// int startIndex 起始位置
            /// int count 查找匹配数据数量
            /// 返回值 int 失败返回负数
            /// </summary>
            IndexOf = 13,
            /// <summary>
            /// [14] 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// T value 
            /// 返回值 int 失败返回负数
            /// </summary>
            IndexOfArray = 14,
            /// <summary>
            /// [15] 插入数据
            /// int index 插入位置
            /// T value 数据
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            Insert = 15,
            /// <summary>
            /// [16] 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// T value 
            /// int startIndex 最后一个匹配位置（起始位置）
            /// int count 查找匹配数据数量
            /// 返回值 int 失败返回负数
            /// </summary>
            LastIndexOf = 16,
            /// <summary>
            /// [17] 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// T value 
            /// 返回值 int 失败返回负数
            /// </summary>
            LastIndexOfArray = 17,
            /// <summary>
            /// [18] 移除第一个匹配数据（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// T value 数据
            /// 返回值 bool 是否存在移除数据
            /// </summary>
            Remove = 18,
            /// <summary>
            /// [19] 移除指定索引位置数据
            /// int index 数据位置
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            RemoveAt = 19,
            /// <summary>
            /// [20] 移除指定索引位置数据并将最后一个数据移动到该指定位置
            /// int index 
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            RemoveToEnd = 20,
            /// <summary>
            /// [21] 反转指定位置数组数据
            /// int startIndex 起始位置
            /// int count 反转数据数量
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            Reverse = 21,
            /// <summary>
            /// [22] 反转整个数组数据
            /// </summary>
            ReverseArray = 22,
            /// <summary>
            /// [23] 置空并释放数组
            /// </summary>
            SetEmpty = 23,
            /// <summary>
            /// [24] 根据索引位置设置数据
            /// int index 索引位置
            /// T value 数据
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            SetValue = 24,
            /// <summary>
            /// [25] 排序指定位置数组数据
            /// int startIndex 起始位置
            /// int count 排序数据数量
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            Sort = 25,
            /// <summary>
            /// [26] 数组排序
            /// </summary>
            SortArray = 26,
            /// <summary>
            /// [27] 当有空闲位置时添加数据
            /// T value 
            /// 返回值 bool 如果数组已满则添加失败并返回 false
            /// </summary>
            TryAdd = 27,
            /// <summary>
            /// [28] 移除最后一个数据
            /// 返回值 bool 是否存在可移除数据
            /// </summary>
            TryPop = 28,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 消息处理节点
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IMessageNodeMethodEnum))]
        public partial interface IMessageNode<T> { }
        /// <summary>
        /// 消息处理节点 节点方法序号映射枚举类型
        /// </summary>
        public enum IMessageNodeMethodEnum
        {
            /// <summary>
            /// [0] 生产者添加新消息
            /// T message 
            /// </summary>
            AppendMessage = 0,
            /// <summary>
            /// [1] 清除所有消息
            /// </summary>
            Clear = 1,
            /// <summary>
            /// [2] 清除所有失败消息
            /// </summary>
            ClearFailed = 2,
            /// <summary>
            /// [3] 消息完成处理
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity identity 
            /// </summary>
            Completed = 3,
            /// <summary>
            /// [4] 消息失败处理
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity identity 
            /// </summary>
            Failed = 4,
            /// <summary>
            /// [5] 获取消费者回调数量
            /// 返回值 int 
            /// </summary>
            GetCallbackCount = 5,
            /// <summary>
            /// [6] 获取未处理完成消息数量（不包括失败消息）
            /// 返回值 int 
            /// </summary>
            GetCount = 6,
            /// <summary>
            /// [7] 获取失败消息数量
            /// 返回值 int 
            /// </summary>
            GetFailedCount = 7,
            /// <summary>
            /// [8] 
            /// int maxCount 
            /// 返回值 T 
            /// </summary>
            GetMessage = 8,
            /// <summary>
            /// [9] 获取未完成处理超时消息数量
            /// 返回值 int 
            /// </summary>
            GetTimeoutCount = 9,
            /// <summary>
            /// [10] 获取未处理完成消息数量（包括失败消息）
            /// 返回值 int 
            /// </summary>
            GetTotalCount = 10,
            /// <summary>
            /// [11] 快照设置数据
            /// T value 数据
            /// </summary>
            SnapshotAdd = 11,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 队列节点接口（先进先出）
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IQueueNodeMethodEnum))]
        public partial interface IQueueNode<T> { }
        /// <summary>
        /// 队列节点接口（先进先出） 节点方法序号映射枚举类型
        /// </summary>
        public enum IQueueNodeMethodEnum
        {
            /// <summary>
            /// [0] 清除所有数据
            /// </summary>
            Clear = 0,
            /// <summary>
            /// [1] 判断队列中是否存在匹配数据（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// T value 匹配数据
            /// 返回值 bool 
            /// </summary>
            Contains = 1,
            /// <summary>
            /// [2] 获取队列数据数量
            /// 返回值 int 
            /// </summary>
            Count = 2,
            /// <summary>
            /// [3] 将数据添加到队列
            /// T value 
            /// </summary>
            Enqueue = 3,
            /// <summary>
            /// [4] 从队列中弹出一个数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 没有可弹出数据则返回无数据
            /// </summary>
            TryDequeue = 4,
            /// <summary>
            /// [5] 获取队列中下一个弹出数据（不弹出数据仅查看）
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 没有可弹出数据则返回无数据
            /// </summary>
            TryPeek = 5,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 二叉搜索树节点
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(ISearchTreeDictionaryNodeMethodEnum))]
        public partial interface ISearchTreeDictionaryNode<KT,VT> { }
        /// <summary>
        /// 二叉搜索树节点 节点方法序号映射枚举类型
        /// </summary>
        public enum ISearchTreeDictionaryNodeMethodEnum
        {
            /// <summary>
            /// [0] 清除数据
            /// </summary>
            Clear = 0,
            /// <summary>
            /// [1] 判断是否包含关键字
            /// KT key 关键字
            /// 返回值 bool 是否包含关键字
            /// </summary>
            ContainsKey = 1,
            /// <summary>
            /// [2] 获取节点数据数量
            /// 返回值 int 
            /// </summary>
            Count = 2,
            /// <summary>
            /// [3] 根据关键字比它小的节点数量
            /// KT key 关键字
            /// 返回值 int 节点数量，失败返回 -1
            /// </summary>
            CountLess = 3,
            /// <summary>
            /// [4] 根据关键字比它大的节点数量
            /// KT key 关键字
            /// 返回值 int 节点数量，失败返回 -1
            /// </summary>
            CountThan = 4,
            /// <summary>
            /// [5] 获取树高度，时间复杂度 O(n)
            /// 返回值 int 
            /// </summary>
            GetHeight = 5,
            /// <summary>
            /// [6] 根据关键字删除节点
            /// KT key 关键字
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT} 被删除数据
            /// </summary>
            GetRemove = 6,
            /// <summary>
            /// [7] 获取范围数据集合
            /// int skipCount 跳过记录数
            /// byte getCount 获取记录数
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT} 
            /// </summary>
            GetValues = 7,
            /// <summary>
            /// [8] 根据关键字获取一个匹配节点位置
            /// KT key 关键字
            /// 返回值 int 一个匹配节点位置,失败返回-1
            /// </summary>
            IndexOf = 8,
            /// <summary>
            /// [9] 根据关键字删除节点
            /// KT key 关键字
            /// 返回值 bool 是否存在关键字
            /// </summary>
            Remove = 9,
            /// <summary>
            /// [10] 设置数据
            /// KT key 关键字
            /// VT value 数据
            /// 返回值 bool 是否添加了关键字
            /// </summary>
            Set = 10,
            /// <summary>
            /// [11] 
            /// AutoCSer.KeyValue{KT,VT} value 
            /// </summary>
            SnapshotAdd = 11,
            /// <summary>
            /// [12] 添加数据
            /// KT key 关键字
            /// VT value 数据
            /// 返回值 bool 是否添加了数据
            /// </summary>
            TryAdd = 12,
            /// <summary>
            /// [13] 获取第一个关键字数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{KT} 第一个关键字数据
            /// </summary>
            TryGetFirstKey = 13,
            /// <summary>
            /// [14] 获取第一组数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{AutoCSer.KeyValue{KT,VT}} 第一组数据
            /// </summary>
            TryGetFirstKeyValue = 14,
            /// <summary>
            /// [15] 获取第一个数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT} 第一个数据
            /// </summary>
            TryGetFirstValue = 15,
            /// <summary>
            /// [16] 根据节点位置获取数据
            /// int index 节点位置
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{AutoCSer.KeyValue{KT,VT}} 数据
            /// </summary>
            TryGetKeyValueByIndex = 16,
            /// <summary>
            /// [17] 获取最后一个关键字数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{KT} 最后一个关键字数据
            /// </summary>
            TryGetLastKey = 17,
            /// <summary>
            /// [18] 获取最后一组数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{AutoCSer.KeyValue{KT,VT}} 最后一组数据
            /// </summary>
            TryGetLastKeyValue = 18,
            /// <summary>
            /// [19] 获取最后一个数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT} 最后一个数据
            /// </summary>
            TryGetLastValue = 19,
            /// <summary>
            /// [20] 根据关键字获取数据
            /// KT key 关键字
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT} 目标数据
            /// </summary>
            TryGetValue = 20,
            /// <summary>
            /// [21] 根据节点位置获取数据
            /// int index 节点位置
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT} 数据
            /// </summary>
            TryGetValueByIndex = 21,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 二叉搜索树集合节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(ISearchTreeSetNodeMethodEnum))]
        public partial interface ISearchTreeSetNode<T> { }
        /// <summary>
        /// 二叉搜索树集合节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum ISearchTreeSetNodeMethodEnum
        {
            /// <summary>
            /// [0] 添加数据
            /// T value 关键字
            /// 返回值 bool 是否添加成功，否则表示关键字已经存在
            /// </summary>
            Add = 0,
            /// <summary>
            /// [1] 清除所有数据
            /// </summary>
            Clear = 1,
            /// <summary>
            /// [2] 判断关键字是否存在
            /// T value 关键字
            /// 返回值 bool 
            /// </summary>
            Contains = 2,
            /// <summary>
            /// [3] 获取数据数量
            /// 返回值 int 
            /// </summary>
            Count = 3,
            /// <summary>
            /// [4] 根据关键字比它小的节点数量
            /// T value 关键字
            /// 返回值 int 节点数量，失败返回 -1
            /// </summary>
            CountLess = 4,
            /// <summary>
            /// [5] 根据关键字比它大的节点数量
            /// T value 关键字
            /// 返回值 int 节点数量，失败返回 -1
            /// </summary>
            CountThan = 5,
            /// <summary>
            /// [6] 根据节点位置获取数据
            /// int index 节点位置
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 数据
            /// </summary>
            GetByIndex = 6,
            /// <summary>
            /// [7] 获取第一个数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 没有数据时返回无返回值
            /// </summary>
            GetFrist = 7,
            /// <summary>
            /// [8] 获取最后一个数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 没有数据时返回无返回值
            /// </summary>
            GetLast = 8,
            /// <summary>
            /// [9] 根据关键字获取一个匹配节点位置
            /// T value 关键字
            /// 返回值 int 一个匹配节点位置,失败返回-1
            /// </summary>
            IndexOf = 9,
            /// <summary>
            /// [10] 删除关键字
            /// T value 关键字
            /// 返回值 bool 是否删除成功
            /// </summary>
            Remove = 10,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 服务基础操作接口方法映射枚举
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IServiceNodeMethodEnum))]
        public partial interface IServiceNode { }
        /// <summary>
        /// 服务基础操作接口方法映射枚举 节点方法序号映射枚举类型
        /// </summary>
        public enum IServiceNodeMethodEnum
        {
            /// <summary>
            /// [0] 创建数组节点 ArrayNode{T}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// int length 数组长度
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateArrayNode = 0,
            /// <summary>
            /// [1] 创建位图节点 BitmapNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// uint capacity 二进制位数量
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateBitmapNode = 1,
            /// <summary>
            /// [2] 创建字典节点 ByteArrayDictionaryNode{KT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateByteArrayDictionaryNode = 2,
            /// <summary>
            /// [3] 创建字典节点 ByteArrayFragmentDictionaryNode{KT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 节点信息
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateByteArrayFragmentDictionaryNode = 3,
            /// <summary>
            /// [4] 创建队列节点（先进先出） ByteArrayQueueNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateByteArrayQueueNode = 4,
            /// <summary>
            /// [5] 创建栈节点（后进先出） ByteArrayStackNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateByteArrayStackNode = 5,
            /// <summary>
            /// [6] 创建字典节点 DictionaryNode{KT,VT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// AutoCSer.Reflection.RemoteType valueType 数据类型
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateDictionaryNode = 6,
            /// <summary>
            /// [7] 创建分布式锁节点 DistributedLockNode{KT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateDistributedLockNode = 7,
            /// <summary>
            /// [8] 创建字典节点 FragmentDictionaryNode{KT,VT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// AutoCSer.Reflection.RemoteType valueType 数据类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateFragmentDictionaryNode = 8,
            /// <summary>
            /// [9] 创建 256 基分片哈希表节点 FragmentHashSetNode{KT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateFragmentHashSetNode = 9,
            /// <summary>
            /// [10] 创建字典节点 HashBytesDictionaryNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateHashBytesDictionaryNode = 10,
            /// <summary>
            /// [11] 创建字典节点 HashBytesFragmentDictionaryNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateHashBytesFragmentDictionaryNode = 11,
            /// <summary>
            /// [12] 创建哈希表节点 HashSetNode{KT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateHashSetNode = 12,
            /// <summary>
            /// [13] 创建 64 位自增ID 节点 IdentityGeneratorNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// long identity 起始分配 ID
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateIdentityGeneratorNode = 13,
            /// <summary>
            /// [14] 创建数组节点 LeftArrayNode{T}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateLeftArrayNode = 14,
            /// <summary>
            /// [15] 创建消息处理节点 MessageNode{T}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType messageType 消息数据类型
            /// int arraySize 正在处理消息数组大小
            /// int timeoutSeconds 消息处理超时秒数
            /// int checkTimeoutSeconds 消息超时检查间隔秒数
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateMessageNode = 15,
            /// <summary>
            /// [16] 创建队列节点（先进先出） QueueNode{T}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateQueueNode = 16,
            /// <summary>
            /// [17] 创建二叉搜索树节点 SearchTreeDictionaryNode{KT,VT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// AutoCSer.Reflection.RemoteType valueType 数据类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateSearchTreeDictionaryNode = 17,
            /// <summary>
            /// [18] 创建二叉搜索树集合节点 SearchTreeSetNode{KT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateSearchTreeSetNode = 18,
            /// <summary>
            /// [19] 创建消息处理节点 MessageNode{ServerByteArrayMessage}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// int arraySize 正在处理消息数组大小
            /// int timeoutSeconds 消息处理超时秒数
            /// int checkTimeoutSeconds 消息超时检查间隔秒数
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateServerByteArrayMessageNode = 19,
            /// <summary>
            /// [20] 创建排序字典节点 SortedDictionaryNode{KT,VT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// AutoCSer.Reflection.RemoteType valueType 数据类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateSortedDictionaryNode = 20,
            /// <summary>
            /// [21] 创建排序列表节点 SortedListNode{KT,VT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// AutoCSer.Reflection.RemoteType valueType 数据类型
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateSortedListNode = 21,
            /// <summary>
            /// [22] 创建排序集合节点 SortedSetNode{KT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateSortedSetNode = 22,
            /// <summary>
            /// [23] 创建栈节点（后进先出） StackNode{T}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateStackNode = 23,
            /// <summary>
            /// [24] 删除节点
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// 返回值 bool 是否成功删除节点，否则表示没有找到节点
            /// </summary>
            RemoveNode = 24,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 排序字典节点
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(ISortedDictionaryNodeMethodEnum))]
        public partial interface ISortedDictionaryNode<KT,VT> { }
        /// <summary>
        /// 排序字典节点 节点方法序号映射枚举类型
        /// </summary>
        public enum ISortedDictionaryNodeMethodEnum
        {
            /// <summary>
            /// [0] 清除所有数据
            /// </summary>
            Clear = 0,
            /// <summary>
            /// [1] 判断关键字是否存在
            /// KT key 
            /// 返回值 bool 
            /// </summary>
            ContainsKey = 1,
            /// <summary>
            /// [2] 判断数据是否存在，时间复杂度 O(n) 不建议调用（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// VT value 
            /// 返回值 bool 
            /// </summary>
            ContainsValue = 2,
            /// <summary>
            /// [3] 获取数据数量
            /// 返回值 int 
            /// </summary>
            Count = 3,
            /// <summary>
            /// [4] 删除关键字并返回被删除数据
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT} 
            /// </summary>
            GetRemove = 4,
            /// <summary>
            /// [5] 删除关键字
            /// KT key 
            /// 返回值 bool 是否删除成功
            /// </summary>
            Remove = 5,
            /// <summary>
            /// [6] 
            /// AutoCSer.KeyValue{KT,VT} value 
            /// </summary>
            SnapshotAdd = 6,
            /// <summary>
            /// [7] 添加数据
            /// KT key 
            /// VT value 
            /// 返回值 bool 是否添加成功，否则表示关键字已经存在
            /// </summary>
            TryAdd = 7,
            /// <summary>
            /// [8] 根据关键字获取数据
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT} 
            /// </summary>
            TryGetValue = 8,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 排序列表节点
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(ISortedListNodeMethodEnum))]
        public partial interface ISortedListNode<KT,VT> { }
        /// <summary>
        /// 排序列表节点 节点方法序号映射枚举类型
        /// </summary>
        public enum ISortedListNodeMethodEnum
        {
            /// <summary>
            /// [0] 清除所有数据
            /// </summary>
            Clear = 0,
            /// <summary>
            /// [1] 判断关键字是否存在
            /// KT key 
            /// 返回值 bool 
            /// </summary>
            ContainsKey = 1,
            /// <summary>
            /// [2] 判断数据是否存在，时间复杂度 O(n) 不建议调用（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// VT value 
            /// 返回值 bool 
            /// </summary>
            ContainsValue = 2,
            /// <summary>
            /// [3] 获取数据数量
            /// 返回值 int 
            /// </summary>
            Count = 3,
            /// <summary>
            /// [4] 获取容器大小
            /// 返回值 int 
            /// </summary>
            GetCapacity = 4,
            /// <summary>
            /// [5] 删除关键字并返回被删除数据
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT} 
            /// </summary>
            GetRemove = 5,
            /// <summary>
            /// [6] 获取关键字排序位置
            /// KT key 
            /// 返回值 int 负数表示没有找到关键字
            /// </summary>
            IndexOfKey = 6,
            /// <summary>
            /// [7] 获取第一个匹配数据排序位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// VT value 
            /// 返回值 int 负数表示没有找到匹配数据
            /// </summary>
            IndexOfValue = 7,
            /// <summary>
            /// [8] 删除关键字
            /// KT key 
            /// 返回值 bool 是否删除成功
            /// </summary>
            Remove = 8,
            /// <summary>
            /// [9] 删除指定排序索引位置数据
            /// int index 
            /// 返回值 bool 索引超出范围返回 false
            /// </summary>
            RemoveAt = 9,
            /// <summary>
            /// [10] 
            /// AutoCSer.KeyValue{KT,VT} value 
            /// </summary>
            SnapshotAdd = 10,
            /// <summary>
            /// [11] 添加数据
            /// KT key 
            /// VT value 
            /// 返回值 bool 是否添加成功，否则表示关键字已经存在
            /// </summary>
            TryAdd = 11,
            /// <summary>
            /// [12] 根据关键字获取数据
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT} 
            /// </summary>
            TryGetValue = 12,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 排序集合节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(ISortedSetNodeMethodEnum))]
        public partial interface ISortedSetNode<T> { }
        /// <summary>
        /// 排序集合节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum ISortedSetNodeMethodEnum
        {
            /// <summary>
            /// [0] 添加数据
            /// T value 
            /// 返回值 bool 是否添加成功，否则表示关键字已经存在
            /// </summary>
            Add = 0,
            /// <summary>
            /// [1] 清除所有数据
            /// </summary>
            Clear = 1,
            /// <summary>
            /// [2] 判断关键字是否存在
            /// T value 
            /// 返回值 bool 
            /// </summary>
            Contains = 2,
            /// <summary>
            /// [3] 获取数据数量
            /// 返回值 int 
            /// </summary>
            Count = 3,
            /// <summary>
            /// [4] 获取最大值
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 没有数据时返回无返回值
            /// </summary>
            GetMax = 4,
            /// <summary>
            /// [5] 获取最小值
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 没有数据时返回无返回值
            /// </summary>
            GetMin = 5,
            /// <summary>
            /// [6] 删除关键字
            /// T value 
            /// 返回值 bool 是否删除成功
            /// </summary>
            Remove = 6,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 栈节点（后进先出）
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IStackNodeMethodEnum))]
        public partial interface IStackNode<T> { }
        /// <summary>
        /// 栈节点（后进先出） 节点方法序号映射枚举类型
        /// </summary>
        public enum IStackNodeMethodEnum
        {
            /// <summary>
            /// [0] 清除所有数据
            /// </summary>
            Clear = 0,
            /// <summary>
            /// [1] 判断是否存在匹配数据（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// T value 匹配数据
            /// 返回值 bool 
            /// </summary>
            Contains = 1,
            /// <summary>
            /// [2] 获取数据数量
            /// 返回值 int 
            /// </summary>
            Count = 2,
            /// <summary>
            /// [3] 将数据添加到栈
            /// T value 
            /// </summary>
            Push = 3,
            /// <summary>
            /// [4] 获取栈中下一个弹出数据（不弹出数据仅查看）
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 没有可弹出数据则返回无数据
            /// </summary>
            TryPeek = 4,
            /// <summary>
            /// [5] 从栈中弹出一个数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 没有可弹出数据则返回无数据
            /// </summary>
            TryPop = 5,
        }
}
#endif