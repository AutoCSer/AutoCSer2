using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 数组节点接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
#if !AOT
    [ServerNode(IsAutoMethodIndex = false, IsLocalClient = true)]
#endif
    public partial interface IArrayNode<T>
    {
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSet(KeyValue<int, T> value);
        /// <summary>
        /// 清除所有数据
        /// </summary>
        void ClearArray();
        /// <summary>
        /// 清除指定位置数据
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">清除数据数量</param>
        /// <returns>超出索引范围则返回 false</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool Clear(int startIndex, int count);
        /// <summary>
        /// 获取数组长度
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int GetLength();
        /// <summary>
        /// 根据索引位置获取数据
        /// </summary>
        /// <param name="index">索引位置</param>
        /// <returns>超出索引返回则无返回值</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<T> GetValue(int index);
        /// <summary>
        /// 根据索引位置设置数据
        /// </summary>
        /// <param name="index">索引位置</param>
        /// <param name="value">数据</param>
        /// <returns>超出索引范围则返回 false</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool SetValue(int index, T value);
        /// <summary>
        /// 根据索引位置设置数据并返回设置之前的数据
        /// </summary>
        /// <param name="index">索引位置</param>
        /// <param name="value">数据</param>
        /// <returns>设置之前的数据，超出索引返回则无返回值</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        ValueResult<T> GetValueSet(int index, T value);
        /// <summary>
        /// 用数据填充整个数组
        /// </summary>
        /// <param name="value"></param>
        void FillArray(T value);
        /// <summary>
        /// 用数据填充数组指定位置
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">填充数据数量</param>
        /// <returns>超出索引范围则返回 false</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool Fill(T value, int startIndex, int count);
        /// <summary>
        /// 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
        /// </summary>
        /// <param name="value"></param>
        /// <returns>失败返回负数</returns>
        [ServerMethod(IsPersistence = false)]
        int IndexOfArray(T value);
        /// <summary>
        /// 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">查找匹配数据数量</param>
        /// <returns>失败返回负数</returns>
        [ServerMethod(IsPersistence = false)]
        int IndexOf(T value, int startIndex, int count);
        /// <summary>
        /// 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
        /// </summary>
        /// <param name="value"></param>
        /// <returns>失败返回负数</returns>
        [ServerMethod(IsPersistence = false)]
        int LastIndexOfArray(T value);
        /// <summary>
        /// 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startIndex">最后一个匹配位置（起始位置）</param>
        /// <param name="count">查找匹配数据数量</param>
        /// <returns>失败返回负数</returns>
        [ServerMethod(IsPersistence = false)]
        int LastIndexOf(T value, int startIndex, int count);
        /// <summary>
        /// 反转整个数组数据
        /// </summary>
        void ReverseArray();
        /// <summary>
        /// 反转指定位置数组数据
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">反转数据数量</param>
        /// <returns>超出索引范围则返回 false</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool Reverse(int startIndex, int count);
        /// <summary>
        /// 数组排序
        /// </summary>
        void SortArray();
        /// <summary>
        /// 排序指定位置数组数据
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>超出索引范围则返回 false</returns>
        bool Sort(int startIndex, int count);
    }
}
