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
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IArrayNode<>))]
        public partial interface IArrayNodeClientNode<T>
        {
            /// <summary>
            /// 清除指定位置数据
            /// </summary>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">清除数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Clear(int startIndex, int count);
            /// <summary>
            /// 清除所有数据
            /// </summary>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> ClearArray();
            /// <summary>
            /// 用数据填充数组指定位置
            /// </summary>
            /// <param name="value"></param>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">填充数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Fill(T value, int startIndex, int count);
            /// <summary>
            /// 用数据填充整个数组
            /// </summary>
            /// <param name="value"></param>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> FillArray(T value);
            /// <summary>
            /// 获取数组长度
            /// </summary>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> GetLength();
            /// <summary>
            /// 根据索引位置获取数据
            /// </summary>
            /// <param name="index">索引位置</param>
            /// <returns>超出索引返回则无返回值</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetValue(int index);
            /// <summary>
            /// 根据索引位置设置数据并返回设置之前的数据
            /// </summary>
            /// <param name="index">索引位置</param>
            /// <param name="value">数据</param>
            /// <returns>设置之前的数据，超出索引返回则无返回值</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetValueSet(int index, T value);
            /// <summary>
            /// 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">查找匹配数据数量</param>
            /// <returns>失败返回负数</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> IndexOf(T value, int startIndex, int count);
            /// <summary>
            /// 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns>失败返回负数</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> IndexOfArray(T value);
            /// <summary>
            /// 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <param name="startIndex">最后一个匹配位置（起始位置）</param>
            /// <param name="count">查找匹配数据数量</param>
            /// <returns>失败返回负数</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> LastIndexOf(T value, int startIndex, int count);
            /// <summary>
            /// 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns>失败返回负数</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> LastIndexOfArray(T value);
            /// <summary>
            /// 反转指定位置数组数据
            /// </summary>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">反转数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Reverse(int startIndex, int count);
            /// <summary>
            /// 反转整个数组数据
            /// </summary>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> ReverseArray();
            /// <summary>
            /// 根据索引位置设置数据
            /// </summary>
            /// <param name="index">索引位置</param>
            /// <param name="value">数据</param>
            /// <returns>超出索引范围则返回 false</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> SetValue(int index, T value);
            /// <summary>
            /// 排序指定位置数组数据
            /// </summary>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">排序数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Sort(int startIndex, int count);
            /// <summary>
            /// 数组排序
            /// </summary>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> SortArray();
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 位图节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IBitmapNode))]
        public partial interface IBitmapNodeClientNode
        {
            /// <summary>
            /// 清除位状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>是否设置成功，失败表示索引超出范围</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> ClearBit(uint index);
            /// <summary>
            /// 清除所有数据
            /// </summary>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> ClearMap();
            /// <summary>
            /// 读取位状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>非 0 表示二进制位为已设置状态，索引超出则无返回值</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<int>>> GetBit(uint index);
            /// <summary>
            /// 清除位状态并返回设置之前的状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>清除操作之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<int>>> GetBitClearBit(uint index);
            /// <summary>
            /// 状态取反并返回操作之前的状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>取反操作之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<int>>> GetBitInvertBit(uint index);
            /// <summary>
            /// 设置位状态并返回设置之前的状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>设置之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<int>>> GetBitSetBit(uint index);
            /// <summary>
            /// 获取二进制位数量
            /// </summary>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<uint>> GetCapacity();
            /// <summary>
            /// 状态取反
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>是否设置成功，失败表示索引超出范围</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> InvertBit(uint index);
            /// <summary>
            /// 设置位状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>是否设置成功，失败表示索引超出范围</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> SetBit(uint index);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 字典节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IDictionaryNode<,>))]
        public partial interface IDictionaryNodeClientNode<KT,VT>
        {
            /// <summary>
            /// 清除所有数据
            /// </summary>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Clear();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> ContainsKey(KT key);
            /// <summary>
            /// 判断数据是否存在，时间复杂度 O(n) 不建议调用（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> ContainsValue(VT value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> Count();
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns>被删除数据</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> GetRemove(KT key);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="key"></param>
            /// <returns>是否删除成功</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Remove(KT key);
            /// <summary>
            /// 强制设置数据，如果关键字已存在则覆盖
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否设置成功</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Set(KT key, VT value);
            /// <summary>
            /// 尝试添加数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> TryAdd(KT key, VT value);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> TryGetValue(KT key);
            /// <summary>
            /// 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            /// <param name="capacity">新容器初始化大小</param>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Renew(int capacity);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 256 基分片字典 节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IFragmentDictionaryNode<,>))]
        public partial interface IFragmentDictionaryNodeClientNode<KT,VT>
        {
            /// <summary>
            /// 清除数据（保留分片数组）
            /// </summary>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Clear();
            /// <summary>
            /// 清除分片数组（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> ClearArray();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> ContainsKey(KT key);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> Count();
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> GetRemove(KT key);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="key"></param>
            /// <returns>是否存在关键字</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Remove(KT key);
            /// <summary>
            /// 强制设置数据，如果关键字已存在则覆盖
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否设置成功</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Set(KT key, VT value);
            /// <summary>
            /// 如果关键字不存在则添加数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> TryAdd(KT key, VT value);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> TryGetValue(KT key);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 256 基分片 哈希表 节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IFragmentHashSetNode<>))]
        public partial interface IFragmentHashSetNodeClientNode<T>
        {
            /// <summary>
            /// 如果关键字不存在则添加数据
            /// </summary>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Add(T value);
            /// <summary>
            /// 清除数据（保留分片数组）
            /// </summary>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Clear();
            /// <summary>
            /// 清除分片数组（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> ClearArray();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Contains(T value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> Count();
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="value"></param>
            /// <returns>是否存在关键字</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Remove(T value);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 哈希表节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IHashSetNode<>))]
        public partial interface IHashSetNodeClientNode<T>
        {
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Add(T value);
            /// <summary>
            /// 清除所有数据
            /// </summary>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Clear();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Contains(T value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> Count();
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="value"></param>
            /// <returns>是否删除成功</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Remove(T value);
            /// <summary>
            /// 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Renew();
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 256 基分片 HashString 字典 节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IHashStringFragmentDictionaryNode<>))]
        public partial interface IHashStringFragmentDictionaryNodeClientNode<T>
        {
            /// <summary>
            /// 清除数据（保留分片数组）
            /// </summary>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Clear();
            /// <summary>
            /// 清除分片数组（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> ClearArray();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> ContainsKey(string key);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> Count();
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns>被删除数据</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetRemove(string key);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="key"></param>
            /// <returns>是否存在关键字</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Remove(string key);
            /// <summary>
            /// 强制设置数据，如果关键字已存在则覆盖
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否设置成功</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Set(string key, T value);
            /// <summary>
            /// 如果关键字不存在则添加数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> TryAdd(string key, T value);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> TryGetValue(string key);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 数组节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ILeftArrayNode<>))]
        public partial interface ILeftArrayNodeClientNode<T>
        {
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="value">数据</param>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Add(T value);
            /// <summary>
            /// 清除指定位置数据
            /// </summary>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">清除数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Clear(int startIndex, int count);
            /// <summary>
            /// 清除所有数据并将数据有效长度设置为 0
            /// </summary>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> ClearLength();
            /// <summary>
            /// 用数据填充数组指定位置
            /// </summary>
            /// <param name="value"></param>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">填充数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Fill(T value, int startIndex, int count);
            /// <summary>
            /// 用数据填充整个数组
            /// </summary>
            /// <param name="value"></param>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> FillArray(T value);
            /// <summary>
            /// 获取数组容器初大小
            /// </summary>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> GetCapacity();
            /// <summary>
            /// 获取容器空闲数量
            /// </summary>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> GetFreeCount();
            /// <summary>
            /// 获取有效数组长度
            /// </summary>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> GetLength();
            /// <summary>
            /// 移除最后一个数据并返回该数据
            /// </summary>
            /// <returns>没有可移除数据则无数据返回</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetTryPopValue();
            /// <summary>
            /// 根据索引位置获取数据
            /// </summary>
            /// <param name="index">索引位置</param>
            /// <returns>超出索引返回则无返回值</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetValue(int index);
            /// <summary>
            /// 移除指定索引位置数据并返回被移除的数据
            /// </summary>
            /// <param name="index">数据位置</param>
            /// <returns>超出索引范围则无数据返回</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetValueRemoveAt(int index);
            /// <summary>
            /// 移除指定索引位置数据，将最后一个数据移动到该指定位置，并返回被移除的数据
            /// </summary>
            /// <param name="index"></param>
            /// <returns>超出索引范围则无数据返回</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetValueRemoveToEnd(int index);
            /// <summary>
            /// 根据索引位置设置数据并返回设置之前的数据
            /// </summary>
            /// <param name="index">索引位置</param>
            /// <param name="value">数据</param>
            /// <returns>设置之前的数据，超出索引返回则无返回值</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetValueSet(int index, T value);
            /// <summary>
            /// 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">查找匹配数据数量</param>
            /// <returns>失败返回负数</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> IndexOf(T value, int startIndex, int count);
            /// <summary>
            /// 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns>失败返回负数</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> IndexOfArray(T value);
            /// <summary>
            /// 插入数据
            /// </summary>
            /// <param name="index">插入位置</param>
            /// <param name="value">数据</param>
            /// <returns>超出索引范围则返回 false</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Insert(int index, T value);
            /// <summary>
            /// 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <param name="startIndex">最后一个匹配位置（起始位置）</param>
            /// <param name="count">查找匹配数据数量</param>
            /// <returns>失败返回负数</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> LastIndexOf(T value, int startIndex, int count);
            /// <summary>
            /// 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns>失败返回负数</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> LastIndexOfArray(T value);
            /// <summary>
            /// 移除第一个匹配数据（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value">数据</param>
            /// <returns>是否存在移除数据</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Remove(T value);
            /// <summary>
            /// 移除指定索引位置数据
            /// </summary>
            /// <param name="index">数据位置</param>
            /// <returns>超出索引范围则返回 false</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> RemoveAt(int index);
            /// <summary>
            /// 移除指定索引位置数据并将最后一个数据移动到该指定位置
            /// </summary>
            /// <param name="index"></param>
            /// <returns>超出索引范围则返回 false</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> RemoveToEnd(int index);
            /// <summary>
            /// 反转指定位置数组数据
            /// </summary>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">反转数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Reverse(int startIndex, int count);
            /// <summary>
            /// 反转整个数组数据
            /// </summary>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> ReverseArray();
            /// <summary>
            /// 置空并释放数组
            /// </summary>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> SetEmpty();
            /// <summary>
            /// 根据索引位置设置数据
            /// </summary>
            /// <param name="index">索引位置</param>
            /// <param name="value">数据</param>
            /// <returns>超出索引范围则返回 false</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> SetValue(int index, T value);
            /// <summary>
            /// 排序指定位置数组数据
            /// </summary>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">排序数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Sort(int startIndex, int count);
            /// <summary>
            /// 数组排序
            /// </summary>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> SortArray();
            /// <summary>
            /// 当有空闲位置时添加数据
            /// </summary>
            /// <param name="value"></param>
            /// <returns>如果数组已满则添加失败并返回 false</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> TryAdd(T value);
            /// <summary>
            /// 移除最后一个数据
            /// </summary>
            /// <returns>是否存在可移除数据</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> TryPop();
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 消息处理节点 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IMessageNode<>))]
        public partial interface IMessageNodeClientNode<T>
        {
            /// <summary>
            /// 生产者添加新消息
            /// </summary>
            /// <param name="message"></param>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> AppendMessage(T message);
            /// <summary>
            /// 清除所有消息
            /// </summary>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Clear();
            /// <summary>
            /// 清除所有失败消息
            /// </summary>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> ClearFailed();
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
            /// 获取未处理完成消息数量（不包括失败消息）
            /// </summary>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> GetCount();
            /// <summary>
            /// 获取失败消息数量
            /// </summary>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> GetFailedCount();
            /// <summary>
            /// 
            /// </summary>
            /// <param name="maxCount"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackResponse<T>> GetMessage(int maxCount);
            /// <summary>
            /// 获取未处理完成消息数量（包括失败消息）
            /// </summary>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> GetTotalCount();
            /// <summary>
            /// 获取消费者回调数量
            /// </summary>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> GetCallbackCount();
            /// <summary>
            /// 获取未完成处理超时消息数量
            /// </summary>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> GetTimeoutCount();
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 队列节点接口（先进先出） 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IQueueNode<>))]
        public partial interface IQueueNodeClientNode<T>
        {
            /// <summary>
            /// 清除所有数据
            /// </summary>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Clear();
            /// <summary>
            /// 判断队列中是否存在匹配数据（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value">匹配数据</param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Contains(T value);
            /// <summary>
            /// 将数据添加到队列
            /// </summary>
            /// <param name="value"></param>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Enqueue(T value);
            /// <summary>
            /// 获取队列数据数量
            /// </summary>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> Count();
            /// <summary>
            /// 从队列中弹出一个数据
            /// </summary>
            /// <returns>没有可弹出数据则返回无数据</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> TryDequeue();
            /// <summary>
            /// 获取队列中下一个弹出数据（不弹出数据仅查看）
            /// </summary>
            /// <returns>没有可弹出数据则返回无数据</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> TryPeek();
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 二叉搜索树节点 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISearchTreeDictionaryNode<,>))]
        public partial interface ISearchTreeDictionaryNodeClientNode<KT,VT>
        {
            /// <summary>
            /// 清除数据
            /// </summary>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Clear();
            /// <summary>
            /// 判断是否包含关键字
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>是否包含关键字</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> ContainsKey(KT key);
            /// <summary>
            /// 获取节点数据数量
            /// </summary>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> Count();
            /// <summary>
            /// 根据关键字比它小的节点数量
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>节点数量，失败返回 -1</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> CountLess(KT key);
            /// <summary>
            /// 根据关键字比它大的节点数量
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>节点数量，失败返回 -1</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> CountThan(KT key);
            /// <summary>
            /// 获取树高度，时间复杂度 O(n)
            /// </summary>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> GetHeight();
            /// <summary>
            /// 根据关键字删除节点
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>被删除数据</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> GetRemove(KT key);
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
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> IndexOf(KT key);
            /// <summary>
            /// 根据关键字删除节点
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>是否存在关键字</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Remove(KT key);
            /// <summary>
            /// 设置数据
            /// </summary>
            /// <param name="key">关键字</param>
            /// <param name="value">数据</param>
            /// <returns>是否添加了关键字</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Set(KT key, VT value);
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="key">关键字</param>
            /// <param name="value">数据</param>
            /// <returns>是否添加了数据</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> TryAdd(KT key, VT value);
            /// <summary>
            /// 获取第一个关键字数据
            /// </summary>
            /// <returns>第一个关键字数据</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<KT>>> TryGetFirstKey();
            /// <summary>
            /// 获取第一组数据
            /// </summary>
            /// <returns>第一组数据</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<AutoCSer.KeyValue<KT,VT>>>> TryGetFirstKeyValue();
            /// <summary>
            /// 获取第一个数据
            /// </summary>
            /// <returns>第一个数据</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> TryGetFirstValue();
            /// <summary>
            /// 根据节点位置获取数据
            /// </summary>
            /// <param name="index">节点位置</param>
            /// <returns>数据</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<AutoCSer.KeyValue<KT,VT>>>> TryGetKeyValueByIndex(int index);
            /// <summary>
            /// 获取最后一个关键字数据
            /// </summary>
            /// <returns>最后一个关键字数据</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<KT>>> TryGetLastKey();
            /// <summary>
            /// 获取最后一组数据
            /// </summary>
            /// <returns>最后一组数据</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<AutoCSer.KeyValue<KT,VT>>>> TryGetLastKeyValue();
            /// <summary>
            /// 获取最后一个数据
            /// </summary>
            /// <returns>最后一个数据</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> TryGetLastValue();
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>目标数据</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> TryGetValue(KT key);
            /// <summary>
            /// 根据节点位置获取数据
            /// </summary>
            /// <param name="index">节点位置</param>
            /// <returns>数据</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> TryGetValueByIndex(int index);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 二叉搜索树集合节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISearchTreeSetNode<>))]
        public partial interface ISearchTreeSetNodeClientNode<T>
        {
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="value">关键字</param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Add(T value);
            /// <summary>
            /// 清除所有数据
            /// </summary>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Clear();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="value">关键字</param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Contains(T value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> Count();
            /// <summary>
            /// 根据关键字比它小的节点数量
            /// </summary>
            /// <param name="value">关键字</param>
            /// <returns>节点数量，失败返回 -1</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> CountLess(T value);
            /// <summary>
            /// 根据关键字比它大的节点数量
            /// </summary>
            /// <param name="value">关键字</param>
            /// <returns>节点数量，失败返回 -1</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> CountThan(T value);
            /// <summary>
            /// 根据节点位置获取数据
            /// </summary>
            /// <param name="index">节点位置</param>
            /// <returns>数据</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetByIndex(int index);
            /// <summary>
            /// 获取第一个数据
            /// </summary>
            /// <returns>没有数据时返回无返回值</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetFrist();
            /// <summary>
            /// 获取最后一个数据
            /// </summary>
            /// <returns>没有数据时返回无返回值</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetLast();
            /// <summary>
            /// 根据关键字获取一个匹配节点位置
            /// </summary>
            /// <param name="value">关键字</param>
            /// <returns>一个匹配节点位置,失败返回-1</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> IndexOf(T value);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="value">关键字</param>
            /// <returns>是否删除成功</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Remove(T value);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 服务基础操作接口方法映射枚举 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNode))]
        public partial interface IServiceNodeClientNode
        {
            /// <summary>
            /// 删除节点
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <returns>是否成功删除节点，否则表示没有找到节点</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> RemoveNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index);
            /// <summary>
            /// 创建字典节点 FragmentHashStringDictionary256{HashString,string}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateFragmentHashStringDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 排序字典节点 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISortedDictionaryNode<,>))]
        public partial interface ISortedDictionaryNodeClientNode<KT,VT>
        {
            /// <summary>
            /// 清除所有数据
            /// </summary>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Clear();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> ContainsKey(KT key);
            /// <summary>
            /// 判断数据是否存在，时间复杂度 O(n) 不建议调用（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> ContainsValue(VT value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> Count();
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> GetRemove(KT key);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="key"></param>
            /// <returns>是否删除成功</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Remove(KT key);
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> TryAdd(KT key, VT value);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> TryGetValue(KT key);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 排序列表节点 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISortedListNode<,>))]
        public partial interface ISortedListNodeClientNode<KT,VT>
        {
            /// <summary>
            /// 清除所有数据
            /// </summary>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Clear();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> ContainsKey(KT key);
            /// <summary>
            /// 判断数据是否存在，时间复杂度 O(n) 不建议调用（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> ContainsValue(VT value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> Count();
            /// <summary>
            /// 获取容器大小
            /// </summary>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> GetCapacity();
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> GetRemove(KT key);
            /// <summary>
            /// 获取关键字排序位置
            /// </summary>
            /// <param name="key"></param>
            /// <returns>负数表示没有找到关键字</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> IndexOfKey(KT key);
            /// <summary>
            /// 获取第一个匹配数据排序位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns>负数表示没有找到匹配数据</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> IndexOfValue(VT value);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="key"></param>
            /// <returns>是否删除成功</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Remove(KT key);
            /// <summary>
            /// 删除指定排序索引位置数据
            /// </summary>
            /// <param name="index"></param>
            /// <returns>索引超出范围返回 false</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> RemoveAt(int index);
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> TryAdd(KT key, VT value);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> TryGetValue(KT key);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 排序集合节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISortedSetNode<>))]
        public partial interface ISortedSetNodeClientNode<T>
        {
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Add(T value);
            /// <summary>
            /// 清除所有数据
            /// </summary>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Clear();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Contains(T value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> Count();
            /// <summary>
            /// 获取最大值
            /// </summary>
            /// <returns>没有数据时返回无返回值</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetMax();
            /// <summary>
            /// 获取最小值
            /// </summary>
            /// <returns>没有数据时返回无返回值</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetMin();
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="value"></param>
            /// <returns>是否删除成功</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Remove(T value);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 栈节点（后进先出） 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IStackNode<>))]
        public partial interface IStackNodeClientNode<T>
        {
            /// <summary>
            /// 清除所有数据
            /// </summary>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Clear();
            /// <summary>
            /// 判断是否存在匹配数据（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value">匹配数据</param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Contains(T value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> Count();
            /// <summary>
            /// 将数据添加到栈
            /// </summary>
            /// <param name="value"></param>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Push(T value);
            /// <summary>
            /// 获取栈中下一个弹出数据（不弹出数据仅查看）
            /// </summary>
            /// <returns>没有可弹出数据则返回无数据</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> TryPeek();
            /// <summary>
            /// 从栈中弹出一个数据
            /// </summary>
            /// <returns>没有可弹出数据则返回无数据</returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> TryPop();
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 数组节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IArrayNode<>))]
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
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IBitmapNode))]
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
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IDictionaryNode<,>))]
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
            /// <summary>
            /// 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            /// <param name="capacity">新容器初始化大小</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Renew(int capacity);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 256 基分片字典 节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IFragmentDictionaryNode<,>))]
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
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IFragmentHashSetNode<>))]
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
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IHashSetNode<>))]
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
        /// 256 基分片 HashString 字典 节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IHashStringFragmentDictionaryNode<>))]
        public partial interface IHashStringFragmentDictionaryNodeLocalClientNode<T>
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
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> ContainsKey(string key);
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
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetRemove(string key);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="key"></param>
            /// <returns>是否存在关键字</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Remove(string key);
            /// <summary>
            /// 强制设置数据，如果关键字已存在则覆盖
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否设置成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> Set(string key, T value);
            /// <summary>
            /// 如果关键字不存在则添加数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> TryAdd(string key, T value);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> TryGetValue(string key);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 数组节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ILeftArrayNode<>))]
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
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IMessageNode<>))]
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
            /// 获取未处理完成消息数量（包括失败消息）
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> GetTotalCount();
            /// <summary>
            /// 获取消费者回调数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> GetCallbackCount();
            /// <summary>
            /// 获取未完成处理超时消息数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> GetTimeoutCount();
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 队列节点接口（先进先出） 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IQueueNode<>))]
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
            /// 将数据添加到队列
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Enqueue(T value);
            /// <summary>
            /// 获取队列数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> Count();
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
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISearchTreeDictionaryNode<,>))]
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
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISearchTreeSetNode<>))]
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
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNode))]
        public partial interface IServiceNodeLocalClientNode
        {
            /// <summary>
            /// 删除节点
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <returns>是否成功删除节点，否则表示没有找到节点</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<bool>> RemoveNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index);
            /// <summary>
            /// 创建字典节点 FragmentHashStringDictionary256{HashString,string}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateFragmentHashStringDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 排序字典节点 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISortedDictionaryNode<,>))]
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
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISortedListNode<,>))]
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
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISortedSetNode<>))]
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
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IStackNode<>))]
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
        /// 数组节点接口方法映射枚举
        /// </summary>
    public enum ArrayNodeMethodEnum
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
            /// [2] 清除指定位置数据 持久化参数检查
            /// int startIndex 起始位置
            /// int count 清除数据数量
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            ClearBeforePersistence = 2,
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
            /// [5] 用数据填充数组指定位置 持久化参数检查
            /// T value 
            /// int startIndex 起始位置
            /// int count 填充数据数量
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            FillBeforePersistence = 5,
            /// <summary>
            /// [6] 获取数组长度
            /// 返回值 int 
            /// </summary>
            GetLength = 6,
            /// <summary>
            /// [7] 根据索引位置获取数据
            /// int index 索引位置
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 超出索引返回则无返回值
            /// </summary>
            GetValue = 7,
            /// <summary>
            /// [8] 根据索引位置设置数据并返回设置之前的数据
            /// int index 索引位置
            /// T value 数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 设置之前的数据，超出索引返回则无返回值
            /// </summary>
            GetValueSet = 8,
            /// <summary>
            /// [9] 根据索引位置设置数据并返回设置之前的数据 持久化参数检查
            /// int index 索引位置
            /// T value 数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T}} 无返回值表示需要继续调用持久化方法
            /// </summary>
            GetValueSetBeforePersistence = 9,
            /// <summary>
            /// [10] 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// T value 
            /// int startIndex 起始位置
            /// int count 查找匹配数据数量
            /// 返回值 int 失败返回负数
            /// </summary>
            IndexOf = 10,
            /// <summary>
            /// [11] 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// T value 
            /// 返回值 int 失败返回负数
            /// </summary>
            IndexOfArray = 11,
            /// <summary>
            /// [12] 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// T value 
            /// int startIndex 最后一个匹配位置（起始位置）
            /// int count 查找匹配数据数量
            /// 返回值 int 失败返回负数
            /// </summary>
            LastIndexOf = 12,
            /// <summary>
            /// [13] 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// T value 
            /// 返回值 int 失败返回负数
            /// </summary>
            LastIndexOfArray = 13,
            /// <summary>
            /// [14] 反转指定位置数组数据
            /// int startIndex 起始位置
            /// int count 反转数据数量
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            Reverse = 14,
            /// <summary>
            /// [15] 反转整个数组数据
            /// </summary>
            ReverseArray = 15,
            /// <summary>
            /// [16] 反转指定位置数组数据 持久化参数检查
            /// int startIndex 起始位置
            /// int count 反转数据数量
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            ReverseBeforePersistence = 16,
            /// <summary>
            /// [17] 根据索引位置设置数据
            /// int index 索引位置
            /// T value 数据
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            SetValue = 17,
            /// <summary>
            /// [18] 根据索引位置设置数据 持久化参数检查
            /// int index 索引位置
            /// T value 数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            SetValueBeforePersistence = 18,
            /// <summary>
            /// [19] 
            /// AutoCSer.KeyValue{int,T} value 
            /// </summary>
            SnapshotSet = 19,
            /// <summary>
            /// [20] 排序指定位置数组数据
            /// int startIndex 起始位置
            /// int count 排序数据数量
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            Sort = 20,
            /// <summary>
            /// [21] 数组排序
            /// </summary>
            SortArray = 21,
            /// <summary>
            /// [22] 排序指定位置数组数据 持久化参数检查
            /// int startIndex 起始位置
            /// int count 排序数据数量
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            SortBeforePersistence = 22,
    }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 位图节点接口方法映射枚举
        /// </summary>
    public enum BitmapNodeMethodEnum
    {
            /// <summary>
            /// [0] 清除位状态
            /// uint index 位索引
            /// 返回值 bool 是否设置成功，失败表示索引超出范围
            /// </summary>
            ClearBit = 0,
            /// <summary>
            /// [1] 清除位状态 持久化参数检查
            /// uint index 位索引
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            ClearBitBeforePersistence = 1,
            /// <summary>
            /// [2] 清除所有数据
            /// </summary>
            ClearMap = 2,
            /// <summary>
            /// [3] 读取位状态
            /// uint index 位索引
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 非 0 表示二进制位为已设置状态，索引超出则无返回值
            /// </summary>
            GetBit = 3,
            /// <summary>
            /// [4] 清除位状态并返回设置之前的状态
            /// uint index 位索引
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 清除操作之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值
            /// </summary>
            GetBitClearBit = 4,
            /// <summary>
            /// [5] 清除位状态并返回设置之前的状态 持久化参数检查
            /// uint index 位索引
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int}} 无返回值表示需要继续调用持久化方法
            /// </summary>
            GetBitClearBitBeforePersistence = 5,
            /// <summary>
            /// [6] 状态取反并返回操作之前的状态
            /// uint index 位索引
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 取反操作之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值
            /// </summary>
            GetBitInvertBit = 6,
            /// <summary>
            /// [7] 状态取反并返回操作之前的状态 持久化参数检查
            /// uint index 位索引
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int}} 无返回值表示需要继续调用持久化方法
            /// </summary>
            GetBitInvertBitBeforePersistence = 7,
            /// <summary>
            /// [8] 设置位状态并返回设置之前的状态
            /// uint index 位索引
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 设置之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值
            /// </summary>
            GetBitSetBit = 8,
            /// <summary>
            /// [9] 设置位状态并返回设置之前的状态 持久化参数检查
            /// uint index 位索引
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int}} 无返回值表示需要继续调用持久化方法
            /// </summary>
            GetBitSetBitBeforePersistence = 9,
            /// <summary>
            /// [10] 获取二进制位数量
            /// 返回值 uint 
            /// </summary>
            GetCapacity = 10,
            /// <summary>
            /// [11] 状态取反
            /// uint index 位索引
            /// 返回值 bool 是否设置成功，失败表示索引超出范围
            /// </summary>
            InvertBit = 11,
            /// <summary>
            /// [12] 状态取反 持久化参数检查
            /// uint index 位索引
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            InvertBitBeforePersistence = 12,
            /// <summary>
            /// [13] 设置位状态
            /// uint index 位索引
            /// 返回值 bool 是否设置成功，失败表示索引超出范围
            /// </summary>
            SetBit = 13,
            /// <summary>
            /// [14] 设置位状态 持久化参数检查
            /// uint index 位索引
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            SetBitBeforePersistence = 14,
            /// <summary>
            /// [15] 快照添加数据
            /// byte[] map 
            /// </summary>
            SnapshotSet = 15,
    }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 字典节点接口方法映射枚举
        /// </summary>
    public enum DictionaryNodeMethodEnum
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
            /// [6] 强制设置数据，如果关键字已存在则覆盖
            /// KT key 
            /// VT value 
            /// 返回值 bool 是否设置成功
            /// </summary>
            Set = 6,
            /// <summary>
            /// [7] 尝试添加数据
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
            /// <summary>
            /// [9] 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// int capacity 新容器初始化大小
            /// </summary>
            Renew = 9,
            /// <summary>
            /// [10] 
            /// AutoCSer.KeyValue{KT,VT} value 
            /// </summary>
            SnapshotAdd = 10,
            /// <summary>
            /// [11] 删除关键字并返回被删除数据 持久化参数检查
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT}} 无返回值表示需要继续调用持久化方法
            /// </summary>
            GetRemoveBeforePersistence = 11,
            /// <summary>
            /// [12] 删除关键字 持久化参数检查
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            RemoveBeforePersistence = 12,
            /// <summary>
            /// [13] 清除所有数据并重建容器 持久化参数检查
            /// int capacity 新容器初始化大小
            /// 返回值 bool 返回 true 表示需要继续调用持久化方法
            /// </summary>
            RenewBeforePersistence = 13,
            /// <summary>
            /// [14] 强制设置数据 持久化参数检查
            /// KT key 
            /// VT value 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            SetBeforePersistence = 14,
            /// <summary>
            /// [15] 添加数据 持久化参数检查
            /// KT key 
            /// VT value 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            TryAddBeforePersistence = 15,
    }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 256 基分片字典 节点接口方法映射枚举
        /// </summary>
    public enum FragmentDictionaryNodeMethodEnum
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
            /// [7] 如果关键字不存在则添加数据
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
            /// <summary>
            /// [9] 
            /// AutoCSer.KeyValue{KT,VT} value 
            /// </summary>
            SnapshotAdd = 9,
            /// <summary>
            /// [10] 删除关键字并返回被删除数据 持久化参数检查
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT}} 无返回值表示需要继续调用持久化方法
            /// </summary>
            GetRemoveBeforePersistence = 10,
            /// <summary>
            /// [11] 删除关键字 持久化参数检查
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            RemoveBeforePersistence = 11,
            /// <summary>
            /// [12] 强制设置数据 持久化参数检查
            /// KT key 
            /// VT value 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            SetBeforePersistence = 12,
            /// <summary>
            /// [13] 如果关键字不存在则添加数据 持久化参数检查
            /// KT key 
            /// VT value 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            TryAddBeforePersistence = 13,
    }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 256 基分片 哈希表 节点接口方法映射枚举
        /// </summary>
    public enum FragmentHashSetNodeMethodEnum
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
            /// <summary>
            /// [6] 如果关键字不存在则添加数据 持久化参数检查
            /// T value 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            AddBeforePersistence = 6,
            /// <summary>
            /// [7] 删除关键字 持久化参数检查
            /// T value 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            RemoveBeforePersistence = 7,
    }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 哈希表节点接口方法映射枚举
        /// </summary>
    public enum HashSetNodeMethodEnum
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
            /// <summary>
            /// [6] 添加数据 持久化参数检查
            /// T value 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            AddBeforePersistence = 6,
            /// <summary>
            /// [7] 删除关键字 持久化参数检查
            /// T value 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            RemoveBeforePersistence = 7,
            /// <summary>
            /// [8] 清除所有数据并重建容器 持久化参数检查
            /// 返回值 bool 返回 true 表示需要继续调用持久化方法
            /// </summary>
            RenewBeforePersistence = 8,
    }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 256 基分片 HashString 字典 节点接口方法映射枚举
        /// </summary>
    public enum HashStringFragmentDictionaryNodeMethodEnum
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
            /// string key 
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
            /// string key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 被删除数据
            /// </summary>
            GetRemove = 4,
            /// <summary>
            /// [5] 删除关键字
            /// string key 
            /// 返回值 bool 是否存在关键字
            /// </summary>
            Remove = 5,
            /// <summary>
            /// [6] 强制设置数据，如果关键字已存在则覆盖
            /// string key 
            /// T value 
            /// 返回值 bool 是否设置成功
            /// </summary>
            Set = 6,
            /// <summary>
            /// [7] 如果关键字不存在则添加数据
            /// string key 
            /// T value 
            /// 返回值 bool 是否添加成功，否则表示关键字已经存在
            /// </summary>
            TryAdd = 7,
            /// <summary>
            /// [8] 根据关键字获取数据
            /// string key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 
            /// </summary>
            TryGetValue = 8,
            /// <summary>
            /// [9] 
            /// AutoCSer.KeyValue{string,T} value 
            /// </summary>
            SnapshotAdd = 9,
            /// <summary>
            /// [10] 删除关键字并返回被删除数据 持久化参数检查
            /// string key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T}} 无返回值表示需要继续调用持久化方法
            /// </summary>
            GetRemoveBeforePersistence = 10,
            /// <summary>
            /// [11] 删除关键字 持久化参数检查
            /// string key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            RemoveBeforePersistence = 11,
            /// <summary>
            /// [12] 强制设置数据 持久化参数检查
            /// string key 
            /// T value 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            SetBeforePersistence = 12,
            /// <summary>
            /// [13] 添加数据 持久化参数检查
            /// string key 
            /// T value 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            TryAddBeforePersistence = 13,
    }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 数组节点接口方法映射枚举
        /// </summary>
    public enum LeftArrayNodeMethodEnum
    {
            /// <summary>
            /// [0] 添加数据
            /// T value 数据
            /// </summary>
            Add = 0,
            /// <summary>
            /// [1] 添加数据 持久化参数检查
            /// T value 数据
            /// 返回值 bool 返回 true 表示需要继续调用持久化方法
            /// </summary>
            AddBeforePersistence = 1,
            /// <summary>
            /// [2] 清除指定位置数据
            /// int startIndex 起始位置
            /// int count 清除数据数量
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            Clear = 2,
            /// <summary>
            /// [3] 清除指定位置数据 持久化参数检查
            /// int startIndex 起始位置
            /// int count 清除数据数量
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            ClearBeforePersistence = 3,
            /// <summary>
            /// [4] 清除所有数据并将数据有效长度设置为 0
            /// </summary>
            ClearLength = 4,
            /// <summary>
            /// [5] 用数据填充数组指定位置
            /// T value 
            /// int startIndex 起始位置
            /// int count 填充数据数量
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            Fill = 5,
            /// <summary>
            /// [6] 用数据填充整个数组
            /// T value 
            /// </summary>
            FillArray = 6,
            /// <summary>
            /// [7] 用数据填充数组指定位置 持久化参数检查
            /// T value 
            /// int startIndex 起始位置
            /// int count 填充数据数量
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            FillBeforePersistence = 7,
            /// <summary>
            /// [8] 获取数组容器初大小
            /// 返回值 int 
            /// </summary>
            GetCapacity = 8,
            /// <summary>
            /// [9] 获取容器空闲数量
            /// 返回值 int 
            /// </summary>
            GetFreeCount = 9,
            /// <summary>
            /// [10] 获取有效数组长度
            /// 返回值 int 
            /// </summary>
            GetLength = 10,
            /// <summary>
            /// [11] 移除最后一个数据并返回该数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 没有可移除数据则无数据返回
            /// </summary>
            GetTryPopValue = 11,
            /// <summary>
            /// [12] 移除最后一个数据并返回该数据 持久化参数检查
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T}} 无返回值表示需要继续调用持久化方法
            /// </summary>
            GetTryPopValueBeforePersistence = 12,
            /// <summary>
            /// [13] 根据索引位置获取数据
            /// int index 索引位置
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 超出索引返回则无返回值
            /// </summary>
            GetValue = 13,
            /// <summary>
            /// [14] 移除指定索引位置数据并返回被移除的数据
            /// int index 数据位置
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 超出索引范围则无数据返回
            /// </summary>
            GetValueRemoveAt = 14,
            /// <summary>
            /// [15] 移除指定索引位置数据并返回被移除的数据 持久化参数检查
            /// int index 数据位置
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T}} 无返回值表示需要继续调用持久化方法
            /// </summary>
            GetValueRemoveAtBeforePersistence = 15,
            /// <summary>
            /// [16] 移除指定索引位置数据，将最后一个数据移动到该指定位置，并返回被移除的数据
            /// int index 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 超出索引范围则无数据返回
            /// </summary>
            GetValueRemoveToEnd = 16,
            /// <summary>
            /// [17] 移除指定索引位置数据，将最后一个数据移动到该指定位置，并返回被移除的数据
            /// int index 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T}} 无返回值表示需要继续调用持久化方法
            /// </summary>
            GetValueRemoveToEndBeforePersistence = 17,
            /// <summary>
            /// [18] 根据索引位置设置数据并返回设置之前的数据
            /// int index 索引位置
            /// T value 数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 设置之前的数据，超出索引返回则无返回值
            /// </summary>
            GetValueSet = 18,
            /// <summary>
            /// [19] 根据索引位置设置数据并返回设置之前的数据 持久化参数检查
            /// int index 索引位置
            /// T value 数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T}} 无返回值表示需要继续调用持久化方法
            /// </summary>
            GetValueSetBeforePersistence = 19,
            /// <summary>
            /// [20] 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// T value 
            /// int startIndex 起始位置
            /// int count 查找匹配数据数量
            /// 返回值 int 失败返回负数
            /// </summary>
            IndexOf = 20,
            /// <summary>
            /// [21] 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// T value 
            /// 返回值 int 失败返回负数
            /// </summary>
            IndexOfArray = 21,
            /// <summary>
            /// [22] 插入数据
            /// int index 插入位置
            /// T value 数据
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            Insert = 22,
            /// <summary>
            /// [23] 插入数据 持久化参数检查
            /// int index 插入位置
            /// T value 数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            InsertBeforePersistence = 23,
            /// <summary>
            /// [24] 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// T value 
            /// int startIndex 最后一个匹配位置（起始位置）
            /// int count 查找匹配数据数量
            /// 返回值 int 失败返回负数
            /// </summary>
            LastIndexOf = 24,
            /// <summary>
            /// [25] 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// T value 
            /// 返回值 int 失败返回负数
            /// </summary>
            LastIndexOfArray = 25,
            /// <summary>
            /// [26] 移除第一个匹配数据（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// T value 数据
            /// 返回值 bool 是否存在移除数据
            /// </summary>
            Remove = 26,
            /// <summary>
            /// [27] 移除指定索引位置数据
            /// int index 数据位置
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            RemoveAt = 27,
            /// <summary>
            /// [28] 移除指定索引位置数据 持久化参数检查
            /// int index 数据位置
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            RemoveAtBeforePersistence = 28,
            /// <summary>
            /// [29] 移除指定索引位置数据并将最后一个数据移动到该指定位置
            /// int index 
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            RemoveToEnd = 29,
            /// <summary>
            /// [30] 移除指定索引位置数据并将最后一个数据移动到该指定位置 持久化参数检查
            /// int index 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            RemoveToEndBeforePersistence = 30,
            /// <summary>
            /// [31] 反转指定位置数组数据
            /// int startIndex 起始位置
            /// int count 反转数据数量
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            Reverse = 31,
            /// <summary>
            /// [32] 反转整个数组数据
            /// </summary>
            ReverseArray = 32,
            /// <summary>
            /// [33] 反转指定位置数组数据 持久化参数检查
            /// int startIndex 起始位置
            /// int count 反转数据数量
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            ReverseBeforePersistence = 33,
            /// <summary>
            /// [34] 置空并释放数组
            /// </summary>
            SetEmpty = 34,
            /// <summary>
            /// [35] 根据索引位置设置数据
            /// int index 索引位置
            /// T value 数据
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            SetValue = 35,
            /// <summary>
            /// [36] 根据索引位置设置数据 持久化参数检查
            /// int index 索引位置
            /// T value 数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            SetValueBeforePersistence = 36,
            /// <summary>
            /// [37] 排序指定位置数组数据
            /// int startIndex 起始位置
            /// int count 排序数据数量
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            Sort = 37,
            /// <summary>
            /// [38] 数组排序
            /// </summary>
            SortArray = 38,
            /// <summary>
            /// [39] 排序指定位置数组数据 持久化参数检查
            /// int startIndex 起始位置
            /// int count 排序数据数量
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            SortBeforePersistence = 39,
            /// <summary>
            /// [40] 当有空闲位置时添加数据
            /// T value 
            /// 返回值 bool 如果数组已满则添加失败并返回 false
            /// </summary>
            TryAdd = 40,
            /// <summary>
            /// [41] 移除最后一个数据
            /// 返回值 bool 是否存在可移除数据
            /// </summary>
            TryPop = 41,
            /// <summary>
            /// [42] 移除最后一个数据 持久化参数检查
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            TryPopBeforePersistence = 42,
    }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 消息处理节点接口方法映射枚举
        /// </summary>
    public enum MessageNodeMethodEnum
    {
            /// <summary>
            /// [0] 生产者添加新消息
            /// T message 
            /// </summary>
            AppendMessage = 0,
            /// <summary>
            /// [1] 生产者添加新消息 持久化参数检查
            /// T message 
            /// 返回值 bool 返回 true 表示需要继续调用持久化方法
            /// </summary>
            AppendMessageBeforePersistence = 1,
            /// <summary>
            /// [2] 清除所有消息
            /// </summary>
            Clear = 2,
            /// <summary>
            /// [3] 清除所有失败消息
            /// </summary>
            ClearFailed = 3,
            /// <summary>
            /// [4] 消息完成处理
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity identity 
            /// </summary>
            Completed = 4,
            /// <summary>
            /// [5] 消息完成处理 持久化参数检查
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity identity 
            /// 返回值 bool 返回 true 表示需要继续调用持久化方法
            /// </summary>
            CompletedBeforePersistence = 5,
            /// <summary>
            /// [6] 消息失败处理
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity identity 
            /// </summary>
            Failed = 6,
            /// <summary>
            /// [7] 消息失败处理 持久化参数检查
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity identity 
            /// 返回值 bool 返回 true 表示需要继续调用持久化方法
            /// </summary>
            FailedBeforePersistence = 7,
            /// <summary>
            /// [8] 获取未处理完成消息数量（不包括失败消息）
            /// 返回值 int 
            /// </summary>
            GetCount = 8,
            /// <summary>
            /// [9] 获取失败消息数量
            /// 返回值 int 
            /// </summary>
            GetFailedCount = 9,
            /// <summary>
            /// [10] 
            /// int maxCount 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodKeepCallback{T} callback 
            /// 返回值 T 
            /// </summary>
            GetMessage = 10,
            /// <summary>
            /// [11] 快照设置数据
            /// T value 数据
            /// </summary>
            SnapshotAdd = 11,
            /// <summary>
            /// [12] 获取未处理完成消息数量（包括失败消息）
            /// 返回值 int 
            /// </summary>
            GetTotalCount = 12,
            /// <summary>
            /// [13] 获取消费者回调数量
            /// 返回值 int 
            /// </summary>
            GetCallbackCount = 13,
            /// <summary>
            /// [14] 获取未完成处理超时消息数量
            /// 返回值 int 
            /// </summary>
            GetTimeoutCount = 14,
    }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 数组节点接口方法映射枚举
        /// </summary>
    public enum QueueNodeMethodEnum
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
            /// [2] 将数据添加到队列
            /// T value 
            /// </summary>
            Enqueue = 2,
            /// <summary>
            /// [3] 获取队列数据数量
            /// 返回值 int 
            /// </summary>
            Count = 3,
            /// <summary>
            /// [4] 从队列中弹出一个数据 持久化参数检查
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T}} 无返回值表示需要继续调用持久化方法
            /// </summary>
            TryDequeueBeforePersistence = 4,
            /// <summary>
            /// [5] 从队列中弹出一个数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 没有可弹出数据则返回无数据
            /// </summary>
            TryDequeue = 5,
            /// <summary>
            /// [6] 获取队列中下一个弹出数据（不弹出数据仅查看）
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 没有可弹出数据则返回无数据
            /// </summary>
            TryPeek = 6,
    }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 二叉搜索树节点接口方法映射枚举
        /// </summary>
    public enum SearchTreeDictionaryNodeMethodEnum
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
            /// [7] 删除关键字并返回被删除数据 持久化参数检查
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT}} 无返回值表示需要继续调用持久化方法
            /// </summary>
            GetRemoveBeforePersistence = 7,
            /// <summary>
            /// [8] 获取范围数据集合
            /// int skipCount 跳过记录数
            /// byte getCount 获取记录数
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT} 
            /// </summary>
            GetValues = 8,
            /// <summary>
            /// [9] 根据关键字获取一个匹配节点位置
            /// KT key 关键字
            /// 返回值 int 一个匹配节点位置,失败返回-1
            /// </summary>
            IndexOf = 9,
            /// <summary>
            /// [10] 根据关键字删除节点
            /// KT key 关键字
            /// 返回值 bool 是否存在关键字
            /// </summary>
            Remove = 10,
            /// <summary>
            /// [11] 删除关键字 持久化参数检查
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            RemoveBeforePersistence = 11,
            /// <summary>
            /// [12] 设置数据
            /// KT key 关键字
            /// VT value 数据
            /// 返回值 bool 是否添加了关键字
            /// </summary>
            Set = 12,
            /// <summary>
            /// [13] 设置数据 持久化参数检查
            /// KT key 关键字
            /// VT value 数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            SetBeforePersistence = 13,
            /// <summary>
            /// [14] 
            /// AutoCSer.KeyValue{KT,VT} value 
            /// </summary>
            SnapshotAdd = 14,
            /// <summary>
            /// [15] 添加数据
            /// KT key 关键字
            /// VT value 数据
            /// 返回值 bool 是否添加了数据
            /// </summary>
            TryAdd = 15,
            /// <summary>
            /// [16] 添加数据 持久化参数检查
            /// KT key 
            /// VT value 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            TryAddBeforePersistence = 16,
            /// <summary>
            /// [17] 获取第一个关键字数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{KT} 第一个关键字数据
            /// </summary>
            TryGetFirstKey = 17,
            /// <summary>
            /// [18] 获取第一组数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{AutoCSer.KeyValue{KT,VT}} 第一组数据
            /// </summary>
            TryGetFirstKeyValue = 18,
            /// <summary>
            /// [19] 获取第一个数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT} 第一个数据
            /// </summary>
            TryGetFirstValue = 19,
            /// <summary>
            /// [20] 根据节点位置获取数据
            /// int index 节点位置
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{AutoCSer.KeyValue{KT,VT}} 数据
            /// </summary>
            TryGetKeyValueByIndex = 20,
            /// <summary>
            /// [21] 获取最后一个关键字数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{KT} 最后一个关键字数据
            /// </summary>
            TryGetLastKey = 21,
            /// <summary>
            /// [22] 获取最后一组数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{AutoCSer.KeyValue{KT,VT}} 最后一组数据
            /// </summary>
            TryGetLastKeyValue = 22,
            /// <summary>
            /// [23] 获取最后一个数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT} 最后一个数据
            /// </summary>
            TryGetLastValue = 23,
            /// <summary>
            /// [24] 根据关键字获取数据
            /// KT key 关键字
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT} 目标数据
            /// </summary>
            TryGetValue = 24,
            /// <summary>
            /// [25] 根据节点位置获取数据
            /// int index 节点位置
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT} 数据
            /// </summary>
            TryGetValueByIndex = 25,
    }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 二叉搜索树集合节点接口方法映射枚举
        /// </summary>
    public enum SearchTreeSetNodeMethodEnum
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
            /// <summary>
            /// [11] 添加数据 持久化参数检查
            /// T value 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            AddBeforePersistence = 11,
            /// <summary>
            /// [12] 删除关键字 持久化参数检查
            /// T value 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            RemoveBeforePersistence = 12,
    }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 服务基础操作接口方法映射枚举
        /// </summary>
    public enum ServiceNodeMethodEnum
    {
            /// <summary>
            /// [0] 删除节点
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// 返回值 bool 是否成功删除节点，否则表示没有找到节点
            /// </summary>
            RemoveNode = 0,
            /// <summary>
            /// [1] 创建字典节点 FragmentHashStringDictionary256{HashString,string}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateFragmentHashStringDictionaryNode = 1,
            /// <summary>
            /// [2] 删除节点持久化参数检查
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            RemoveNodeBeforePersistence = 2,
    }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 排序字典节点接口方法映射枚举
        /// </summary>
    public enum SortedDictionaryNodeMethodEnum
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
            /// [5] 删除关键字并返回被删除数据 持久化参数检查
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT}} 无返回值表示需要继续调用持久化方法
            /// </summary>
            GetRemoveBeforePersistence = 5,
            /// <summary>
            /// [6] 删除关键字
            /// KT key 
            /// 返回值 bool 是否删除成功
            /// </summary>
            Remove = 6,
            /// <summary>
            /// [7] 删除关键字 持久化参数检查
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            RemoveBeforePersistence = 7,
            /// <summary>
            /// [8] 
            /// AutoCSer.KeyValue{KT,VT} value 
            /// </summary>
            SnapshotAdd = 8,
            /// <summary>
            /// [9] 添加数据
            /// KT key 
            /// VT value 
            /// 返回值 bool 是否添加成功，否则表示关键字已经存在
            /// </summary>
            TryAdd = 9,
            /// <summary>
            /// [10] 添加数据 持久化参数检查
            /// KT key 
            /// VT value 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            TryAddBeforePersistence = 10,
            /// <summary>
            /// [11] 根据关键字获取数据
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT} 
            /// </summary>
            TryGetValue = 11,
    }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 排序列表节点接口方法映射枚举
        /// </summary>
    public enum SortedListNodeMethodEnum
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
            /// [6] 删除关键字并返回被删除数据 持久化参数检查
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT}} 无返回值表示需要继续调用持久化方法
            /// </summary>
            GetRemoveBeforePersistence = 6,
            /// <summary>
            /// [7] 获取关键字排序位置
            /// KT key 
            /// 返回值 int 负数表示没有找到关键字
            /// </summary>
            IndexOfKey = 7,
            /// <summary>
            /// [8] 获取第一个匹配数据排序位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// VT value 
            /// 返回值 int 负数表示没有找到匹配数据
            /// </summary>
            IndexOfValue = 8,
            /// <summary>
            /// [9] 删除关键字
            /// KT key 
            /// 返回值 bool 是否删除成功
            /// </summary>
            Remove = 9,
            /// <summary>
            /// [10] 删除指定排序索引位置数据
            /// int index 
            /// 返回值 bool 索引超出范围返回 false
            /// </summary>
            RemoveAt = 10,
            /// <summary>
            /// [11] 删除指定排序索引位置数据 持久化参数检查
            /// int index 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            RemoveAtBeforePersistence = 11,
            /// <summary>
            /// [12] 删除关键字 持久化参数检查
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            RemoveBeforePersistence = 12,
            /// <summary>
            /// [13] 
            /// AutoCSer.KeyValue{KT,VT} value 
            /// </summary>
            SnapshotAdd = 13,
            /// <summary>
            /// [14] 添加数据
            /// KT key 
            /// VT value 
            /// 返回值 bool 是否添加成功，否则表示关键字已经存在
            /// </summary>
            TryAdd = 14,
            /// <summary>
            /// [15] 添加数据 持久化参数检查
            /// KT key 
            /// VT value 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            TryAddBeforePersistence = 15,
            /// <summary>
            /// [16] 根据关键字获取数据
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT} 
            /// </summary>
            TryGetValue = 16,
    }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 哈希表节点接口方法映射枚举
        /// </summary>
    public enum SortedSetNodeMethodEnum
    {
            /// <summary>
            /// [0] 添加数据
            /// T value 
            /// 返回值 bool 是否添加成功，否则表示关键字已经存在
            /// </summary>
            Add = 0,
            /// <summary>
            /// [1] 添加数据 持久化参数检查
            /// T value 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            AddBeforePersistence = 1,
            /// <summary>
            /// [2] 清除所有数据
            /// </summary>
            Clear = 2,
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
            /// [5] 获取最大值
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 没有数据时返回无返回值
            /// </summary>
            GetMax = 5,
            /// <summary>
            /// [6] 获取最小值
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 没有数据时返回无返回值
            /// </summary>
            GetMin = 6,
            /// <summary>
            /// [7] 删除关键字
            /// T value 
            /// 返回值 bool 是否删除成功
            /// </summary>
            Remove = 7,
            /// <summary>
            /// [8] 删除关键字 持久化参数检查
            /// T value 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            RemoveBeforePersistence = 8,
    }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 栈节点接口方法映射枚举
        /// </summary>
    public enum StackNodeMethodEnum
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
            /// <summary>
            /// [6] 从栈中弹出一个数据 持久化参数检查
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T}} 无返回值表示需要继续调用持久化方法
            /// </summary>
            TryPopBeforePersistence = 6,
    }
}
#endif