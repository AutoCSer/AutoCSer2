using System;

namespace AutoCSer.CommandService.Search
{
    /// <summary>
    /// 非索引条件查询数据更新状态
    /// </summary>
    public enum ConditionDataUpdateStateEnum : byte
    {
        /// <summary>
        /// 未知错误或者异常
        /// </summary>
        Unknown,
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 未找到关键字触发删除操作
        /// </summary>
        DeletedNotFoundKey,
        /// <summary>
        /// 不支持删除存在数据的关键字
        /// </summary>
        NotSupportDeleteKey,
        /// <summary>
        /// 关键字不允许为 null
        /// </summary>
        NullKey,
        /// <summary>
        /// 获取数据失败
        /// </summary>
        GetDataFailed,

        /// <summary>
        /// 已回调
        /// </summary>
        Callbacked,
    }
}
