using AutoCSer.Net;
using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 本地服务调用节点方法队列节点回调对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class LocalServiceKeepCallbackNodeCallback<T> : CommandServerKeepCallback<KeepCallbackResponseParameter>
    {
        /// <summary>
        /// 本地服务调用节点方法队列节点回调输出
        /// </summary>
        internal readonly LocalServiceKeepCallbackNodeKeepCallbackResponse<T> Response;
        /// <summary>
        /// 本地服务调用节点方法队列节点回调对象
        /// </summary>
        internal LocalServiceKeepCallbackNodeCallback()
        {
            IsCancelKeep = 0;
            Response = new LocalServiceKeepCallbackNodeKeepCallbackResponse<T>(this);
        }
        /// <summary>
        /// 取消保持回调命令
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="exception"></param>
        public override void CancelKeep(CommandClientReturnTypeEnum returnType = CommandClientReturnTypeEnum.Success, Exception exception = null)
        {
            if (IsCancelKeep == 0)
            {
                SetCancelKeep();
                Response.CancelKeep(returnType, exception);
            }
        }
        /// <summary>
        /// 返回值回调并结束回调
        /// </summary>
        /// <param name="returnValue"></param>
        public override void VirtualCallbackCancelKeep(KeepCallbackResponseParameter returnValue)
        {
            if (VirtualCallback(returnValue)) CancelKeep();
        }
        /// <summary>
        /// 返回值回调
        /// </summary>
        /// <param name="returnValue"></param>
        public override bool VirtualCallback(KeepCallbackResponseParameter returnValue)
        {
            if(IsCancelKeep == 0)
            {
                Response.Callback(returnValue);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 返回数据集合
        /// </summary>
        /// <param name="values"></param>
        /// <param name="isCancel">回调完成之后是否关闭</param>
        /// <returns></returns>
        public override bool Callback(IEnumerable<KeepCallbackResponseParameter> values, bool isCancel = true)
        {
            if (IsCancelKeep == 0)
            {
                if (values != null)
                {
                    try
                    {
                        foreach(KeepCallbackResponseParameter response in values) Response.Callback(response);
                    }
                    catch (Exception exception)
                    {
                        SetCancelKeep();
                        Response.CancelKeep(CommandClientReturnTypeEnum.ClientException, exception);
                    }
                }
                if (isCancel && IsCancelKeep == 0)
                {
                    SetCancelKeep();
                    Response.CancelKeep(CommandClientReturnTypeEnum.Success);
                }
                return true;
            }
            return false;
        }
    }
}
