using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 字段扩展操作
    /// </summary>
    internal static class FieldInfoExtension
    {
        /// <summary>
        /// 获取匿名字段名称（如果是属性生成则转换为属性名称）
        /// </summary>
        /// <param name="field"></param>
        /// <param name="name">字段名称或者属性名称</param>
        /// <returns>是否属性</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool getAnonymousName(this FieldInfo field, out string name)
        {
            name = field.Name;
            if (name[0] != '<') return false;
            name = name.Substring(1, name.IndexOf('>') - 1);
            return true;
        }
    }
}
