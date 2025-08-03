using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using System;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase
{
    /// <summary>
    /// 远程表达式委托客户端测试
    /// </summary>
    internal partial class ClientRemoteExpressionDelegateController
    {
        /// <summary>
        /// 命令客户端测试
        /// </summary>
        /// <param name="client"></param>
        /// <param name="clientSessionObject"></param>
        /// <returns></returns>
        internal static async Task<bool> TestCase(CommandClientSocketEvent client, CommandServerSessionObject clientSessionObject)
        {
            ulong value64 = AutoCSer.Random.Default.NextULong();
            int value = AutoCSer.Random.Default.Next();
            CommandClientReturnValue<int> returnValue = await client.ClientRemoteExpressionDelegateController.Func(RemoteExpressionFunc.Get(() => value + (int)value64 + 1));
            if (!returnValue.IsSuccess || returnValue.Value != value + (int)value64 + 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            value = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientRemoteExpressionDelegateController.Func1(new RemoteExpressionFunc<CommandServerSessionObject, int>(session => session.Value - value));
            if (!returnValue.IsSuccess || returnValue.Value != clientSessionObject.Value - value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            int parameter = AutoCSer.Random.Default.Next();
            value = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientRemoteExpressionDelegateController.Func2(new RemoteExpressionFunc<CommandServerSessionObject, int, int>((session, p) => session.Value + (-p) - (+value)), parameter);
            if (!returnValue.IsSuccess || returnValue.Value != clientSessionObject.Value - parameter - value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            int parameter2 = AutoCSer.Random.Default.Next();
            parameter = AutoCSer.Random.Default.Next();
            value = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientRemoteExpressionDelegateController.Func3(new RemoteExpressionFunc<CommandServerSessionObject, int, int, int>((session, p1, p2) => session.Value ^ p1 & ~p2 | value), parameter, parameter2);
            if (!returnValue.IsSuccess || returnValue.Value != (clientSessionObject.Value ^ parameter & ~parameter2 | value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            int[] data = new int[] { AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next() };
            clientSessionObject.Set(data[1] << data.Length);
            returnValue = await client.ClientRemoteExpressionDelegateController.Action(new RemoteExpressionAction(() => ServerSynchronousController.SessionObject.Set(data[1] << data.Length)));
            if (!returnValue.IsSuccess || returnValue.Value != clientSessionObject.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            value = AutoCSer.Random.Default.Next();
            clientSessionObject.Set(value >> 1);
            returnValue = await client.ClientRemoteExpressionDelegateController.Action1(new RemoteExpressionAction<CommandServerSessionObject>(session => session.Set(value >> 1)));
            if (!returnValue.IsSuccess || returnValue.Value != clientSessionObject.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            parameter = AutoCSer.Random.Default.Next();
            value = AutoCSer.Random.Default.Next();
            clientSessionObject.Set(parameter + value);
            returnValue = await client.ClientRemoteExpressionDelegateController.Action2(new RemoteExpressionAction<CommandServerSessionObject, int>((session, p) => session.Set(p + value)), parameter);
            if (!returnValue.IsSuccess || returnValue.Value != clientSessionObject.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            parameter2 = AutoCSer.Random.Default.Next();
            parameter = AutoCSer.Random.Default.Next();
            value = AutoCSer.Random.Default.Next();
            clientSessionObject.Set(parameter % value * parameter2 / 3);
            returnValue = await client.ClientRemoteExpressionDelegateController.Action3(new RemoteExpressionAction<CommandServerSessionObject, int, int>((session, p1, p2) => session.Set(p1 % value * p2 / 3)), parameter, parameter2);
            if (!returnValue.IsSuccess || returnValue.Value != clientSessionObject.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            parameter = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientRemoteExpressionDelegateController.Unbox(new RemoteExpressionFunc<CommandServerSessionObject, object, int>((session, p) => session.Value + (int)p), parameter);
            if (!returnValue.IsSuccess || returnValue.Value != clientSessionObject.Value + parameter)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            parameter = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientRemoteExpressionDelegateController.TypeBinaryExpression(new RemoteExpressionFunc<object, int, int>((session, p) => (session is CommandServerSessionObject) ? parameter + 1 : parameter), parameter);
            if (!returnValue.IsSuccess || returnValue.Value != parameter + 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            string stringValue = new string(new char[] { (char)AutoCSer.Random.Default.NextUShort(), (char)AutoCSer.Random.Default.NextUShort() });
            parameter = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientRemoteExpressionDelegateController.Index(new RemoteExpressionFunc<object, int>(session => (session as CommandServerSessionObject).Value + stringValue[1]));
            if (!returnValue.IsSuccess || returnValue.Value != clientSessionObject.Value + stringValue[1])
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            parameter = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientRemoteExpressionDelegateController.Default(new RemoteExpressionFunc<CommandServerSessionObject, CommandServerSessionObject, int>((session, session2) => (session ?? session2).Value + default(int)));
            if (!returnValue.IsSuccess || returnValue.Value != clientSessionObject.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            parameter = AutoCSer.Random.Default.Next();
            if (parameter == int.MaxValue) parameter = 1;
            parameter2 = parameter + 1;
            CommandClientReturnValue<bool> boolReturnValue = await client.ClientRemoteExpressionDelegateController.Logical(new RemoteExpressionFunc<int, int, bool>((p, p2) => p + 1 == p2 && p <= p2), parameter, parameter2);
            if (!boolReturnValue.IsSuccess || !boolReturnValue.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            parameter = AutoCSer.Random.Default.Next();
            if (parameter == int.MinValue) parameter = 1;
            parameter2 = parameter - 1;
            boolReturnValue = await client.ClientRemoteExpressionDelegateController.Logical(new RemoteExpressionFunc<int, int, bool>((p, p2) => p == p2 || p > p2), parameter, parameter2);
            if (!boolReturnValue.IsSuccess || !boolReturnValue.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            return true;
        }
    }
}
