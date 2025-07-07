using AutoCSer.Metadata;
using AutoCSer.Net;
using System;

namespace AutoCSer.TestCase.BusinessClient
{
    /// <summary>
    /// 基本操作测试服务客户端接口
    /// </summary>
    /// <typeparam name="T">持久化表格模型类型</typeparam>
    /// <typeparam name="KT">Keyword type
    /// 关键字类型</typeparam>
    public interface IBaseClient<T, KT>
        where T : class
        where KT : IEquatable<KT>
    {
        /// <summary>
        /// Get data based on keywords
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Return null on failure</returns>
        ReturnCommand<T> Get(KT key);
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        ReturnCommand<bool> Update(MemberMapValue<T> value);
        /// <summary>
        /// 根据关键字删除数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        ReturnCommand<bool> Delete(T value);
        /// <summary>
        /// 根据关键字删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        ReturnCommand<bool> DeleteKey(KT key);
    }
}
