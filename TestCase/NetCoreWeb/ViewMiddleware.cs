using AutoCSer.NetCoreWeb;
using System;
using System.IO;

namespace AutoCSer.TestCase.NetCoreWeb
{
    /// <summary>
    /// 数据视图中间件
    /// </summary>
    public sealed partial class ViewMiddleware : AutoCSer.NetCoreWeb.ViewMiddleware
    {
        /// <summary>
        /// 收集客户端错误信息的请求地址
        /// </summary>
        public const string DefaultErrorRequestPath = "/ClientError";

        /// <summary>
        /// 收集客户端错误信息的请求地址，默认为 null 表示不采集客户端错误，在代码生成中替换标记字符串 __ERRORPATH__
        /// </summary>
        public override string ErrorRequestPath { get { return DefaultErrorRequestPath; } }
        /// <summary>
        /// 检查来源页面，用于跨域验证
        /// </summary>
        /// <param name="referer">来源页面</param>
        /// <returns></returns>
        public override ResponseResult CheckReferer(string referer)
        {
            //实际应用场景应该检查 referer
            return ResponseStateEnum.Success;
        }
    }
}
