using AutoCSer.Metadata;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.BusinessService
{
    /// <summary>
    /// 基本操作测试服务接口
    /// </summary>
    /// <typeparam name="BT">表格业务模型类型</typeparam>
    /// <typeparam name="MT">持久化表格模型类型</typeparam>
    /// <typeparam name="KT">关键字类型</typeparam>
    public interface IBaseService<BT, MT, KT>
        where MT : class
        where BT : class, MT
        where KT : IEquatable<KT>
    {
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns>失败返回 null</returns>
        Task<BT> Get(KT key);
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> Update(MemberMapValue<MT, BT> value);
        /// <summary>
        /// 根据关键字删除数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> Delete(BT value);
        /// <summary>
        /// 根据关键字删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> DeleteKey(KT key);
    }
}
