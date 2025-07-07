using System;

namespace AutoCSer.ORM.Cache.Synchronous
{
    /// <summary>
    /// 同步操作类型
    /// </summary>
    public enum OperationTypeEnum : byte
    {
        /// <summary>
        /// 未知错误
        /// </summary>
        Unknown,
        /// <summary>
        /// 缓存数据传输
        /// </summary>
        Cache,
        /// <summary>
        /// 缓存数据初始化传输完毕
        /// </summary>
        Loaded,
        /// <summary>
        /// Add data
        /// </summary>
        Insert,
        /// <summary>
        /// 更新数据
        /// </summary>
        Update,
        /// <summary>
        /// Delete data
        /// 删除数据
        /// </summary>
        Delete,
    }
}
