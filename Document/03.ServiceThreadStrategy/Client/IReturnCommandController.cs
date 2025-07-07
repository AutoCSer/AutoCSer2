using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Client
{
    /// <summary>
    /// One-time response API client await callback API sample interface
    /// 一次性响应 API 客户端 await 回调 API 示例接口
    /// </summary>
    public interface IReturnCommandController
    {
        /// <summary>
        /// One-time response API client await callback API sample
        /// 一次性响应 API 客户端 await 回调 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>The return value type must be AutoCSer.Net.ReturnCommand or AutoCSer.Net.ReturnCommand{T}</returns>
        [AutoCSer.Net.CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.RunTask)]
        AutoCSer.Net.ReturnCommand<int> Add(int left, int right);
        /// <summary>
        /// One-time response API client Task API sample
        /// 一次性响应 API 客户端 Task API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.Net.CommandClientMethod(MatchMethodName = nameof(Add))]
        System.Threading.Tasks.Task<int> AddAsync(int left, int right);
    }
}
