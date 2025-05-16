using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 接口方法编号配置
    /// </summary>
    public abstract class InterfaceMethodIndexAttribute : Attribute
    {
        /// <summary>
        /// 默认为 true 表示自动分配未知方法编号，否则产生匹配错误
        /// </summary>
        public bool IsAutoMethodIndex = true;
        /// <summary>
        /// 默认为 false 表示不生成调用方法参数创建工具，如果需要手动调用触发持久化操作则需要指定该参数为 true
        /// </summary>
        public bool IsMethodParameterCreator;
    }
}
