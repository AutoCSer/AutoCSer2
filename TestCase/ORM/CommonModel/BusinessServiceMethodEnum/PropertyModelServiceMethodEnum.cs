//本文件由程序自动生成，请不要自行修改
using System;
using System.Numerics;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.TestCase.CommonModel.BusinessServiceMethodEnum
{
        /// <summary>
        /// 属性测试模型业务数据服务接口方法编号，编号只允许往后追加不允许中间插入枚举值（由于 API 路由采用数字编号，当服务接口定义变更以后不能保证同一个 API 的路由一致，所以建议采用枚举映射）
        /// </summary>
    public enum PropertyModelServiceMethodEnum
    {
            /// <summary>
            /// [0] 
            /// string key 
            /// 返回值 AutoCSer.TestCase.BusinessService.PropertyModel 
            /// </summary>
            Get = 0,
            /// <summary>
            /// [1] 
            /// AutoCSer.TestCase.BusinessService.PropertyModel value 
            /// 返回值 bool 
            /// </summary>
            Insert = 1,
            /// <summary>
            /// [2] 
            /// AutoCSer.Metadata.MemberMapValue{AutoCSer.TestCase.CommonModel.TableModel.PropertyModel,AutoCSer.TestCase.BusinessService.PropertyModel} value 
            /// 返回值 bool 
            /// </summary>
            Update = 2,
            /// <summary>
            /// [3] 
            /// AutoCSer.TestCase.BusinessService.PropertyModel value 
            /// 返回值 bool 
            /// </summary>
            Delete = 3,
            /// <summary>
            /// [4] 
            /// string key 
            /// 返回值 bool 
            /// </summary>
            DeleteKey = 4,
            /// <summary>
            /// [5] 
            /// 返回值 int 
            /// </summary>
            CustomColumnQuery = 5,
    }
}
#endif