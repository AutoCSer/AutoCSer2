using AutoCSer.Net;
using AutoCSer.TestCase.CommonModel.BusinessServiceMethodEnum;
using AutoCSer.TestCase.CommonModel.TableModel;
using System;

namespace AutoCSer.TestCase.BusinessClient
{
    /// <summary>
    /// 自增ID与其它混合测试模型业务数据服务客户端接口
    /// </summary>
    [AutoCSer.Net.CommandServerControllerInterface(MethodIndexEnumType = typeof(AutoIdentityModelServiceMethodEnum), IsAutoMethodIndex = false, IsCodeGeneratorClientInterface = false)]
    public interface IAutoIdentityModelClient : IAutoIdentityClient<AutoIdentityModel>
    {
        /// <summary>
        /// 自定义列查询测试
        /// </summary>
        /// <returns>查询数据数量</returns>
        ReturnCommand<int> CustomColumnQuery();
    }
}
