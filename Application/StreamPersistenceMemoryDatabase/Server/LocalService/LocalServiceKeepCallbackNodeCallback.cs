using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 本地服务调用节点方法队列节点回调对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class LocalServiceKeepCallbackNodeCallback<T> : CommandServerKeepCallback<KeepCallbackResponseParameter>, IDisposable
    {
        /// <summary>
        /// 回调委托
        /// </summary>
        private readonly Action<LocalResult<T>> callback;
        /// <summary>
        /// Whether to synchronize the callback of the IO thread
        /// 是否 IO 线程同步回调
        /// </summary>
        private readonly bool isSynchronousCallback;
        /// <summary>
        /// 本地服务调用节点方法队列节点回调对象
        /// </summary>
        /// <param name="callback">回调委托</param>
        /// <param name="isSynchronousCallback">Whether to synchronize the callback of the IO thread
        /// 是否 IO 线程同步回调</param>
        internal LocalServiceKeepCallbackNodeCallback(Action<LocalResult<T>> callback, bool isSynchronousCallback)
        {
            IsCancelKeep = 0;
            this.callback = callback;
            this.isSynchronousCallback = isSynchronousCallback;
        }
        /// <summary>
        /// Release resources
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            SetCancelKeep();
        }
        /// <summary>
        /// 取消保持回调命令
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="exception"></param>
#if NetStandard21
        public override void CancelKeep(CommandClientReturnTypeEnum returnType = CommandClientReturnTypeEnum.Success, Exception? exception = null)
#else
        public override void CancelKeep(CommandClientReturnTypeEnum returnType = CommandClientReturnTypeEnum.Success, Exception exception = null)
#endif
        {
            if (IsCancelKeep == 0)
            {
                SetCancelKeep();
                if (returnType != CommandClientReturnTypeEnum.Success) callbackResult(new LocalResult<T>(CallStateEnum.Unknown, exception));
            }
        }
        /// <summary>
        /// 回调操作
        /// </summary>
        /// <param name="reuslt"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void callbackResult(LocalResult<T> reuslt)
        {
            if (isSynchronousCallback) callbackTask(reuslt);
            else Task.Run(() => callbackTask(reuslt));
        }
        /// <summary>
        /// 回调任务操作
        /// </summary>
        /// <param name="reuslt"></param>
        private void callbackTask(LocalResult<T> reuslt)
        {
            try
            {
                callback(reuslt);
            }
            catch (Exception excepption)
            {
                AutoCSer.LogHelper.ExceptionIgnoreException(excepption);
            }
        }
        /// <summary>
        /// Return value callback
        /// 返回值回调
        /// </summary>
        /// <param name="returnValue"></param>
        private bool callbackResponseParameter(ref KeepCallbackResponseParameter returnValue)
        {
            if (IsCancelKeep == 0)
            {
                if (returnValue.State == CallStateEnum.Success)
                {
                    if ((returnValue.flag & MethodFlagsEnum.IsSimpleSerializeParamter) != 0) callbackResult(((ResponseParameterSimpleSerializer<T>)returnValue.Serializer).Value.ReturnValue);
                    else callbackResult(((ResponseParameterBinarySerializer<T>)returnValue.Serializer).Value.ReturnValue);
                }
                else callbackResult(returnValue.State);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 返回值回调并结束回调
        /// </summary>
        /// <param name="returnValue"></param>
        public override void VirtualCallbackCancelKeep(KeepCallbackResponseParameter returnValue)
        {
            if (callbackResponseParameter(ref returnValue)) CancelKeep();
        }
        /// <summary>
        /// Return value callback
        /// 返回值回调
        /// </summary>
        /// <param name="returnValue"></param>
        public override bool VirtualCallback(KeepCallbackResponseParameter returnValue)
        {
            return callbackResponseParameter(ref returnValue);
        }
        /// <summary>
        /// Return a collection of data
        /// 返回数据集合
        /// </summary>
        /// <param name="values"></param>
        /// <param name="isCancel">Whether to close the callback after the callback is completed
        /// 回调完成之后是否关闭回调</param>
        /// <returns></returns>
        public override bool Callback(IEnumerable<KeepCallbackResponseParameter> values, bool isCancel = true)
        {
            if (IsCancelKeep == 0)
            {
                if (isCancel) SetCancelKeep();
                if (values != null)
                {
                    if (isSynchronousCallback) callbackTask(values);
                    else Task.Run(() => callbackTask(values));
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 回调任务操作
        /// </summary>
        /// <param name="values"></param>
        private void callbackTask(IEnumerable<KeepCallbackResponseParameter> values)
        {
            try
            {
                foreach (KeepCallbackResponseParameter returnValue in values)
                {
                    if (returnValue.State == CallStateEnum.Success)
                    {
                        if ((returnValue.flag & MethodFlagsEnum.IsSimpleSerializeParamter) != 0) callbackTask(((ResponseParameterSimpleSerializer<T>)returnValue.Serializer).Value.ReturnValue);
                        else callbackTask(((ResponseParameterBinarySerializer<T>)returnValue.Serializer).Value.ReturnValue);
                    }
                    else callbackTask(returnValue.State);
                }
            }
            catch (Exception exception)
            {
                SetCancelKeep();
                callbackTask(new LocalResult<T>(CallStateEnum.Unknown, exception));
            }
        }
    }
}