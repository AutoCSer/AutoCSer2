using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// TimeSpan expansion operation
    /// 时间扩展操作
    /// </summary>
    public partial struct TimeSpanExtensions
    {
        /// <summary>
        /// Time is converted to a string (HH:mm:ss.fffffff)
        /// 时间转换成字符串（HH:mm:ss.fffffff）
        /// </summary>
        /// <returns></returns>
        public new string ToString()
        {
            return value.toString();
        }
    }
}
