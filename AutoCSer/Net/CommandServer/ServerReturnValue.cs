using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// Return value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct ServerReturnValue<T>
    {
        /// <summary>
        /// Return value
        /// </summary>
#if NetStandard21
        public T? ReturnValue;
#else
        public T ReturnValue;
#endif
        /// <summary>
        /// 异步返回值
        /// </summary>
        /// <param name="value"></param>
#if NetStandard21
        public ServerReturnValue(T? value)
#else
        public ServerReturnValue(T value)
#endif
        {
            ReturnValue = value;
        }
        ///// <summary>
        ///// 异步返回值
        ///// </summary>
        ///// <param name="value"></param>
        //public ServerReturnValue(ref T value)
        //{
        //    ReturnValue = value;
        //}
        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static T? GetReturnValue(ServerReturnValue<T> returnValue)
#else
        internal static T GetReturnValue(ServerReturnValue<T> returnValue)
#endif
        {
            return returnValue.ReturnValue;
        }
        /// <summary>
        /// Set the return value
        /// 设置返回值
        /// </summary>
        /// <param name="returnValue"></param>
        /// <param name="value"></param>
        internal delegate void SetReturnValueDelegate(ref ServerReturnValue<T> returnValue, ref T value);
        /// <summary>
        /// Set the return value
        /// 设置返回值
        /// </summary>
        /// <param name="returnValue"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void SetReturnValue(ref ServerReturnValue<T> returnValue, ref T value)
        {
            returnValue.ReturnValue = value;
        }
    }
}
