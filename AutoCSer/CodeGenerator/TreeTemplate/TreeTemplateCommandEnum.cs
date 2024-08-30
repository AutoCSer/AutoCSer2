using System;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 模板命令类型
    /// </summary>
    internal enum TreeTemplateCommandEnum : byte
    {
        /// <summary>
        /// 输出绑定的数据
        /// </summary>
        AT,
        /// <summary>
        /// 子代码段
        /// </summary>
        PUSH,
        /// <summary>
        /// 循环
        /// </summary>
        LOOP,
        /// <summary>
        /// 循环=LOOP
        /// </summary>
        FOR,
        /// <summary>
        /// 屏蔽代码段输出
        /// </summary>
        NOTE,
        /// <summary>
        /// 绑定的数据为true非0非null时输出代码
        /// </summary>
        IF,
        /// <summary>
        /// 绑定的数据为false或者0或者null时输出代码
        /// </summary>
        NOT,
        /// <summary>
        /// 用于标识一个子段模板，可以被别的模板引用
        /// </summary>
        NAME,
        /// <summary>
        /// 引用NAME标识一个子段模板
        /// </summary>
        FROMNAME,
        /// <summary>
        /// 用于标识一个子段程序代码，用于代码的分类输出
        /// </summary>
        PART,
    }
}
