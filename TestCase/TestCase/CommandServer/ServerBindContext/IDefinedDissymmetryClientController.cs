using AutoCSer.Net;
using System;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase.ServerBindContext
{
    /// <summary>
    /// 客户端定义非对称测试接口（套接字上下文绑定服务端）
    /// </summary>
    [CommandServerControllerInterface(MethodIndexEnumType = typeof(ServerBindContextDefinedDissymmetryControllerMethodEnum), IsAutoMethodIndex = false, IsCodeGeneratorClientInterface = false)]
    public interface IDefinedDissymmetryClientController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">数据类型定义与服务端定义不一致，但需要保证二者的序列化行为一致</param>
        /// <returns></returns>
        CommandClientReturnValue SetSocket(Data.ORM.ModelGeneric value);
        /// <summary>
        /// 
        /// </summary>
        /// <returns>数据类型定义与服务端定义不一致，但需要保证二者的序列化行为一致</returns>
        CommandClientReturnValue<Data.ORM.ModelGeneric> GetSocket();

        ReturnCommand SetSocketTask(Data.ORM.ModelGeneric value);
        ReturnCommand<Data.ORM.ModelGeneric> GetSocketTask();
    }
    /// <summary>
    /// 命令客户端测试（套接字上下文绑定服务端）
    /// </summary>
    internal static class DefinedDissymmetryClientController
    {
        /// <summary>
        /// 命令客户端测试
        /// </summary>
        /// <param name="client"></param>
        /// <param name="clientSessionObject"></param>
        /// <returns></returns>
        internal static async Task<bool> TestCase(CommandClientSocketEvent client, CommandServerSessionObject clientSessionObject)
        {
            Data.ORM.ModelGeneric model = AutoCSer.RandomObject.Creator<Data.ORM.ModelGeneric>.Create();
            CommandClientReturnValue returnType = client.ServerBindContextDefinedDissymmetryClientController.SetSocket(model);
            if (!returnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(model))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            CommandClientReturnValue<Data.ORM.ModelGeneric> returnValue = client.ServerBindContextDefinedDissymmetryClientController.GetSocket();
            if (!returnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            model = AutoCSer.RandomObject.Creator<Data.ORM.ModelGeneric>.Create();
            returnType = await client.ServerBindContextDefinedDissymmetryClientController.SetSocketTask(model);
            if (!returnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(model))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = await client.ServerBindContextDefinedDissymmetryClientController.GetSocketTask();
            if (!returnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            return true;
        }
    }
}
