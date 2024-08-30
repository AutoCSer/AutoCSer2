using AutoCSer.Metadata;
using AutoCSer.TestCase.Common.Data;
using AutoCSer.TestCase.CommonModel.BusinessServiceMethodEnum;
using AutoCSer.TestCase.CommonModel.TableModel.CustomColumn;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.BusinessService
{
    /// <summary>
    /// 自定义字段列测试模型业务数据服务接口
    /// </summary>
    [AutoCSer.Net.CommandServerControllerInterface(MethodIndexEnumType = typeof(CustomColumnFieldModelServiceMethodEnum), IsAutoMethodIndex = false, MethodIndexEnumTypeCodeGeneratorPath = Persistence.CommandServerMethodIndexEnumTypePath)]
    public interface ICustomColumnFieldModelService : IPrimaryKeyService<CustomColumnFieldModel, AutoCSer.TestCase.CommonModel.TableModel.CustomColumnFieldModel, AutoCSer.TestCase.CommonModel.TableModel.CustomColumn.CustomColumnFieldPrimaryKey>
    {
        /// <summary>
        /// 自定义列查询测试
        /// </summary>
        /// <returns>查询数据数量</returns>
        Task<int> CustomColumnQuery();
    }
    /// <summary>
    /// 自定义字段列测试业务模型业务数据服务
    /// </summary>
    public sealed class CustomColumnFieldModelService : ICustomColumnFieldModelService
    {
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns>失败返回 null</returns>
        public async Task<CustomColumnFieldModel> Get(AutoCSer.TestCase.CommonModel.TableModel.CustomColumn.CustomColumnFieldPrimaryKey key)
        {
            return await CustomColumnFieldModel.Get(key);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> Insert(CustomColumnFieldModel value)
        {
            return value != null && await value.Insert();
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> Update(MemberMapValue<AutoCSer.TestCase.CommonModel.TableModel.CustomColumnFieldModel, CustomColumnFieldModel> value)
        {
            return value.Value != null && await value.Value.Update(value.MemberMap);
        }
        /// <summary>
        /// 根据关键字删除数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> Delete(CustomColumnFieldModel value)
        {
            return value != null && await value.Delete();
        }
        /// <summary>
        /// 根据关键字删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> DeleteKey(AutoCSer.TestCase.CommonModel.TableModel.CustomColumn.CustomColumnFieldPrimaryKey key)
        {
            return await CustomColumnFieldModel.Delete(key);
        }
#pragma warning disable
        /// <summary>
        /// 自定义列查询测试
        /// </summary>
        /// <returns>查询数据数量</returns>
        public async Task<int> CustomColumnQuery()
        {
            CustomColumnFieldPrimaryKey key = new CustomColumnFieldPrimaryKey();
            //return (await Persistence.CustomColumnFieldModelQuery.Query(p => p.Key == key)).Count;
            //return (await Persistence.CustomColumnFieldModelQuery.Query(p => p.Key.ByteEnumKey == key.ByteEnumKey)).Count;
            //return (await Persistence.CustomColumnFieldModelQuery.Query(p => p.CustomColumnField.ByteEnum == ByteEnum.C)).Count;
            //return (await Persistence.CustomColumnFieldModelQuery.Query(p => p.CustomColumnField.Byte == (byte)ByteEnum.C)).Count;
            //return (await Persistence.CustomColumnFieldModelQuery.Query(p => p.CustomColumnField.DateTime == DateTime.Now)).Count;
            return (await Persistence.CustomColumnFieldModelQuery.Query(p => p.CustomColumnField.DateTime2 == DateTime.Now)).Count;
        }
    }
}
