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
    public struct GenericTypeExtensions<T>
    {
        /// <summary>
        /// 泛型对象
        /// </summary>
        private readonly T value;
        /// <summary>
        /// 泛型扩展操作
        /// </summary>
        /// <param name="value"></param>
        public GenericTypeExtensions(T value)
        {
            this.value = value;
        }
        /// <summary>
        /// 单个数据转换为可枚举集合
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public IEnumerable<T> ToEnumerable()
        {
            yield return value;
        }
        /// <summary>
        /// Generic values are converted to object
        /// 泛型值转换为 object
        /// </summary>
        /// <returns></returns>

        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public object CastObject()
        {
            return value.castObject();
        }
        /// <summary>
        /// 对象成员复制
        /// </summary>
        /// <param name="writeValue">写入数据对象</param>
        /// <param name="memberMap">成员位图</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void MemberCopyTo(ref T writeValue, AutoCSer.Metadata.MemberMap<T>? memberMap = null)
#else
        public void MemberCopyTo(ref T writeValue, AutoCSer.Metadata.MemberMap<T> memberMap = null)
#endif
        {
            value.memberCopyTo(ref writeValue, memberMap);
        }
        /// <summary>
        /// 对象浅复制
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public new T MemberwiseClone()
        {
            return value.memberwiseClone();
        }
    }
}
