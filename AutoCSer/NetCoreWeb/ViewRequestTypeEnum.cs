using System;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// 数据视图请求类型
    /// </summary>
    internal enum ViewRequestTypeEnum : byte
    {
        /// <summary>
        /// HTML 模板文件
        /// </summary>
        Html,
        /// <summary>
        /// JavaScript 脚本文件
        /// </summary>
        JavaScript,
        /// <summary>
        /// 视图数据
        /// </summary>
        ViewData,
        /// <summary>
        /// JSON API
        /// </summary>
        JsonApi,
    }
}
