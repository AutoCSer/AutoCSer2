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
    public partial interface ILeftArrayNode<T>
    {
        /// <summary>
        /// 获取有效数组长度
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int GetLength();
        /// <summary>
        /// 获取数组容器初大小
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int GetCapacity();
        /// <summary>
        /// 获取容器空闲数量
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int GetFreeCount();
        /// <summary>
        /// 置空并释放数组
        /// </summary>
        void SetEmpty();
        /// <summary>
        /// 清除所有数据并将数据有效长度设置为 0
        /// </summary>
        void ClearLength();
        /// <summary>
        /// 清除指定位置数据
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">清除数据数量</param>
        /// <returns>超出索引范围则返回 false</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool Clear(int startIndex, int count);
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">数据</param>
        [ServerMethod(SnapshotMethodSort = 1, IsIgnorePersistenceCallbackException = true)]
        void Add(T value);
        /// <summary>
        /// 当有空闲位置时添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>如果数组已满则添加失败并返回 false</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool TryAdd(T value);
        /// <summary>
        /// 根据索引位置设置数据
        /// </summary>
        /// <param name="index">索引位置</param>
        /// <param name="value">数据</param>
        /// <returns>超出索引范围则返回 false</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool SetValue(int index, T value);
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="index">插入位置</param>
        /// <param name="value">数据</param>
        /// <returns>超出索引范围则返回 false</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool Insert(int index, T value);
        /// <summary>
        /// 根据索引位置获取数据
        /// </summary>
        /// <param name="index">索引位置</param>
        /// <returns>超出索引返回则无返回值</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<T> GetValue(int index);
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
        /// <summary>
        /// 移除第一个匹配数据（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否存在移除数据</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool Remove(T value);
        /// <summary>
        /// 移除指定索引位置数据
        /// </summary>
        /// <param name="index">数据位置</param>
        /// <returns>超出索引范围则返回 false</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool RemoveAt(int index);
        /// <summary>
        /// 移除指定索引位置数据并返回被移除的数据
        /// </summary>
        /// <param name="index">数据位置</param>
        /// <returns>超出索引范围则无数据返回</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        ValueResult<T> GetValueRemoveAt(int index);
        /// <summary>
        /// 移除指定索引位置数据并将最后一个数据移动到该指定位置
        /// </summary>
        /// <param name="index"></param>
        /// <returns>超出索引范围则返回 false</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool RemoveToEnd(int index);
        /// <summary>
        /// 移除指定索引位置数据，将最后一个数据移动到该指定位置，并返回被移除的数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns>超出索引范围则无数据返回</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        ValueResult<T> GetValueRemoveToEnd(int index);
        /// <summary>
        /// 移除最后一个数据并返回该数据
        /// </summary>
        /// <returns>没有可移除数据则无数据返回</returns>
        ValueResult<T> GetTryPopValue();
        /// <summary>
        /// 移除最后一个数据
        /// </summary>
        /// <returns>是否存在可移除数据</returns>
        bool TryPop();
    }
}
