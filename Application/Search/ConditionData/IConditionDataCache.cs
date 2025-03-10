using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.Search
{
    /// <summary>
    /// 非索引条件查询数据缓存
    /// </summary>
    /// <typeparam name="KT">查询数据关键字类型</typeparam>
    /// <typeparam name="VT">查询条件数据</typeparam>
    public interface IConditionDataCache<KT, VT>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
#else
        where KT : IEquatable<KT>
#endif
    {
        ///// <summary>
        ///// 获取所有数据（应该修改为内存数据库节点模式）
        ///// </summary>
        ///// <param name="queryParameter">查询参数</param>
        ///// <returns></returns>
        //EnumeratorCommand<VT> GetAllData(PageParameter queryParameter);
        ///// <summary>
        ///// 设置数据
        ///// </summary>
        ///// <param name="value"></param>
        //void Set(VT value);
    }
}
