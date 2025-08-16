using AutoCSer.Net;
using System;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase
{
    /// <summary>
    /// 服务端定义非对称测试接口
    /// </summary>
    [CommandServerControllerInterface(IsCodeGeneratorClientInterface = false, IsCodeGeneratorControllerAttribute = false)]
    public partial interface IDefinedDissymmetryServerController
    {
        void SetSocket(CommandServerSocket socket, Data.ORM.BusinessModel value);
        Data.ORM.BusinessModel GetSocket(CommandServerSocket socket);
        Task SetSocketTask(CommandServerSocket socket, Data.ORM.BusinessModel value);
        Task<Data.ORM.BusinessModel> GetSocketTask(CommandServerSocket socket);
    }
#if !AOT
    /// <summary>
    /// 服务端定义非对称测试接口实例
    /// </summary>
    internal sealed class DefinedDissymmetryServerController : IDefinedDissymmetryServerController
    {
        void IDefinedDissymmetryServerController.SetSocket(CommandServerSocket socket, Data.ORM.BusinessModel value)
        {
            AutoCSer.TestCase.ServerSynchronousController.GetSessionObject(socket).Model = value;
        }
        Data.ORM.BusinessModel IDefinedDissymmetryServerController.GetSocket(CommandServerSocket socket)
        {
            Data.ORM.BusinessModel value = AutoCSer.RandomObject.Creator<Data.ORM.BusinessModel>.Create();
            AutoCSer.TestCase.ServerSynchronousController.GetSessionObject(socket).Model = value;
            return value;
        }
        Task IDefinedDissymmetryServerController.SetSocketTask(CommandServerSocket socket, Data.ORM.BusinessModel value)
        {
            AutoCSer.TestCase.ServerSynchronousController.GetSessionObject(socket).Model = value;
            return AutoCSer.Common.CompletedTask;
        }
        Task<Data.ORM.BusinessModel> IDefinedDissymmetryServerController.GetSocketTask(CommandServerSocket socket)
        {
            Data.ORM.BusinessModel value = AutoCSer.RandomObject.Creator<Data.ORM.BusinessModel>.Create();
            AutoCSer.TestCase.ServerSynchronousController.GetSessionObject(socket).Model = value;
            return Task.FromResult(value);
        }
    }
#endif
}
