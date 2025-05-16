using System;

namespace AutoCSer.Json
{
    /// <summary>
    /// 时间序列化输出类型
    /// </summary>
    public enum DateTimeTypeEnum : byte
    {
        /// <summary>
        /// yyyy-MM-ddTHH:mm:ss...
        /// </summary>
        Default,
        /// <summary>
        /// 第三方格式 /Date(xxx)/
        /// </summary>
        ThirdParty,
        /// <summary>
        /// JS格式 new Date(xxx)
        /// </summary>
        JavaScript,
        /// <summary>
        /// 自定义 ToString("xxx") 格式
        /// </summary>
        CustomFormat,
    }
}
