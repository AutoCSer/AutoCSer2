using System;

namespace AutoCSer.Document.MemoryDatabaseCustomNode
{
    /// <summary>
    /// Test data with identity
    /// 带有 ID 的测试数据
    /// </summary>
    [AutoCSer.BinarySerialize(IsJsonMix = true)]
    public sealed class IdentityEntity : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotCloneObject<IdentityEntity>
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Identity;
        /// <summary>
        /// Current count
        /// 当前计数
        /// </summary>
        public long Count;
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
