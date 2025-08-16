using AutoCSer.Extensions;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 默认构造函数
    /// </summary>
    public class DefaultConstructor
    {
        /// <summary>
        /// 获取自定义创建对象的默认构造函数，用于反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>委托返回值的 T 类型对象不允许为 null</returns>
#if NetStandard21
        public virtual Func<T>? GetConstructor<T>()
#else
        public virtual Func<T> GetConstructor<T>()
#endif
        {
            Type type = typeof(T);
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(LeftArray<>))
            {
#if AOT
                return (Func<T>)new CallDefaultConstructor(type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, EmptyArray<Type>.Array, null).notNull()).Call<T>;
#else
                return (Func<T>)GenericType.Get(type.GetGenericArguments()[0]).LeftArrayDefaultConstructorDelegate;
#endif
            }
            return null;
        }
        /// <summary>
        /// 数组字串构造函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal static LeftArray<T> LeftArrayDefaultConstructor<T>()
        {
            return new LeftArray<T>(0);
        }
        /// <summary>
        /// 是否存在默认构造函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
#if NetStandard21
        public static object? GetIsSerializeConstructor
#else
        public static object GetIsSerializeConstructor
#endif
            <
#if AOT
            [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.NonPublicMethods)] 
#endif
            T>()
        {
            return DefaultConstructor<T>.Type != DefaultConstructorTypeEnum.None ? AutoCSer.Common.EmptyObject : null;
        }

#if AOT
        /// <summary>
        /// 是否存在默认构造函数
        /// </summary>
        internal static readonly MethodInfo GetIsSerializeConstructorMethod = typeof(DefaultConstructor).GetMethod(nameof(GetIsSerializeConstructor), BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, EmptyArray<Type>.Array).notNull();
#endif
        /// <summary>
        /// 对象浅复制
        /// </summary>
        internal static readonly Func<object, object> CallMemberwiseClone = (Func<object, object>)Delegate.CreateDelegate(typeof(Func<object, object>), ((Func<object>)new DefaultConstructor().MemberwiseClone).Method);
    }
    /// <summary>
    /// 默认构造函数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal static class DefaultConstructor<
#if AOT
        [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
#endif
    T>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
#if NetStandard21
        internal static readonly Func<T?> Constructor;
#else
        internal static readonly Func<T> Constructor;
#endif
        /// <summary>
        /// 未初始化对象，用于Clone
        /// </summary>
        private static readonly object uninitializedObject;
        /// <summary>
        /// 获取未初始化对象，用于Clone
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static T cloneUninitializedObject()
        {
            return (T)DefaultConstructor.CallMemberwiseClone(uninitializedObject);
        }
        /// <summary>
        /// 是否存在默认构造函数
        /// </summary>
        internal static readonly DefaultConstructorTypeEnum Type;
        ///// <summary>
        ///// 默认空值
        ///// </summary>
        ///// <returns>默认空值</returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //public static T Default()
        //{
        //    return default(T);
        //}

        static DefaultConstructor()
        {
            Type type = typeof(T);
            var constructor = AutoCSer.Common.DefaultConstructor.GetConstructor<T>();
            if (constructor == null)
            {
                if (type.IsValueType) Type = DefaultConstructorTypeEnum.Default;
                else if (!type.IsArray && type != typeof(string) && !type.IsInterface && !type.IsAbstract)
                {
                    var constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, EmptyArray<Type>.Array, null);
                    if (constructorInfo != null)
                    {
#if AOT
                        if (type.IsGenericType) Constructor = (Func<T>)new CallDefaultConstructor(constructorInfo).Call<T>;
                        else
                        {
                            var method = type.GetMethod(AutoCSer.CodeGenerator.DefaultConstructorAttribute.DefaultConstructorMethodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, EmptyArray<Type>.Array);
                            if (method != null && !method.IsGenericMethod && method.ReturnType == type)
                            {
                                Constructor = (Func<T>)method.CreateDelegate(typeof(Func<T>));
                            }
                            else
                            {
                                Constructor = (Func<T>)new CallDefaultConstructor(constructorInfo).Call<T>;
                                if (BinarySerializer.CustomConfig.IsReflectionLog()) AutoCSer.LogHelper.ExceptionIgnoreException(new MissingMethodException(type.fullName(), AutoCSer.CodeGenerator.DefaultConstructorAttribute.DefaultConstructorMethodName));
                            }
                        }
#else
                        DynamicMethod dynamicMethod = new DynamicMethod(AutoCSer.Common.NamePrefix + "DefaultConstructor", type, EmptyArray<Type>.Array, type, true);
                        ILGenerator generator = dynamicMethod.GetILGenerator();
                        generator.Emit(OpCodes.Newobj, constructorInfo);
                        generator.ret();
                        Constructor = (Func<T>)dynamicMethod.CreateDelegate(typeof(Func<T>));
#endif
                        Type = DefaultConstructorTypeEnum.Constructor;
                        uninitializedObject = Common.EmptyObject;
                        return;
                    }
#if NET8
                    uninitializedObject = (T)System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(type);
#else
                    uninitializedObject = (T)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(type);
#endif
                    if (uninitializedObject != null)
                    {
                        Constructor = cloneUninitializedObject;
                        Type = DefaultConstructorTypeEnum.UninitializedObjectClone;
                        return;
                    }
                }
                Constructor = AutoCSer.Common.GetDefault<T>;
            }
            else
            {
                Constructor = constructor;
                Type = DefaultConstructorTypeEnum.Custom;
            }
            uninitializedObject = Common.EmptyObject;
        }
    }
}
