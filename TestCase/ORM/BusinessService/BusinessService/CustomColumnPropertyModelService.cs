using AutoCSer.Metadata;
using AutoCSer.TestCase.Common.Data;
using AutoCSer.TestCase.CommonModel.TableModel.CustomColumn;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.BusinessService
{
    /// <summary>
    /// 自定义属性列测试模型业务数据服务接口
    /// </summary>
    [AutoCSer.Net.CommandServerControllerInterface(MethodIndexEnumTypeCodeGeneratorPath = Persistence.CommandServerMethodIndexEnumTypePath, IsCodeGeneratorClientInterface = false)]
    public partial interface ICustomColumnPropertyModelService : IPrimaryKeyService<CustomColumnPropertyModel, AutoCSer.TestCase.CommonModel.TableModel.CustomColumnPropertyModel, AutoCSer.TestCase.CommonModel.TableModel.CustomColumn.CustomColumnPropertyPrimaryKey>
    {
        /// <summary>
        /// 自定义列查询测试
        /// </summary>
        /// <returns>查询数据数量</returns>
        Task<int> CustomColumnQuery();
    }
    /// <summary>
    /// 自定义属性列测试业务模型业务数据服务
    /// </summary>
    public sealed class CustomColumnPropertyModelService : ICustomColumnPropertyModelService
    {
        /// <summary>
        /// Get data based on keywords
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Return null on failure</returns>
        public async Task<CustomColumnPropertyModel> Get(AutoCSer.TestCase.CommonModel.TableModel.CustomColumn.CustomColumnPropertyPrimaryKey key)
        {
            return await CustomColumnPropertyModel.Get(key);
        }
        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> Insert(CustomColumnPropertyModel value)
        {
            return value != null && await value.Insert();
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> Update(MemberMapValue<AutoCSer.TestCase.CommonModel.TableModel.CustomColumnPropertyModel, CustomColumnPropertyModel> value)
        {
            return value.Value != null && await value.Value.Update(value.MemberMap);
        }
        /// <summary>
        /// 根据关键字删除数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> Delete(CustomColumnPropertyModel value)
        {
            return value != null && await value.Delete();
        }
        /// <summary>
        /// 根据关键字删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> DeleteKey(AutoCSer.TestCase.CommonModel.TableModel.CustomColumn.CustomColumnPropertyPrimaryKey key)
        {
            return await CustomColumnPropertyModel.Delete(key);
        }
#pragma warning disable
        /// <summary>
        /// 自定义列查询测试
        /// </summary>
        /// <returns>查询数据数量</returns>
        public async Task<int> CustomColumnQuery()
        {
            CustomColumnPropertyPrimaryKey key = new CustomColumnPropertyPrimaryKey();
            //return (await Persistence.CustomColumnPropertyModelQuery.Query(p => p.Key == key)).Count;
            //return (await Persistence.CustomColumnPropertyModelQuery.Query(p => p.Key.ByteEnumKey == key.ByteEnumKey)).Count;
            //return (await Persistence.CustomColumnPropertyModelQuery.Query(p => p.CustomColumnProperty.ByteEnum == ByteEnum.C)).Count;
            //return (await Persistence.CustomColumnPropertyModelQuery.Query(p => p.CustomColumnProperty.Byte == (byte)ByteEnum.C)).Count;
            //return (await Persistence.CustomColumnPropertyModelQuery.Query(p => p.CustomColumnProperty.DateTime == DateTime.Now)).Count;
            return (await Persistence.CustomColumnPropertyModelQuery.Query(p => p.CustomColumnProperty.DateTime2 == DateTime.Now)).Count;
        }
    }
}
