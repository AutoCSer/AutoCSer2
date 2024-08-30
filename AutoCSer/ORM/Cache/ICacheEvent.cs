using AutoCSer.Metadata;
using System;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 缓存事件
    /// </summary>
    /// <typeparam name="T">持久化表格模型类型</typeparam>
    /// <typeparam name="VT">缓存数据类型</typeparam>
    public interface ICacheEvent<T, VT>
        where T : class
        where VT : class, T
    {
        /// <summary>
        /// 添加事件缓存数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        void OnInserted(VT value);
        /// <summary>
        /// 更新事件缓存数据之前的操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        void BeforeUpdate(VT value, MemberMap<T> memberMap);
        /// <summary>
        /// 更新事件缓存数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        void OnUpdated(VT value, MemberMap<T> memberMap);
        /// <summary>
        /// 删除事件数据之后的操作
        /// </summary>
        /// <param name="value"></param>
        void OnDeleted(VT value);
    }
}
