using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 参数忽略初始化，允许随机初始化数据以降低反序列化开销
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct)]
    public sealed class IgnoreInitobjParameterAttribute : Attribute
    {
    }
}
