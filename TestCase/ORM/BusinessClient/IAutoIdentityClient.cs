using AutoCSer.Net;
using System;

namespace AutoCSer.TestCase.BusinessClient
{
    /// <summary>
    /// 自增ID基本操作测试服务客户端接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAutoIdentityClient<T> : IBaseClient<T, long>
        where T : class
    {
        /// <summary>
        /// 添加数据并返回自增ID
        /// </summary>
        /// <returns>失败返回-1</returns>
        ReturnCommand<long> Insert(T value);
    }
}
