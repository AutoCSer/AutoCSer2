using AutoCSer.Extensions;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace AutoCSer.ORM.Reflection.Emit
{
    /// <summary>
    /// 字段操作
    /// </summary>
    internal static class Field
    {
        /// <summary>
        /// 创建获取字段委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="FT"></typeparam>
        /// <param name="field"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        internal static Func<T, FT> UnsafeGetField<T, FT>(FieldInfo field, string methodName) where T : class
        {
            DynamicMethod dynamicMethod = new DynamicMethod(methodName, typeof(FT), new Type[] { typeof(T) }, typeof(T), true);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldfld, field);
            generator.ret();
            return (Func<T, FT>)dynamicMethod.CreateDelegate(typeof(Func<T, FT>));
        }
    }
}
