using AutoCSer.NetCoreWeb;
using Microsoft.AspNetCore.Http;

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
        /// 当前调用监视标识
        /// </summary>
        private long callIdentity;
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
        /// <summary>
        /// 获取调用监视标识
        /// </summary>
        /// <param name="httpContext">HTTP 上下文</param>
        /// <param name="view">数据视图</param>
        /// <returns>调用监视标识，long.MinValue 表示不监视</returns>
        public override long GetCallIdentity(HttpContext httpContext, AutoCSer.NetCoreWeb.View view) 
        {
            var interfaceRealTimeCallMonitor = InterfaceRealTimeCallMonitorCommandClientSocketEvent.CommandClient.SocketEvent.InterfaceRealTimeCallMonitor;
            if (interfaceRealTimeCallMonitor != null)
            {
                long callIdentity = System.Threading.Interlocked.Increment(ref this.callIdentity);
                interfaceRealTimeCallMonitor.Start(callIdentity, nameof(AutoCSer.NetCoreWeb.View), view.GetType().Name, view.MonitorTimeoutMilliseconds, (ushort)MonitorCallTypeEnum.View).Discard();
                return callIdentity;
            }
            return long.MinValue;
        }
        /// <summary>
        /// 获取调用监视标识
        /// </summary>
        /// <param name="httpContext">HTTP 上下文</param>
        /// <param name="request">JSON API 请求实例</param>
        /// <returns>调用监视标识，long.MinValue 表示不监视</returns>
        public override long GetCallIdentity(HttpContext httpContext, JsonApiRequest request)
        {
            var interfaceRealTimeCallMonitor = InterfaceRealTimeCallMonitorCommandClientSocketEvent.CommandClient.SocketEvent.InterfaceRealTimeCallMonitor;
            if (interfaceRealTimeCallMonitor != null)
            {
                long callIdentity = System.Threading.Interlocked.Increment(ref this.callIdentity);
                interfaceRealTimeCallMonitor.Start(callIdentity, request.ControllerType.Name, request.MethodName, request.MonitorTimeoutMilliseconds, (ushort)MonitorCallTypeEnum.JsonApi).Discard();
                return callIdentity;
            }
            return long.MinValue;
        }
        /// <summary>
        /// 调用完成
        /// </summary>
        /// <param name="callIdentity">调用监视标识</param>
        /// <param name="isException">接口是否执行异常</param>
        public override void OnCallCompleted(long callIdentity, bool isException)
        {
            InterfaceRealTimeCallMonitorCommandClientSocketEvent.CommandClient.SocketEvent.InterfaceRealTimeCallMonitor.Completed(callIdentity, isException);
        }
    }
}
