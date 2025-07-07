using AutoCSer.Metadata;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.BusinessService
{
    /// <summary>
    /// 自增ID与其它混合测试业务模型
    /// </summary>
    public sealed class AutoIdentityModel : AutoCSer.TestCase.CommonModel.TableModel.AutoIdentityModel
    {
        /// <summary>
        /// Get data based on keywords
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        internal static async Task<AutoIdentityModel> Get(long identity)
        {
            return await Persistence.AutoIdentityModelQuery.GetByPrimaryKey(identity);
        }
        /// <summary>
        /// 添加数据并返回自增ID
        /// </summary>
        /// <returns>失败返回-1</returns>
        internal async Task<long> Insert()
        {
            if (await Persistence.AutoIdentityModelWriter.Insert(this)) return Identity;
            return -1;
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="memberMap"></param>
        /// <returns></returns>
        internal async Task<bool> Update(MemberMap<AutoCSer.TestCase.CommonModel.TableModel.AutoIdentityModel> memberMap)
        {
            return await Persistence.AutoIdentityModelWriter.Update(this, memberMap);
        }
        /// <summary>
        /// 根据关键字删除数据
        /// </summary>
        /// <returns></returns>
        internal async Task<bool> Delete()
        {
            return await Persistence.AutoIdentityModelWriter.Delete(this);
        }
        /// <summary>
        /// 根据关键字删除数据
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        internal static async Task<bool> Delete(long identity)
        {
            return await Persistence.AutoIdentityModelWriter.Delete(identity);
        }
    }
}
