using AutoCSer.Metadata;
using AutoCSer.TestCase.CommonModel.TableModel.CustomColumn;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.BusinessService
{
    /// <summary>
    /// 自增ID与其它混合测试模型业务数据服务接口
    /// </summary>
    [AutoCSer.Net.CommandServerControllerInterface(MethodIndexEnumTypeCodeGeneratorPath = Persistence.CommandServerMethodIndexEnumTypePath, IsCodeGeneratorClientInterface = false)]
    public partial interface IAutoIdentityModelService : IAutoIdentityService<AutoIdentityModel, AutoCSer.TestCase.CommonModel.TableModel.AutoIdentityModel>
    {
        /// <summary>
        /// 自定义列查询测试
        /// </summary>
        /// <returns>查询数据数量</returns>
        Task<int> CustomColumnQuery();
    }
    /// <summary>
    /// 自增ID与其它混合测试模型业务数据服务
    /// </summary>
    public sealed class AutoIdentityModelService : IAutoIdentityModelService
    {
        /// <summary>
        /// Get data based on keywords
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="identity"></param>
        /// <returns>Return null on failure</returns>
        public async Task<AutoIdentityModel> Get(long identity)
        {
            return await AutoIdentityModel.Get(identity);
        }
        /// <summary>
        /// 添加数据并返回自增ID
        /// </summary>
        /// <param name="value"></param>
        /// <returns>失败返回-1</returns>
        public async Task<long> Insert(AutoIdentityModel value)
        {
            if (value == null) return -1;
            return await value.Insert();
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> Update(MemberMapValue<AutoCSer.TestCase.CommonModel.TableModel.AutoIdentityModel, AutoIdentityModel> value)
        {
            return value.Value != null && await value.Value.Update(value.MemberMap);
        }
        /// <summary>
        /// 根据关键字删除数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> Delete(AutoIdentityModel value)
        {
            return value != null && await value.Delete();
        }
        /// <summary>
        /// 根据关键字删除数据
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public async Task<bool> DeleteKey(long identity)
        {
            return await AutoIdentityModel.Delete(identity);
        }
        /// <summary>
        /// 自定义列查询测试
        /// </summary>
        /// <returns>查询数据数量</returns>
        public async Task<int> CustomColumnQuery()
        {
            DateTimeRange dateTimeRange = new DateTimeRange { Start = DateTime.Now, End = DateTime.Now.AddDays(1) };
            //return (await Persistence.AutoIdentityModelQuery.Query(p => p.RangeTime.Start <= DateTime.Now)).Count;
            //return (await Persistence.AutoIdentityModelQuery.Query(p => DateTime.Now <= p.RangeTime.Start)).Count;
            //return (await Persistence.AutoIdentityModelQuery.Query(p => p.RangeTime.End > p.RangeTime.Start)).Count;
            //return (await Persistence.AutoIdentityModelQuery.Query(p => p.Email == "a")).Count;
            //return (await Persistence.AutoIdentityModelQuery.Query(p => p.RangeTime == dateTimeRange)).Count;
            return (await Persistence.AutoIdentityModelQuery.Query(p => p.RangeTime != dateTimeRange)).Count;

        }
    }
}
