using System;

namespace AutoCSer.Document.ServiceAuthentication.CustomVerify
{
    /// <summary>
    /// A service authentication interface for customizing user identity authentication
    /// 自定义用户身份鉴权服务认证接口
    /// </summary>
    [AutoCSer.Net.CommandServerControllerInterface]
    public partial interface ICustomVerifyService
    {
        /// <summary>
        /// Verification method
        /// 验证方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="verifyData">Customize user identity authentication data
        /// 自定义用户身份鉴权数据</param>
        /// <param name="hashData">Hash data to be verified
        /// 待验证 Hash 数据</param>
        /// <returns></returns>
        System.Threading.Tasks.Task<AutoCSer.Net.CommandServerVerifyStateEnum> Verify(AutoCSer.Net.CommandServerSocket socket, CustomVerifyData verifyData, byte[] hashData);
    }
}
