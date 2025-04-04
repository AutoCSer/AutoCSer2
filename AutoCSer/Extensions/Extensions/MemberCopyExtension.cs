using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 成员复制扩展操作
    /// </summary>
    public static class MemberCopyExtension
    {
        /// <summary>
        /// 对象成员复制
        /// </summary>
        /// <typeparam name="VT">对象类型</typeparam>
        /// <typeparam name="BT">复制成员对象类型</typeparam>
        /// <param name="value">被复制对象</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static VT? memberCopy<VT, BT>(this BT? value, AutoCSer.Metadata.MemberMap<BT>? memberMap = null)
#else
        public static VT memberCopy<VT, BT>(this BT value, AutoCSer.Metadata.MemberMap<BT> memberMap = null)
#endif
            where VT : class, BT
        {
            if (value == null) return null;
            VT newValue = AutoCSer.Metadata.DefaultConstructor<VT>.Constructor().notNull();
            AutoCSer.MemberCopy<BT>.Copy(newValue, value, memberMap);
            return newValue;
        }
        /// <summary>
        /// 对象成员复制
        /// </summary>
        /// <typeparam name="VT">对象类型</typeparam>
        /// <typeparam name="BT">复制成员对象类型</typeparam>
        /// <param name="value">被复制对象</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static VT? memberCopy<VT, BT>(this VT? value, AutoCSer.Metadata.MemberMap<BT>? memberMap = null)
#else
        public static VT memberCopy<VT, BT>(this VT value, AutoCSer.Metadata.MemberMap<BT> memberMap = null)
#endif
            where VT : class, BT
        {
#if NetStandard21
            return memberCopy<VT, BT>((BT?)value, memberMap);
#else
            return memberCopy<VT, BT>((BT)value, memberMap);
#endif
        }
        /// <summary>
        /// 对象成员复制
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="value">被复制对象</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static T? memberCopy<T>(this T? value, AutoCSer.Metadata.MemberMap<T>? memberMap = null)
#else
        public static T memberCopy<T>(this T value, AutoCSer.Metadata.MemberMap<T> memberMap = null)
#endif
            where T : class
        {
            if (value == null) return null;
            T newValue = AutoCSer.Metadata.DefaultConstructor<T>.Constructor().notNull();
            AutoCSer.MemberCopy<T>.Copy(newValue, value, memberMap);
            return newValue;
        }
        /// <summary>
        /// 对象成员复制
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="value">读取数据对象</param>
        /// <param name="writeValue">写入数据对象</param>
        /// <param name="memberMap">成员位图</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void memberCopyTo<T>(this T value, T writeValue, AutoCSer.Metadata.MemberMap<T>? memberMap = null)
#else
        public static void memberCopyTo<T>(this T value, T writeValue, AutoCSer.Metadata.MemberMap<T> memberMap = null)
#endif
            where T : class
        {
            AutoCSer.MemberCopy<T>.Copy(writeValue, value, memberMap);
        }
        /// <summary>
        /// 对象成员复制
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="value">读取数据对象</param>
        /// <param name="writeValue">写入数据对象</param>
        /// <param name="memberMap">成员位图</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void memberCopyTo<T>(this T value, ref T writeValue, AutoCSer.Metadata.MemberMap<T>? memberMap = null)
#else
        public static void memberCopyTo<T>(this T value, ref T writeValue, AutoCSer.Metadata.MemberMap<T> memberMap = null)
#endif
        {
            AutoCSer.MemberCopy<T>.Copy(ref writeValue, value, memberMap);
        }
        /// <summary>
        /// 对象成员复制
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="value">写入数据对象</param>
        /// <param name="readValue">读取数据对象</param>
        /// <param name="memberMap">成员位图</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void memberCopyFrom<T>(this T value, T readValue, AutoCSer.Metadata.MemberMap<T>? memberMap = null)
#else
        public static void memberCopyFrom<T>(this T value, T readValue, AutoCSer.Metadata.MemberMap<T> memberMap = null)
#endif
            where T : class
        {
            AutoCSer.MemberCopy<T>.Copy(value, readValue, memberMap);
        }

        /// <summary>
        /// 对象浅复制
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T memberwiseClone<T>(this T value)
        {
            return AutoCSer.MemberCopy<T>.MemberwiseClone(value);
        }
    }
}
