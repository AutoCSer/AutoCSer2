using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AutoCSer.Extensions;

namespace AutoCSer.CommandService.InterfaceRealTimeCallMonitor
{
    /// <summary>
    /// 异常调用统计信息节点
    /// </summary>
    public sealed class ExceptionStatisticsNode : MethodParameterCreatorNode<IExceptionStatisticsNode>, IExceptionStatisticsNode, IEnumerableSnapshot<LeftArray<string>>, IEnumerableSnapshot<BinarySerializeKeyValue<long, ExceptionStatistics>>
    {
        /// <summary>
        /// 节点自动移除时间
        /// </summary>
        private readonly DateTime removeTime;
        /// <summary>
        /// 字符串缓存集合
        /// </summary>
        private readonly Dictionary<string, int> strings;
        /// <summary>
        /// 字符串数组
        /// </summary>
        private LeftArray<string> stringArray;
        /// <summary>
        /// Snapshot collection
        /// 快照集合
        /// </summary>
        ISnapshotEnumerable<LeftArray<string>> IEnumerableSnapshot<LeftArray<string>>.SnapshotEnumerable { get { return new SnapshotGetValue<LeftArray<string>>(getStringArray); } }
        /// <summary>
        /// 异常统计信息集合
        /// </summary>
        private readonly SnapshotDictionary<long, ExceptionStatistics> statistics;
        /// <summary>
        /// Snapshot collection
        /// 快照集合
        /// </summary>
        ISnapshotEnumerable<BinarySerializeKeyValue<long, ExceptionStatistics>> IEnumerableSnapshot<BinarySerializeKeyValue<long, ExceptionStatistics>>.SnapshotEnumerable { get { return statistics.BinarySerializeKeyValueSnapshot; } }
        /// <summary>
        /// 异常调用总次数
        /// </summary>
        private long count;
        /// <summary>
        /// 保存调用时间数量
        /// </summary>
        internal readonly int CallTimeCount;
        /// <summary>
        /// 是否已经发起移除调用
        /// </summary>
        private bool isRemove;
        /// <summary>
        /// 异常调用统计信息节点
        /// </summary>
        /// <param name="removeTime">节点自动移除时间</param>
        /// <param name="callTimeCount">保存调用时间数量</param>
        public ExceptionStatisticsNode(DateTime removeTime, int callTimeCount)
        {
            this.removeTime = removeTime;
            CallTimeCount = Math.Max(CallTimeCount, 0);
            stringArray.SetEmpty();
            strings = DictionaryCreator<string>.Create<int>();
            statistics = new SnapshotDictionary<long, ExceptionStatistics>();
        }
        /// <summary>
        /// Initialization loading is completed and processed
        /// 初始化加载完毕处理
        /// </summary>
        /// <returns>The new node that has been loaded and replaced
        /// 加载完毕替换的新节点</returns>
#if NetStandard21
        public override IExceptionStatisticsNode? StreamPersistenceMemoryDatabaseServiceLoaded()
#else
        public override IExceptionStatisticsNode StreamPersistenceMemoryDatabaseServiceLoaded()
#endif
        {
            checkRemoveTime();
            return null;
        }
        /// <summary>
        /// 检查节点自动移除时间
        /// </summary>
        /// <returns></returns>
        private bool checkRemoveTime()
        {
            if (AutoCSer.Threading.SecondTimer.UtcNow >= removeTime)
            {
                if (!isRemove)
                {
                    isRemove = true;
                    StreamPersistenceMemoryDatabaseMethodParameterCreator.RemoveNode();
                }
                return false;
            }
            return true;
        }
        /// <summary>
        /// Remove the current node
        /// 移除当前节点
        /// </summary>
        public void RemoveNode()
        {
            statistics.ClearCount();
            stringArray.SetEmpty();
            StreamPersistenceMemoryDatabaseService.RemoveNode(StreamPersistenceMemoryDatabaseNode.Index);
        }
        /// <summary>
        /// 获取字符串数组
        /// </summary>
        /// <returns></returns>
        private LeftArray<string> getStringArray()
        {
            checkRemoveTime();
            return stringArray;
        }
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="stringArray">String cache
        /// 字符串缓存</param>
        public void SnapshotSetStringArray(LeftArray<string> stringArray)
        {
            this.stringArray = stringArray;
            foreach(string value in stringArray) strings.Add(value, strings.Count);
        }
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        public void SnapshotSet(BinarySerializeKeyValue<long, ExceptionStatistics> value)
        {
            statistics.Set(ref value);
        }
        /// <summary>
        ///  获取字符串缓存编号
        /// </summary>
        /// <param name="value"></param>
        /// <returns>缓存编号</returns>
        private int getStringIndex(string value)
        {
            int index;
            if (!strings.TryGetValue(value, out index))
            {
                stringArray.PrepLength(1);
                strings.Add(value, index = stringArray.Length);
                stringArray.Add(value);
            }
            return index;
        }
        /// <summary>
        ///  获取异常统计信息关键字
        /// </summary>
        /// <param name="callType">Call interface type
        /// 调用接口类型</param>
        /// <param name="callName">The name of the interface method to be called
        /// 调用接口方法名称</param>
        /// <returns>异常统计信息关键字</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private long getIndex(string callType, string callName)
        {
            return ((long)getStringIndex(callType) << 32) + getStringIndex(callName);
        }
        /// <summary>
        /// Get the total number of exception calls
        /// 获取异常调用总次数
        /// </summary>
        /// <returns></returns>
        public long GetCount()
        {
            return count;
        }
        /// <summary>
        /// Get the quantity of exception statistical information
        /// 获取异常统计信息数量
        /// </summary>
        /// <returns></returns>
        public int GetStatisticsCount()
        {
            return statistics.Count;
        }
        /// <summary>
        /// Add exception call time
        /// 添加异常调用时间
        /// </summary>
        /// <param name="callType">Call interface type
        /// 调用接口类型</param>
        /// <param name="callName">The name of the interface method to be called
        /// 调用接口方法名称</param>
        /// <param name="callTime">Exception call time
        /// 异常调用时间</param>
        public void Append(string callType, string callName, DateTime callTime)
        {
            if (callType != null && callName != null)
            {
                var count = default(ExceptionStatistics);
                long index = getIndex(callType, callName);
                if (statistics.TryGetValue(index, out count)) count.Append(callTime);
                else statistics.Set(index, new ExceptionStatistics(this, callTime));
                ++this.count;
            }
        }
        /// <summary>
        /// Remove exception statistics
        /// 移除异常统计信息
        /// </summary>
        /// <param name="callType">Call interface type
        /// 调用接口类型</param>
        /// <param name="callName">The name of the interface method to be called
        /// 调用接口方法名称</param>
        public void Remove(string callType, string callName)
        {
            if (callType != null && callName != null)
            {
                var count = default(ExceptionStatistics);
                if (statistics.Remove(getIndex(callType, callName), out count)) this.count -= count.Count;
            }
        }
        /// <summary>
        /// Get the statistical information of call exceptions
        /// 获取调用异常统计信息
        /// </summary>
        /// <param name="callType">Call interface type
        /// 调用接口类型</param>
        /// <param name="callName">The name of the interface method to be called
        /// 调用接口方法名称</param>
        /// <returns>Exception statistical information, failure returns null
        /// 异常统计信息，失败返回 null</returns>
#if NetStandard21
        public ExceptionStatistics? GetStatistics(string callType, string callName)
#else
        public ExceptionStatistics GetStatistics(string callType, string callName)
#endif
        {
            if (callType != null && callName != null)
            {
                var count = default(ExceptionStatistics);
                long index = getIndex(callType, callName);
                if (statistics.TryGetValue(index, out count)) return count;
            }
            return null;
        }
        /// <summary>
        /// Get the statistics of the specified number of call exceptions
        /// 获取指定数量调用异常统计信息
        /// </summary>
        /// <param name="count">The number of get the statistical information of call exceptions
        /// 获取调用异常统计信息数量</param>
        /// <param name="callback">The callback delegation for get the statistical information of the call exception
        /// 获取数量调用异常统计信息回调委托</param>
        public void GetManyStatistics(int count, MethodKeepCallback<CallExceptionStatistics> callback)
        {
            foreach (BinarySerializeKeyValue<long, ExceptionStatistics> exceptionStatistics in statistics.KeyValues)
            {
                if (!callback.Callback(new CallExceptionStatistics(stringArray[(int)(exceptionStatistics.Key >> 32)], stringArray[(int)exceptionStatistics.Key], exceptionStatistics.Value))) return;
                if (--count <= 0) return;
            }
            callback.CancelKeep();
        }
    }
}
