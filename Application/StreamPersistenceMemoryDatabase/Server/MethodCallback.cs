using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 方法调用回调包装
    /// </summary>
    /// <typeparam name="T">返回数据类型</typeparam>
    public sealed class MethodCallback<T> : AutoCSer.Threading.Link<MethodCallback<T>>
    {
        /// <summary>
        /// 服务接口回调委托
        /// </summary>
#if NetStandard21
        private readonly CommandServerCallback<ResponseParameter>? callback;
#else
        private readonly CommandServerCallback<ResponseParameter> callback;
#endif
        /// <summary>
        /// 是否简单序列化输出数据
        /// </summary>
        private readonly bool isSimpleSerialize;
        /// <summary>
        /// 返回值类型是否 ResponseParameter
        /// </summary>
        private readonly bool isResponseParameter;
        /// <summary>
        /// 保留
        /// </summary>
        internal ushort Reserve;
        /// <summary>
        /// 无回调
        /// </summary>
        private MethodCallback() { }
        /// <summary>
        /// 方法调用回调包装
        /// </summary>
        /// <param name="callback">服务接口回调委托</param>
        /// <param name="isSimpleSerialize">是否简单序列化输出数据</param>
        internal MethodCallback(CommandServerCallback<ResponseParameter> callback, bool isSimpleSerialize)
        {
            this.callback = callback;
            this.isSimpleSerialize = isSimpleSerialize;
            isResponseParameter = typeof(T) == typeof(ResponseParameter);
        }
        /// <summary>
        /// 成功回调
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Callback(ResponseParameter value)
        {
            if (callback != null)
            {
                if (isResponseParameter) return callback.Callback(value);
                throw new InvalidCastException();
            }
            return true;
        }
        /// <summary>
        /// 成功回调
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Callback(T value)
        {
            if (callback != null)
            {
                if (!isResponseParameter)
                {
                    bool isCallback;
                    ResponseParameter responseParameter = new ResponseParameter(CallStateEnum.Unknown);
                    try
                    {
                        responseParameter = ResponseParameter.Create(value, isSimpleSerialize);
                    }
                    finally { isCallback = callback.Callback(responseParameter); }
                    return isCallback;
                }
                return callback.Callback(value.notNullCastType<ResponseParameter>());
            }
            return true;
        }
        /// <summary>
        /// 成功回调
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal bool SynchronousCallback(T value)
        {
            if (callback != null)
            {
                bool isCallback;
                ResponseParameter responseParameter = new ResponseParameter(CallStateEnum.Unknown);
                try
                {
                    responseParameter = ResponseParameter.Create(value, isSimpleSerialize);
                }
                finally { isCallback = callback.SynchronousCallback(responseParameter); }
                return isCallback;
            }
            return true;
        }
        /// <summary>
        /// 失败回调
        /// </summary>
        /// <param name="state">失败状态</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Callback(CallStateEnum state)
        {
            return callback == null || callback.Callback(new ResponseParameter(state));
        }
        ///// <summary>
        ///// 失败回调
        ///// </summary>
        ///// <param name="state">失败状态</param>
        ///// <returns></returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal bool SynchronousCallback(CallStateEnum state)
        //{
        //    return callback == null || callback.SynchronousCallback(new ResponseParameter(state));
        //}

        /// <summary>
        /// 创建方法调用回调包装对象委托类型
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="isSimpleSerialize"></param>
        /// <returns></returns>
#if NetStandard21
        internal delegate MethodCallback<T> CreateDelegate(ref CommandServerCallback<ResponseParameter>? callback, bool isSimpleSerialize);
#else
        internal delegate MethodCallback<T> CreateDelegate(ref CommandServerCallback<ResponseParameter> callback, bool isSimpleSerialize);
#endif
        /// <summary>
        /// 创建方法调用回调包装对象
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="isSimpleSerialize"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static MethodCallback<T> Create(ref CommandServerCallback<ResponseParameter>? callback, bool isSimpleSerialize)
#else
        internal static MethodCallback<T> Create(ref CommandServerCallback<ResponseParameter> callback, bool isSimpleSerialize)
#endif
        {
            if (callback != null)
            {
                MethodCallback<T> methodCallback = new MethodCallback<T>(callback, isSimpleSerialize);
                callback = null;
                return methodCallback;
            }
            return NullCallback;
        }
        /// <summary>
        /// 创建方法调用回调包装对象
        /// </summary>
        /// <param name="methodParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static MethodCallback<T> Create(CallInputOutputMethodParameter methodParameter)
        {
            return methodParameter.CreateMethodCallback<T>();
        }
        /// <summary>
        /// 无回调
        /// </summary>
        internal static readonly MethodCallback<T> NullCallback = new MethodCallback<T>();
    }
}
