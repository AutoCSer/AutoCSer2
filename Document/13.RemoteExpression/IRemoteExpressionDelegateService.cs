using System;

namespace AutoCSer.Document.RemoteExpression
{
    /// <summary>
    /// Definition of the test service interface for remote expression delegates
    /// 远程表达式委托测试服务接口定义
    /// </summary>
    [AutoCSer.Net.CommandServerControllerInterface]
    public partial interface IRemoteExpressionDelegateService
    {
        /// <summary>
        /// The test API of the parameterless delegate Func{T}
        /// 无参委托 Func{T} 测试 API
        /// </summary>
        /// <param name="func">Remote expression delegate Func{RT}
        /// 远程表达式委托 Func{RT}</param>
        /// <returns></returns>
        int Func(AutoCSer.Net.CommandServer.RemoteExpressionFunc<int> func);
        /// <summary>
        /// The test API of the delegate Func{T, RT}
        /// 委托 Func{T, RT} 测试 API
        /// </summary>
        /// <param name="func">Remote expression delegate Func{T, RT}
        /// 远程表达式委托 Func{T, RT}</param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        int Func1(AutoCSer.Net.CommandServer.RemoteExpressionFunc<int, int> func, int parameter);
        /// <summary>
        /// The test API of the delegate Func{T1, T2, RT}
        /// 委托 Func{T1, T2, RT} 测试 API
        /// </summary>
        /// <param name="func">Remote expression delegate Func{T1, T2, RT}
        /// 远程表达式委托 Func{T1, T2, RT}</param>
        /// <param name="parameter1"></param>
        /// <param name="parameter2"></param>
        /// <returns></returns>
        int Func2(AutoCSer.Net.CommandServer.RemoteExpressionFunc<int, int, int> func, int parameter1, int parameter2);
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
        int Func3(AutoCSer.Net.CommandServer.RemoteExpressionFunc<int, int, int, int> func, int parameter1, int parameter2, int parameter3);
        /// <summary>
        /// The test API of the parameterless delegate Action
        /// 委托 Action 测试 API
        /// </summary>
        /// <param name="action">Remote expression delegate Action
        /// 远程表达式委托 Action</param>
        /// <returns></returns>
        int Action(AutoCSer.Net.CommandServer.RemoteExpressionAction action);
        /// <summary>
        /// The test API of the delegate Action{T}
        /// 委托 Action{T} 测试 API
        /// </summary>
        /// <param name="action">Remote expression delegate Action{T}
        /// 远程表达式委托 Action{T}</param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        int Action1(AutoCSer.Net.CommandServer.RemoteExpressionAction<int> action, int parameter);
        /// <summary>
        /// The test API of the delegate Action{T1, T2}
        /// 委托 Action{T1, T2} 测试 API
        /// </summary>
        /// <param name="action">Remote expression delegate Action{T1, T2}
        /// 远程表达式委托 Action{T1, T2}</param>
        /// <param name="parameter1"></param>
        /// <param name="parameter2"></param>
        /// <returns></returns>
        int Action2(AutoCSer.Net.CommandServer.RemoteExpressionAction<int, int> action, int parameter1, int parameter2);
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
        int Action3(AutoCSer.Net.CommandServer.RemoteExpressionAction<int, int, int> action, int parameter1, int parameter2, int parameter3);
    }
}
