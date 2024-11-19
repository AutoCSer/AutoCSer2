using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 快照克隆对象
    /// </summary>
    /// <typeparam name="T">目标对象类型</typeparam>
    public abstract class SnapshotCloneObject<T> where T : SnapshotCloneObject<T>
    {
        /// <summary>
        /// 快照对象
        /// </summary>
        [AutoCSer.BinarySerializeMember(IsIgnoreCurrent = true)]
        [AutoCSer.JsonSerializeMember(IsIgnoreCurrent = true)]
#if NetStandard21
        internal T? SnapshotValue;
#else
        internal T SnapshotValue;
#endif
        /// <summary>
        /// 持久化获取快照对象
        /// </summary>
        /// <returns>快照对象</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal T GetSnapshotValue()
        {
            return Interlocked.Exchange(ref SnapshotValue, null).notNull();
        }
        /// <summary>
        /// 创建快照对象，默认为 MemberwiseClone 浅克隆，复杂对象需自行定义克隆行为
        /// </summary>
        /// <returns></returns>
        public virtual T SnapshotClone()
        {
            return (T)this.MemberwiseClone();
        }
        /// <summary>
        /// 修改对象之前检查快照对象
        /// </summary>
        public void CheckSnapshotValue()
        {
            if (object.ReferenceEquals(SnapshotValue, this) && Interlocked.Exchange(ref SnapshotValue, SnapshotClone()) == null)
            {
                SnapshotValue = null;
            }
        }
    }
}
