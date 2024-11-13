using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 字段扩展操作
    /// </summary>
    public static class FieldInfoExtension
    {
        /// <summary>
        /// 获取匿名字段名称（如果是属性生成则转换为属性名称）
        /// </summary>
        /// <param name="field"></param>
        /// <param name="name">字段名称或者属性名称</param>
        /// <returns>是否属性</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool getAnonymousName(this FieldInfo field, out string name)
        {
            name = field.Name;
            if (name[0] != '<') return false;
            name = name.Substring(1, name.IndexOf('>') - 1);
            return true;
        }
        /// <summary>
        /// 根据匿名字段获取对应属性
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
#if NetStandard21
        public static MemberInfo? getPropertyMemberInfo(this FieldInfo field)
#else
        public static MemberInfo getPropertyMemberInfo(this FieldInfo field)
#endif
        {
            string name;
            if (!getAnonymousName(field, out name)) return null;
            return field.DeclaringType.notNull().GetProperty(name, field.FieldType, EmptyArray<Type>.Array);
        }
    }
}
