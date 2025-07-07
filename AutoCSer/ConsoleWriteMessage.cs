using System;

namespace AutoCSer
{
    /// <summary>
    /// Console output message
    /// 控制台输出信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct ConsoleWriteMessage
    {
        /// <summary>
        /// Output message
        /// 输出信息
        /// </summary>
        private readonly string message;
        /// <summary>
        /// Text color
        /// 文字颜色
        /// </summary>
        private readonly byte foregroundColor;
        /// <summary>
        /// Background color
        /// 背景颜色
        /// </summary>
        private readonly byte backgroundColor;
        /// <summary>
        /// Whether to restore the text and background color after outputting the message
        /// 输出信息以后是否恢复文字与背景颜色
        /// </summary>
        private readonly bool restoreColor;
        /// <summary>
        /// Whether to add a line break after outputting the message
        /// 输出信息之后是否添加换行
        /// </summary>
        private readonly bool isWriteLine;
        /// <summary>
        /// Console output message
        /// 控制台输出信息
        /// </summary>
        /// <param name="message">Output message
        /// 输出信息</param>
        /// <param name="foregroundColor">Text color
        /// 文字颜色</param>
        /// <param name="backgroundColor">Background color
        /// 背景颜色</param>
        /// <param name="restoreColor">Whether to restore the text and background color after outputting the message
        /// 输出信息以后是否恢复文字与背景颜色</param>
        /// <param name="isWriteLine">Whether to add a line break after outputting the message
        /// 输出信息之后是否添加换行</param>
        internal ConsoleWriteMessage(string message, ConsoleColor foregroundColor, ConsoleColor backgroundColor, bool restoreColor, bool isWriteLine)
        {
            this.message = message;
            this.foregroundColor = (byte)(int)foregroundColor;
            this.backgroundColor = (byte)(int)backgroundColor;
            this.restoreColor = restoreColor;
            this.isWriteLine = isWriteLine;
        }
        /// <summary>
        /// Console output
        /// 控制台输出
        /// </summary>
        internal void Write()
        {
            ConsoleColor foregroundColor = Console.ForegroundColor, backgroundColor = Console.BackgroundColor;
            if (foregroundColor != (ConsoleColor)(int)this.foregroundColor) Console.ForegroundColor = (ConsoleColor)(int)this.foregroundColor;
            if (backgroundColor != (ConsoleColor)(int)this.backgroundColor) Console.BackgroundColor = (ConsoleColor)(int)this.backgroundColor;
            if (isWriteLine)
            {
                if (string.IsNullOrEmpty(message)) Console.WriteLine();
                else Console.WriteLine(message);
            }
            else Console.Write(message);
            if (restoreColor)
            {
                if (foregroundColor != (ConsoleColor)(int)this.foregroundColor) Console.ForegroundColor = foregroundColor;
                if (backgroundColor != (ConsoleColor)(int)this.backgroundColor) Console.BackgroundColor = backgroundColor;
            }
        }
    }
}
