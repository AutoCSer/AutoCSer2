using System;

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
        /// 清除指定位置数据 持久化参数检查
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">清除数据数量</param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<bool> ClearBeforePersistence(int startIndex, int count)
        {
            if (count != 0)
            {
                if (startIndex >= 0 && count > 0 && (uint)(startIndex + count) <= (uint)array.Length) return default(ValueResult<bool>);
                return false;
            }
            return (uint)startIndex <= (uint)array.Length;
        }
        /// <summary>
        /// 清除指定位置数据
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">清除数据数量</param>
        /// <returns>超出索引范围则返回 false</returns>
        public bool Clear(int startIndex, int count)
        {
            Array.Clear(array, startIndex, count);
            return true;
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
        /// 根据索引位置设置数据 持久化参数检查
        /// </summary>
        /// <param name="index">索引位置</param>
        /// <param name="value">数据</param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<bool> SetValueBeforePersistence(int index, T value)
        {
            if ((uint)index < (uint)array.Length) return default(ValueResult<bool>);
            return false;
        }
        /// <summary>
        /// 根据索引位置设置数据
        /// </summary>
        /// <param name="index">索引位置</param>
        /// <param name="value">数据</param>
        /// <returns>超出索引范围则返回 false</returns>
        public bool SetValue(int index, T value)
        {
            array[index] = value;
            return true;
        }
        /// <summary>
        /// 根据索引位置设置数据并返回设置之前的数据 持久化参数检查
        /// </summary>
        /// <param name="index">索引位置</param>
        /// <param name="value">数据</param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<ValueResult<T>> GetValueSetBeforePersistence(int index, T value)
        {
            if ((uint)index < (uint)array.Length) return default(ValueResult<ValueResult<T>>);
            return default(ValueResult<T>);
        }
        /// <summary>
        /// 根据索引位置设置数据并返回设置之前的数据
        /// </summary>
        /// <param name="index">索引位置</param>
        /// <param name="value">数据</param>
        /// <returns>设置之前的数据，超出索引返回则无返回值</returns>
        public ValueResult<T> GetValueSet(int index, T value)
        {
            T arrayValue = array[index];
            array[index] = value;
            return arrayValue;
        }
        /// <summary>
        /// 用数据填充整个数组
        /// </summary>
        /// <param name="value"></param>
        public void FillArray(T value)
        {
            AutoCSer.Common.Config.Fill(array, value);
        }
        /// <summary>
        /// 用数据填充数组指定位置 持久化参数检查
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">填充数据数量</param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<bool> FillBeforePersistence(T value, int startIndex, int count)
        {
            return ClearBeforePersistence(startIndex, count);
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
            AutoCSer.Common.Config.Fill(array, value, startIndex, count);
            return true;
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
        /// 反转指定位置数组数据 持久化参数检查
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">反转数据数量</param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<bool> ReverseBeforePersistence(int startIndex, int count)
        {
            return ClearBeforePersistence(startIndex, count);
        }
        /// <summary>
        /// 反转指定位置数组数据
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">反转数据数量</param>
        /// <returns>超出索引范围则返回 false</returns>
        public bool Reverse(int startIndex, int count)
        {
            Array.Reverse(array, startIndex, count);
            return true;
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        public void SortArray()
        {
            Array.Sort(array);
        }
        /// <summary>
        /// 排序指定位置数组数据 持久化参数检查
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<bool> SortBeforePersistence(int startIndex, int count)
        {
            return ClearBeforePersistence(startIndex, count);
        }
        /// <summary>
        /// 排序指定位置数组数据
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>超出索引范围则返回 false</returns>
        public bool Sort(int startIndex, int count)
        {
            Array.Sort(array, startIndex, count);
            return true;
        }
    }
}
