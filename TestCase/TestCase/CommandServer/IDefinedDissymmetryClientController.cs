using AutoCSer.Net;
using System;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase
{
    /// <summary>
    /// 客户端定义非对称测试接口
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.CommandClientController(typeof(void), true)]
#endif
    [AutoCSer.Net.CommandServer.ServerControllerInterfaceAttribute(typeof(IDefinedDissymmetryServerControllerMethodEnum))]
    [CommandServerControllerInterface(IsCodeGeneratorMethodEnum = false, IsCodeGeneratorClientInterface = false)]
    public partial interface IDefinedDissymmetryClientController
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
    internal partial class DefinedDissymmetryClientController
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
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            CommandClientReturnValue<Data.ORM.ModelGeneric> returnValue = client.DefinedDissymmetryClientController.GetSocket();
            if (!returnValue.IsSuccess || !ServerSynchronousController.SessionObject.Check(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            model = AutoCSer.RandomObject.Creator<Data.ORM.ModelGeneric>.Create();
            returnType = await client.DefinedDissymmetryClientController.SetSocketTask(model);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(model))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            Data.ORM.ModelGeneric result = await client.DefinedDissymmetryClientController.GetSocketTask().GetCommandReturnValue();
            if (!ServerSynchronousController.SessionObject.Check(result))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            return true;
        }
        /// <summary>
        /// 默认控制器测试
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal static async Task<bool> DefaultControllerTestCase(CommandClientSocketEvent client)
        {
            CommandClientReturnValue returnType = client.DefinedDissymmetryClientController.SetSocket(null);
            if (returnType.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            CommandClientReturnValue<Data.ORM.ModelGeneric> returnValue = client.DefinedDissymmetryClientController.GetSocket();
            if (returnValue.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnType = await client.DefinedDissymmetryClientController.SetSocketTask(null);
            if (returnType.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = await client.DefinedDissymmetryClientController.GetSocketTask();
            if (returnValue.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            return true;
        }
        /// <summary>
        /// 短连接命令客户端测试
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> ShortLinkTestCase()
        {
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.DefinedDissymmetryClientController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                Data.ORM.ModelGeneric model = AutoCSer.RandomObject.Creator<Data.ORM.ModelGeneric>.Create();
                CommandClientReturnValue returnType = client.DefinedDissymmetryClientController.SetSocket(model);
                if (!returnType.IsSuccess)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.DefinedDissymmetryClientController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                CommandClientReturnValue<Data.ORM.ModelGeneric> returnValue = client.DefinedDissymmetryClientController.GetSocket();
                if (!returnValue.IsSuccess || returnValue.Value == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.DefinedDissymmetryClientController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                Data.ORM.ModelGeneric model = AutoCSer.RandomObject.Creator<Data.ORM.ModelGeneric>.Create();
                CommandClientReturnValue returnType = await client.DefinedDissymmetryClientController.SetSocketTask(model);
                if (!returnType.IsSuccess)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.DefinedDissymmetryClientController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                CommandClientReturnValue<Data.ORM.ModelGeneric> returnValue = await client.DefinedDissymmetryClientController.GetSocketTask();
                if (!returnValue.IsSuccess || returnValue.Value == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            return true;
        }
    }
}
