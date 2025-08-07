using System;

namespace AutoCSer.Net.CommandServer.RemoteExpression
{
    /// <summary>
    /// 远程表达式节点序列化头部默认值
    /// </summary>
    internal enum NodeHeaderEnum
    {
        /// <summary>
        /// 类型编号
        /// </summary>
        TypeIndex = 0x100,
        /// <summary>
        /// 类型信息
        /// </summary>
        Type = 0x200,
        /// <summary>
        /// 方法编号
        /// </summary>
        MethodIndex = 0x400,
        /// <summary>
        /// 方法信息
        /// </summary>
        Method = 0x800,
        /// <summary>
        /// 属性编号
        /// </summary>
        PropertyIndex = 0x1000,
        /// <summary>
        /// 属性信息
        /// </summary>
        Property = 0x2000,
        /// <summary>
        /// 字段编号
        /// </summary>
        FieldIndex = 0x4000,
        /// <summary>
        /// 字段信息
        /// </summary>
        Field = 0x8000,
        /// <summary>
        /// null 值
        /// </summary>
        NullValue = 0x10000,
        /// <summary>
        /// 非枚举类型常量值
        /// </summary>
        ConstantNotEnum = 0x20000,
        /// <summary>
        /// 是否数组类型
        /// </summary>
        IsArray = 0x40000,
    }
}
