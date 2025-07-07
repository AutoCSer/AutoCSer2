using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Client
{
    /// <summary>
    /// Keep response API client await API sample interface
    /// 持续响应 API 客户端 await API 示例接口
    /// </summary>
    public interface IEnumeratorCommandController
    {
        /// <summary>
        /// Keep response API client await API sample
        /// 持续响应 API 客户端 await API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>The return value type must be AutoCSer.Net.EnumeratorCommand or AutoCSer.Net.EnumeratorCommand{T}</returns>
        AutoCSer.Net.EnumeratorCommand<int> Callback(int left, int right);
    }
}
