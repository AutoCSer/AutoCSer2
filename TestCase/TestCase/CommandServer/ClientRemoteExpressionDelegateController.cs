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
            CommandClientReturnValue<int> returnValue = await client.ClientRemoteExpressionDelegateController.Func(() => value + (int)value64 + 1);
            if (!returnValue.IsSuccess || returnValue.Value != value + (int)value64 + 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            value = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientRemoteExpressionDelegateController.Func1(session => session.Value - value);
            if (!returnValue.IsSuccess || returnValue.Value != clientSessionObject.Value - value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            int parameter = AutoCSer.Random.Default.Next();
            value = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientRemoteExpressionDelegateController.Func2((session, p) => session.Value + (-p) - (+value), parameter);
            if (!returnValue.IsSuccess || returnValue.Value != clientSessionObject.Value - parameter - value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            int parameter2 = AutoCSer.Random.Default.Next();
            parameter = AutoCSer.Random.Default.Next();
            value = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientRemoteExpressionDelegateController.Func3((session, p1, p2) => session.Value ^ p1 & ~p2 | value, parameter, parameter2);
            if (!returnValue.IsSuccess || returnValue.Value != (clientSessionObject.Value ^ parameter & ~parameter2 | value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            int[] data = new int[] { AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next() };
            clientSessionObject.Set(data[1] << data.Length);
            returnValue = await client.ClientRemoteExpressionDelegateController.Action(() => ServerSynchronousController.SessionObject.Set(data[1] << data.Length));
            if (!returnValue.IsSuccess || returnValue.Value != clientSessionObject.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            value = AutoCSer.Random.Default.Next();
            clientSessionObject.Set(value >> 1);
            returnValue = await client.ClientRemoteExpressionDelegateController.Action1(session => session.Set(value >> 1));
            if (!returnValue.IsSuccess || returnValue.Value != clientSessionObject.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            parameter = AutoCSer.Random.Default.Next();
            value = AutoCSer.Random.Default.Next();
            clientSessionObject.Set(parameter + value);
            returnValue = await client.ClientRemoteExpressionDelegateController.Action2((session, p) => session.Set(p + value), parameter);
            if (!returnValue.IsSuccess || returnValue.Value != clientSessionObject.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            parameter2 = AutoCSer.Random.Default.Next();
            parameter = AutoCSer.Random.Default.Next();
            value = AutoCSer.Random.Default.Next();
            clientSessionObject.Set(parameter % value * parameter2 / 3);
            returnValue = await client.ClientRemoteExpressionDelegateController.Action3((session, p1, p2) => session.Set(p1 % value * p2 / 3), parameter, parameter2);
            if (!returnValue.IsSuccess || returnValue.Value != clientSessionObject.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            parameter = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientRemoteExpressionDelegateController.Unbox((session, p) => session.Value + (int)p, parameter);
            if (!returnValue.IsSuccess || returnValue.Value != clientSessionObject.Value + parameter)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            parameter = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientRemoteExpressionDelegateController.TypeBinaryExpression((session, p) => (session is CommandServerSessionObject) ? parameter + 1 : parameter, parameter);
            if (!returnValue.IsSuccess || returnValue.Value != parameter + 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            string stringValue = new string(new char[] { (char)AutoCSer.Random.Default.NextUShort(), (char)AutoCSer.Random.Default.NextUShort() });
            parameter = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientRemoteExpressionDelegateController.Index(session => (session as CommandServerSessionObject).Value + stringValue[1]);
            if (!returnValue.IsSuccess || returnValue.Value != clientSessionObject.Value + stringValue[1])
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            parameter = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientRemoteExpressionDelegateController.Default((session, session2) => (session ?? session2).Value + new int[default(int)].Length);
            if (!returnValue.IsSuccess || returnValue.Value != clientSessionObject.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            parameter = AutoCSer.Random.Default.Next();
            if (parameter == int.MaxValue) parameter = 1;
            parameter2 = parameter + 1;
            CommandServerSessionObject nullableValue = new CommandServerSessionObject { NullableValue = parameter };
            CommandClientReturnValue<bool> boolReturnValue = await client.ClientRemoteExpressionDelegateController.Logical((p, p2) => nullableValue.NullableValue + 1 == p2 && nullableValue.NullableValue <= p2 && p + 1 == p2 && p <= p2, parameter, parameter2);
            if (!boolReturnValue.IsSuccess || !boolReturnValue.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            parameter = AutoCSer.Random.Default.Next();
            if (parameter == int.MinValue) parameter = 1;
            parameter2 = parameter - 1;
            nullableValue.NullableValue = parameter;
            boolReturnValue = await client.ClientRemoteExpressionDelegateController.Logical((p, p2) => (nullableValue.NullableValue == p2 || nullableValue.NullableValue > p2) && (p == p2 || p > p2), parameter, parameter2);
            if (!boolReturnValue.IsSuccess || !boolReturnValue.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            parameter = AutoCSer.Random.Default.Next();
            value = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientRemoteExpressionDelegateController.Func2((session, p) => new CommandServerSessionObject { Property = session.Value }.Property - new CommandServerSessionObject(value).Value + new int[] { AutoCSer.Random.Default.Next(), p }[1], parameter);
            if (!returnValue.IsSuccess || returnValue.Value != clientSessionObject.Value - value + parameter)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }


            value64 = AutoCSer.Random.Default.NextULong();
            value = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientRemoteExpressionDelegateController.LambdaFunc(() => value + (int)value64 + 1);
            if (!returnValue.IsSuccess || returnValue.Value != value + (int)value64 + 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            value = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientRemoteExpressionDelegateController.LambdaFunc1(session => session.Value - value);
            if (!returnValue.IsSuccess || returnValue.Value != clientSessionObject.Value - value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            parameter = AutoCSer.Random.Default.Next();
            value = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientRemoteExpressionDelegateController.LambdaFunc2((session, p) => session.Value + (-p) - (+value), parameter);
            if (!returnValue.IsSuccess || returnValue.Value != clientSessionObject.Value - parameter - value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            parameter2 = AutoCSer.Random.Default.Next();
            parameter = AutoCSer.Random.Default.Next();
            value = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientRemoteExpressionDelegateController.LambdaFunc3((session, p1, p2) => session.Value ^ p1 & ~p2 | value, parameter, parameter2);
            if (!returnValue.IsSuccess || returnValue.Value != (clientSessionObject.Value ^ parameter & ~parameter2 | value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            data = new int[] { AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next() };
            clientSessionObject.Set(data[1] << data.Length);
            returnValue = await client.ClientRemoteExpressionDelegateController.LambdaAction(() => ServerSynchronousController.SessionObject.Set(data[1] << data.Length));
            if (!returnValue.IsSuccess || returnValue.Value != clientSessionObject.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            value = AutoCSer.Random.Default.Next();
            clientSessionObject.Set(value >> 1);
            returnValue = await client.ClientRemoteExpressionDelegateController.LambdaAction1(session => session.Set(value >> 1));
            if (!returnValue.IsSuccess || returnValue.Value != clientSessionObject.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            parameter = AutoCSer.Random.Default.Next();
            value = AutoCSer.Random.Default.Next();
            clientSessionObject.Set(parameter + value);
            returnValue = await client.ClientRemoteExpressionDelegateController.LambdaAction2((session, p) => session.Set(p + value), parameter);
            if (!returnValue.IsSuccess || returnValue.Value != clientSessionObject.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            parameter2 = AutoCSer.Random.Default.Next();
            parameter = AutoCSer.Random.Default.Next();
            value = AutoCSer.Random.Default.Next();
            clientSessionObject.Set(parameter % value * parameter2 / 3);
            returnValue = await client.ClientRemoteExpressionDelegateController.LambdaAction3((session, p1, p2) => session.Set(p1 % value * p2 / 3), parameter, parameter2);
            if (!returnValue.IsSuccess || returnValue.Value != clientSessionObject.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            parameter = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientRemoteExpressionDelegateController.LambdaUnbox((session, p) => session.Value + (int)p, parameter);
            if (!returnValue.IsSuccess || returnValue.Value != clientSessionObject.Value + parameter)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            parameter = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientRemoteExpressionDelegateController.LambdaTypeBinaryExpression((session, p) => (session is CommandServerSessionObject) ? parameter + 1 : parameter, parameter);
            if (!returnValue.IsSuccess || returnValue.Value != parameter + 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            stringValue = new string(new char[] { (char)AutoCSer.Random.Default.NextUShort(), (char)AutoCSer.Random.Default.NextUShort() });
            parameter = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientRemoteExpressionDelegateController.LambdaIndex(session => (session as CommandServerSessionObject).Value + stringValue[1]);
            if (!returnValue.IsSuccess || returnValue.Value != clientSessionObject.Value + stringValue[1])
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            parameter = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientRemoteExpressionDelegateController.LambdaDefault((session, session2) => (session ?? session2).Value + new int[default(int) + 1].Length);
            if (!returnValue.IsSuccess || returnValue.Value != clientSessionObject.Value + 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            parameter = AutoCSer.Random.Default.Next();
            if (parameter == int.MaxValue) parameter = 1;
            parameter2 = parameter + 1;
            boolReturnValue = await client.ClientRemoteExpressionDelegateController.LambdaLogical((p, p2) => nullableValue.NullableValue != null && p + 1 == p2 && p <= p2, parameter, parameter2);
            if (!boolReturnValue.IsSuccess || !boolReturnValue.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            parameter = AutoCSer.Random.Default.Next();
            if (parameter == int.MinValue) parameter = 1;
            parameter2 = parameter - 1;
            boolReturnValue = await client.ClientRemoteExpressionDelegateController.LambdaLogical((p, p2) => nullableValue.NullableValue == null || p == p2 || p > p2, parameter, parameter2);
            if (!boolReturnValue.IsSuccess || !boolReturnValue.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            parameter = AutoCSer.Random.Default.Next();
            value = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientRemoteExpressionDelegateController.LambdaFunc2((session, p) => new CommandServerSessionObject { Value = session.Value }.Value - new CommandServerSessionObject(value).Value + new int[] { AutoCSer.Random.Default.Next(), p }[1], parameter);
            if (!returnValue.IsSuccess || returnValue.Value != clientSessionObject.Value - value + parameter)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            return true;
        }
    }
}
