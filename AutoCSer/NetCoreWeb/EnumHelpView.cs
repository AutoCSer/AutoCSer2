using AutoCSer.Extensions;
using System;
using System.Reflection;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// 枚举值类型帮助文档视图数据
    /// </summary>
    public struct EnumHelpView
    {
        /// <summary>
        /// 枚举值名称
        /// </summary>
        public readonly string Name;
        /// <summary>
        /// 枚举值数值
        /// </summary>
        public readonly ulong Value;
        /// <summary>
        /// 枚举值类型帮助文档视图数据
        /// </summary>
        /// <param name="field">枚举字段信息</param>
        internal EnumHelpView(FieldInfo field)
        {
            Name = field.Name;
            Value = ((IConvertible)field.GetValue(null).notNull()).ToUInt64(null);
        }
    }
}
