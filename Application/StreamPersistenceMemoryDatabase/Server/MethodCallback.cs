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
        /// 保留
        /// </summary>
        internal int Reserve;
        /// <summary>
        /// 服务端节点方法标记
        /// </summary>
        private readonly MethodFlagsEnum flag;
        /// <summary>
        /// 返回值类型是否 ResponseParameter
        /// </summary>
        private readonly bool isResponseParameter;
        /// <summary>
        /// 是否已经回调操作
        /// </summary>
        private bool isCallback;
        /// <summary>
        /// 无回调
        /// </summary>
        private MethodCallback() { }
        /// <summary>
        /// 方法调用回调包装
        /// </summary>
        /// <param name="callback">服务接口回调委托</param>
        /// <param name="flag">服务端节点方法标记</param>
        internal MethodCallback(CommandServerCallback<ResponseParameter> callback, MethodFlagsEnum flag)
        {
            this.callback = callback;
            this.flag = flag;
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
                if (isResponseParameter)
                {
                    if (!isCallback)
                    {
                        isCallback = true;
                        return callback.Callback(value);
                    }
                    return false;
                }
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
                if (!this.isCallback)
                {
                    this.isCallback = true;
                    if (!isResponseParameter)
                    {
                        bool isCallback;
                        ResponseParameter responseParameter = ResponseParameter.CallStates[(byte)CallStateEnum.Unknown];
                        try
                        {
                            responseParameter = ResponseParameter.Create(value, flag);
                        }
                        finally { isCallback = callback.Callback(responseParameter); }
                        return isCallback;
                    }
                    return callback.Callback(value.notNullCastType<ResponseParameter>());
                }
                return false;
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
                if (!this.isCallback)
                {
                    this.isCallback = true;
                    bool isCallback;
                    ResponseParameter responseParameter = ResponseParameter.CallStates[(byte)CallStateEnum.Unknown];
                    try
                    {
                        responseParameter = ResponseParameter.Create(value, flag);
                    }
                    finally { isCallback = callback.SynchronousCallback(responseParameter); }
                    return isCallback;
                }
                return false;
            }
            return true;
        }
        /// <summary>
        /// 失败回调
        /// </summary>
        /// <param name="state">失败状态</param>
        /// <returns></returns>
        public bool Callback(CallStateEnum state)
        {
            if (callback == null) return true;
            if (!isCallback)
            {
                isCallback = true;
                return callback.Callback(ResponseParameter.CallStates[(byte)state]);
            }
            return false;
        }
        /// <summary>
        /// 失败回调
        /// </summary>
        /// <param name="state">失败状态</param>
        /// <returns></returns>
        internal bool SynchronousCallback(CallStateEnum state)
        {
            if (callback == null) return true;
            if (!isCallback)
            {
                isCallback = true;
                return callback.SynchronousCallback(ResponseParameter.CallStates[(byte)state]);
            }
            return false;
        }
        /// <summary>
        /// 获取服务接口回调委托
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static CommandServerCallback<ResponseParameter>? GetCallback(MethodCallback<T> callback)
#else
        public static CommandServerCallback<ResponseParameter> GetCallback(MethodCallback<T> callback)
#endif
        {
            return callback.callback;
        }

        /// <summary>
        /// 创建方法调用回调包装对象委托类型
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
#if NetStandard21
        internal delegate MethodCallback<T> CreateDelegate(ref CommandServerCallback<ResponseParameter>? callback, MethodFlagsEnum flag);
#else
        internal delegate MethodCallback<T> CreateDelegate(ref CommandServerCallback<ResponseParameter> callback,MethodFlagsEnum flag);
#endif
        /// <summary>
        /// 创建方法调用回调包装对象
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="flag">服务端节点方法标记</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static MethodCallback<T> Create(ref CommandServerCallback<ResponseParameter>? callback, MethodFlagsEnum flag)
#else
        internal static MethodCallback<T> Create(ref CommandServerCallback<ResponseParameter> callback, MethodFlagsEnum flag)
#endif
        {
            if (callback != null)
            {
                MethodCallback<T> methodCallback = new MethodCallback<T>(callback, flag);
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
        public static MethodCallback<T> Create(CallInputOutputMethodParameter methodParameter)
        {
            return methodParameter.CreateMethodCallback<T>();
        }
        /// <summary>
        /// 无回调
        /// </summary>
        public static readonly MethodCallback<T> NullCallback = new MethodCallback<T>();
    }
}
