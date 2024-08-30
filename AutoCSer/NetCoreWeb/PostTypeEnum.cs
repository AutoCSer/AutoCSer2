using System;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// POST 数据类型
    /// </summary>
    public enum PostTypeEnum : byte
    {
        /// <summary>
        /// 未指定
        /// </summary>
        None,
        /// <summary>
        /// text/json ; application/json
        /// </summary>
        Json,
        /// <summary>
        /// text/xml ; application/xml
        /// </summary>
        Xml,
        /// <summary>
        /// application/x-www-form-urlencoded
        /// </summary>
        Form,
        /// <summary>
        /// multipart/form-data
        /// </summary>
        FormData,
        /// <summary>
        /// 未知
        /// </summary>
        Unknown,
    }
}
