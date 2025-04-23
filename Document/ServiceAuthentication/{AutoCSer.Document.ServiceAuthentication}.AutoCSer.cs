//本文件由程序自动生成，请不要自行修改
using System;
using System.Numerics;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.Document.ServiceAuthentication.CustomVerify
{
        /// <summary>
        /// 自定义服务认证接口 客户端接口
        /// </summary>
        public partial interface ICustomVerifyServiceClientController
        {
            /// <summary>
            /// 验证函数
            /// </summary>
            /// <param name="verifyData">自定义服务认证数据</param>
            /// <param name="hashData">验证 Hash 数据</param>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<AutoCSer.Net.CommandServerVerifyStateEnum> Verify(AutoCSer.Document.ServiceAuthentication.CustomVerify.CustomVerifyData verifyData, byte[] hashData);
        }
}namespace AutoCSer.Document.ServiceAuthentication
{
        /// <summary>
        /// 测试接口定义 客户端接口
        /// </summary>
        public partial interface ITestServiceClientController
        {
            /// <summary>
            /// 异步 API 定义
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<int> Add(int left, int right);
            /// <summary>
            /// 不允许访问的 API
            /// </summary>
            AutoCSer.Net.ReturnCommand NotSetCommand();
        }
}
#endif