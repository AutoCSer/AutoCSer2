using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 数组节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
#if AOT
    public abstract class ArrayNode<T> : ISnapshot<KeyValue<int, T>>
#else
    public sealed class ArrayNode<T> : IArrayNode<T>, ISnapshot<KeyValue<int, T>>
#endif
    {
        /// <summary>
        /// 数组
        /// </summary>
        private T[] array;
        /// <summary>
        /// 数组节点
        /// </summary>
        /// <param name="length">Array length</param>
        public ArrayNode(int length)
        {
            array = new T[length];
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
            return array.Length;
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
        public SnapshotResult<KeyValue<int, T>> GetSnapshotResult(KeyValue<int, T>[] snapshotArray, object customObject)
        {
            int index = 0, snapshotIndex = 0;
            foreach (T value in array)
            {
                if (value != null) snapshotArray[snapshotIndex++].Set(index, value);
                ++index;
            }
            return new SnapshotResult<KeyValue<int, T>>(snapshotIndex);
        }
        /// <summary>
        /// Reorganize the snapshot data before persistence
        /// 持久化之前重组快照数据
        /// </summary>
        /// <param name="array">Pre-applied snapshot data container
        /// 预申请的快照数据容器</param>
        /// <param name="newArray">Snapshot data collection that exceed the pre-application scope
        /// 超出预申请范围的快照数据集合</param>
        public void SetSnapshotResult(ref LeftArray<KeyValue<int, T>> array, ref LeftArray<KeyValue<int, T>> newArray) { }
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        public void SnapshotSet(KeyValue<int, T> value)
        {
            array[value.Key] = value.Value;
        }
        /// <summary>
        /// Clear all data
        /// 清除所有数据
        /// </summary>
        public void ClearArray()
        {
            Array.Clear(array, 0, array.Length);
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
                    Array.Clear(array, startIndex, count);
                    return true;
                case NullableBoolEnum.True: return true;
                default: return false;
            }
        }
        /// <summary>
        /// Get the array length
        /// 获取数组长度
        /// </summary>
        /// <returns></returns>
        public int GetLength()
        {
            return array.Length;
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
            if ((uint)index < (uint)array.Length) return array[index];
            return default(ValueResult<T>);
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
                array[index] = value;
                return true;
            }
            return false;
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
                T arrayValue = array[index];
                array[index] = value;
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
            AutoCSer.Common.Fill(array, value);
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
                    AutoCSer.Common.Fill(array, value, startIndex, count);
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
            return Array.IndexOf(array, value);
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
            if (startIndex >= 0 && count > 0 && (uint)(startIndex + count) <= (uint)array.Length) return Array.IndexOf(array, value, startIndex, count);
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
            return Array.LastIndexOf(array, value);
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
            if (startIndex >= 0 && count > 0 && startIndex - count >= -1) return Array.LastIndexOf(array, value, startIndex, count);
            return -1;
        }
        /// <summary>
        /// Reverse the entire array data
        /// 反转整个数组数据
        /// </summary>
        public void ReverseArray()
        {
            Array.Reverse(array);
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
                    Array.Reverse(array, startIndex, count);
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
            Array.Sort(array);
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
                    Array.Sort(array, startIndex, count);
                    return true;
                case NullableBoolEnum.True: return true;
                default: return false;
            }
        }
    }
}
