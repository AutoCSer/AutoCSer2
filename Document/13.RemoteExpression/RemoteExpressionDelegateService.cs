using AutoCSer.Extensions;
using System.Data.SqlTypes;

namespace AutoCSer.Document.RemoteExpression
{
    /// <summary>
    /// Remote expression delegate test service instance
    /// 远程表达式委托测试服务实例
    /// </summary>
    internal sealed class RemoteExpressionDelegateService : IRemoteExpressionDelegateService
    {
        /// <summary>
        /// The test API of the parameterless delegate Func{T}
        /// 无参委托 Func{T} 测试 API
        /// </summary>
        /// <param name="func">Remote expression delegate Func{RT}
        /// 远程表达式委托 Func{RT}</param>
        /// <returns></returns>
        int IRemoteExpressionDelegateService.Func(AutoCSer.Net.CommandServer.RemoteExpressionFunc<int> func)
        {
            return func.Call();
        }
        /// <summary>
        /// The test API of the delegate Func{T, RT}
        /// 委托 Func{T, RT} 测试 API
        /// </summary>
        /// <param name="func">Remote expression delegate Func{T, RT}
        /// 远程表达式委托 Func{T, RT}</param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        int IRemoteExpressionDelegateService.Func1(AutoCSer.Net.CommandServer.RemoteExpressionFunc<int, int> func, int parameter)
        {
            return func.Call(parameter);
        }
        /// <summary>
        /// The test API of the delegate Func{T1, T2, RT}
        /// 委托 Func{T1, T2, RT} 测试 API
        /// </summary>
        /// <param name="func">Remote expression delegate Func{T1, T2, RT}
        /// 远程表达式委托 Func{T1, T2, RT}</param>
        /// <param name="parameter1"></param>
        /// <param name="parameter2"></param>
        /// <returns></returns>
        int IRemoteExpressionDelegateService.Func2(AutoCSer.Net.CommandServer.RemoteExpressionFunc<int, int, int> func, int parameter1, int parameter2)
        {
            return func.Call(parameter1, parameter2);
        }
        /// <summary>
        /// The test API of the delegate Func{T1, T2, T3, RT}
        /// 委托 Func{T1, T2, T3, RT} 测试 API
        /// </summary>
        /// <param name="func">Remote expression delegate Func{T1, T2, T3, RT}
        /// 远程表达式委托 Func{T1, T2, T3, RT}</param>
        /// <param name="parameter1"></param>
        /// <param name="parameter2"></param>
        /// <param name="parameter3"></param>
        /// <returns></returns>
        int IRemoteExpressionDelegateService.Func3(AutoCSer.Net.CommandServer.RemoteExpressionFunc<int, int, int, int> func, int parameter1, int parameter2, int parameter3)
        {
            return func.Call(parameter1, parameter2, parameter3);
        }

        /// <summary>
        /// The test API of the parameterless delegate Action
        /// 委托 Action 测试 API
        /// </summary>
        /// <param name="action">Remote expression delegate Action
        /// 远程表达式委托 Action</param>
        /// <returns></returns>
        int IRemoteExpressionDelegateService.Action(AutoCSer.Net.CommandServer.RemoteExpressionAction action)
        {
            action.Call();
            return ActionTarget.Default.Value;
        }
        /// <summary>
        /// The test API of the delegate Action{T}
        /// 委托 Action{T} 测试 API
        /// </summary>
        /// <param name="action">Remote expression delegate Action{T}
        /// 远程表达式委托 Action{T}</param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        int IRemoteExpressionDelegateService.Action1(AutoCSer.Net.CommandServer.RemoteExpressionAction<int> action, int parameter)
        {
            action.Call(parameter);
            return ActionTarget.Default.Value;
        }
        /// <summary>
        /// The test API of the delegate Action{T1, T2}
        /// 委托 Action{T1, T2} 测试 API
        /// </summary>
        /// <param name="action">Remote expression delegate Action{T1, T2}
        /// 远程表达式委托 Action{T1, T2}</param>
        /// <param name="parameter1"></param>
        /// <param name="parameter2"></param>
        /// <returns></returns>
        int IRemoteExpressionDelegateService.Action2(AutoCSer.Net.CommandServer.RemoteExpressionAction<int, int> action, int parameter1, int parameter2)
        {
            action.Call(parameter1, parameter2);
            return ActionTarget.Default.Value;
        }
        /// <summary>
        /// The test API of the delegate Action{T1, T2, T3}
        /// 委托 Action{T1, T2, T3} 测试 API
        /// </summary>
        /// <param name="action">Remote expression delegate Action{T1, T2, T3}
        /// 远程表达式委托 Action{T1, T2, T3}</param>
        /// <param name="parameter1"></param>
        /// <param name="parameter2"></param>
        /// <param name="parameter3"></param>
        /// <returns></returns>
        int IRemoteExpressionDelegateService.Action3(AutoCSer.Net.CommandServer.RemoteExpressionAction<int, int, int> action, int parameter1, int parameter2, int parameter3)
        {
            action.Call(parameter1, parameter2, parameter3);
            return ActionTarget.Default.Value;
        }

        /// <summary>
        /// Test API for persistent Lambda expressions
        /// 可持久化的 Lambda 表达式测试 API
        /// </summary>
        /// <param name="func">Persistent remote Lambda expressions
        /// 可持久化的远程 Lambda 表达式</param>
        /// <returns></returns>
        int IRemoteExpressionDelegateService.Persistent(AutoCSer.Net.CommandServer.RemoteLambdaExpression<Func<int>> func)
        {
            return func.Compile()();
        }
    }
}
