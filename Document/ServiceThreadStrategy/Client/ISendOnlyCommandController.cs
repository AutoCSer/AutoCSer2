using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Client
{
    /// <summary>
    /// 服务端仅执行 API 客户端示例接口
    /// </summary>
    public interface ISendOnlyCommandController
    {
        /// <summary>
        /// 服务端仅执行 API 客户端
        /// </summary>
        /// <param name="value"></param>
        /// <returns>返回值类型必须为 AutoCSer.Net.SendOnlyCommand</returns>
        AutoCSer.Net.SendOnlyCommand Call(int value);
    }
}
