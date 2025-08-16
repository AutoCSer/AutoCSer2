using AutoCSer.Metadata;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// MSIL生成
    /// </summary>
    internal static class EmitGenerator
    {
        /// <summary>
        /// 加载 1/0
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void int32(this ILGenerator generator, bool value)
        {
            generator.Emit(value ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
        }
        /// <summary>
        /// 加载Int32数据
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="value"></param>
        internal static void int32(this ILGenerator generator, int value)
        {
            switch (value)
            {
                case 0: generator.Emit(OpCodes.Ldc_I4_0); return;
                case 1: generator.Emit(OpCodes.Ldc_I4_1); return;
                case 2: generator.Emit(OpCodes.Ldc_I4_2); return;
                case 3: generator.Emit(OpCodes.Ldc_I4_3); return;
                case 4: generator.Emit(OpCodes.Ldc_I4_4); return;
                case 5: generator.Emit(OpCodes.Ldc_I4_5); return;
                case 6: generator.Emit(OpCodes.Ldc_I4_6); return;
                case 7: generator.Emit(OpCodes.Ldc_I4_7); return;
                case 8: generator.Emit(OpCodes.Ldc_I4_8); return;
            }
            if (value == -1) generator.Emit(OpCodes.Ldc_I4_M1);
            else generator.Emit((uint)value <= sbyte.MaxValue ? OpCodes.Ldc_I4_S : OpCodes.Ldc_I4, value);
        }
        /// <summary>
        /// 加载参数
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="index"></param>
        internal static void ldarg(this ILGenerator generator, int index)
        {
            switch (index)
            {
                case 0: generator.Emit(OpCodes.Ldarg_0); return;
                case 1: generator.Emit(OpCodes.Ldarg_1); return;
                case 2: generator.Emit(OpCodes.Ldarg_2); return;
                case 3: generator.Emit(OpCodes.Ldarg_3); return;
            }
            generator.Emit((uint)index <= sbyte.MaxValue ? OpCodes.Ldarg_S : OpCodes.Ldarg, index);
        }
        /// <summary>
        /// 函数调用
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="method"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void call(this ILGenerator generator, MethodInfo method)
        {
            generator.Emit(method.IsFinal || !method.IsVirtual ? OpCodes.Call : OpCodes.Callvirt, method);
        }
        /// <summary>
        /// 加载 null 值
        /// </summary>
        /// <param name="generator"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void loadNull(this ILGenerator generator)
        {
            generator.Emit(OpCodes.Ldnull);
        }
        /// <summary>
        /// 返回方法调用
        /// </summary>
        /// <param name="generator"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void ret(this ILGenerator generator)
        {
            generator.Emit(OpCodes.Ret);
        }
        /// <summary>
        /// 加载字符串
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void ldstr(this ILGenerator generator, string value)
        {
            generator.Emit(OpCodes.Ldstr, value);
        }
        /// <summary>
        /// 对象初始化
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="type"></param>
        /// <param name="local"></param>
        internal static void initobjShort(this ILGenerator generator, Type type, LocalBuilder local)
        {
            if (DynamicArray.IsClearArray(type))
            {
                if (type.IsValueType)
                {
                    generator.Emit(OpCodes.Ldloca_S, local);
                    generator.Emit(OpCodes.Initobj, type);
                }
                else
                {
                    generator.loadNull();
                    generator.Emit(OpCodes.Stloc_S, local);
                }
            }
        }
        /// <summary>
        /// 对象初始化
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="type"></param>
        /// <param name="local"></param>
        internal static void initobj(this ILGenerator generator, Type type, LocalBuilder local)
        {
            if (DynamicArray.IsClearArray(type))
            {
                if (type.IsValueType)
                {
                    generator.Emit(OpCodes.Ldloca, local);
                    generator.Emit(OpCodes.Initobj, type);
                }
                else
                {
                    generator.loadNull();
                    generator.Emit(OpCodes.Stloc, local);
                }
            }
        }
        /// <summary>
        /// 判断成员位图是否匹配成员索引
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <param name="genericType"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void memberMapObjectIsMember(this ILGenerator generator, OpCode target, int value, GenericType genericType)
        {
            generator.Emit(target);
            generator.int32(value);
            generator.call(genericType.GetMemberMapIsMemberDelegate.Method);
        }
        /// <summary>
        /// 设置成员索引
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <param name="genericType"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void memberMapObjectSetMember(this ILGenerator generator, OpCode target, int value, GenericType genericType)
        {
            generator.Emit(target);
            generator.int32(value);
            generator.call(genericType.GetMemberMapSetMemberDelegate.Method);
        }

        ///// <summary>
        ///// 判断成员位图是否匹配成员索引
        ///// </summary>
        //private static readonly MethodInfo memberMapIsMemberMethod = ((Func<MemberMap, int, bool>)MemberMap.IsMember).Method;
        ///// <summary>
        ///// 判断成员位图是否匹配成员索引
        ///// </summary>
        ///// <param name="generator"></param>
        ///// <param name="target"></param>
        ///// <param name="value"></param>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal static void memberMapIsMember(this ILGenerator generator, OpCode target, int value)
        //{
        //    generator.Emit(target);
        //    generator.int32(value);
        //    generator.call(memberMapIsMemberMethod);
        //}
        ///// <summary>
        ///// 设置成员索引
        ///// </summary>
        //internal static readonly MethodInfo memberMapSetMemberMethod = ((Action<MemberMap, int>)MemberMap.SetMember).Method;
        ///// <summary>
        ///// 设置成员索引
        ///// </summary>
        ///// <param name="generator"></param>
        ///// <param name="target"></param>
        ///// <param name="value"></param>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal static void memberMapSetMember(this ILGenerator generator, OpCode target, int value)
        //{
        //    generator.Emit(target);
        //    generator.int32(value);
        //    generator.call(memberMapSetMemberMethod);
        //}
    }
}
