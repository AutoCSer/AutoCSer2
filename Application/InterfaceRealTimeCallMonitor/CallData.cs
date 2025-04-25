using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.InterfaceRealTimeCallMonitor
{
    /// <summary>
    /// 实时调用信息序列化数据
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.BinarySerialize(IsSerialize = false)]
#endif
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    public partial class CallData
    {
        /// <summary>
        /// 调用接口类型
        /// </summary>
        public string CallType;
        /// <summary>
        /// 调用接口方法名称
        /// </summary>
        public string CallName;
        /// <summary>
        /// 开始调用时间戳
        /// </summary>
        public long StartTimestamp;
        /// <summary>
        /// 调用超时时间戳
        /// </summary>
        public long TimeoutTimestamp;
        /// <summary>
        /// 调用完成时间戳
        /// </summary>
        public long CompletedTimestamp;
        /// <summary>
        /// 最后一次设置自定义调用步骤时间戳
        /// </summary>
        public long StepTimestamp;
        /// <summary>
        /// 最后一次设置的自定义调用步骤
        /// </summary>
        public int Step;
        /// <summary>
        /// 调用类型
        /// </summary>
        public ushort Type;
        /// <summary>
        /// 接口是否执行异常
        /// </summary>
        public bool IsException;
        /// <summary>
        /// 是否已经触发超时操作
        /// </summary>
        [AutoCSer.BinarySerializeMember(IsIgnoreCurrent = true)]
        private bool isTimeout;
        /// <summary>
        /// 实时调用信息序列化数据
        /// </summary>
        private CallData()
        {
#if NetStandard21
            CallType = CallName = string.Empty;
#endif
        }
        /// <summary>
        /// 实时调用信息
        /// </summary>
        /// <param name="callType">调用接口类型</param>
        /// <param name="callName">调用接口方法名称</param>
        /// <param name="timeoutMilliseconds">调用超时毫秒数</param>
        /// <param name="type">调用类型</param>
        internal CallData(string callType, string callName, int timeoutMilliseconds, ushort type)
        {
            CallType = callType;
            CallName = callName;
            Type = type;
            TimeoutTimestamp = AutoCSer.Date.GetTimestampByMilliseconds(timeoutMilliseconds);
            StartTimestamp = Stopwatch.GetTimestamp();
            TimeoutTimestamp += StartTimestamp;
        }
        /// <summary>
        /// 调用完成
        /// </summary>
        /// <param name="isException">接口是否执行异常</param>
        /// <returns>是否需要新增超时调用</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool Completed(bool isException)
        {
            IsException = isException;
            return checkTimeout(CompletedTimestamp = Stopwatch.GetTimestamp());
        }
        /// <summary>
        /// 检查是否需要新增超时调用
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private bool checkTimeout(long timestamp)
        {
            if (!isTimeout) return isTimeout = timestamp > TimeoutTimestamp;
            return false;
        }
        /// <summary>
        /// 设置自定义调用步骤
        /// </summary>
        /// <param name="step">自定义调用步骤</param>
        /// <returns>是否需要新增超时调用</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool SetStep(int step)
        {
            Step = step;
            return checkTimeout(StepTimestamp = Stopwatch.GetTimestamp());
        }
        /// <summary>
        /// 检查调用是否超时
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool CheckTimeout(long timestamp)
        {
            return isTimeout || timestamp > TimeoutTimestamp;
        }
        /// <summary>
        /// 检查是否需要新增超时调用
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool CheckTimeout()
        {
            if (!isTimeout) return isTimeout = true;
            return false;
        }
    }
}
