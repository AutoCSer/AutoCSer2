using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 数组节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
#if AOT
    public abstract class LeftArrayNode<T> : ISnapshot<T>
#else
    public sealed class LeftArrayNode<T> : ILeftArrayNode<T>, ISnapshot<T>
#endif
    {
        /// <summary>
        /// 数组
        /// </summary>
        private LeftArray<T> array;
        /// <summary>
        /// 数组节点
        /// </summary>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        public LeftArrayNode(int capacity)
        {
            array = new LeftArray<T>(capacity);
        }
        /// <summary>
        /// Get the snapshot data collection container size for pre-applying snapshot data containers
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">Custom objects for pre-generating auxiliary data
        /// 自定义对象，用于预生成辅助数据</param>
        /// <returns>The size of the snapshot data collection container
        /// 快照数据集合容器大小</returns>
        public int GetSnapshotCapacity(ref object customObject)
        {
            return array.Count;
        }
        /// <summary>
        /// Get the snapshot data collection. If the data object may be modified, the cloned data object should be returned to prevent the data from being modified during the snapshot establishment
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">Pre-applied snapshot data container
        /// 预申请的快照数据容器</param>
        /// <param name="customObject">Custom objects for pre-generating auxiliary data
        /// 自定义对象，用于预生成辅助数据</param>
        /// <returns>Snapshot data
        /// 快照数据</returns>
        public SnapshotResult<T> GetSnapshotResult(T[] snapshotArray, object customObject)
        {
            if (array.Count <= snapshotArray.Length)
            {
                array.CopyTo(snapshotArray, 0);
                return new SnapshotResult<T>(array.Count);
            }
            AutoCSer.Common.CopyTo(array.Array, snapshotArray, 0, snapshotArray.Length);
            T[] newArray = new T[array.Count - snapshotArray.Length];
            Array.Copy(array.Array, snapshotArray.Length, newArray, 0, newArray.Length);
            return new SnapshotResult<T>(snapshotArray.Length, newArray);
        }
        /// <summary>
        /// Reorganize the snapshot data before persistence
        /// 持久化之前重组快照数据
        /// </summary>
        /// <param name="array">Pre-applied snapshot data container
        /// 预申请的快照数据容器</param>
        /// <param name="newArray">Snapshot data collection that exceed the pre-application scope
        /// 超出预申请范围的快照数据集合</param>
        public void SetSnapshotResult(ref LeftArray<T> array, ref LeftArray<T> newArray) { }
        /// <summary>
        /// Get the valid length of the array
        /// 获取数组有效长度
        /// </summary>
        /// <returns></returns>
        public int GetLength()
        {
            return array.Length;
        }
        /// <summary>
        /// Get the size of the array container
        /// 获取数组容器大小
        /// </summary>
        /// <returns></returns>
        public int GetCapacity()
        {
            return array.Array.Length;
        }
        /// <summary>
        /// Get the number of containers free
        /// 获取容器空闲数量
        /// </summary>
        /// <returns></returns>
        public int GetFreeCount()
        {
            return array.FreeCount;
        }
        /// <summary>
        /// Empty and release the array
        /// 置空并释放数组
        /// </summary>
        public void SetEmpty()
        {
            array.SetEmpty();
        }
        /// <summary>
        /// Clear all the data and set the valid length of the data to 0
        /// 清除所有数据并将数据有效长度设置为 0
        /// </summary>
        public void ClearLength()
        {
            array.ClearLength();
        }
        /// <summary>
        /// 检查索引范围
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private NullableBoolEnum checkRange(int startIndex, int count)
        {
            if (count != 0)
            {
                if (startIndex >= 0 && count > 0 && (uint)(startIndex + count) <= (uint)array.Length) return NullableBoolEnum.Null;
                return NullableBoolEnum.False;
            }
            return (uint)startIndex <= (uint)array.Length ? NullableBoolEnum.True : NullableBoolEnum.False;
        }
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
        public bool Clear(int startIndex, int count)
        {
            switch (checkRange(startIndex, count))
            {
                case NullableBoolEnum.Null:
                    Array.Clear(array.Array, startIndex, count);
                    return true;
                case NullableBoolEnum.True: return true;
                default: return false;
            }
        }
        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="value">data</param>
        public void Add(T value)
        {
            array.Add(value);
        }
        /// <summary>
        /// Add data when there is a free place
        /// 当有空闲位置时添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returning false indicates that the array is full and the addition failed
        /// 返回 false 表示数组已满，添加失败</returns>
        public bool TryAdd(T value)
        {
            return array.TryAdd(value);
        }
        /// <summary>
        /// Set the data according to the index position
        /// 根据索引位置设置数据
        /// </summary>
        /// <param name="index">Index position
        /// 索引位置</param>
        /// <param name="value">data</param>
        /// <returns>Return false if it exceeds the index range
        /// 超出索引范围则返回 false</returns>
        public bool SetValue(int index, T value)
        {
            if ((uint)index < (uint)array.Length)
            {
                array.Array[index] = value;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Insert data
        /// 插入数据
        /// </summary>
        /// <param name="index">Insert position
        /// 插入位置</param>
        /// <param name="value">data</param>
        /// <returns>Return false if it exceeds the index range
        /// 超出索引范围则返回 false</returns>
        public bool Insert(int index, T value)
        {
            if ((uint)index < (uint)array.Length)
            {
                array.Insert(index, value);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Get data based on index location
        /// 根据索引位置获取数据
        /// </summary>
        /// <param name="index">Index position
        /// 索引位置</param>
        /// <returns>If the return exceeds the index, there will be no return value
        /// 超出索引返回则无返回值</returns>
        public ValueResult<T> GetValue(int index)
        {
            if ((uint)index < (uint)array.Length) return array.Array[index];
            return default(ValueResult<T>);
        }
        /// <summary>
        /// Set the data according to the index position and return the data before the setting
        /// 根据索引位置设置数据并返回设置之前的数据
        /// </summary>
        /// <param name="index">Index position
        /// 索引位置</param>
        /// <param name="value">data</param>
        /// <returns>Set the previous data. If it exceeds the index and returns, there will be no return value
        /// 设置之前的数据，超出索引返回则无返回值</returns>
        public ValueResult<T> GetValueSet(int index, T value)
        {
            if ((uint)index < (uint)array.Length)
            {
                T arrayValue = array.Array[index];
                array.Array[index] = value;
                return arrayValue;
            }
            return default(ValueResult<T>);
        }
        /// <summary>
        /// Fill the entire array with data
        /// 用数据填充整个数组
        /// </summary>
        /// <param name="value"></param>
        public void FillArray(T value)
        {
            AutoCSer.Common.Fill(array.Array, value, 0, array.Length);
        }
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
        public bool Fill(T value, int startIndex, int count)
        {
            switch (checkRange(startIndex, count))
            {
                case NullableBoolEnum.Null:
                    AutoCSer.Common.Fill(array.Array, value, startIndex, count);
                    return true;
                case NullableBoolEnum.True: return true;
                default: return false;
            }
        }
        /// <summary>
        /// Find the position of the first matching data from the array. (Since the cached data is a serialized copy of the object, the prerequisite for determining whether the objects are equal is to implement IEquatable{VT})
        /// 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Failure returns a negative number
        /// 失败返回负数</returns>
        public int IndexOfArray(T value)
        {
            return array.IndexOf(value);
        }
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
        public int IndexOf(T value, int startIndex, int count)
        {
            if (startIndex >= 0 && count > 0 && (uint)(startIndex + count) <= (uint)array.Length) return Array.IndexOf(array.Array, value, startIndex, count);
            return -1;
        }
        /// <summary>
        /// Find the position of the last matching data from the array. (Since the cached data is a serialized copy of the object, the prerequisite for determining whether the objects are equal is to implement IEquatable{VT})
        /// 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Failure returns a negative number
        /// 失败返回负数</returns>
        public int LastIndexOfArray(T value)
        {
            return Array.LastIndexOf(array.Array, value, array.Length - 1, array.Length);
        }
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
        public int LastIndexOf(T value, int startIndex, int count)
        {
            if (startIndex >= 0 && count > 0 && startIndex - count >= -1) return Array.LastIndexOf(array.Array, value, startIndex, count);
            return -1;
        }
        /// <summary>
        /// Reverse the entire array data
        /// 反转整个数组数据
        /// </summary>
        public void ReverseArray()
        {
            array.Reverse();
        }
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
        public bool Reverse(int startIndex, int count)
        {
            switch (checkRange(startIndex, count))
            {
                case NullableBoolEnum.Null:
                    Array.Reverse(array.Array, startIndex, count);
                    return true;
                case NullableBoolEnum.True: return true;
                default: return false;
            }
        }
        /// <summary>
        /// Array sorting
        /// 数组排序
        /// </summary>
        public void SortArray()
        {
            Array.Sort(array.Array, 0, array.Length);
        }
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
        public bool Sort(int startIndex, int count)
        {
            switch (checkRange(startIndex, count))
            {
                case NullableBoolEnum.Null:
                    Array.Sort(array.Array, startIndex, count);
                    return true;
                case NullableBoolEnum.True: return true;
                default: return false;
            }
        }
        /// <summary>
        /// Remove the first matching data (Since the cached data is a serialized copy of the object, the prerequisite for determining whether the objects are equal is to implement IEquatable{VT})
        /// 移除第一个匹配数据（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
        /// </summary>
        /// <param name="value">data</param>
        /// <returns>Returning false indicates that there is no data match
        /// 返回 false 表示不存在数据匹配</returns>
        public bool Remove(T value)
        {
            return array.Remove(value);
        }
        /// <summary>
        /// Remove the data at the specified index position
        /// 移除指定索引位置数据
        /// </summary>
        /// <param name="index">Data location
        /// 数据位置</param>
        /// <returns>Return false if it exceeds the index range
        /// 超出索引范围则返回 false</returns>
        public bool RemoveAt(int index)
        {
            if ((uint)index < (uint)array.Length)
            {
                array.RemoveAt(index);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Remove the data at the specified index position and return the removed data
        /// 移除指定索引位置数据并返回被移除的数据
        /// </summary>
        /// <param name="index">Data location
        /// 数据位置</param>
        /// <returns>No data will be returned if the index range is exceeded
        /// 超出索引范围则无数据返回</returns>
        public ValueResult<T> GetValueRemoveAt(int index)
        {
            if ((uint)index < (uint)array.Length)
            {
                T value = array.Array[index];
                array.RemoveAt(index);
                return value;
            }
            return default(ValueResult<T>);
        }
        /// <summary>
        /// Remove the data at the specified index position and move the last data to that specified position
        /// 移除指定索引位置数据并将最后一个数据移动到该指定位置
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Return false if it exceeds the index range
        /// 超出索引范围则返回 false</returns>
        public bool RemoveToEnd(int index)
        {
            if ((uint)index < (uint)array.Length)
            {
                array.RemoveToEnd(index);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Remove the data at the specified index position, move the last data to the specified position, and return the removed data
        /// 移除指定索引位置数据，将最后一个数据移动到该指定位置，并返回被移除的数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns>No data will be returned if the index range is exceeded
        /// 超出索引范围则无数据返回</returns>
        public ValueResult<T> GetValueRemoveToEnd(int index)
        {
            if ((uint)index < (uint)array.Length)
            {
                T value = array.Array[index];
                array.RemoveToEnd(index);
                return value;
            }
            return default(ValueResult<T>);
        }
        /// <summary>
        /// Remove the last data and return it
        /// 移除最后一个数据并返回该数据
        /// </summary>
        /// <returns>No data will be returned if there is no removable data
        /// 没有可移除数据则无数据返回</returns>
        public ValueResult<T> GetTryPopValue()
        {
            var value = default(T);
            if (array.TryPop(out value)) return value;
            return default(ValueResult<T>);
        }
        /// <summary>
        /// Try to remove the last data
        /// 尝试移除最后一个数据
        /// </summary>
        /// <returns>Is there any removable data
        /// 是否存在可移除数据</returns>
        public bool TryPop()
        {
            var value = default(T);
            return array.TryPop(out value);
        }
    }
}
