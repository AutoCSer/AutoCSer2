using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// Interface method number configuration
    /// 接口方法编号配置
    /// </summary>
    public abstract class InterfaceMethodIndexAttribute : Attribute
    {
        /// <summary>
        /// By default, false indicates that the runtime does not automatically assign unknown method numbers (excluding the code generation matching logic), and a matching error is generated when encountering an unknown method number
        /// 默认为 false 表示运行时不自动分配未知方法编号（不包括代码生成匹配逻辑），遇到未知方法编号则产生匹配错误
        /// </summary>
        public bool IsAutoMethodIndex;
        /// <summary>
        /// By default, false indicates that no tool for creating the parameters of the calling method is generated. If you need to manually call to trigger a persistence operation, you need to specify this parameter as true
        /// 默认为 false 表示不生成调用方法参数创建工具，如果需要手动调用触发持久化操作则需要指定该参数为 true
        /// </summary>
        public bool IsMethodParameterCreator;
    }
}
