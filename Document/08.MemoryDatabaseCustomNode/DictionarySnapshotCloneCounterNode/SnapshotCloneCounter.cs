using System;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer.Document.MemoryDatabaseCustomNode
{
    /// <summary>
    /// Reference type counter object (snapshot data that supports manual triggering of clone operations for snapshot data objects in the persistence API)
    /// 引用类型计数器对象（支持快照数据对象在持久化 API 中手动触发克隆操作的快照数据）
    /// </summary>
    /// <typeparam name="T">Keyword type
    /// 关键字类型</typeparam>
    [AutoCSer.BinarySerialize(IsJsonMix = true)]
    public sealed class SnapshotCloneCounter<T> : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotCloneObject<SnapshotCloneCounter<T>>
    {
        /// <summary>
        /// Parameterless construction, used for reflecting to generate objects
        /// 无参构造，用于反射生成对象
        /// </summary>
        private SnapshotCloneCounter() { }
        /// <summary>
        /// Reference type counter object
        /// 引用类型计数器对象
        /// </summary>
        /// <param name="key">keyword</param>
        public SnapshotCloneCounter(T key)
        {
            Key = key;
            Count = 1;
        }
        /// <summary>
        /// Keyword
        /// </summary>
        [AllowNull]
        public T Key { get; private set; }
        /// <summary>
        /// Current count
        /// 当前计数
        /// </summary>
        public long Count { get; private set; }
        /// <summary>
        /// Count +1
        /// 计数 +1
        /// </summary>
        public void Increment()
        {
            //This operation involves both value retrieval and assignment operations simultaneously, which is equivalent to Count = Count + 1;
            //该操作同时存在取值与赋值操作，相当于 Count = Count + 1;
            ++Count;
        }
    }
}
