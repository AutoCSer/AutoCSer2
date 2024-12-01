using AutoCSer.Metadata;
using AutoCSer.TestCase.Common.Data;
using AutoCSer.TestCase.CommonModel.BusinessServiceMethodEnum;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.BusinessService
{
    /// <summary>
    /// 字段测试模型业务数据服务接口
    /// </summary>
    [AutoCSer.Net.CommandServerControllerInterface(MethodIndexEnumType = typeof(FieldModelServiceMethodEnum), IsAutoMethodIndex = false, MethodIndexEnumTypeCodeGeneratorPath = Persistence.CommandServerMethodIndexEnumTypePath, IsCodeGeneratorClientInterface = false)]
    public interface IFieldModelService : IPrimaryKeyService<FieldModel, AutoCSer.TestCase.CommonModel.TableModel.FieldModel, long>
    {
        /// <summary>
        /// 自定义列查询测试
        /// </summary>
        /// <returns>查询数据数量</returns>
        Task<int> CustomColumnQuery();
    }
    /// <summary>
    /// 字段测试业务模型业务数据服务
    /// </summary>
    public sealed class FieldModelService : IFieldModelService
    {
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns>失败返回 null</returns>
        public async Task<FieldModel> Get(long key)
        {
            return await FieldModel.Get(key);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> Insert(FieldModel value)
        {
            return value != null && await value.Insert();
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> Update(MemberMapValue<AutoCSer.TestCase.CommonModel.TableModel.FieldModel, FieldModel> value)
        {
            return value.Value != null && await value.Value.Update(value.MemberMap);
        }
        /// <summary>
        /// 根据关键字删除数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> Delete(FieldModel value)
        {
            return value != null && await value.Delete();
        }
        /// <summary>
        /// 根据关键字删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> DeleteKey(long key)
        {
            return await FieldModel.Delete(key);
        }
        /// <summary>
        /// 自定义列查询测试
        /// </summary>
        /// <returns>查询数据数量</returns>
        public async Task<int> CustomColumnQuery()
        {
            //return (await Persistence.FieldModelQuery.Query(p => p.DateTimeNull == null)).Count;
            //return (await Persistence.FieldModelQuery.Query(p => p.ByteEnum == ByteEnum.C)).Count;
            //return (await Persistence.FieldModelQuery.Query(p => p.Byte == (byte)ByteEnum.C)).Count;
            //return (await Persistence.FieldModelQuery.Query(p => p.DateTime2 == DateTime.Now)).Count;
            //return (await Persistence.FieldModelQuery.Query(p => p.SmallDateTime == DateTime.Now)).Count;
            return (await Persistence.FieldModelQuery.Query(p => p.Date == DateTime.Now)).Count;
        }
    }
}
