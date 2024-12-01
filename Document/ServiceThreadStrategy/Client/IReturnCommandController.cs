using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Client
{
    /// <summary>
    /// 服务端一次性响应 API 客户端示例接口
    /// </summary>
    public interface IReturnCommandController
    {
        /// <summary>
        /// 一次性响应 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>返回值类型必须为 AutoCSer.Net.ReturnCommand 或者 AutoCSer.Net.ReturnCommand{T}</returns>
        //[AutoCSer.Net.CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.RunTask)]
        AutoCSer.Net.ReturnCommand<int> Add(int left, int right);
    }
}
