using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Array node interface
    /// 数组节点接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [ServerNode(IsLocalClient = true)]
    public partial interface ILeftArrayNode<T>
    {
        /// <summary>
        /// Get the valid length of the array
        /// 获取数组有效长度
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int GetLength();
        /// <summary>
        /// Get the size of the array container
        /// 获取数组容器大小
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int GetCapacity();
        /// <summary>
        /// Get the number of containers free
        /// 获取容器空闲数量
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int GetFreeCount();
        /// <summary>
        /// Empty and release the array
        /// 置空并释放数组
        /// </summary>
        void SetEmpty();
        /// <summary>
        /// Clear all the data and set the valid length of the data to 0
        /// 清除所有数据并将数据有效长度设置为 0
        /// </summary>
        void ClearLength();
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
        /// Add data
        /// </summary>
        /// <param name="value">data</param>
        [ServerMethod(SnapshotMethodSort = 1, IsIgnorePersistenceCallbackException = true)]
        void Add(T value);
        /// <summary>
        /// Add data when there is a free place
        /// 当有空闲位置时添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returning false indicates that the array is full and the addition failed
        /// 返回 false 表示数组已满，添加失败</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool TryAdd(T value);
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
        /// Insert data
        /// 插入数据
        /// </summary>
        /// <param name="index">Insert position
        /// 插入位置</param>
        /// <param name="value">data</param>
        /// <returns>Return false if it exceeds the index range
        /// 超出索引范围则返回 false</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool Insert(int index, T value);
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
        /// <summary>
        /// Remove the first matching data (Since the cached data is a serialized copy of the object, the prerequisite for determining whether the objects are equal is to implement IEquatable{VT})
        /// 移除第一个匹配数据（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
        /// </summary>
        /// <param name="value">data</param>
        /// <returns>Returning false indicates that there is no data match
        /// 返回 false 表示不存在数据匹配</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool Remove(T value);
        /// <summary>
        /// Remove the data at the specified index position
        /// 移除指定索引位置数据
        /// </summary>
        /// <param name="index">Data location
        /// 数据位置</param>
        /// <returns>Return false if it exceeds the index range
        /// 超出索引范围则返回 false</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool RemoveAt(int index);
        /// <summary>
        /// Remove the data at the specified index position and return the removed data
        /// 移除指定索引位置数据并返回被移除的数据
        /// </summary>
        /// <param name="index">Data location
        /// 数据位置</param>
        /// <returns>No data will be returned if the index range is exceeded
        /// 超出索引范围则无数据返回</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        ValueResult<T> GetValueRemoveAt(int index);
        /// <summary>
        /// Remove the data at the specified index position and move the last data to that specified position
        /// 移除指定索引位置数据并将最后一个数据移动到该指定位置
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Return false if it exceeds the index range
        /// 超出索引范围则返回 false</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool RemoveToEnd(int index);
        /// <summary>
        /// Remove the data at the specified index position, move the last data to the specified position, and return the removed data
        /// 移除指定索引位置数据，将最后一个数据移动到该指定位置，并返回被移除的数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns>No data will be returned if the index range is exceeded
        /// 超出索引范围则无数据返回</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        ValueResult<T> GetValueRemoveToEnd(int index);
        /// <summary>
        /// Remove the last data and return it
        /// 移除最后一个数据并返回该数据
        /// </summary>
        /// <returns>No data will be returned if there is no removable data
        /// 没有可移除数据则无数据返回</returns>
        ValueResult<T> GetTryPopValue();
        /// <summary>
        /// Try to remove the last data
        /// 尝试移除最后一个数据
        /// </summary>
        /// <returns>Is there any removable data
        /// 是否存在可移除数据</returns>
        bool TryPop();
    }
}
