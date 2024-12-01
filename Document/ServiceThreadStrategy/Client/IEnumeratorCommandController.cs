using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Client
{
    /// <summary>
    /// 持续响应 API 客户端示例接口
    /// </summary>
    public interface IEnumeratorCommandController
    {
        /// <summary>
        /// 一次性响应 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>返回值类型必须为 AutoCSer.Net.EnumeratorCommand 或者 AutoCSer.Net.EnumeratorCommand{T}</returns>
        AutoCSer.Net.EnumeratorCommand<int> Callback(int left, int right);
    }
}
