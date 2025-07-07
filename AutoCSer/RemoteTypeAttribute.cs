using System;

namespace AutoCSer
{
    /// <summary>
    /// Legal remote type tagging (effective when assemblies are configured)
    /// 合法远程类型标记（程序集在配置中时生效）
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class)]
    public sealed class RemoteTypeAttribute : Attribute
    {
    }
}
