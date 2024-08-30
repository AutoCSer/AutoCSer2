using AutoCSer.Extensions;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace AutoCSer.ORM.Reflection.Emit
{
    /// <summary>
    /// 属性操作
    /// </summary>
    internal static class Property
    {
        /// <summary>
        /// 创建设置属性委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="PT"></typeparam>
        /// <param name="property"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        internal static Action<T, PT> UnsafeSetProperty<T, PT>(PropertyInfo property, string methodName) where T : class
        {
            DynamicMethod dynamicMethod = new DynamicMethod(methodName, null, new Type[] { typeof(T), typeof(PT) }, typeof(T), true);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            generator.call(property.GetSetMethod(true));
            generator.Emit(OpCodes.Ret);
            return (Action<T, PT>)dynamicMethod.CreateDelegate(typeof(Action<T, PT>));
        }
        /// <summary>
        /// 创建获取属性委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="PT"></typeparam>
        /// <param name="property"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        internal static Func<T, PT> UnsafeGetProperty<T, PT>(PropertyInfo property, string methodName) where T : class
        {
            DynamicMethod dynamicMethod = new DynamicMethod(methodName, typeof(PT), new Type[] { typeof(T) }, typeof(T), true);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.call(property.GetGetMethod(true));
            generator.Emit(OpCodes.Ret);
            return (Func<T, PT>)dynamicMethod.CreateDelegate(typeof(Func<T, PT>));
        }
    }
}
