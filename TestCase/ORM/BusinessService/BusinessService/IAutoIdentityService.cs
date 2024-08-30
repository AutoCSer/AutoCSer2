using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.BusinessService
{
    /// <summary>
    /// 自增ID基本操作测试服务接口
    /// </summary>
    /// <typeparam name="MT">持久化表格模型类型</typeparam>
    /// <typeparam name="BT">表格业务模型类型</typeparam>
    public interface IAutoIdentityService<BT, MT> : IBaseService<BT, MT, long>
        where MT : class
        where BT : class, MT
    {
        /// <summary>
        /// 添加数据并返回自增ID
        /// </summary>
        /// <returns>失败返回-1</returns>
        Task<long> Insert(BT value);
    }
}
