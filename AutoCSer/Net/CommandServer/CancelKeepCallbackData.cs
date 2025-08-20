using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 取消异步保持回调数据
    /// </summary>
    [AutoCSer.CodeGenerator.BinarySerialize(IsSerialize = false)]
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal partial struct CancelKeepCallbackData
    {
        /// <summary>
        /// 会话序号
        /// </summary>
        public int Index;
        /// <summary>
        /// 会话标识
        /// </summary>
        public uint Identity;
        /// <summary>
        /// 返回值类型
        /// </summary>
        public CommandClientReturnTypeEnum ReturnType;
        /// <summary>
        /// Error message
        /// </summary>
#if NetStandard21
        public string? ErrorMessage;
#else
        public string ErrorMessage;
#endif
        /// <summary>
        /// 取消异步保持回调数据
        /// </summary>
        /// <param name="callbackIdentity"></param>
        internal CancelKeepCallbackData(ref CallbackIdentity callbackIdentity)
        {
            Index = (int)callbackIdentity.Index;
            Identity = callbackIdentity.Identity;
            ReturnType = CommandClientReturnTypeEnum.Unknown;
            ErrorMessage = null;
        }
        /// <summary>
        /// 取消异步保持回调数据
        /// </summary>
        /// <param name="callbackIdentity"></param>
        /// <param name="returnType"></param>
        /// <param name="exception"></param>
#if NetStandard21
        internal void Set(CallbackIdentity callbackIdentity, CommandClientReturnTypeEnum returnType, Exception? exception)
#else
        internal void Set(CallbackIdentity callbackIdentity, CommandClientReturnTypeEnum returnType, Exception exception)
#endif
        {
            Index = callbackIdentity.CallbackIndex;
            Identity = callbackIdentity.Identity;
            ReturnType = returnType;
            ErrorMessage = exception?.Message;
        }
    }
}
