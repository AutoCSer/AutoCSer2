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
    public sealed class ExceptionStatisticsNode : MethodParameterCreatorNode<IExceptionStatisticsNode, BinarySerializeKeyValue<long, ExceptionStatistics>>, IExceptionStatisticsNode, ISnapshot<LeftArray<string>>, ISnapshot<BinarySerializeKeyValue<long, ExceptionStatistics>>
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
        /// 异常统计信息集合
        /// </summary>
        private readonly Dictionary<long, ExceptionStatistics> statistics;
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
            strings = DictionaryCreator.CreateAny<string, int>();
            statistics = DictionaryCreator.CreateLong<ExceptionStatistics>();
        }
        /// <summary>
        /// 初始化加载完毕处理
        /// </summary>
        /// <returns>加载完毕替换的新节点</returns>
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
        /// 移除当前节点
        /// </summary>
        public void RemoveNode()
        {
            StreamPersistenceMemoryDatabaseService.RemoveNode(StreamPersistenceMemoryDatabaseNode.Index);
        }
        /// <summary>
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据集合容器大小</returns>
        int ISnapshot<LeftArray<string>>.GetSnapshotCapacity(ref object customObject)
        {
            return checkRemoveTime() ? 1 : 0;
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据信息</returns>
        SnapshotResult<LeftArray<string>> ISnapshot<LeftArray<string>>.GetSnapshotResult(LeftArray<string>[] snapshotArray, object customObject)
        {
            if (!isRemove) snapshotArray[0] = stringArray;
            return new SnapshotResult<LeftArray<string>>(snapshotArray.Length);
        }
        /// <summary>
        /// 持久化之前重组快照数据
        /// </summary>
        /// <param name="array">预申请快照容器数组</param>
        /// <param name="newArray">超预申请快照数据</param>
        void ISnapshot<LeftArray<string>>.SetSnapshotResult(ref LeftArray<LeftArray<string>> array, ref LeftArray<LeftArray<string>> newArray) { }
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="stringArray">数据</param>
        public void SnapshotSetStringArray(LeftArray<string> stringArray)
        {
            this.stringArray = stringArray;
            foreach(string value in stringArray) strings.Add(value, strings.Count);
        }
        /// <summary>
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据集合容器大小</returns>
        int ISnapshot<BinarySerializeKeyValue<long, ExceptionStatistics>>.GetSnapshotCapacity(ref object customObject)
        {
            return checkRemoveTime() ? statistics.Count : 0;
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据信息</returns>
        SnapshotResult<BinarySerializeKeyValue<long, ExceptionStatistics>> ISnapshot<BinarySerializeKeyValue<long, ExceptionStatistics>>.GetSnapshotResult(BinarySerializeKeyValue<long, ExceptionStatistics>[] snapshotArray, object customObject)
        {
            if (!isRemove) return ServerNode.GetSnapshotResult(statistics, snapshotArray);
            return new SnapshotResult<BinarySerializeKeyValue<long, ExceptionStatistics>>(0);
        }
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        public void SnapshotSet(BinarySerializeKeyValue<long, ExceptionStatistics> value)
        {
            statistics.Add(value.Key, value.Value);
        }
        /// <summary>
        ///  获取字符串缓存编号
        /// </summary>
        /// <param name="value">字符串</param>
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
        /// <param name="callType">调用接口类型</param>
        /// <param name="callName">调用接口方法名称</param>
        /// <returns>异常统计信息关键字</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private long getIndex(string callType, string callName)
        {
            return ((long)getStringIndex(callType) << 32) + getStringIndex(callName);
        }
        /// <summary>
        /// 获取异常调用总次数
        /// </summary>
        /// <returns></returns>
        public long GetCount()
        {
            return count;
        }
        /// <summary>
        /// 获取异常统计信息数量
        /// </summary>
        /// <returns></returns>
        public int GetStatisticsCount()
        {
            return statistics.Count;
        }
        /// <summary>
        /// 添加异常调用时间
        /// </summary>
        /// <param name="callType">调用接口类型</param>
        /// <param name="callName">调用接口方法名称</param>
        /// <param name="callTime">异常调用时间</param>
        public void Append(string callType, string callName, DateTime callTime)
        {
            if (callType != null && callName != null)
            {
                var count = default(ExceptionStatistics);
                long index = getIndex(callType, callName);
                if (statistics.TryGetValue(index, out count)) count.Append(callTime);
                else statistics.Add(index, new ExceptionStatistics(this, callTime));
                ++this.count;
            }
        }
        /// <summary>
        /// 移除异常统计信息
        /// </summary>
        /// <param name="callType">调用接口类型</param>
        /// <param name="callName">调用接口方法名称</param>
        public void Remove(string callType, string callName)
        {
            if (callType != null && callName != null)
            {
                var count = default(ExceptionStatistics);
                if (statistics.Remove(getIndex(callType, callName), out count)) this.count -= count.Count;
            }
        }
        /// <summary>
        /// 获取调用异常统计信息
        /// </summary>
        /// <param name="callType">调用接口类型</param>
        /// <param name="callName">调用接口方法名称</param>
        /// <returns>异常统计信息，失败返回 null</returns>
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
        /// 获取指定数量调用异常统计信息
        /// </summary>
        /// <param name="count">获取调用异常统计信息数量</param>
        /// <param name="callback">获取数量调用异常统计信息回调委托</param>
        public void GetManyStatistics(int count, MethodKeepCallback<CallExceptionStatistics> callback)
        {
            foreach (KeyValuePair<long, ExceptionStatistics> exceptionStatistics in statistics)
            {
                if (!callback.Callback(new CallExceptionStatistics(stringArray[(int)(exceptionStatistics.Key >> 32)], stringArray[(int)exceptionStatistics.Key], exceptionStatistics.Value))) return;
                if (--count <= 0) return;
            }
            callback.CancelKeep();
        }
    }
}
