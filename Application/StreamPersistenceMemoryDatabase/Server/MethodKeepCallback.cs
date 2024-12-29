using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 方法调用回调包装
    /// </summary>
    /// <typeparam name="T">返回数据类型</typeparam>
    public sealed class MethodKeepCallback<T>// : AutoCSer.Threading.Link<MethodKeepCallback<T>>
    {
        /// <summary>
        /// 服务接口回调委托
        /// </summary>
#if NetStandard21
        internal CommandServerKeepCallback<KeepCallbackResponseParameter>? callback;
#else
        internal CommandServerKeepCallback<KeepCallbackResponseParameter> callback;
#endif
        /// <summary>
        /// 服务端节点方法标记
        /// </summary>
        internal readonly MethodFlagsEnum flag;
        /// <summary>
        /// 返回值类型是否 ResponseParameterSerializer
        /// </summary>
        private readonly bool isResponseParameter;
        /// <summary>
        /// 保留
        /// </summary>
        internal int Reserve;
        /// <summary>
        /// 是否已经取消保持回调
        /// </summary>
        public bool IsCancelKeepCallback { get { return callback == null || callback.IsCancelKeepCallback; } }
        /// <summary>
        /// 无回调
        /// </summary>
        private MethodKeepCallback() { }
        /// <summary>
        /// 方法调用回调包装
        /// </summary>
        /// <param name="callback">服务接口回调委托</param>
        /// <param name="flag">服务端节点方法标记</param>
        internal MethodKeepCallback(CommandServerKeepCallback<KeepCallbackResponseParameter> callback, MethodFlagsEnum flag)
        {
            this.callback = callback;
            this.flag = flag;
            isResponseParameter = typeof(T) == typeof(ResponseParameterSerializer);
        }
        /// <summary>
        /// 成功回调
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Callback(ResponseParameterSerializer value)
        {
            if (callback != null)
            {
                if (isResponseParameter)
                {
                    try
                    {
                        return callback.VirtualCallback(new KeepCallbackResponseParameter(value, 0));
                    }
                    catch (Exception exception)
                    {
                        AutoCSer.LogHelper.ExceptionIgnoreException(exception);
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
                if (!isResponseParameter)
                {
                    bool isParameter = false;
                    try
                    {
                        KeepCallbackResponseParameter responseParameter = KeepCallbackResponseParameter.Create(value, flag);
                        isParameter = true;
                        return callback.VirtualCallback(responseParameter);
                    }
                    catch (Exception exception)
                    {
                        AutoCSer.LogHelper.ExceptionIgnoreException(exception);
                    }
                    finally
                    {
                        if (!isParameter) CallbackCancelKeep(CallStateEnum.Unknown);
                    }
                }
                else
                {
                    try
                    {
                        return callback.VirtualCallback(new KeepCallbackResponseParameter(value.notNullCastType<ResponseParameterSerializer>(), 0));
                    }
                    catch (Exception exception)
                    {
                        AutoCSer.LogHelper.ExceptionIgnoreException(exception);
                    }
                }
                return false;
            }
            return true;
        }
        /// <summary>
        /// 成功回调
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal bool Callback(CommandServerCallQueue queue, ResponseParameterSerializer value)
        {
            if (callback != null)
            {
                if (isResponseParameter)
                {
                    try
                    {
                        return callback.VirtualCallback(new KeepCallbackResponseParameter(value, 0));
                    }
                    catch (Exception exception)
                    {
                        AutoCSer.LogHelper.ExceptionIgnoreException(exception);
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
        /// <param name="queue"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal bool Callback(CommandServerCallQueue queue, T value)
        {
            if (callback != null)
            {
                if (!isResponseParameter)
                {
                    bool isParameter = false;
                    try
                    {
                        KeepCallbackResponseParameter responseParameter = KeepCallbackResponseParameter.Create(value, flag);
                        isParameter = true;
                        return callback.VirtualCallback(responseParameter);
                    }
                    catch (Exception exception)
                    {
                        AutoCSer.LogHelper.ExceptionIgnoreException(exception);
                    }
                    finally
                    {
                        if (!isParameter) CallbackCancelKeep(CallStateEnum.Unknown);
                    }
                }
                else
                {
                    try
                    {
                        return callback.VirtualCallback(new KeepCallbackResponseParameter(value.notNullCastType<ResponseParameterSerializer>(), 0));
                    }
                    catch (Exception exception)
                    {
                        AutoCSer.LogHelper.ExceptionIgnoreException(exception);
                    }
                }
                return false;
            }
            return true;
        }
        /// <summary>
        /// 成功回调
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public bool Callback(IEnumerable<T> values)
        {
            if (callback != null && values != null)
            {
                bool isParameter = false, isCallback = false;
                try
                {
                    IEnumerable<KeepCallbackResponseParameter> responseParameter = KeepCallbackResponseParameter.CreateValues(values, flag);
                    isParameter = true;
                    isCallback = callback.Callback(responseParameter, false);
                }
                catch (Exception exception)
                {
                    AutoCSer.LogHelper.ExceptionIgnoreException(exception);
                }
                finally
                {
                    if (!isParameter) CallbackCancelKeep(CallStateEnum.Unknown);
                }
                return isCallback;
            }
            return true;
        }
        /// <summary>
        /// 失败回调
        /// </summary>
        /// <param name="state">失败状态</param>
        public void CallbackCancelKeep(CallStateEnum state)
        {
            if (callback != null)
            {
                try
                {
                    callback.VirtualCallbackCancelKeep(new KeepCallbackResponseParameter(state));
                }
                catch (Exception exception)
                {
                    AutoCSer.LogHelper.ExceptionIgnoreException(exception);
                }
                callback = null;
            }
        }
        /// <summary>
        /// 取消保持回调命令
        /// </summary>
        public void CancelKeep()
        {
            if (callback != null)
            {
                try
                {
                    callback.CancelKeep();
                }
                catch(Exception exception)
                {
                    AutoCSer.LogHelper.ExceptionIgnoreException(exception);
                }
                callback = null;
            }
        }

        /// <summary>
        /// 创建方法调用回调包装对象委托类型
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="flag">服务端节点方法标记</param>
        /// <returns></returns>
#if NetStandard21
        internal delegate MethodKeepCallback<T> CreateDelegate(ref CommandServerKeepCallback<KeepCallbackResponseParameter>? callback, MethodFlagsEnum flag);
#else
        internal delegate MethodKeepCallback<T> CreateDelegate(ref CommandServerKeepCallback<KeepCallbackResponseParameter> callback, MethodFlagsEnum flag);
#endif
        /// <summary>
        /// 创建方法调用回调包装对象
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static MethodKeepCallback<T> Create(ref CommandServerKeepCallback<KeepCallbackResponseParameter>? callback, MethodFlagsEnum flag)
#else
        internal static MethodKeepCallback<T> Create(ref CommandServerKeepCallback<KeepCallbackResponseParameter> callback, MethodFlagsEnum flag)
#endif
        {
            if (callback != null)
            {
                MethodKeepCallback<T> methodKeepCallback = new MethodKeepCallback<T>(callback, flag);
                callback = null;
                return methodKeepCallback;
            }
            return NullCallback;
        }
        /// <summary>
        /// 创建方法调用回调包装对象
        /// </summary>
        /// <param name="methodParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static MethodKeepCallback<T> Create(InputKeepCallbackMethodParameter methodParameter)
        {
            return methodParameter.CreateMethodKeepCallback<T>();
        }
        /// <summary>
        /// 无回调
        /// </summary>
        internal static readonly MethodKeepCallback<T> NullCallback = new MethodKeepCallback<T>();
    }
}
