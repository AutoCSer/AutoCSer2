using AutoCSer.Extensions;
using AutoCSer.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
#if !AOT
using AutoCSer.FieldEquals.Metadata;
#endif

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
        public static bool CallEquals<
#if AOT
            [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.NonPublicMethods)]
#endif
        T>(T left, T right)
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
        public static bool ObjectEquals(object left, object right)
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
        public static bool Equals(Half left, Half right)
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
        public static bool Equals(float left, float right)
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
        public static bool Equals(double left, double right)
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
        /// 数据比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool Equals(System.Numerics.Complex left, System.Numerics.Complex right)
        {
            if (Equals(left.Real, right.Real) && Equals(left.Imaginary, right.Imaginary)) return true;
            return false;
        }
        /// <summary>
        /// 数据比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool Equals(System.Numerics.Matrix4x4 left, System.Numerics.Matrix4x4 right)
        {
            if (Equals(left.M11, right.M11) && Equals(left.M12, right.M12) && Equals(left.M13, right.M13) && Equals(left.M14, right.M14)
                && Equals(left.M21, right.M21) && Equals(left.M22, right.M22) && Equals(left.M23, right.M23) && Equals(left.M24, right.M24)
                && Equals(left.M31, right.M31) && Equals(left.M32, right.M32) && Equals(left.M33, right.M33) && Equals(left.M34, right.M34)
                && Equals(left.M41, right.M41) && Equals(left.M42, right.M42) && Equals(left.M43, right.M43) && Equals(left.M44, right.M44)) return true;
            return false;
        }
        /// <summary>
        /// 数据比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool Equals(System.Numerics.Matrix3x2 left, System.Numerics.Matrix3x2 right)
        {
            if (Equals(left.M11, right.M11) && Equals(left.M12, right.M12)
                && Equals(left.M21, right.M21) && Equals(left.M22, right.M22)
                && Equals(left.M31, right.M31) && Equals(left.M32, right.M32)) return true;
            return false;
        }
        /// <summary>
        /// 数据比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool Equals(System.Numerics.Plane left, System.Numerics.Plane right)
        {
            if (Equals(left.Normal, right.Normal) && Equals(left.D, right.D)) return true;
            return false;
        }
        /// <summary>
        /// 数据比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool Equals(System.Numerics.Quaternion left, System.Numerics.Quaternion right)
        {
            if (Equals(left.X, right.X) && Equals(left.Y, right.Y) && Equals(left.Z, right.Z) && Equals(left.W, right.W)) return true;
            return false;
        }
        /// <summary>
        /// 数据比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool Equals(System.Numerics.Vector2 left, System.Numerics.Vector2 right)
        {
            if (Equals(left.X, right.X) && Equals(left.Y, right.Y)) return true;
            return false;
        }
        /// <summary>
        /// 数据比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool Equals(System.Numerics.Vector3 left, System.Numerics.Vector3 right)
        {
            if (Equals(left.X, right.X) && Equals(left.Y, right.Y) && Equals(left.Z, right.Z)) return true;
            return false;
        }
        /// <summary>
        /// 数据比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool Equals(System.Numerics.Vector4 left, System.Numerics.Vector4 right)
        {
            if (Equals(left.X, right.X) && Equals(left.Y, right.Y) && Equals(left.Z, right.Z) && Equals(left.W, right.W)) return true;
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
        public static bool EquatableEquals<T>(T left, T right) where T : IEquatable<T>
        {
            if (left.Equals(right)) return true;
            Breakpoint(typeof(T), left, right);
            return false;
        }
#if AOT
        /// <summary>
        /// 对象比较
        /// </summary>
        internal static readonly MethodInfo EquatableEqualsMethod = typeof(Comparor).GetMethod(nameof(EquatableEquals), BindingFlags.Static | BindingFlags.Public).notNull();
        /// <summary>
        /// 对象比较
        /// </summary>
        internal static readonly MethodInfo ReferenceEqualsMethod = typeof(Comparor).GetMethod(nameof(ReferenceEquals), BindingFlags.Static | BindingFlags.Public).notNull();
        /// <summary>
        /// 数组比较
        /// </summary>
        internal static readonly MethodInfo ArrayEqualsMethod = typeof(Comparor).GetMethod(nameof(ArrayEquals), BindingFlags.Static | BindingFlags.Public).notNull();
        /// <summary>
        /// 对象比较
        /// </summary>
        internal static readonly MethodInfo NullableEqualsMethod = typeof(Comparor).GetMethod(nameof(NullableEquals), BindingFlags.Static | BindingFlags.Public).notNull();
        /// <summary>
        /// 字典比较
        /// </summary>
        internal static readonly MethodInfo DictionaryEqualsMethod = typeof(Comparor).GetMethod(nameof(DictionaryEquals), BindingFlags.Static | BindingFlags.Public).notNull();
        /// <summary>
        /// 集合比较
        /// </summary>
        internal static readonly MethodInfo CollectionEqualsMethod = typeof(Comparor).GetMethod(nameof(CollectionEquals), BindingFlags.Static | BindingFlags.Public).notNull();
        /// <summary>
        /// 键值对比较
        /// </summary>
        internal static readonly MethodInfo KeyValueEqualsMethod = typeof(Comparor).GetMethod(nameof(KeyValueEquals), BindingFlags.Static | BindingFlags.Public).notNull();
        /// <summary>
        /// 键值对比较
        /// </summary>
        internal static readonly MethodInfo KeyValuePairEqualsMethod = typeof(Comparor).GetMethod(nameof(KeyValuePairEquals), BindingFlags.Static | BindingFlags.Public).notNull();
        /// <summary>
        /// 代码生成模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static bool ReflectionMethodName<T>(params object[] value) { return false; }
        /// <summary>
        /// 代码生成模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static bool EqualsMethodName<T>(params object[] value) { return false; }
#endif
        /// <summary>
        /// 对象比较
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool ReferenceEquals<T>(T left, T right) where T : IEquatable<T>
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
        public static bool NullableEquals<T>(T? left, T? right) where T : struct
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
        public static bool ArrayEquals<T>(T[] leftArray, T[] rightArray)
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
        public static bool CollectionEquals<T, VT>(T leftArray, T rightArray) where T : ICollection<VT>
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
        public static bool DictionaryEquals<T, KT, VT>(T leftArray, T rightArray) where T : IDictionary<KT, VT>
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
        /// 键值对比较
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool KeyValueEquals<KT, VT>(KeyValue<KT, VT> left, KeyValue<KT, VT> right)
        {
            if (Comparor<KT>.EqualsComparor(left.Key, right.Key) && Comparor<VT>.EqualsComparor(left.Value, right.Value)) return true;
            Breakpoint(typeof(KeyValue<KT, VT>));
            return false;
        }
        /// <summary>
        /// 键值对比较
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool KeyValuePairEquals<KT, VT>(KeyValuePair<KT, VT> left, KeyValuePair<KT, VT> right)
        {
            if (Comparor<KT>.EqualsComparor(left.Key, right.Key) && Comparor<VT>.EqualsComparor(left.Value, right.Value)) return true;
            Breakpoint(typeof(KeyValue<KT, VT>));
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

        /// <summary>
        /// 获取对象对比成员
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        internal static IEnumerable<Member> GetFieldEqualsFields(FieldInfo[] fields)
        {
            foreach (FieldInfo field in fields)
            {
                MemberInfo member = field.getPropertyMemberInfo() ?? field;
                if (member.GetCustomAttribute(typeof(IgnoreAttribute), false) == null) yield return new Member(field, member);
            }
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
                if (type == typeof(System.Numerics.Complex)) return (Func<System.Numerics.Complex, System.Numerics.Complex, bool>)Comparor.Equals;
                if (type == typeof(System.Numerics.Matrix3x2)) return (Func<System.Numerics.Matrix3x2, System.Numerics.Matrix3x2, bool>)Comparor.Equals;
                if (type == typeof(System.Numerics.Matrix4x4)) return (Func<System.Numerics.Matrix4x4, System.Numerics.Matrix4x4, bool>)Comparor.Equals;
                if (type == typeof(System.Numerics.Plane)) return (Func<System.Numerics.Plane, System.Numerics.Plane, bool>)Comparor.Equals;
                if (type == typeof(System.Numerics.Quaternion)) return (Func<System.Numerics.Quaternion, System.Numerics.Quaternion, bool>)Comparor.Equals;
                if (type == typeof(System.Numerics.Vector2)) return (Func<System.Numerics.Vector2, System.Numerics.Vector2, bool>)Comparor.Equals;
                if (type == typeof(System.Numerics.Vector3)) return (Func<System.Numerics.Vector3, System.Numerics.Vector3, bool>)Comparor.Equals;
                if (type == typeof(System.Numerics.Vector4)) return (Func<System.Numerics.Vector4, System.Numerics.Vector4, bool>)Comparor.Equals;
#if AOT
                if (type.IsValueType) return Comparor.EquatableEqualsMethod.MakeGenericMethod(type).CreateDelegate(typeof(Func<T, T, bool>));
                return Comparor.ReferenceEqualsMethod.MakeGenericMethod(type).CreateDelegate(typeof(Func<T, T, bool>));
#else
                EquatableGenericType equatableGenericType = EquatableGenericType.Get(type);
                return (type.IsValueType ? equatableGenericType.EquatableEqualsDelegate : equatableGenericType.ReferenceEqualsDelegate);
#endif
            }
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1)
                {
#if AOT
                    return Comparor.ArrayEqualsMethod.MakeGenericMethod(type.GetElementType().notNull()).CreateDelegate(typeof(Func<T, T, bool>));
#else
                    return GenericType.Get(type.GetElementType().notNull()).ArrayDelegate;
#endif
                }
                return (Func<T, T, bool>)notSupport;
            }
            if (type.IsEnum)
            {
#if AOT
                switch (AutoCSer.Metadata.EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(type)))
                {
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Int: return Comparor.EnumIntMethod.MakeGenericMethod(type).CreateDelegate(typeof(Func<T, T, bool>));
                    case AutoCSer.Metadata.UnderlyingTypeEnum.UInt: return Comparor.EnumUIntMethod.MakeGenericMethod(type).CreateDelegate(typeof(Func<T, T, bool>));
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Byte: return Comparor.EnumByteMethod.MakeGenericMethod(type).CreateDelegate(typeof(Func<T, T, bool>));
                    case AutoCSer.Metadata.UnderlyingTypeEnum.ULong: return Comparor.EnumULongMethod.MakeGenericMethod(type).CreateDelegate(typeof(Func<T, T, bool>));
                    case AutoCSer.Metadata.UnderlyingTypeEnum.UShort: return Comparor.EnumUShortMethod.MakeGenericMethod(type).CreateDelegate(typeof(Func<T, T, bool>));
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Long: return Comparor.EnumLongMethod.MakeGenericMethod(type).CreateDelegate(typeof(Func<T, T, bool>));
                    case AutoCSer.Metadata.UnderlyingTypeEnum.Short: return Comparor.EnumShortMethod.MakeGenericMethod(type).CreateDelegate(typeof(Func<T, T, bool>));
                    case AutoCSer.Metadata.UnderlyingTypeEnum.SByte: return Comparor.EnumSByteMethod.MakeGenericMethod(type).CreateDelegate(typeof(Func<T, T, bool>));
                }
#else
                return EnumGenericType.Get(type).EqualsDelegate;
#endif
            }
            if (type.isValueTypeNullable())
            {
#if AOT
                return Comparor.NullableEqualsMethod.MakeGenericMethod(type.GetGenericArguments()[0]).CreateDelegate(typeof(Func<T, T, bool>));
#else
                return StructGenericType.Get(type.GetGenericArguments()[0]).NullableDelegate;
#endif
            }
#if AOT
            if (type.IsValueType && type.IsGenericType)
            {
                Type genericType = type.GetGenericTypeDefinition();
                if (genericType == typeof(KeyValuePair<,>)) return Comparor.KeyValuePairEqualsMethod.MakeGenericMethod(type.GetGenericArguments()).CreateDelegate(typeof(Func<T, T, bool>));
                if (genericType == typeof(KeyValue<,>)) return Comparor.KeyValueEqualsMethod.MakeGenericMethod(type.GetGenericArguments()).CreateDelegate(typeof(Func<T, T, bool>));
            }
#endif
            var collectionType = default(Type);
            foreach (Type interfaceType in type.getGenericInterface())
            {
                Type genericType = interfaceType.GetGenericTypeDefinition();
                if (genericType == typeof(IDictionary<,>))
                {
#if AOT
                    Type[] types = interfaceType.GetGenericArguments();
                    return Comparor.DictionaryEqualsMethod.MakeGenericMethod(type, types[0], types[1]).CreateDelegate(typeof(Func<T, T, bool>));
#else
                    return DictionaryGenericType.Get(type, interfaceType).EqualsDelegate;
#endif
                }
                if (collectionType == null && genericType == typeof(ICollection<>)) collectionType = interfaceType;
            }
            if (collectionType != null)
            {
#if AOT
                return Comparor.CollectionEqualsMethod.MakeGenericMethod(type, collectionType.GetGenericArguments()[0]).CreateDelegate(typeof(Func<T, T, bool>));
#else
                return CollectionGenericType.Get(type, collectionType).EqualsDelegate;
#endif
            }
            if (type.IsPointer || type.IsInterface) return (Func<T, T, bool>)notSupport;
            if (type == typeof(object)) return (Func<object, object, bool>)Comparor.ObjectEquals;
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
            Type type = typeof(T);
#if AOT

            var method = type.GetMethod(AutoCSer.CodeGenerator.FieldEqualsAttribute.FieldEqualsMethodName, BindingFlags.Static | BindingFlags.NonPublic, new Type[] { type, type });
            if (method != null && !method.IsGenericMethod && method.ReturnType == typeof(bool))
            {
                var memberMapMethod = type.GetMethod(AutoCSer.CodeGenerator.FieldEqualsAttribute.MemberMapFieldEqualsMethodName, BindingFlags.Static | BindingFlags.NonPublic, new Type[] { type, type, typeof(AutoCSer.Metadata.MemberMap<T>) });
                if (memberMapMethod != null && !memberMapMethod.IsGenericMethod && memberMapMethod.ReturnType == typeof(bool))
                {
                    EqualsComparor = (Func<T, T, bool>)method.CreateDelegate(typeof(Func<T, T, bool>));
                    MemberMapEqualsComparor = (Func<T, T, AutoCSer.Metadata.MemberMap<T>, bool>)memberMapMethod.CreateDelegate(typeof(Func<T, T, AutoCSer.Metadata.MemberMap<T>, bool>));
                    return;
                }
                throw new MissingMethodException(type.fullName(), AutoCSer.CodeGenerator.FieldEqualsAttribute.MemberMapFieldEqualsMethodName);
            }
            throw new MissingMethodException(type.fullName(), AutoCSer.CodeGenerator.FieldEqualsAttribute.FieldEqualsMethodName);
#else
            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly);
            LeftArray<AutoCSer.Metadata.FieldIndex> memberMapFields = AutoCSer.Metadata.MemberIndexGroup.GetAnonymousFields(type, AutoCSer.Metadata.MemberFiltersEnum.InstanceField);
            AutoCSer.Metadata.GenericType genericType = new AutoCSer.Metadata.GenericType<T>();
            MemberDynamicMethod dynamicMethod = new MemberDynamicMethod(genericType, false);
            foreach (Member member in Comparor.GetFieldEqualsFields(fields)) dynamicMethod.Push(member.Field);
            dynamicMethod.Base();
            EqualsComparor = (Func<T, T, bool>)dynamicMethod.Create(typeof(Func<T, T, bool>));

            dynamicMethod = new MemberDynamicMethod(genericType, true);
            foreach (AutoCSer.Metadata.FieldIndex field in memberMapFields)
            {
                dynamicMethod.Push(field.Member, field.MemberIndex);
            }
            MemberMapEqualsComparor = (Func<T, T, AutoCSer.Metadata.MemberMap<T>, bool>)dynamicMethod.Create(typeof(Func<T, T, AutoCSer.Metadata.MemberMap<T>, bool>));
#endif
        }
    }
}
