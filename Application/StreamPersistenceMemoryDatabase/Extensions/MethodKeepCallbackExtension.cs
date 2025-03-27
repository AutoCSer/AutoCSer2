using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using System;
using System.Reflection;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 方法调用回调包装扩展操作
    /// </summary>
    public static class MethodKeepCallbackExtension
    {
        ///// <summary>
        ///// 当前程序集
        ///// </summary>
        //private static readonly Assembly currentAssembly = typeof(MethodKeepCallbackExtension).Assembly;

        /// <summary>
        /// 返回数据链表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methodCallback"></param>
        /// <param name="head">开始节点</param>
        /// <param name="end">结束节点</param>
        /// <returns></returns>
#if NetStandard21
        internal static bool Callback<T>(this MethodKeepCallback<T> methodCallback, T head, T? end)
#else
        internal static bool Callback<T>(this MethodKeepCallback<T> methodCallback, T head, T end)
#endif
             where T : KeepCallbackReturnValueLink<T>
        {
#if DEBUG
            if (head == null) throw new ArgumentNullException("head is null");
#endif
            var callback = methodCallback.callback;
            if (callback != null && callback.IsCancelKeep == 0)
            {
                bool isPush = false;
                var next = head;
                try
                {
                    do
                    {
                        if (!callback.VirtualCallback(KeepCallbackResponseParameter.Create(next, methodCallback.flag))) return false;
                    }
                    while ((next = next.notNull().LinkNext) != end);
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
