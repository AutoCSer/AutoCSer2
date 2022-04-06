using System;

namespace AutoCSer.TestCase.ReverseLogCollectionCommon
{
    /// <summary>
    /// 测试日志数据定义
    /// </summary>
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    public class LogInfo
    {
        /// <summary>
        /// 日志发生时间
        /// </summary>
        public DateTime LogTime;
        /// <summary>
        /// 错误消息
        /// </summary>
        public string Message;

        /// <summary>
        /// 反向日志收集服务名称
        /// </summary>
        public const string ServiceName = "AutoCSer.TestCase.ReverseLogCollection";
    }
}
