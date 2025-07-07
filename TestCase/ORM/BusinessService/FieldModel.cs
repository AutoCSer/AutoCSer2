using AutoCSer.Metadata;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.BusinessService
{
    /// <summary>
    /// 字段测试业务模型
    /// </summary>
    public sealed class FieldModel : AutoCSer.TestCase.CommonModel.TableModel.FieldModel
    {
        /// <summary>
        /// Get data based on keywords
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static async Task<FieldModel> Get(long key)
        {
            return await Persistence.FieldModelQuery.GetByPrimaryKey(key);
        }
        /// <summary>
        /// Add data
        /// </summary>
        /// <returns></returns>
        internal async Task<bool> Insert()
        {
            return await Persistence.FieldModelWriter.Insert(this);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="memberMap"></param>
        /// <returns></returns>
        internal async Task<bool> Update(MemberMap<AutoCSer.TestCase.CommonModel.TableModel.FieldModel> memberMap)
        {
            return await Persistence.FieldModelWriter.Update(this, memberMap);
        }
        /// <summary>
        /// 根据关键字删除数据
        /// </summary>
        /// <returns></returns>
        internal async Task<bool> Delete()
        {
            return await Persistence.FieldModelWriter.Delete(this);
        }
        /// <summary>
        /// 根据关键字删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static async Task<bool> Delete(long key)
        {
            return await Persistence.FieldModelWriter.Delete(key);
        }
    }
}
