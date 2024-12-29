using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 支持快照数据对象在持久化 API 中手动触发克隆操作的快照数据
    /// </summary>
    /// <typeparam name="T">快照对象类型</typeparam>
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
        /// 创建快照对象，默认为 MemberwiseClone 浅克隆，复杂对象需自行定义克隆行为
        /// </summary>
        /// <returns></returns>
        public virtual T SnapshotClone()
        {
            return (T)this.MemberwiseClone();
        }
        /// <summary>
        /// 读取对象成员数据或者修改对象成员数据之前检查快照对象
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void CheckSnapshotValue()
        {
            if (object.ReferenceEquals(SnapshotValue, this)) checkSnapshotValue();
        }
        /// <summary>
        /// 读取对象成员数据或者修改对象成员数据之前检查快照对象
        /// </summary>
        private void checkSnapshotValue()
        {
            Monitor.Enter(this);
            if (object.ReferenceEquals(SnapshotValue, this))
            {
                try
                {
                    SnapshotValue = SnapshotClone();
                }
                finally { Monitor.Exit(this); }
            }
            else Monitor.Exit(this);
        }
        /// <summary>
        /// 取消快照操作
        /// </summary>
        public void CancelSnapshotValue()
        {
            if (SnapshotValue != null)
            {
                Monitor.Enter(this);
                SnapshotValue = null;
                Monitor.Exit(this);
            }
        }

        /// <summary>
        /// 获取快照数据信息
        /// </summary>
        /// <param name="snapshotArray">预申请快照数据容器</param>
        /// <param name="values">快照数据集合</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static SnapshotResult<T> GetSnapshotResult(T[] snapshotArray, ICollection<CheckSnapshotCloneObject<T>> values)
        {
            return GetSnapshotResult(snapshotArray, values, values.Count);
        }
        /// <summary>
        /// 获取快照数据信息
        /// </summary>
        /// <param name="snapshotArray">预申请快照数据容器</param>
        /// <param name="values">快照数据集合</param>
        /// <param name="count">快照数据总数</param>
        /// <returns></returns>
        public static SnapshotResult<T> GetSnapshotResult(T[] snapshotArray, IEnumerable<CheckSnapshotCloneObject<T>> values, int count)
        {
            SnapshotResult<T> result = new SnapshotResult<T>(count, snapshotArray.Length);
            foreach (CheckSnapshotCloneObject<T> value in values) result.Add(snapshotArray, value.NoCheckNotNull());
            return result;
        }
    }
}
