//本文件由程序自动生成，请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.CommandService
{
        /// <summary>
        /// 基于递增登录时间戳验证的反向服务认证接口（配合 HASH 防止重放登录操作） 客户端接口
        /// </summary>
        public partial interface ITimestampVerifyReverseServiceClientController<T>
        {
            /// <summary>
            /// 获取验证数据
            /// </summary>
            /// <param name="timestamp">待验证时间戳</param>
            /// <returns>反向服务验证数据</returns>
            AutoCSer.Net.ReturnCommand<AutoCSer.CommandService.TimestampVerify.ReverseServiceVerifyData<T>> GetVerifyData(long timestamp);
        }
}
#endif