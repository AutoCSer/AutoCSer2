﻿using AutoCSer.Net;
using System;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase.ServerBindContext
{
    /// <summary>
    /// 服务端定义非对称测试接口（套接字上下文绑定服务端）
    /// </summary>
    [CommandServerControllerInterface(MethodIndexEnumType = typeof(ServerBindContextDefinedDissymmetryControllerMethodEnum), IsAutoMethodIndex = false, MethodIndexEnumTypeCodeGeneratorPath = "")]
    public interface IDefinedDissymmetryServerController
    {
        void SetSocket(Data.ORM.BusinessModel value);
        Data.ORM.BusinessModel GetSocket();
        Task SetSocketTask(Data.ORM.BusinessModel value);
        Task<Data.ORM.BusinessModel> GetSocketTask();
    }
    /// <summary>
    /// 服务端定义非对称测试接口实例（套接字上下文绑定服务端）
    /// </summary>
    internal sealed class DefinedDissymmetryServerController : CommandServerBindContextController, IDefinedDissymmetryServerController
    {
        void IDefinedDissymmetryServerController.SetSocket(Data.ORM.BusinessModel value)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Model = value;
        }
        Data.ORM.BusinessModel IDefinedDissymmetryServerController.GetSocket()
        {
            Data.ORM.BusinessModel value = AutoCSer.RandomObject.Creator<Data.ORM.BusinessModel>.Create();
            ((CommandServerSessionObject)Socket.SessionObject).Model = value;
            return value;
        }
        Task IDefinedDissymmetryServerController.SetSocketTask(Data.ORM.BusinessModel value)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Model = value;
            return AutoCSer.Common.CompletedTask;
        }
        Task<Data.ORM.BusinessModel> IDefinedDissymmetryServerController.GetSocketTask()
        {
            Data.ORM.BusinessModel value = AutoCSer.RandomObject.Creator<Data.ORM.BusinessModel>.Create();
            ((CommandServerSessionObject)Socket.SessionObject).Model = value;
            return Task.FromResult(value);
        }
    }
}