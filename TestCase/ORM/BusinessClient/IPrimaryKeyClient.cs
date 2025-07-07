using AutoCSer.Net;
using System;

namespace AutoCSer.TestCase.BusinessClient
{
    /// <summary>
    /// 关键字基本操作测试服务接口
    /// </summary>
    /// <typeparam name="T">持久化表格模型类型</typeparam>
    /// <typeparam name="KT">Keyword type
    /// 关键字类型</typeparam>
    public interface IPrimaryKeyClient<T, KT> : IBaseClient<T, KT>
        where T : class
        where KT : IEquatable<KT>
    {
        /// <summary>
        /// Add data
        /// </summary>
        /// <returns></returns>
        ReturnCommand<bool> Insert(T value);
    }
}
