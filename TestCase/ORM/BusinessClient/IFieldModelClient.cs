﻿using AutoCSer.Net;
using AutoCSer.TestCase.BusinessService;
using AutoCSer.TestCase.CommonModel.TableModel;
using System;

namespace AutoCSer.TestCase.BusinessClient
{
    /// <summary>
    /// 字段测试模型业务数据服务客户端接口
    /// </summary>
    [AutoCSer.Net.CommandServer.ServerControllerInterface(typeof(IFieldModelServiceMethodEnum))]
    [AutoCSer.Net.CommandServerControllerInterface(IsCodeGeneratorMethodEnum = false, IsCodeGeneratorClientInterface = false)]
    public interface IFieldModelClient : IPrimaryKeyClient<FieldModel, long>
    {
        /// <summary>
        /// 自定义列查询测试
        /// </summary>
        /// <returns>查询数据数量</returns>
        ReturnCommand<int> CustomColumnQuery();
    }
}