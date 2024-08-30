using System;

namespace AutoCSer.CodeGenerator.NetCoreWebView
{
    /// <summary>
    /// HTML 模板指令类型
    /// </summary>
    internal enum TreeTemplateCommandEnum : byte
    {
        /// <summary>
        /// 客户端代码段
        /// </summary>
        CLINET,
        /// <summary>
        /// {{x}}取值代码
        /// </summary>
        GET,
        /// <summary>
        /// 子代码段
        /// </summary>
        PUSH,
        /// <summary>
        /// 循环
        /// </summary>
        LOOP,
        /// <summary>
        /// 绑定的数据为 true / 非0 / 非null 时输出代码
        /// </summary>
        IF,
        /// <summary>
        /// 绑定的数据为 false / 0 / null 时输出代码
        /// </summary>
        NOT,
    }
}
