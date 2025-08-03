using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 泛型扩展操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct ClassGenericTypeExtensions<T> where T : class
    {
        /// <summary>
        /// 泛型对象
        /// </summary>
#if NetStandard21
        private readonly T? value;
#else
        private readonly T value;
#endif
        /// <summary>
        /// 泛型扩展操作
        /// </summary>
        /// <param name="value"></param>
#if NetStandard21
        public ClassGenericTypeExtensions(T? value)
#else
        public ClassGenericTypeExtensions(T value)
#endif
        {
            this.value = value;
        }
        /// <summary>
        /// Non-empty object type conversion
        /// 非空对象类型转换
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public T NotNull()
        {
            return value.notNull();
        }
        /// <summary>
        /// Convert an array of nullable reference types to an array
        /// 可空引用类型数组转换为数组
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public T?[] CastArray()
#else
        public T[] CastArray()
#endif
        {
            return value.castArray();
        }
        /// <summary>
        /// 对象成员复制
        /// </summary>
        /// <typeparam name="VT">对象类型</typeparam>
        /// <param name="memberMap">成员位图</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public VT? MemberCopy<
#if AOT
                [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
#endif
        VT>(AutoCSer.Metadata.MemberMap<T>? memberMap = null)
#else
        public VT MemberCopy<VT>(AutoCSer.Metadata.MemberMap<T> memberMap = null)
#endif
            where VT : class, T
        {
            return value.memberCopy<VT, T>(memberMap);
        }
        /// <summary>
        /// 对象成员复制
        /// </summary>
        /// <param name="memberMap">成员位图</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public T? MemberCopy(AutoCSer.Metadata.MemberMap<T>? memberMap = null)
#else
        public T MemberCopy(AutoCSer.Metadata.MemberMap<T> memberMap = null)
#endif
        {
            return value.memberCopy<T>(memberMap);
        }
        /// <summary>
        /// 对象成员复制
        /// </summary>
        /// <param name="writeValue">写入数据对象</param>
        /// <param name="memberMap">成员位图</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void MemberCopyTo(T writeValue, AutoCSer.Metadata.MemberMap<T>? memberMap = null)
#else
        public void MemberCopyTo(T writeValue, AutoCSer.Metadata.MemberMap<T> memberMap = null)
#endif
        {
            value.notNull().memberCopyTo<T>(writeValue, memberMap);
        }
        /// <summary>
        /// 对象成员复制
        /// </summary>
        /// <param name="readValue">读取数据对象</param>
        /// <param name="memberMap">成员位图</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void MemberCopyFrom(T readValue, AutoCSer.Metadata.MemberMap<T>? memberMap = null)
#else
        public void MemberCopyFrom(T readValue, AutoCSer.Metadata.MemberMap<T> memberMap = null)
#endif
        {
            value.notNull().memberCopyFrom(readValue, memberMap);
        }
    }
}
