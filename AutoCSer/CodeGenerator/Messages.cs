using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 错误信息
    /// </summary>
    internal static class Messages
    {
        /// <summary>
        /// 提示信息集合
        /// </summary>
        private static readonly HashSet<string> messages = HashSetCreator<string>.Create();
        /// <summary>
        /// 是否存在提示信息
        /// </summary>
        private static bool isMessage;
        /// <summary>
        /// 是否存在提示信息
        /// </summary>
        internal static bool IsMessage
        {
            get { return isMessage || messages.Count != 0; }
        }
        /// <summary>
        /// 添加提示信息
        /// </summary>
        /// <param name="value">提示信息</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Message(string value)
        {
            messages.Add(value);
        }
        /// <summary>
        /// 错误信息集合
        /// </summary>
        private static readonly HashSet<string> errors = HashSetCreator<string>.Create();
        /// <summary>
        /// 是否存在错误或者异常信息
        /// </summary>
        private static bool isError;
        /// <summary>
        /// 是否存在错误或者异常信息
        /// </summary>
        internal static bool IsError
        {
            get { return isError || errors.Count != 0 || exceptions.Length != 0; }
        }
        /// <summary>
        /// 添加错误信息
        /// </summary>
        /// <param name="error">错误信息</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Error(string error)
        {
            errors.Add(error);
        }
        /// <summary>
        /// 异常集合
        /// </summary>
        private static LeftArray<Exception> exceptions = new LeftArray<Exception>(0);
        /// <summary>
        /// 添加异常
        /// </summary>
        /// <param name="exception">异常</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Exception(Exception exception)
        {
            exceptions.Add(exception);
        }
        /// <summary>
        /// 清除所有信息
        /// </summary>
        internal static void Clear()
        {
            errors.Clear();
            exceptions.SetEmpty();
            messages.Clear();
            isError = isMessage = false;
        }
        /// <summary>
        /// 信息输出到日志
        /// </summary>
        /// <returns>是否存在错误信息</returns>
        private static async Task<bool> output()
        {
            if (messages.Count != 0)
            {
                await AutoCSer.LogHelper.Error(string.Join(@"
- - - - - - - -
", messages), LogLevelEnum.All);
                await AutoCSer.LogHelper.Flush();
                messages.Clear();
                isMessage = true;
            }
            if (errors.Count != 0)
            {
               await AutoCSer.LogHelper.Error(string.Join(@"
- - - - - - - -
", errors), LogLevelEnum.All);
                await AutoCSer.LogHelper.Flush();
                errors.Clear();
                isError = true;
            }
            if (exceptions.Length != 0)
            {
                foreach (Exception error in exceptions) await AutoCSer.LogHelper.Exception(error, null, LogLevelEnum.All);
                await AutoCSer.LogHelper.Flush();
                exceptions.Length = 0;
                isError = true;
            }
            return isError;
        }
        /// <summary>
        /// 输出错误信息到日志并打开日志文件
        /// </summary>
        /// <returns>安装是否顺利,没有错误或者提示信息</returns>
        public static async Task<bool> Open()
        {
            if (!await output() && !IsMessage) return true;
            await AutoCSer.LogHelper.Flush();
            string fileName = await (AutoCSer.LogHelper.Default as AutoCSer.Log.File).MoveBak();
            if (fileName != null)
            {
                string notepad = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.System), "notepad.exe");
                if (await AutoCSer.Common.FileExists(notepad)) Process.Start(notepad, fileName);
                else Process.Start(fileName);
            }
            Clear();
            return false;
        }
    }
}
