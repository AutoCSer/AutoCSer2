using AutoCSer.Net;
using System;

namespace AutoCSer.Example.CommandServer.Client.AsyncTask
{
    /// <summary>
    /// 服务端 async Task 调用 不返回数据（不应答客户端） 示例接口（客户端）
    /// </summary>
    public interface ISendOnlyController
    {
        /// <summary>
        /// 只发送数据
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.SendOnlyCommand</returns>
        SendOnlyCommand SendOnly(int parameter1, int parameter2);
        /// <summary>
        /// 只发送数据）
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.SendOnlyCommand</returns>
        SendOnlyCommand SendOnly(int parameter);
    }
}
