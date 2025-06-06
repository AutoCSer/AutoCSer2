﻿using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 枚举命令
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class EnumeratorCommand<T> : EnumeratorCommand
        where T : struct
    {
        /// <summary>
        /// 输入参数
        /// </summary>
        private T inputParameter;
        /// <summary>
        /// 枚举命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="inputParameter"></param>
        internal EnumeratorCommand(CommandClientController controller, int methodIndex, ref T inputParameter) : base(controller, methodIndex)
        {
            this.inputParameter = inputParameter;
            Push();
        }
        /// <summary>
        /// 创建命令输入数据
        /// </summary>
        /// <param name="buildInfo">TCP 客户端创建命令参数</param>
        /// <returns>是否成功</returns>
#if NetStandard21
        internal override Command? Build(ref ClientBuildInfo buildInfo)
#else
        internal override Command Build(ref ClientBuildInfo buildInfo)
#endif
        {
            return BuildKeep(ref buildInfo, ref inputParameter);
        }
    }
#if AOT
    /// <summary>
    /// 枚举命令
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="RT"></typeparam>
    /// <typeparam name="OT"></typeparam>
    internal sealed class EnumeratorCommand<T, RT, OT> : AutoCSer.Net.EnumeratorCommand<RT, OT>
        where OT : struct
#else
    /// <summary>
    /// 枚举命令
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="RT"></typeparam>
    internal sealed class EnumeratorCommand<T, RT> : AutoCSer.Net.EnumeratorCommand<RT>
#endif
        where T : struct
    {
        /// <summary>
        /// 输入参数
        /// </summary>
        private T inputParameter;
#if AOT
        /// <summary>
        /// 枚举命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="inputParameter"></param>
        internal EnumeratorCommand(CommandClientController controller, int methodIndex, Func<OT, RT> getReturnValue, ref T inputParameter) : base(controller, methodIndex, getReturnValue)
        {
            this.inputParameter = inputParameter;
            Push();
        }
        /// <summary>
        /// 枚举命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="inputParameter"></param>
        /// <param name="outputParameter"></param>
        internal EnumeratorCommand(CommandClientController controller, int methodIndex, Func<OT, RT> getReturnValue, ref T inputParameter, OT outputParameter) : base(controller, methodIndex, getReturnValue, outputParameter)
        {
            this.inputParameter = inputParameter;
            Push();
        }
#else
        /// <summary>
        /// 枚举命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="inputParameter"></param>
        internal EnumeratorCommand(CommandClientController controller, int methodIndex, ref T inputParameter) : base(controller, methodIndex)
        {
            this.inputParameter = inputParameter;
            Push();
        }
        /// <summary>
        /// 枚举命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="inputParameter"></param>
        /// <param name="returnValue"></param>
        internal EnumeratorCommand(CommandClientController controller, int methodIndex, ref T inputParameter, ref RT returnValue) : base(controller, methodIndex, ref returnValue)
        {
            this.inputParameter = inputParameter;
            Push();
        }
#endif
        /// <summary>
        /// 创建命令输入数据
        /// </summary>
        /// <param name="buildInfo">TCP 客户端创建命令参数</param>
        /// <returns>是否成功</returns>
#if NetStandard21
        internal override Command? Build(ref ClientBuildInfo buildInfo)
#else
        internal override Command Build(ref ClientBuildInfo buildInfo)
#endif
        {
            return BuildKeep(ref buildInfo, ref inputParameter);
        }
    }
}
