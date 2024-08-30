using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using System;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 方法调用回调包装扩展操作
    /// </summary>
    public static class MethodKeepCallbackExtension
    {
        /// <summary>
        /// 返回数据链表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methodCallback"></param>
        /// <param name="head">开始节点</param>
        /// <param name="end">结束节点</param>
        /// <returns></returns>
        internal static bool Callback<T>(this MethodKeepCallback<T> methodCallback, T head, T end)
             where T : KeepCallbackReturnValueLink<T>
        {
#if DEBUG
            if (head == null) throw new ArgumentNullException("head is null");
#endif
            CommandServerKeepCallback<KeepCallbackResponseParameter> callback = methodCallback.callback;
            if (callback != null && callback.IsCancelKeep == 0 && !callback.Socket.IsClose)
            {
                bool isPush = false;
                try
                {
                    ServerOutput outputHead = null, outputEnd = null;
                    do
                    {
                        ServerReturnValue<KeepCallbackResponseParameter> outputParameter = new ServerReturnValue<KeepCallbackResponseParameter>(KeepCallbackResponseParameter.Create(head, methodCallback.IsSimpleSerialize));
                        ServerOutput output = callback.Socket.GetOutput(callback.CallbackIdentity, callback.Method, ref outputParameter);
                        if (outputHead == null) outputHead = output;
                        else outputEnd.LinkNext = output;
                        outputEnd = output;
                    }
                    while ((head = head.LinkNext) != end);
                    callback.Socket.Push(outputHead, outputEnd);
                    return isPush = true;
                }
                catch (Exception exception)
                {
                    AutoCSer.LogHelper.ExceptionIgnoreException(exception);
                }
                finally
                {
                    if (!isPush) methodCallback.CallbackCancelKeep(CallStateEnum.Unknown);
                }
            }
            return false;
        }
    }
}
