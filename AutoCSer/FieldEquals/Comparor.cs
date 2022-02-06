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
    internal static partial class Comparor
    {
        /// <summary>
        /// 对象对比
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static bool CallEquals<T>(T left, T right)
        {
            return Comparor<T>.Equals(left, right);
        }
        /// <summary>
        /// 浮点数比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static bool Equals(float left, float right)
        {
            if (float.IsNaN(left)) return check(float.IsNaN(right));
            return check(left == right || (!float.IsNaN(right) && float.Parse(left.ToString()) == right));
        }
        /// <summary>
        /// 浮点数比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static bool Equals(double left, double right)
        {
            if (double.IsNaN(left)) return check(double.IsNaN(right));
            return check(left == right || (!double.IsNaN(right) && double.Parse(left.ToString(), System.Globalization.CultureInfo.InvariantCulture) == right));
        }
        /// <summary>
        /// 对象比较
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static bool Equals<T>(T left, T right) where T : IEquatable<T>
        {
            return check(left.Equals(right));
        }
        /// <summary>
        /// 对象比较
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static bool ReferenceEquals<T>(T left, T right) where T : IEquatable<T>
        {
            return check(object.ReferenceEquals(left, right) || left.Equals(right));
        }
        /// <summary>
        /// 对象比较
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static bool Equals<T>(T? left, T? right) where T : struct
        {
            if (!left.HasValue) return check(!right.HasValue);
            if (right.HasValue) return Comparor<T>.Equals(left.Value, right.Value);
            return check(false);
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
            if (leftArray == null || rightArray == null || leftArray.Length != rightArray.Length) return check(false);
            int index = 0;
            foreach (T left in leftArray)
            {
                if (!Comparor<T>.Equals(left, rightArray[index++])) return false;
            }
            return true;
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
            if (leftArray == null || rightArray == null || leftArray.Count != rightArray.Count) return check(false);
            IEnumerator<VT> right = rightArray.GetEnumerator();
            foreach (VT left in leftArray)
            {
                if (!right.MoveNext()) return check(false);
                if (!Comparor<VT>.Equals(left, right.Current)) return false;
            }
            return true;
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
            if (leftArray == null || rightArray == null || leftArray.Count != rightArray.Count) return check(false);
            foreach (KeyValuePair<KT, VT> left in leftArray)
            {
                VT right;
                if (!rightArray.TryGetValue(left.Key, out right)) return check(false);
                if (!Comparor<VT>.Equals(left.Value, right)) return false;
            }
            return true;
        }
#if DEBUG
        /// <summary>
        /// 检查返回数据
        /// </summary>
        /// <param name="result"></param>
        /// <param name="callerMemberName"></param>
        /// <param name="callerFilePath"></param>
        /// <param name="callerLineNumber"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static bool check(bool result, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
        {
            if (!result)
            {
                CatchTask.AddIgnoreException(AutoCSer.LogHelper.Debug(null, LogLevel.Debug, callerMemberName, callerFilePath, callerLineNumber));
            }
            return result;
        }
#else
        /// <summary>
        /// 检查返回数据
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static bool check(bool result)
        {
            return result;
        }
#endif
    }
    /// <summary>
    /// 对象对比
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class Comparor<T>
    {
        /// <summary>
        /// 对象对比委托
        /// </summary>
        public static new readonly Func<T, T, bool> Equals;
        /// <summary>
        /// 对象对比委托
        /// </summary>
        public static readonly Func<T, T, AutoCSer.Metadata.MemberMap, bool> MemberMapEquals;

        /// <summary>
        /// 对象对比
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="memberMap"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static bool memberMapEquals(T left, T right, AutoCSer.Metadata.MemberMap memberMap)
        {
            return Equals(left, right);
        }
        /// <summary>
        /// 不支持类型
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static bool notSupport(T left, T right)
        {
            return false;
        }
        /// <summary>
        /// 获取对象比较委托
        /// </summary>
        /// <returns>对象比较委托</returns>
        private static Delegate getDelegate()
        {
            Type type = typeof(T);
            if (typeof(IEquatable<T>).IsAssignableFrom(type))
            {
                if (type == typeof(float)) return (Func<float, float, bool>)Comparor.Equals;
                if (type == typeof(double)) return (Func<double, double, bool>)Comparor.Equals;
                EquatableGenericType equatableGenericType = EquatableGenericType.Get(type);
                return (type.IsValueType ? equatableGenericType.EqualsDelegate : equatableGenericType.ReferenceEqualsDelegate);
            }
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1) return GenericType.Get(type.GetElementType()).ArrayDelegate;
                return (Func<T, T, bool>)notSupport;
            }
            if (type.IsEnum) return EnumGenericType.Get(type).EqualsDelegate;
            if (type.IsGenericType)
            {
                Type genericType = type.GetGenericTypeDefinition();
                if (genericType == typeof(Nullable<>)) return StructGenericType.Get(type.GetGenericArguments()[0]).NullableDelegate;
            }
            foreach (Type interfaceType in type.GetInterfaces())
            {
                if (interfaceType.IsGenericType)
                {
                    Type genericType = interfaceType.GetGenericTypeDefinition();
                    if (genericType == typeof(IDictionary<,>)) return DictionaryGenericType.Get(type, interfaceType).EqualsDelegate;
                    if (genericType == typeof(ICollection<>)) return CollectionGenericType.Get(type, interfaceType).EqualsDelegate;
                }
            }
            if (type.IsPointer || type.IsInterface) return (Func<T, T, bool>)notSupport;
            return null;
        }
        static Comparor()
        {
            Delegate equalsDelegate = getDelegate();
            if (equalsDelegate != null)
            {
                Equals = (Func<T, T, bool>)equalsDelegate;
                MemberMapEquals = memberMapEquals;
                return;
            }
            Type type = typeof(T);
            MemberDynamicMethod dynamicMethod = new MemberDynamicMethod(type, false);
            foreach (FieldInfo field in type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly))
            {
                dynamicMethod.Push(field);
            }
            dynamicMethod.Base();
            Equals = (Func<T, T, bool>)dynamicMethod.Create<Func<T, T, bool>>();

            dynamicMethod = new MemberDynamicMethod(type, true);
            foreach (AutoCSer.Metadata.FieldIndex field in AutoCSer.Metadata.MemberIndexGroup<T>.GetFields(AutoCSer.Metadata.MemberFilters.InstanceField))
            {
                dynamicMethod.Push(field.Member, field.MemberIndex);
            }
            MemberMapEquals = (Func<T, T, AutoCSer.Metadata.MemberMap, bool>)dynamicMethod.Create<Func<T, T, AutoCSer.Metadata.MemberMap, bool>>();
        }
    }
}
