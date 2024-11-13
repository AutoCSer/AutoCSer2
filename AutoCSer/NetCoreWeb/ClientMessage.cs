using System;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// 客户端错误信息定义
    /// </summary>
    public class ClientMessage
    {
        /// <summary>
        /// 浏览器名称
        /// </summary>
#if NetStandard21
        public string? AppName;
#else
        public string AppName;
#endif
        /// <summary>
        /// 浏览器的平台和版本信息
        /// </summary>
#if NetStandard21
        public string? AppVersion;
#else
        public string AppVersion;
#endif
        /// <summary>
        /// 当前访问 URL
        /// </summary>
#if NetStandard21
        public string? Location;
#else
        public string Location;
#endif
        /// <summary>
        /// 出错文件名称（如果出错文件为 Location 则不传递该参数）
        /// </summary>
#if NetStandard21
        public string? FileName;
#else
        public string FileName;
#endif
        /// <summary>
        /// 出错行号
        /// </summary>
        public int LineNo;
        /// <summary>
        /// 出错列号
        /// </summary>
        public int ColNo;
        /// <summary>
        /// 具体错误信息
        /// </summary>
#if NetStandard21
        public string? Message;
#else
        public string Message;
#endif
    }
}
