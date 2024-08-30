using AutoCSer.Metadata;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.BusinessService
{
    /// <summary>
    /// 自定义字段列测试业务模型
    /// </summary>
    public sealed class CustomColumnFieldModel : AutoCSer.TestCase.CommonModel.TableModel.CustomColumnFieldModel
    {
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static async Task<CustomColumnFieldModel> Get(AutoCSer.TestCase.CommonModel.TableModel.CustomColumn.CustomColumnFieldPrimaryKey key)
        {
            return await Persistence.CustomColumnFieldModelQuery.GetByPrimaryKey(key);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <returns></returns>
        internal async Task<bool> Insert()
        {
            return await Persistence.CustomColumnFieldModelWriter.Insert(this);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="memberMap"></param>
        /// <returns></returns>
        internal async Task<bool> Update(MemberMap<AutoCSer.TestCase.CommonModel.TableModel.CustomColumnFieldModel> memberMap)
        {
            return await Persistence.CustomColumnFieldModelWriter.Update(this, memberMap);
        }
        /// <summary>
        /// 根据关键字删除数据
        /// </summary>
        /// <returns></returns>
        internal async Task<bool> Delete()
        {
            return await Persistence.CustomColumnFieldModelWriter.Delete(this);
        }
        /// <summary>
        /// 根据关键字删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static async Task<bool> Delete(AutoCSer.TestCase.CommonModel.TableModel.CustomColumn.CustomColumnFieldPrimaryKey key)
        {
            return await Persistence.CustomColumnFieldModelWriter.Delete(key);
        }
    }
}
