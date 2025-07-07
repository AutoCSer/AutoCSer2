using AutoCSer.Extensions;
using AutoCSer.Metadata;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 成员复制
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    public static class MemberCopy<
#if AOT
        [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.NonPublicMethods)]
#endif
    T>
    {
        /// <summary>
        /// 对象成员复制
        /// </summary>
        /// <param name="writeValue">写入数据对象</param>
        /// <param name="readValue">读取数据对象</param>
        /// <param name="memberMap">成员位图</param>
#if NetStandard21
        public static void Copy(ref T writeValue, T readValue, MemberMap<T>? memberMap = null)
#else
        public static void Copy(ref T writeValue, T readValue, MemberMap<T> memberMap = null)
#endif
        {
            if (isValueCopy) writeValue = readValue;
            else if (memberMap == null || memberMap.IsDefault) defaultCopyer(ref writeValue, readValue);
            else defaultMemberCopyer(ref writeValue, readValue, memberMap);
        }
        /// <summary>
        /// 对象成员复制
        /// </summary>
        /// <param name="writeValue">写入数据对象</param>
        /// <param name="readValue">读取数据对象</param>
        /// <param name="memberMap">成员位图</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void Copy(T writeValue, T readValue, MemberMap<T>? memberMap = null)
#else
        public static void Copy(T writeValue, T readValue, MemberMap<T> memberMap = null)
#endif
        {
            if (memberMap == null || memberMap.IsDefault) defaultCopyer(ref writeValue, readValue);
            else defaultMemberCopyer(ref writeValue, readValue, memberMap);
        }
        /// <summary>
        /// 成员复制委托
        /// </summary>
        /// <param name="writeValue">写入数据对象</param>
        /// <param name="readValue">读取数据对象</param>
        internal delegate void Copyer(ref T writeValue, T readValue);
        /// <summary>
        /// 成员复制委托
        /// </summary>
        /// <param name="writeValue">写入数据对象</param>
        /// <param name="readValue">读取数据对象</param>
        /// <param name="memberMap">成员位图</param>
        internal delegate void MemberMapCopyer(ref T writeValue, T readValue, MemberMap<T> memberMap);
        /// <summary>
        /// Whether it is a value type
        /// 是否值类型
        /// </summary>
        private static readonly bool isValueType;
        /// <summary>
        /// 是否采用值类型复制模式
        /// </summary>
        private static readonly bool isValueCopy;
        /// <summary>
        /// 默认成员复制委托
        /// </summary>
        private static readonly Copyer defaultCopyer;
        /// <summary>
        /// 默认成员复制委托
        /// </summary>
        private static readonly MemberMapCopyer defaultMemberCopyer;
        /// <summary>
        /// Copy the array
        /// </summary>
        /// <param name="writeArray"></param>
        /// <param name="readArray"></param>
        internal static void CopyArray(ref T[] writeArray, T[] readArray)
        {
            if (readArray != null)
            {
                if (readArray.Length == 0)
                {
                    if (writeArray == null) writeArray = EmptyArray<T>.Array;
                    return;
                }
                if (writeArray == null || writeArray.Length < readArray.Length) System.Array.Resize(ref writeArray, readArray.Length);
                System.Array.Copy(readArray, 0, writeArray, 0, readArray.Length);
            }
        }
        /// <summary>
        /// Copy the array
        /// </summary>
        /// <param name="writeArray"></param>
        /// <param name="readArray"></param>
        /// <param name="memberMap"></param>
        internal static void CopyArray(ref T[] writeArray, T[] readArray, MemberMap<T[]> memberMap)
        {
            CopyArray(ref writeArray, readArray);
        }
        /// <summary>
        /// 没有复制字段
        /// </summary>
        /// <param name="writeValue"></param>
        /// <param name="readValue"></param>
        private static void noCopy(ref T writeValue, T readValue)
        {
        }
        /// <summary>
        /// 没有复制字段
        /// </summary>
        /// <param name="writeValue"></param>
        /// <param name="readValue"></param>
        /// <param name="memberMap"></param>
        private static void noCopy(ref T writeValue, T readValue, MemberMap<T> memberMap)
        {
        }
        /// <summary>
        /// 对象浅复制
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T MemberwiseClone(T value)
        {
            return !isValueType && value != null ? (T)DefaultConstructor.CallMemberwiseClone(value) : value;
        }

        static MemberCopy()
        {
            Type type = typeof(T);
            isValueType = type.IsValueType;
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1)
                {
#if AOT
                    //Type elementType = type.GetElementType().notNull();
                    //defaultCopyer = (Copyer)MemberCopyMethod.CopyArrayMethod.MakeGenericMethod(elementType).CreateDelegate(typeof(Copyer));
                    //defaultMemberCopyer = (MemberMapCopyer)MemberCopyMethod.CopyArrayMemberMapMethod.MakeGenericMethod(elementType).CreateDelegate(typeof(MemberMapCopyer));
#else
                    AutoCSer.Extensions.Metadata.GenericType genericType = AutoCSer.Extensions.Metadata.GenericType.Get(type.GetElementType().notNull());
                    defaultCopyer = (Copyer)genericType.MemberCopyArrayDelegate;
                    defaultMemberCopyer = (MemberMapCopyer)genericType.MemberMapCopyArrayDelegate;
#endif
                    return;
                }
            }
            else
            {
                if (type.IsEnum || type.IsPointer || type.IsInterface || typeof(Delegate).IsAssignableFrom(type)) isValueCopy = true;
                else
                {
#if AOT
                    Type refType = type.MakeByRefType();
                    var method = type.GetMethod(AutoCSer.CodeGenerator.MemberCopyAttribute.MemberCopyMethodName, BindingFlags.Static | BindingFlags.NonPublic, new Type[] { refType, type });
                    if (method != null && !method.IsGenericMethod && method.ReturnType == typeof(void))
                    {
                        var memberMapMethod = type.GetMethod(AutoCSer.CodeGenerator.MemberCopyAttribute.MemberMapCopyMethodName, BindingFlags.Static | BindingFlags.NonPublic, new Type[] { refType, type, typeof(MemberMap<T>) });
                        if (memberMapMethod != null && !memberMapMethod.IsGenericMethod && memberMapMethod.ReturnType == typeof(void))
                        {
                            defaultCopyer = (Copyer)method.CreateDelegate(typeof(Copyer));
                            defaultMemberCopyer = (MemberMapCopyer)memberMapMethod.CreateDelegate(typeof(MemberMapCopyer));
                            return;
                        }
                        throw new MissingMethodException(typeof(T).fullName(), AutoCSer.CodeGenerator.MemberCopyAttribute.MemberMapCopyMethodName);
                    }
                    throw new MissingMethodException(typeof(T).fullName(), AutoCSer.CodeGenerator.MemberCopyAttribute.MemberCopyMethodName);
#else
                    LeftArray<FieldIndex> fields = MemberIndexGroup.GetAnonymousFields(type);
                    if (fields.Length != 0)
                    {
                        Type refType = type.MakeByRefType();
                        GenericType genericType = new GenericType<T>();
                        AutoCSer.MemberCopy.MemberDynamicMethod dynamicMethod = new AutoCSer.MemberCopy.MemberDynamicMethod(type, new DynamicMethod("MemberCopyer", null, new Type[] { refType, type }, type, true));
                        AutoCSer.MemberCopy.MemberDynamicMethod memberMapDynamicMethod = new AutoCSer.MemberCopy.MemberDynamicMethod(type, new DynamicMethod("MemberMapCopyer", null, new Type[] { refType, type, typeof(MemberMap<T>) }, type, true));
                        foreach (FieldIndex field in fields)
                        {
                            dynamicMethod.Push(field);
                            memberMapDynamicMethod.PushMemberMap(field, genericType);
                        }
                        defaultCopyer = (Copyer)dynamicMethod.Create(typeof(Copyer));
                        defaultMemberCopyer = (MemberMapCopyer)memberMapDynamicMethod.Create(typeof(MemberMapCopyer));
                        return;
                    }
#endif
                }
            }
            defaultCopyer = noCopy;
            defaultMemberCopyer = noCopy;
        }
    }
}
