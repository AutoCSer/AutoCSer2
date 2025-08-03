using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.CommandServer.RemoteExpression
{
    /// <summary>
    /// 封装表达式
    /// </summary>
    internal abstract class CallParameterAction : CallDelegate
    {
        /// <summary>
        /// 调用表达式委托
        /// </summary>
        internal abstract void Call();
    }
    /// <summary>
    /// 封装表达式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal abstract class CallParameterAction<T> : CallParameterAction
    {
        /// <summary>
        /// 委托参数
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        protected T parameter;
        /// <summary>
        /// 调用表达式委托
        /// </summary>
        /// <param name="parameter"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Call(T parameter)
        {
            this.parameter = parameter;
            Call();
        }
    }
    /// <summary>
    /// 封装表达式
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    internal abstract class CallParameterAction<T1, T2> : CallParameterAction
    {
        /// <summary>
        /// 委托参数
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        protected T1 parameter1;
        /// <summary>
        /// 委托参数
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        protected T2 parameter2;
        /// <summary>
        /// 调用表达式委托
        /// </summary>
        /// <param name="parameter1"></param>
        /// <param name="parameter2"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Call(T1 parameter1, T2 parameter2)
        {
            this.parameter1 = parameter1;
            this.parameter2 = parameter2;
            Call();
        }
    }
    /// <summary>
    /// 封装表达式
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    internal abstract class CallParameterAction<T1, T2, T3> : CallParameterAction
    {
        /// <summary>
        /// 委托参数
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        protected T1 parameter1;
        /// <summary>
        /// 委托参数
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        protected T2 parameter2;
        /// <summary>
        /// 委托参数
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        protected T3 parameter3;
        /// <summary>
        /// 调用表达式委托
        /// </summary>
        /// <param name="parameter1"></param>
        /// <param name="parameter2"></param>
        /// <param name="parameter3"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Call(T1 parameter1, T2 parameter2, T3 parameter3)
        {
            this.parameter1 = parameter1;
            this.parameter2 = parameter2;
            this.parameter3 = parameter3;
            Call();
        }
    }
}
