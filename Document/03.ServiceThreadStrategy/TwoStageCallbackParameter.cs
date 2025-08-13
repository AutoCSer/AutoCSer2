using System;

namespace AutoCSer.Document.ServiceThreadStrategy
{
    /// <summary>
    /// The first-stage callback parameters of the two-stage callback test
    /// 二阶段回调测试的第一阶段回调参数
    /// </summary>
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    public struct TwoStageCallbackParameter
    {
        /// <summary>
        /// Initial return value
        /// 起始返回值
        /// </summary>
        public int Start;
        /// <summary>
        /// The number of return values
        /// 返回值数量
        /// </summary>
        public int Count;
    }
}
