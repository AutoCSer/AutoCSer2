using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 数组节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LeftArrayNode<T> : ILeftArrayNode<T>, ISnapshot<T>
    {
        /// <summary>
        /// 数组
        /// </summary>
        private LeftArray<T> array;
        /// <summary>
        /// 数组节点
        /// </summary>
        /// <param name="capacity">容器初始化大小</param>
        public LeftArrayNode(int capacity)
        {
            array = new LeftArray<T>(capacity);
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <returns>快照数据集合</returns>
        public LeftArray<T> GetSnapshotArray()
        {
            return new LeftArray<T>(array.GetArray());
        }
        /// <summary>
        /// 获取有效数组长度
        /// </summary>
        /// <returns></returns>
        public int GetLength()
        {
            return array.Length;
        }
        /// <summary>
        /// 获取数组容器初大小
        /// </summary>
        /// <returns></returns>
        public int GetCapacity()
        {
            return array.Array.Length;
        }
        /// <summary>
        /// 获取容器空闲数量
        /// </summary>
        /// <returns></returns>
        public int GetFreeCount()
        {
            return array.FreeCount;
        }
        /// <summary>
        /// 置空并释放数组
        /// </summary>
        public void SetEmpty()
        {
            array.SetEmpty();
        }
        /// <summary>
        /// 清除所有数据并将数据有效长度设置为 0
        /// </summary>
        public void ClearLength()
        {
            array.ClearLength();
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
            if ((uint)(startIndex + count) <= (uint)array.Length)
            {
                Array.Clear(array.Array, startIndex, count);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 添加数据 持久化参数检查
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>返回 true 表示需要继续调用持久化方法</returns>
        public bool AddBeforePersistence(T value)
        {
            array.PrepLength(1);
            return true;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">数据</param>
        public void Add(T value)
        {
            array.Add(value);
        }
        /// <summary>
        /// 当有空闲位置时添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>如果数组已满则添加失败并返回 false</returns>
        public bool TryAdd(T value)
        {
            return array.TryAdd(value);
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
            if ((uint)index < (uint)array.Length)
            {
                array.Array[index] = value;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 插入数据 持久化参数检查
        /// </summary>
        /// <param name="index">插入位置</param>
        /// <param name="value">数据</param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<bool> InsertBeforePersistence(int index, T value)
        {
            if ((uint)index < (uint)array.Length)
            {
                array.PrepLength(1);
                return default(ValueResult<bool>);
            }
            return false;
        }
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="index">插入位置</param>
        /// <param name="value">数据</param>
        /// <returns>超出索引范围则返回 false</returns>
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
        /// 根据索引位置获取数据
        /// </summary>
        /// <param name="index">索引位置</param>
        /// <returns>超出索引返回则无返回值</returns>
        public ValueResult<T> GetValue(int index)
        {
            if ((uint)index < (uint)array.Length) return array.Array[index];
            return default(ValueResult<T>);
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
            if ((uint)index < (uint)array.Length)
            {
                T arrayValue = array.Array[index];
                array.Array[index] = value;
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
            AutoCSer.Common.Config.Fill(array.Array, value, 0, array.Length);
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
            if ((uint)(startIndex + count) <= (uint)array.Length)
            {
                AutoCSer.Common.Config.Fill(array.Array, value, startIndex, count);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
        /// </summary>
        /// <param name="value"></param>
        /// <returns>失败返回负数</returns>
        public int IndexOfArray(T value)
        {
            return array.IndexOf(value);
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
            if (startIndex >= 0 && count > 0 && (uint)(startIndex + count) <= (uint)array.Length) return Array.IndexOf(array.Array, value, startIndex, count);
            return -1;
        }
        /// <summary>
        /// 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
        /// </summary>
        /// <param name="value"></param>
        /// <returns>失败返回负数</returns>
        public int LastIndexOfArray(T value)
        {
            return Array.LastIndexOf(array.Array, value, array.Length - 1, array.Length);
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
            if (startIndex >= 0 && count > 0 && startIndex - count >= -1) return Array.LastIndexOf(array.Array, value, startIndex, count);
            return -1;
        }
        /// <summary>
        /// 反转整个数组数据
        /// </summary>
        public void ReverseArray()
        {
            array.Reverse();
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
            if ((uint)(startIndex + count) <= (uint)array.Length)
            {
                Array.Reverse(array.Array, startIndex, count);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        public void SortArray()
        {
            Array.Sort(array.Array, 0, array.Length);
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
            if ((uint)(startIndex + count) <= (uint)array.Length)
            {
                Array.Sort(array.Array, startIndex, count);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 移除第一个匹配数据（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否存在移除数据</returns>
        public bool Remove(T value)
        {
            return array.Remove(value);
        }
        /// <summary>
        /// 移除指定索引位置数据 持久化参数检查
        /// </summary>
        /// <param name="index">数据位置</param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<bool> RemoveAtBeforePersistence(int index)
        {
            if ((uint)index < (uint)array.Length) return default(ValueResult<bool>);
            return false;
        }
        /// <summary>
        /// 移除指定索引位置数据
        /// </summary>
        /// <param name="index">数据位置</param>
        /// <returns>超出索引范围则返回 false</returns>
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
        /// 移除指定索引位置数据并返回被移除的数据 持久化参数检查
        /// </summary>
        /// <param name="index">数据位置</param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<ValueResult<T>> GetValueRemoveAtBeforePersistence(int index)
        {
            if ((uint)index < (uint)array.Length) return default(ValueResult<ValueResult<T>>);
            return default(ValueResult<T>);
        }
        /// <summary>
        /// 移除指定索引位置数据并返回被移除的数据
        /// </summary>
        /// <param name="index">数据位置</param>
        /// <returns>超出索引范围则无数据返回</returns>
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
        /// 移除指定索引位置数据并将最后一个数据移动到该指定位置 持久化参数检查
        /// </summary>
        /// <param name="index"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<bool> RemoveToEndBeforePersistence(int index)
        {
            if ((uint)index < (uint)array.Length) return default(ValueResult<bool>);
            return false;
        }
        /// <summary>
        /// 移除指定索引位置数据并将最后一个数据移动到该指定位置
        /// </summary>
        /// <param name="index"></param>
        /// <returns>超出索引范围则返回 false</returns>
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
        /// 移除指定索引位置数据，将最后一个数据移动到该指定位置，并返回被移除的数据 持久化参数检查
        /// </summary>
        /// <param name="index"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<ValueResult<T>> GetValueRemoveToEndBeforePersistence(int index)
        {
            if ((uint)index < (uint)array.Length) return default(ValueResult<ValueResult<T>>);
            return default(ValueResult<T>);
        }
        /// <summary>
        /// 移除指定索引位置数据，将最后一个数据移动到该指定位置，并返回被移除的数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns>超出索引范围则无数据返回</returns>
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
        /// 移除最后一个数据并返回该数据 持久化参数检查
        /// </summary>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<ValueResult<T>> GetTryPopValueBeforePersistence()
        {
            if (array.Length != 0) return default(ValueResult<ValueResult<T>>);
            return default(ValueResult<T>);
        }
        /// <summary>
        /// 移除最后一个数据并返回该数据
        /// </summary>
        /// <returns>没有可移除数据则无数据返回</returns>
        public ValueResult<T> GetTryPopValue()
        {
            var value = default(T);
            if (array.TryPop(out value)) return value;
            return default(ValueResult<T>);
        }
        /// <summary>
        /// 移除最后一个数据 持久化参数检查
        /// </summary>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<bool> TryPopBeforePersistence()
        {
            if (array.Length == 0) return false;
            return default(ValueResult<bool>);
        }
        /// <summary>
        /// 移除最后一个数据
        /// </summary>
        /// <returns>是否存在可移除数据</returns>
        public bool TryPop()
        {
            var value = default(T);
            return array.TryPop(out value);
        }
    }
}
