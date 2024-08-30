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
        internal CommandServerKeepCallback<KeepCallbackResponseParameter> callback;
        /// <summary>
        /// 是否简单序列化输出数据
        /// </summary>
        internal readonly bool IsSimpleSerialize;
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
        /// <param name="isSimpleSerialize">是否简单序列化输出数据</param>
        internal MethodKeepCallback(ref CommandServerKeepCallback<KeepCallbackResponseParameter> callback, bool isSimpleSerialize)
        {
            this.callback = callback;
            this.IsSimpleSerialize = isSimpleSerialize;
            callback = null;
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
                bool isParameter = false, isCallback = false;
                try
                {
                    KeepCallbackResponseParameter responseParameter = KeepCallbackResponseParameter.Create(value, IsSimpleSerialize);
                    isParameter = true;
                    isCallback = callback.Callback(responseParameter);
                }
                catch(Exception exception)
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
                    IEnumerable<KeepCallbackResponseParameter> responseParameter = KeepCallbackResponseParameter.CreateValues(values, IsSimpleSerialize);
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
                    callback.CallbackCancelKeep(new KeepCallbackResponseParameter(state));
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
        /// <param name="isSimpleSerialize"></param>
        /// <returns></returns>
        internal delegate MethodKeepCallback<T> CreateDelegate(ref CommandServerKeepCallback<KeepCallbackResponseParameter> callback, bool isSimpleSerialize);
        /// <summary>
        /// 创建方法调用回调包装对象
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="isSimpleSerialize"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static MethodKeepCallback<T> Create(ref CommandServerKeepCallback<KeepCallbackResponseParameter> callback, bool isSimpleSerialize)
        {
            return callback != null ? new MethodKeepCallback<T>(ref callback, isSimpleSerialize) : NullCallback;
        }
        /// <summary>
        /// 创建方法调用回调包装对象
        /// </summary>
        /// <param name="methodParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static MethodKeepCallback<T> Create(InputKeepCallbackMethodParameter methodParameter)
        {
            return methodParameter.callback != null ? new MethodKeepCallback<T>(ref methodParameter.callback, methodParameter.Method.IsSimpleSerializeParamter) : NullCallback;
        }
        /// <summary>
        /// 无回调
        /// </summary>
        internal static readonly MethodKeepCallback<T> NullCallback = new MethodKeepCallback<T>();
    }
}
