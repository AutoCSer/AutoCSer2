using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.BusinessService
{
    /// <summary>
    /// 关键字基本操作测试服务接口
    /// </summary>
    /// <typeparam name="MT">持久化表格模型类型</typeparam>
    /// <typeparam name="BT">表格业务模型类型</typeparam>
    /// <typeparam name="KT">Keyword type
    /// 关键字类型</typeparam>
    public interface IPrimaryKeyService<BT, MT, KT> : IBaseService<BT, MT, KT>
        where MT : class
        where BT : class, MT
        where KT : IEquatable<KT>
    {
        /// <summary>
        /// Add data
        /// </summary>
        /// <returns></returns>
        Task<bool> Insert(BT value);
    }
}
