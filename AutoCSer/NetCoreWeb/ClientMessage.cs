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
        public string AppName;
        /// <summary>
        /// 浏览器的平台和版本信息
        /// </summary>
        public string AppVersion;
        /// <summary>
        /// 当前访问 URL
        /// </summary>
        public string Location;
        /// <summary>
        /// 出错文件名称（如果出错文件为 Location 则不传递该参数）
        /// </summary>
        public string FileName;
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
        public string Message;
    }
}
