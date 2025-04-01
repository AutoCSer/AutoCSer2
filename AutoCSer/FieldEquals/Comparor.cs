using AutoCSer.Extensions;
using AutoCSer.FieldEquals.Metadata;
using AutoCSer.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.FieldEquals
{
    /// <summary>
    /// 对象对比
    /// </summary>
    public static partial class Comparor
    {
        /// <summary>
        /// 对象对比
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool CallEquals<T>(T left, T right)
        {
            return Comparor<T>.EqualsComparor(left, right);
        }
        /// <summary>
        /// 对象比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static new bool Equals(object left, object right)
        {
            if (left == null ? right == null : right != null) return true;
            Breakpoint(typeof(object), left != null, right != null);
            return false;
        }
#if NET8
        /// <summary>
        /// 浮点数比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        internal static bool Equals(Half left, Half right)
        {
            if (Half.IsNaN(left))
            {
                if (Half.IsNaN(right)) return true;
            }
            if (left == right) return true;
            if ((!Half.IsNaN(right) && Half.Parse(left.ToString()) == right)) return true;
            Breakpoint(typeof(Half), left, right);
            return false;
        }
#endif
        /// <summary>
        /// 浮点数比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        internal static bool Equals(float left, float right)
        {
            if (float.IsNaN(left))
            {
                if (float.IsNaN(right)) return true;
            }
            if (left == right) return true;
            if ((!float.IsNaN(right) && float.Parse(left.ToString()) == right)) return true;
            Breakpoint(typeof(float), left, right);
            return false;
        }
        /// <summary>
        /// 浮点数比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        internal static bool Equals(double left, double right)
        {
            if (double.IsNaN(left))
            {
                if (double.IsNaN(right)) return true;
            }
            if (left == right) return true;
            if ((!double.IsNaN(right) && double.Parse(left.ToString(), System.Globalization.CultureInfo.InvariantCulture) == right)) return true;
            Breakpoint(typeof(double), left, right);
            return false;
        }
        /// <summary>
        /// 对象比较
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool EquatableEquals<T>(T left, T right) where T : IEquatable<T>
        {
            if (left.Equals(right)) return true;
            Breakpoint(typeof(T));
            return false;
        }
        /// <summary>
        /// 对象比较
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool ReferenceEquals<T>(T left, T right) where T : IEquatable<T>
        {
            if (object.ReferenceEquals(left, right) || left.Equals(right)) return true;
            Breakpoint(typeof(T));
            return false;
        }
        /// <summary>
        /// 对象比较
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        internal static bool Equals<T>(T? left, T? right) where T : struct
        {
            if (!left.HasValue)
            {
                if (!right.HasValue) return true;
            }
            else if (right.HasValue) return Comparor<T>.EqualsComparor(left.Value, right.Value);
            Breakpoint(typeof(T?), right.HasValue, right.HasValue);
            return false;
        }
        /// <summary>
        /// 数组比较
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="leftArray"></param>
        /// <param name="rightArray"></param>
        /// <returns></returns>
        internal static bool Equals<T>(T[] leftArray, T[] rightArray)
        {
            if (object.ReferenceEquals(leftArray, rightArray)) return true;
            if (leftArray != null && rightArray != null)
            {
                if (leftArray.Length == rightArray.Length)
                {
                    int index = 0;
                    foreach (T left in leftArray)
                    {
                        if (!Comparor<T>.EqualsComparor(left, rightArray[index++])) return false;
                    }
                    return true;
                }
                else Breakpoint(typeof(T), leftArray.Length, rightArray.Length);
            }
            else Breakpoint(typeof(T));
            return false;
        }
        /// <summary>
        /// 集合比较
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="leftArray"></param>
        /// <param name="rightArray"></param>
        /// <returns></returns>
        internal static bool Equals<T, VT>(T leftArray, T rightArray) where T : ICollection<VT>
        {
            if (object.ReferenceEquals(leftArray, rightArray)) return true;
            if (leftArray != null && rightArray != null)
            {
                if (leftArray.Count == rightArray.Count)
                {
                    IEnumerator<VT> right = rightArray.GetEnumerator();
                    foreach (VT left in leftArray)
                    {
                        if (right.MoveNext())
                        {
                            if (!Comparor<VT>.EqualsComparor(left, right.Current)) return false;
                        }
                        else
                        {
                            Breakpoint(typeof(T));
                            return false;
                        }
                    }
                    return true;
                }
                else Breakpoint(typeof(T), leftArray.Count, rightArray.Count);
            }
            else Breakpoint(typeof(T));
            return false;
        }
        /// <summary>
        /// 字典比较
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="leftArray"></param>
        /// <param name="rightArray"></param>
        /// <returns></returns>
        internal static bool Equals<T, KT, VT>(T leftArray, T rightArray) where T : IDictionary<KT, VT>
        {
            if (object.ReferenceEquals(leftArray, rightArray)) return true;
            if(leftArray != null && rightArray != null)
            {
                if (leftArray.Count == rightArray.Count)
                {
                    var right = default(VT);
                    foreach (KeyValuePair<KT, VT> left in leftArray)
                    {
                        if (rightArray.TryGetValue(left.Key, out right))
                        {
                            if (!Comparor<VT>.EqualsComparor(left.Value, right)) return false;
                        }
                        else
                        {
                            Breakpoint(typeof(T));
                            //Comparor<KT>.EqualsComparor(left.Key, default(KT));
                            return false;
                        }
                    }
                    return true;
                }
                else Breakpoint(typeof(T), leftArray.Count, rightArray.Count);
            }
            else Breakpoint(typeof(T));
            return false;
        }
        /// <summary>
        /// 测试断点信息添加到输出队列（DEBUG 有效）
        /// </summary>
        public static bool IsBreakpoint = false;
#if DEBUG
        /// <summary>
        /// 检查返回数据
        /// </summary>
        /// <param name="type">比较类型</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
#if NetStandard21
        internal static void Breakpoint(Type type, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        internal static void Breakpoint(Type type, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            if (IsBreakpoint) AutoCSer.ConsoleWriteQueue.Breakpoint(type.fullName(), callerMemberName, callerFilePath, callerLineNumber);
        }
        /// <summary>
        /// 测试断点信息添加到输出队列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
#if NetStandard21
        internal static void Breakpoint<T>(T left, T right, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        internal static void Breakpoint<T>(T left, T right, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            if (IsBreakpoint) AutoCSer.ConsoleWriteQueue.Breakpoint($"{typeof(T).fullName()} {left} <> {right}", callerMemberName, callerFilePath, callerLineNumber);
        }
        /// <summary>
        /// 测试断点信息添加到输出队列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">比较类型</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
#if NetStandard21
        internal static void Breakpoint<T>(Type type, T left, T right, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        internal static void Breakpoint<T>(Type type, T left, T right, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            if (IsBreakpoint) AutoCSer.ConsoleWriteQueue.Breakpoint($"{type.fullName()} {left} <> {right}", callerMemberName, callerFilePath, callerLineNumber);
        }
#else
        /// <summary>
        /// 检查返回数据
        /// </summary>
        /// <param name="type"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Breakpoint(Type type) { }
        /// <summary>
        /// 测试断点信息添加到输出队列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Breakpoint<T>(T left, T right) { }
        /// <summary>
        /// 测试断点信息添加到输出队列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Breakpoint<T>(Type type, T left, T right) { }
#endif
        /// <summary>
        /// 对象对比
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool Equals<T>(T left, T right)
        {
            if (left == null)
            {
                if (right == null) return true;
            }
            else if (right != null) return Comparor<T>.EqualsComparor(left, right);
            Breakpoint(typeof(T));
            return false;
        }
        /// <summary>
        /// 对象对比
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="memberMap"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool Equals<T>(T left, T right, AutoCSer.Metadata.MemberMap<T> memberMap)
        {
            return Comparor<T>.MemberMapEquals(left, right, memberMap);
        }
    }
    /// <summary>
    /// 对象对比
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal static class Comparor<T>
    {
        /// <summary>
        /// 对象对比委托
        /// </summary>
        internal static readonly Func<T, T, bool> EqualsComparor;
        /// <summary>
        /// 对象对比委托
        /// </summary>
        internal static readonly Func<T, T, AutoCSer.Metadata.MemberMap<T>, bool> MemberMapEqualsComparor;

        /// <summary>
        /// 对象对比
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="memberMap"></param>
        /// <returns></returns>
        internal static bool MemberMapEquals(T left, T right, AutoCSer.Metadata.MemberMap<T> memberMap)
        {
            if (left == null)
            {
                if (right == null) return true;
            }
            else if (right != null) return MemberMapEqualsComparor(left, right, memberMap);
            Comparor.Breakpoint(typeof(T));
            return false;
        }
        /// <summary>
        /// 不支持类型
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static bool notSupport(T left, T right)
        {
            Comparor.Breakpoint(typeof(T));
            return false;
        }
        /// <summary>
        /// 获取对象比较委托
        /// </summary>
        /// <returns>对象比较委托</returns>
#if NetStandard21
        private static Delegate? getDelegate()
#else
        private static Delegate getDelegate()
#endif
        {
            Type type = typeof(T);
            if (typeof(IEquatable<T>).IsAssignableFrom(type))
            {
#if NET8
                if (type == typeof(Half)) return (Func<Half, Half, bool>)Comparor.Equals;
#endif
                if (type == typeof(float)) return (Func<float, float, bool>)Comparor.Equals;
                if (type == typeof(double)) return (Func<double, double, bool>)Comparor.Equals;
                EquatableGenericType equatableGenericType = EquatableGenericType.Get(type);
                return (type.IsValueType ? equatableGenericType.EquatableEqualsDelegate : equatableGenericType.ReferenceEqualsDelegate);
            }
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1) return GenericType.Get(type.GetElementType().notNull()).ArrayDelegate;
                return (Func<T, T, bool>)notSupport;
            }
            if (type.IsEnum) return EnumGenericType.Get(type).EqualsDelegate;
            if (type.isValueTypeNullable()) return StructGenericType.Get(type.GetGenericArguments()[0]).NullableDelegate;
            foreach (Type interfaceType in type.getGenericInterface())
            {
                Type genericType = interfaceType.GetGenericTypeDefinition();
                if (genericType == typeof(IDictionary<,>)) return DictionaryGenericType.Get(type, interfaceType).EqualsDelegate;
                if (genericType == typeof(ICollection<>)) return CollectionGenericType.Get(type, interfaceType).EqualsDelegate;
            }
            if (type.IsPointer || type.IsInterface) return (Func<T, T, bool>)notSupport;
            if (type == typeof(object)) return (Func<object, object, bool>)Comparor.Equals;
            return null;
        }
        static Comparor()
        {
            var equalsDelegate = getDelegate();
            if (equalsDelegate != null)
            {
                EqualsComparor = (Func<T, T, bool>)equalsDelegate;
                MemberMapEqualsComparor = MemberMapEquals;
                return;
            }
            AutoCSer.Metadata.GenericType genericType =  new AutoCSer.Metadata.GenericType<T>();
            MemberDynamicMethod dynamicMethod = new MemberDynamicMethod(genericType, false);
            foreach (FieldInfo field in typeof(T).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly))
            {
                if ((field.getPropertyMemberInfo() ?? field).GetCustomAttribute(typeof(IgnoreAttribute), false) == null) dynamicMethod.Push(field);
            }
            dynamicMethod.Base();
            EqualsComparor = (Func<T, T, bool>)dynamicMethod.Create(typeof(Func<T, T, bool>));

            dynamicMethod = new MemberDynamicMethod(genericType, true);
            foreach (AutoCSer.Metadata.FieldIndex field in AutoCSer.Metadata.MemberIndexGroup<T>.GetFields(AutoCSer.Metadata.MemberFiltersEnum.InstanceField))
            {
                dynamicMethod.Push(field.Member, field.MemberIndex);
            }
            MemberMapEqualsComparor = (Func<T, T, AutoCSer.Metadata.MemberMap<T>, bool>)dynamicMethod.Create(typeof(Func<T, T, AutoCSer.Metadata.MemberMap<T>, bool>));
        }
    }
}
