using AutoCSer.Extensions;
using System;
using System.Reflection;

namespace AutoCSer.Configuration
{
    /// <summary>
    /// 字段配置创建
    /// </summary>
    internal sealed class FieldCreator : Creator
    {
        /// <summary>
        /// 目标字段
        /// </summary>
        private readonly FieldInfo field;
        /// <summary>
        /// 字段配置创建
        /// </summary>
        /// <param name="field">目标字段</param>
        internal FieldCreator(FieldInfo field)
        {
            this.field = field;
        }
        /// <summary>
        /// 创建配置对象
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        internal override ConfigObject? Create()
#else
        internal override ConfigObject Create()
#endif
        {
            return field.GetValue(null).castClass<ConfigObject>();
        }
    }
}
