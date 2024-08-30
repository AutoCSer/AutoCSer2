using System;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 关键字类型
    /// </summary>
    public enum PrimaryKeyTypeEnum : byte
    {
        /// <summary>
        /// 非关键字
        /// </summary>
        None,
        /// <summary>
        /// 普通关键字，关键字必须实现 IEquatable{T} 接口
        /// </summary>
        PrimaryKey,
        /// <summary>
        /// 自增ID，只支持 int 与 long（不是数据库特性，是程序逻辑自增，增删改操作必须在队列中调用）
        /// </summary>
        AutoIdentity,
    }
}
