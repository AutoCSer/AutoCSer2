using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 数组节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ArrayNode<T> : IArrayNode<T>, ISnapshot<KeyValue<int, T>>
    {
        /// <summary>
        /// 数组
        /// </summary>
        private T[] array;
        /// <summary>
        /// 数组节点
        /// </summary>
        /// <param name="length">数组长度</param>
        public ArrayNode(int length)
        {
            array = new T[length];
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <returns>快照数据集合</returns>
        public LeftArray<KeyValue<int, T>> GetSnapshotArray()
        {
            LeftArray<KeyValue<int, T>> snapshotArray = new LeftArray<KeyValue<int, T>>(array.Length);
            int index = 0;
            foreach (T value in array)
            {
                if (value != null) snapshotArray.Array[snapshotArray.Length++].Set(index, value);
                ++index;
            }
            return snapshotArray;
        }
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        public void SnapshotSet(KeyValue<int, T> value)
        {
            array[value.Key] = value.Value;
        }
        /// <summary>
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
        /// 清除指定位置数据
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">清除数据数量</param>
        /// <returns>超出索引范围则返回 false</returns>
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
        /// 获取数组长度
        /// </summary>
        /// <returns></returns>
        public int GetLength()
        {
            return array.Length;
        }
        /// <summary>
        /// 根据索引位置获取数据
        /// </summary>
        /// <param name="index">索引位置</param>
        /// <returns>超出索引返回则无返回值</returns>
        public ValueResult<T> GetValue(int index)
        {
            if ((uint)index < (uint)array.Length) return array[index];
            return default(ValueResult<T>);
        }
        /// <summary>
        /// 根据索引位置设置数据
        /// </summary>
        /// <param name="index">索引位置</param>
        /// <param name="value">数据</param>
        /// <returns>超出索引范围则返回 false</returns>
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
        /// 根据索引位置设置数据并返回设置之前的数据
        /// </summary>
        /// <param name="index">索引位置</param>
        /// <param name="value">数据</param>
        /// <returns>设置之前的数据，超出索引返回则无返回值</returns>
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
        /// 用数据填充整个数组
        /// </summary>
        /// <param name="value"></param>
        public void FillArray(T value)
        {
            AutoCSer.Common.Fill(array, value);
        }
        /// <summary>
        /// 用数据填充数组指定位置
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">填充数据数量</param>
        /// <returns>超出索引范围则返回 false</returns>
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
        /// 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
        /// </summary>
        /// <param name="value"></param>
        /// <returns>失败返回负数</returns>
        public int IndexOfArray(T value)
        {
            return Array.IndexOf(array, value);
        }
        /// <summary>
        /// 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">查找匹配数据数量</param>
        /// <returns>失败返回负数</returns>
        public int IndexOf(T value, int startIndex, int count)
        {
            if (startIndex >= 0 && count > 0 && (uint)(startIndex + count) <= (uint)array.Length) return Array.IndexOf(array, value, startIndex, count);
            return -1;
        }
        /// <summary>
        /// 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
        /// </summary>
        /// <param name="value"></param>
        /// <returns>失败返回负数</returns>
        public int LastIndexOfArray(T value)
        {
            return Array.LastIndexOf(array, value);
        }
        /// <summary>
        /// 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startIndex">最后一个匹配位置（起始位置）</param>
        /// <param name="count">查找匹配数据数量</param>
        /// <returns>失败返回负数</returns>
        public int LastIndexOf(T value, int startIndex, int count)
        {
            if (startIndex >= 0 && count > 0 && startIndex - count >= -1) return Array.LastIndexOf(array, value, startIndex, count);
            return -1;
        }
        /// <summary>
        /// 反转整个数组数据
        /// </summary>
        public void ReverseArray()
        {
            Array.Reverse(array);
        }
        /// <summary>
        /// 反转指定位置数组数据
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">反转数据数量</param>
        /// <returns>超出索引范围则返回 false</returns>
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
        /// 数组排序
        /// </summary>
        public void SortArray()
        {
            Array.Sort(array);
        }
        /// <summary>
        /// 排序指定位置数组数据
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>超出索引范围则返回 false</returns>
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
