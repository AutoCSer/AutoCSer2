using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using System;

#pragma warning disable
namespace AutoCSer.TestCase
{
    /// <summary>
    /// 远程表达式委托服务端测试接口
    /// </summary>
    [CommandServerControllerInterface]
    public partial interface IServerRemoteExpressionDelegateController
    {
        int Func(RemoteExpressionFunc<int> func);
        int Func1(CommandServerSocket socket, RemoteExpressionFunc<CommandServerSessionObject, int> func);
        int Func2(CommandServerSocket socket, RemoteExpressionFunc<CommandServerSessionObject, int, int> func, int parameter);
        int Func3(CommandServerSocket socket, RemoteExpressionFunc<CommandServerSessionObject, int, int, int> func, int parameter, int parameter2);
        int Action(CommandServerSocket socket, RemoteExpressionAction action);
        int Action1(CommandServerSocket socket, RemoteExpressionAction<CommandServerSessionObject> action);
        int Action2(CommandServerSocket socket, RemoteExpressionAction<CommandServerSessionObject, int> action, int parameter);
        int Action3(CommandServerSocket socket, RemoteExpressionAction<CommandServerSessionObject, int, int> action, int parameter, int parameter2);

        int Unbox(CommandServerSocket socket, RemoteExpressionFunc<CommandServerSessionObject, object, int> func, int parameter);
        int TypeBinaryExpression(CommandServerSocket socket, RemoteExpressionFunc<object, int, int> func, int parameter);
        int Index(CommandServerSocket socket, RemoteExpressionFunc<object, int> func);
        int Default(CommandServerSocket socket, RemoteExpressionFunc<CommandServerSessionObject, CommandServerSessionObject, int> func);
        bool Logical(RemoteExpressionFunc<int, int, bool> func, int parameter, int parameter2);

        int LambdaFunc(RemoteLambdaExpression<Func<int>> func);
        int LambdaFunc1(CommandServerSocket socket, RemoteLambdaExpression<Func<CommandServerSessionObject, int>> func);
        int LambdaFunc2(CommandServerSocket socket, RemoteLambdaExpression<Func<CommandServerSessionObject, int, int>> func, int parameter);
        int LambdaFunc3(CommandServerSocket socket, RemoteLambdaExpression<Func<CommandServerSessionObject, int, int, int>> func, int parameter, int parameter2);
        int LambdaAction(CommandServerSocket socket, RemoteLambdaExpression<Action> action);
        int LambdaAction1(CommandServerSocket socket, RemoteLambdaExpression<Action<CommandServerSessionObject>> action);
        int LambdaAction2(CommandServerSocket socket, RemoteLambdaExpression<Action<CommandServerSessionObject, int>> action, int parameter);
        int LambdaAction3(CommandServerSocket socket, RemoteLambdaExpression<Action<CommandServerSessionObject, int, int>> action, int parameter, int parameter2);

        int LambdaUnbox(CommandServerSocket socket, RemoteLambdaExpression<Func<CommandServerSessionObject, object, int>> func, int parameter);
        int LambdaTypeBinaryExpression(CommandServerSocket socket, RemoteLambdaExpression<Func<object, int, int>> func, int parameter);
        int LambdaIndex(CommandServerSocket socket, RemoteLambdaExpression<Func<object, int>> func);
        int LambdaDefault(CommandServerSocket socket, RemoteLambdaExpression<Func<CommandServerSessionObject, CommandServerSessionObject, int>> func);
        bool LambdaLogical(RemoteLambdaExpression<Func<int, int, bool>> func, int parameter, int parameter2);
    }
    /// <summary>
    /// 远程表达式委托服务端测试接口
    /// </summary>
    internal sealed class ServerRemoteExpressionDelegateController : IServerRemoteExpressionDelegateController
    {
        int IServerRemoteExpressionDelegateController.Func(RemoteExpressionFunc<int> func)
        {
            return func.Call();
        }
        int IServerRemoteExpressionDelegateController.Func1(CommandServerSocket socket, RemoteExpressionFunc<CommandServerSessionObject, int> func)
        {
            return func.Call((CommandServerSessionObject)socket.SessionObject);
        }
        int IServerRemoteExpressionDelegateController.Func2(CommandServerSocket socket, RemoteExpressionFunc<CommandServerSessionObject, int, int> func, int parameter)
        {
            return func.Call((CommandServerSessionObject)socket.SessionObject, parameter);
        }
        int IServerRemoteExpressionDelegateController.Func3(CommandServerSocket socket, RemoteExpressionFunc<CommandServerSessionObject, int, int, int> func, int parameter, int parameter2)
        {
            return func.Call((CommandServerSessionObject)socket.SessionObject, parameter, parameter2);
        }
        int IServerRemoteExpressionDelegateController.Action(CommandServerSocket socket, RemoteExpressionAction action)
        {
            action.Call();
            return ((CommandServerSessionObject)socket.SessionObject).Value;
        }
        int IServerRemoteExpressionDelegateController.Action1(CommandServerSocket socket, RemoteExpressionAction<CommandServerSessionObject> action)
        {
            CommandServerSessionObject sessionObject = (CommandServerSessionObject)socket.SessionObject;
            action.Call(sessionObject);
            return sessionObject.Value;
        }
        int IServerRemoteExpressionDelegateController.Action2(CommandServerSocket socket, RemoteExpressionAction<CommandServerSessionObject, int> action, int parameter)
        {
            CommandServerSessionObject sessionObject = (CommandServerSessionObject)socket.SessionObject;
            action.Call(sessionObject, parameter);
            return sessionObject.Value;
        }
        int IServerRemoteExpressionDelegateController.Action3(CommandServerSocket socket, RemoteExpressionAction<CommandServerSessionObject, int, int> action, int parameter, int parameter2)
        {
            CommandServerSessionObject sessionObject = (CommandServerSessionObject)socket.SessionObject;
            action.Call(sessionObject, parameter, parameter2);
            return sessionObject.Value;
        }

        int IServerRemoteExpressionDelegateController.Unbox(CommandServerSocket socket, RemoteExpressionFunc<CommandServerSessionObject, object, int> func, int parameter)
        {
            object value = parameter;
            return func.Call((CommandServerSessionObject)socket.SessionObject, value);
        }
        int IServerRemoteExpressionDelegateController.TypeBinaryExpression(CommandServerSocket socket, RemoteExpressionFunc<object, int, int> func, int parameter)
        {
            return func.Call(socket.SessionObject, parameter);
        }
        int IServerRemoteExpressionDelegateController.Index(CommandServerSocket socket, RemoteExpressionFunc<object, int> func)
        {
            return func.Call((CommandServerSessionObject)socket.SessionObject);
        }
        int IServerRemoteExpressionDelegateController.Default(CommandServerSocket socket, RemoteExpressionFunc<CommandServerSessionObject, CommandServerSessionObject, int> func)
        {
            return func.Call(null, (CommandServerSessionObject)socket.SessionObject);
        }
        bool IServerRemoteExpressionDelegateController.Logical(RemoteExpressionFunc<int, int, bool> func, int parameter, int parameter2)
        {
            return func.Call(parameter, parameter2);
        }

        int IServerRemoteExpressionDelegateController.LambdaFunc(RemoteLambdaExpression<Func<int>> func)
        {
            return func.Compile()();
        }
        int IServerRemoteExpressionDelegateController.LambdaFunc1(CommandServerSocket socket, RemoteLambdaExpression<Func<CommandServerSessionObject, int>> func)
        {
            return func.Compile()((CommandServerSessionObject)socket.SessionObject);
        }
        int IServerRemoteExpressionDelegateController.LambdaFunc2(CommandServerSocket socket, RemoteLambdaExpression<Func<CommandServerSessionObject, int, int>> func, int parameter)
        {
            return func.Compile()((CommandServerSessionObject)socket.SessionObject, parameter);
        }
        int IServerRemoteExpressionDelegateController.LambdaFunc3(CommandServerSocket socket, RemoteLambdaExpression<Func<CommandServerSessionObject, int, int, int>> func, int parameter, int parameter2)
        {
            return func.Compile()((CommandServerSessionObject)socket.SessionObject, parameter, parameter2);
        }
        int IServerRemoteExpressionDelegateController.LambdaAction(CommandServerSocket socket, RemoteLambdaExpression<Action> action)
        {
            action.Compile()();
            return ((CommandServerSessionObject)socket.SessionObject).Value;
        }
        int IServerRemoteExpressionDelegateController.LambdaAction1(CommandServerSocket socket, RemoteLambdaExpression<Action<CommandServerSessionObject>> action)
        {
            CommandServerSessionObject sessionObject = (CommandServerSessionObject)socket.SessionObject;
            action.Compile()(sessionObject);
            return sessionObject.Value;
        }
        int IServerRemoteExpressionDelegateController.LambdaAction2(CommandServerSocket socket, RemoteLambdaExpression<Action<CommandServerSessionObject, int>> action, int parameter)
        {
            CommandServerSessionObject sessionObject = (CommandServerSessionObject)socket.SessionObject;
            action.Compile()(sessionObject, parameter);
            return sessionObject.Value;
        }
        int IServerRemoteExpressionDelegateController.LambdaAction3(CommandServerSocket socket, RemoteLambdaExpression<Action<CommandServerSessionObject, int, int>> action, int parameter, int parameter2)
        {
            CommandServerSessionObject sessionObject = (CommandServerSessionObject)socket.SessionObject;
            action.Compile()(sessionObject, parameter, parameter2);
            return sessionObject.Value;
        }

        int IServerRemoteExpressionDelegateController.LambdaUnbox(CommandServerSocket socket, RemoteLambdaExpression<Func<CommandServerSessionObject, object, int>> func, int parameter)
        {
            object value = parameter;
            return func.Compile()((CommandServerSessionObject)socket.SessionObject, value);
        }
        int IServerRemoteExpressionDelegateController.LambdaTypeBinaryExpression(CommandServerSocket socket, RemoteLambdaExpression<Func<object, int, int>> func, int parameter)
        {
            return func.Compile()(socket.SessionObject, parameter);
        }
        int IServerRemoteExpressionDelegateController.LambdaIndex(CommandServerSocket socket, RemoteLambdaExpression<Func<object, int>> func)
        {
            return func.Compile()((CommandServerSessionObject)socket.SessionObject);
        }
        int IServerRemoteExpressionDelegateController.LambdaDefault(CommandServerSocket socket, RemoteLambdaExpression<Func<CommandServerSessionObject, CommandServerSessionObject, int>> func)
        {
            return func.Compile()(null, (CommandServerSessionObject)socket.SessionObject);
        }
        bool IServerRemoteExpressionDelegateController.LambdaLogical(RemoteLambdaExpression<Func<int, int, bool>> func, int parameter, int parameter2)
        {
            return func.Compile()(parameter, parameter2);
        }
    }
}
