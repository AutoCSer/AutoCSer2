using AutoCSer.Net;
using System;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase
{
    /// <summary>
    /// 客户端定义非对称测试接口
    /// </summary>
    [CommandServerControllerInterface(MethodIndexEnumType = typeof(DefinedDissymmetryControllerMethodEnum), IsAutoMethodIndex = false)]
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
    /// 命令客户端测试
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
            CommandClientReturnValue returnType = client.DefinedDissymmetryClientController.SetSocket(model);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(model))
            {
                return false;
            }

            CommandClientReturnValue<Data.ORM.ModelGeneric> returnValue = client.DefinedDissymmetryClientController.GetSocket();
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(returnValue.Value))
            {
                return false;
            }

            model = AutoCSer.RandomObject.Creator<Data.ORM.ModelGeneric>.Create();
            returnType = await client.DefinedDissymmetryClientController.SetSocketTask(model);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(model))
            {
                return false;
            }

            returnValue = await client.DefinedDissymmetryClientController.GetSocketTask();
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(returnValue.Value))
            {
                return false;
            }

            return true;
        }
    }
}
