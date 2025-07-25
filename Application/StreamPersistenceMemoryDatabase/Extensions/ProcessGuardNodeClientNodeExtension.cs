﻿using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 进程守护节点扩展
    /// </summary>
    public static class ProcessGuardNodeClientNodeExtension
    {
        /// <summary>
        /// 删除当前进程守护
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ResponseResultAwaiter RemoveCurrentProcess(this IProcessGuardNodeClientNode node)
        {
            Process process = AutoCSer.Common.CurrentProcess;
            return node.Remove(process.Id, process.StartTime, process.ProcessName);
        }
        /// <summary>
        /// 添加当前进程守护
        /// </summary>
        /// <param name="node"></param>
        /// <param name="arguments">Main 函数传参</param>
        /// <returns>Add failed and return false
        /// 添加失败返回 false</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static ResponseParameterAwaiter<bool> GuardCurrentProcess(this IProcessGuardNodeClientNode node, string[]? arguments = null)
#else
        public static ResponseParameterAwaiter<bool> GuardCurrentProcess(this IProcessGuardNodeClientNode node, string[] arguments = null)
#endif
        {
            return node.Guard(ProcessGuardInfo.GetCurrent(arguments));
        }
    }
}
