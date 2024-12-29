using System;

namespace AutoCSer.Document.MemoryDatabaseCustomNode
{
    /// <summary>
    /// 带有 ID 的数据
    /// </summary>
    [AutoCSer.BinarySerialize(IsMixJsonSerialize = true)]//二进制混杂 JSON 序列化
    public sealed class IdentityEntity : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotCloneObject<IdentityEntity>
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Identity;
        /// <summary>
        /// 当前计数
        /// </summary>
        public long Count;
        /// <summary>
        /// 计数 +1
        /// </summary>
        public void Increment()
        {
            ++Count;//该操作同时存在取值与赋值操作，相当于 Count = Count + 1;
        }
    }
}
