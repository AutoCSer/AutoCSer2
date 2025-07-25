﻿using AutoCSer.Net;
using System;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// TCP 服务器端异步保持回调扩展操作
    /// </summary>
    public static class CommandServerKeepCallbackExtension
    {
        /// <summary>
        /// 返回数据链表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callback"></param>
        /// <param name="head">Head node
        /// 头节点</param>
        /// <param name="getCount">Get the quantity
        /// 获取数量</param>
        /// <param name="end">End node
        /// 结束节点</param>
        /// <param name="endCount">Actual end number
        /// 实际结束数量</param>
        /// <param name="isCancel">Whether to close the callback after the callback is completed
        /// 回调完成之后是否关闭回调</param>
        /// <returns></returns>
#if NetStandard21
        public static bool Callback<T>(this CommandServerKeepCallback<T> callback, T head, int getCount, out T? end, out int endCount, bool isCancel = true)
#else
        public static bool Callback<T>(this CommandServerKeepCallback<T> callback, T head, int getCount, out T end, out int endCount, bool isCancel = true)
#endif
             where T : KeepCallbackReturnValueLink<T>
        {
            if (callback.IsCancelKeep == 0)
            {
                end = KeepCallbackReturnValueLink<T>.GetEnd(head, getCount, out endCount);
                if (endCount != 0)
                {
                    try
                    {
                        if (!callback.Socket.SendKeepCallbackLink(callback.CallbackIdentity, callback.Method, head, end)) callback.SetCancelKeep();
                    }
                    catch (Exception exception)
                    {
                        callback.SetCancelKeep();
                        callback.Socket.RemoveKeepCallback(callback.CallbackIdentity, exception);
                    }
                }
                if (isCancel && callback.IsCancelKeep == 0)
                {
                    callback.SetCancelKeep();
                    callback.Socket.RemoveKeepCallback(callback.CallbackIdentity, CommandClientReturnTypeEnum.Success);
                }
                return true;
            }
            end = head;
            endCount = 0;
            return false;
        }
    }
}
