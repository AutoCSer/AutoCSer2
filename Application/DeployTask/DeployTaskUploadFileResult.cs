using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 上传文件结果
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct DeployTaskUploadFileResult
    {
        /// <summary>
        /// 文件上传状态
        /// </summary>
        public DeployTaskUploadFileStateEnum State;
        /// <summary>
        /// 客户端调用返回值
        /// </summary>
        public CommandClientReturnTypeEnum ReturnType;

        /// <summary>
        /// 设置上传文件结果
        /// </summary>
        /// <param name="state"></param>
        /// <param name="returnType"></param>
        internal void Set(DeployTaskUploadFileStateEnum state, CommandClientReturnTypeEnum returnType)
        {
            State = state;
            ReturnType = returnType;
        }
    }
}
