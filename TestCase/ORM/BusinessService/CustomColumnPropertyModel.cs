using AutoCSer.Metadata;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.BusinessService
{
    /// <summary>
    /// 自定义属性列测试业务模型
    /// </summary>
    public sealed class CustomColumnPropertyModel : AutoCSer.TestCase.CommonModel.TableModel.CustomColumnPropertyModel
    {
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static async Task<CustomColumnPropertyModel> Get(AutoCSer.TestCase.CommonModel.TableModel.CustomColumn.CustomColumnPropertyPrimaryKey key)
        {
            return await Persistence.CustomColumnPropertyModelQuery.GetByPrimaryKey(key);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <returns></returns>
        internal async Task<bool> Insert()
        {
            return await Persistence.CustomColumnPropertyModelWriter.Insert(this);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="memberMap"></param>
        /// <returns></returns>
        internal async Task<bool> Update(MemberMap<AutoCSer.TestCase.CommonModel.TableModel.CustomColumnPropertyModel> memberMap)
        {
            return await Persistence.CustomColumnPropertyModelWriter.Update(this, memberMap);
        }
        /// <summary>
        /// 根据关键字删除数据
        /// </summary>
        /// <returns></returns>
        internal async Task<bool> Delete()
        {
            return await Persistence.CustomColumnPropertyModelWriter.Delete(this);
        }
        /// <summary>
        /// 根据关键字删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static async Task<bool> Delete(AutoCSer.TestCase.CommonModel.TableModel.CustomColumn.CustomColumnPropertyPrimaryKey key)
        {
            return await Persistence.CustomColumnPropertyModelWriter.Delete(key);
        }
    }
}
