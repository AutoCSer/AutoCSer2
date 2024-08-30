using AutoCSer.Metadata;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.BusinessService
{
    /// <summary>
    /// 属性测试业务模型
    /// </summary>
    public sealed class PropertyModel : AutoCSer.TestCase.CommonModel.TableModel.PropertyModel
    {
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static async Task<PropertyModel> Get(string key)
        {
            return await Persistence.PropertyModelQuery.GetByPrimaryKey(key);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <returns></returns>
        internal async Task<bool> Insert()
        {
            return await Persistence.PropertyModelWriter.Insert(this);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="memberMap"></param>
        /// <returns></returns>
        internal async Task<bool> Update(MemberMap<AutoCSer.TestCase.CommonModel.TableModel.PropertyModel> memberMap)
        {
            return await Persistence.PropertyModelWriter.Update(this, memberMap);
        }
        /// <summary>
        /// 根据关键字删除数据
        /// </summary>
        /// <returns></returns>
        internal async Task<bool> Delete()
        {
            return await Persistence.PropertyModelWriter.Delete(this);
        }
        /// <summary>
        /// 根据关键字删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static async Task<bool> Delete(string key)
        {
            return await Persistence.PropertyModelWriter.Delete(key);
        }
    }
}
