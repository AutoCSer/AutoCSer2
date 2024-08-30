using System;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 检测节点回合状态
    /// </summary>
    internal enum TreeBuilderCheckRoundTypeEnum
    {
        /// <summary>
        /// 节点回合成功
        /// </summary>
        Ok,
        /// <summary>
        /// 缺少回合节点
        /// </summary>
        LessRound,
        /// <summary>
        /// 未知的回合节点
        /// </summary>
        UnknownRound,
    }
}
