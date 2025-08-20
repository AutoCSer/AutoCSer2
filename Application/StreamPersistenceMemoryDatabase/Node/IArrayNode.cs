using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Array node interface
    /// 数组节点接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [ServerNode(IsLocalClient = true)]
    public partial interface IArrayNode<T>
    {
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSet(KeyValue<int, T> value);
        /// <summary>
        /// Clear all data
        /// 清除所有数据
        /// </summary>
        void ClearArray();
        /// <summary>
        /// Clear the data at the specified location
        /// 清除指定位置数据
        /// </summary>
        /// <param name="startIndex">Starting position
        /// 起始位置</param>
        /// <param name="count">Clear data quantity
        /// 清除数据数量</param>
        /// <returns>Return false if it exceeds the index range
        /// 超出索引范围则返回 false</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool Clear(int startIndex, int count);
        /// <summary>
        /// Get the array length
        /// 获取数组长度
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int GetLength();
        /// <summary>
        /// Get data based on index location
        /// 根据索引位置获取数据
        /// </summary>
        /// <param name="index">Index position
        /// 索引位置</param>
        /// <returns>If the return exceeds the index, there will be no return value
        /// 超出索引返回则无返回值</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<T> GetValue(int index);
        /// <summary>
        /// Set the data according to the index position
        /// 根据索引位置设置数据
        /// </summary>
        /// <param name="index">Index position
        /// 索引位置</param>
        /// <param name="value">data</param>
        /// <returns>Return false if it exceeds the index range
        /// 超出索引范围则返回 false</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool SetValue(int index, T value);
        /// <summary>
        /// Set the data according to the index position and return the data before the setting
        /// 根据索引位置设置数据并返回设置之前的数据
        /// </summary>
        /// <param name="index">Index position
        /// 索引位置</param>
        /// <param name="value">data</param>
        /// <returns>Set the previous data. If it exceeds the index and returns, there will be no return value
        /// 设置之前的数据，超出索引返回则无返回值</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        ValueResult<T> GetValueSet(int index, T value);
        /// <summary>
        /// Fill the entire array with data
        /// 用数据填充整个数组
        /// </summary>
        /// <param name="value"></param>
        void FillArray(T value);
        /// <summary>
        /// Fill the array with data to specify the position
        /// 用数据填充数组指定位置
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startIndex">Starting position
        /// 起始位置</param>
        /// <param name="count">The number of filled data
        /// 填充数据数量</param>
        /// <returns>Return false if it exceeds the index range
        /// 超出索引范围则返回 false</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool Fill(T value, int startIndex, int count);
        /// <summary>
        /// Find the position of the first matching data from the array. (Since the cached data is a serialized copy of the object, the prerequisite for determining whether the objects are equal is to implement IEquatable{VT})
        /// 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Failure returns a negative number
        /// 失败返回负数</returns>
        [ServerMethod(IsPersistence = false)]
        int IndexOfArray(T value);
        /// <summary>
        /// Find the position of the first matching data from the array. (Since the cached data is a serialized copy of the object, the prerequisite for determining whether the objects are equal is to implement IEquatable{VT})
        /// 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startIndex">Starting position
        /// 起始位置</param>
        /// <param name="count">Search for the number of matching data
        /// 查找匹配数据数量</param>
        /// <returns>Failure returns a negative number
        /// 失败返回负数</returns>
        [ServerMethod(IsPersistence = false)]
        int IndexOf(T value, int startIndex, int count);
        /// <summary>
        /// Find the position of the last matching data from the array. (Since the cached data is a serialized copy of the object, the prerequisite for determining whether the objects are equal is to implement IEquatable{VT})
        /// 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Failure returns a negative number
        /// 失败返回负数</returns>
        [ServerMethod(IsPersistence = false)]
        int LastIndexOfArray(T value);
        /// <summary>
        /// Find the position of the last matching data from the array. (Since the cached data is a serialized copy of the object, the prerequisite for determining whether the objects are equal is to implement IEquatable{VT})
        /// 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startIndex">The last matching position (the starting position)
        /// 最后一个匹配位置（起始位置）</param>
        /// <param name="count">Search for the number of matching data
        /// 查找匹配数据数量</param>
        /// <returns>Failure returns a negative number
        /// 失败返回负数</returns>
        [ServerMethod(IsPersistence = false)]
        int LastIndexOf(T value, int startIndex, int count);
        /// <summary>
        /// Reverse the entire array data
        /// 反转整个数组数据
        /// </summary>
        void ReverseArray();
        /// <summary>
        /// Reverse the array data at the specified position
        /// 反转指定位置数组数据
        /// </summary>
        /// <param name="startIndex">Starting position
        /// 起始位置</param>
        /// <param name="count">Reverse the amount of data
        /// 反转数据数量</param>
        /// <returns>Return false if it exceeds the index range
        /// 超出索引范围则返回 false</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool Reverse(int startIndex, int count);
        /// <summary>
        /// Array sorting
        /// 数组排序
        /// </summary>
        void SortArray();
        /// <summary>
        /// Sort the array data at the specified position
        /// 排序指定位置数组数据
        /// </summary>
        /// <param name="startIndex">Starting position
        /// 起始位置</param>
        /// <param name="count">The quantity of data to be sorted
        /// 排序数据数量</param>
        /// <returns>Return false if it exceeds the index range
        /// 超出索引范围则返回 false</returns>
        bool Sort(int startIndex, int count);
    }
}
