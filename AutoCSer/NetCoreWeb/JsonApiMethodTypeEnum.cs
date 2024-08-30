using System;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// JSON API 方法类型
    /// </summary>
    internal enum JsonApiMethodTypeEnum : byte
    {
        /// <summary>
        /// 返回值类型为 ResponseResult{T}
        /// </summary>
        ResponseResult,
        /// <summary>
        /// 返回值类型为 ResponseResult
        /// </summary>
        ResponseState
    }
}
