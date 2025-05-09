using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 性能测试二叉搜索树字典节点
    /// </summary>
    internal sealed class PerformanceSearchTreeDictionaryNode : SearchTreeDictionaryNode<int, int, PerformanceKeyValue>, IPerformanceSearchTreeDictionaryNode, ISnapshot<PerformanceKeyValue>
    {
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据信息</returns>
        SnapshotResult<PerformanceKeyValue> ISnapshot<PerformanceKeyValue>.GetSnapshotResult(PerformanceKeyValue[] snapshotArray, object customObject)
        {
            SnapshotResult<PerformanceKeyValue> result = new SnapshotResult<PerformanceKeyValue>(dictionary.Count, snapshotArray.Length);
            foreach (KeyValue<int, int> keyValue in dictionary.KeyValues) result.Add(snapshotArray, new PerformanceKeyValue(keyValue));
            return result;
        }
        /// <summary>
        /// 快照添加数据
        /// </summary>
        /// <param name="value"></param>
        public void SnapshotAdd(PerformanceKeyValue value)
        {
            dictionary[value.Key] = value.Value;
        }
        /// <summary>
        /// 根据节点位置获取数据
        /// </summary>
        /// <param name="index">节点位置</param>
        /// <returns>数据</returns>
        public ValueResult<PerformanceKeyValue> TryGetKeyValueByIndex(int index)
        {
            if ((uint)index < (uint)dictionary.Count) return new PerformanceKeyValue(dictionary.At(index));
            return default(ValueResult<PerformanceKeyValue>);
        }
        /// <summary>
        /// 获取第一组数据
        /// </summary>
        /// <returns>第一组数据</returns>
        public ValueResult<PerformanceKeyValue> TryGetFirstKeyValue()
        {
            if (dictionary.Count != 0) return new PerformanceKeyValue(dictionary.FristKeyValue);
            return default(ValueResult<PerformanceKeyValue>);
        }
        /// <summary>
        /// 获取最后一组数据
        /// </summary>
        /// <returns>最后一组数据</returns>
        public ValueResult<PerformanceKeyValue> TryGetLastKeyValue()
        {
            if (dictionary.Count != 0) return new PerformanceKeyValue(dictionary.LastKeyValue);
            return default(ValueResult<PerformanceKeyValue>);
        }
    }
}
