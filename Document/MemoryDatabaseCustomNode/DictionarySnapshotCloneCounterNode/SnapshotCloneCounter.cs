using System;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer.Document.MemoryDatabaseCustomNode
{
    /// <summary>
    /// 引用类型计数器对象（支持快照数据对象在持久化 API 中手动触发克隆操作的快照数据）
    /// </summary>
    /// <typeparam name="T">关键字类型</typeparam>
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false, IsAnonymousFields = true)]//属性支持二进制序列化需要设置 IsAnonymousFields 为 true 本质上是序列化匿名字段
    public sealed class SnapshotCloneCounter<T> : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotCloneObject<SnapshotCloneCounter<T>>
    {
        /// <summary>
        /// 无参构造，用户反射生成对象
        /// </summary>
        private SnapshotCloneCounter() { }
        /// <summary>
        /// 引用类型计数器对象
        /// </summary>
        /// <param name="key">关键字</param>
        public SnapshotCloneCounter(T key)
        {
            Key = key;
            Count = 1;
        }
        /// <summary>
        /// 关键字
        /// </summary>
        [AllowNull]
        public T Key { get; private set; }
        /// <summary>
        /// 当前计数
        /// </summary>
        public long Count { get; private set; }
        /// <summary>
        /// 计数 +1
        /// </summary>
        public void Increment()
        {
            ++Count;//该操作同时存在取值与赋值操作，相当于 Count = Count + 1;
        }
    }
}
