using AutoCSer.Extensions;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace AutoCSer.Reflection.Emit
{
    /// <summary>
    /// 字段操作
    /// </summary>
    internal static class Field
    {
        /// <summary>
        /// 创建设置字段委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="FT"></typeparam>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static Action<T, FT>? UnsafeSetField<T, FT>(string fieldName) where T : class
#else
        internal static Action<T, FT> UnsafeSetField<T, FT>(string fieldName) where T : class
#endif
        {
            var field = typeof(T).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            return field != null && field.FieldType == typeof(FT) ? UnsafeSetField<T, FT>(field, AutoCSer.Common.NamePrefix + "Set" + field.Name) : null;
        }
        /// <summary>
        /// 创建设置字段委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="FT"></typeparam>
        /// <param name="field"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        internal static Action<T, FT> UnsafeSetField<T, FT>(FieldInfo field, string methodName) where T : class
        {
            DynamicMethod dynamicMethod = new DynamicMethod(methodName, null, new Type[] { typeof(T), typeof(FT) }, typeof(T), true);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Stfld, field);
            generator.ret();
            return (Action<T, FT>)dynamicMethod.CreateDelegate(typeof(Action<T, FT>));
        }
    }
}
