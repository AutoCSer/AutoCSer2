using System;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 单值查询结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class ValueResult<T>
    {
#pragma warning disable
        /// <summary>
        /// 记录总数
        /// </summary>
        public T Value;
#pragma warning restore
    }
}
