using System;

namespace AutoCSer.Document.ServiceAuthentication.CustomVerify
{
    /// <summary>
    /// 自定义服务认证接口
    /// </summary>
    [AutoCSer.Net.CommandServerControllerInterface]
    public interface ICustomVerifyService
    {
        /// <summary>
        /// 验证函数
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="verifyData">自定义服务认证数据</param>
        /// <param name="hashData">验证 Hash 数据</param>
        /// <returns></returns>
        Task<AutoCSer.Net.CommandServerVerifyStateEnum> Verify(AutoCSer.Net.CommandServerSocket socket, CustomVerifyData verifyData, byte[] hashData);
    }
}
