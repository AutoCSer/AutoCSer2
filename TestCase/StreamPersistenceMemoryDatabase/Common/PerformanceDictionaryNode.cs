using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 性能测试字典节点
    /// </summary>
    internal sealed class PerformanceDictionaryNode : DictionaryNode<int, int, PerformanceKeyValue>, IPerformanceDictionaryNode, ISnapshot<PerformanceKeyValue>
    {
        /// <summary>
        /// 字典节点
        /// </summary>
        /// <param name="capacity">容器初始化大小</param>
        /// <param name="groupType">可重用字典重组操作类型</param>
        public PerformanceDictionaryNode(int capacity = 0, ReusableDictionaryGroupTypeEnum groupType = ReusableDictionaryGroupTypeEnum.HashIndex) : base(capacity, groupType) { }
        /// <summary>
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据集合容器大小</returns>
        int ISnapshot<PerformanceKeyValue>.GetSnapshotCapacity(ref object customObject) { return dictionary.Count; }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据信息</returns>
        SnapshotResult<PerformanceKeyValue> ISnapshot<PerformanceKeyValue>.GetSnapshotResult(PerformanceKeyValue[] snapshotArray, object customObject)
        {
            SnapshotResult<PerformanceKeyValue> result = new SnapshotResult<PerformanceKeyValue>(dictionary.Count, snapshotArray.Length);
            foreach (BinarySerializeKeyValue<int, int> keyValue in dictionary.KeyValues) result.Add(snapshotArray, new PerformanceKeyValue(keyValue));
            return result;
        }
        /// <summary>
        /// 持久化之前重组快照数据
        /// </summary>
        /// <param name="array">预申请快照容器数组</param>
        /// <param name="newArray">超预申请快照数据</param>
        void ISnapshot<PerformanceKeyValue>.SetSnapshotResult(ref LeftArray<PerformanceKeyValue> array, ref LeftArray<PerformanceKeyValue> newArray) { }
        /// <summary>
        /// 快照添加数据
        /// </summary>
        /// <param name="value"></param>
        public void SnapshotAdd(PerformanceKeyValue value)
        {
            dictionary[value.Key] = value.Value;
        }
    }
}
