using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// DateTime expansion operation
    /// 时间扩展操作
    /// </summary>
    public partial struct DateTimeExtensions
    {
        /// <summary>
        /// Time is converted to string (yyyy-MM-ddTHH:mm:ss.fffffff)
        /// 时间转换成字符串（yyyy-MM-ddTHH:mm:ss.fffffff）
        /// </summary>
        /// <param name="dateSplit">The separator between years, months and days
        /// 年月日之间的分隔符</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public string ToString(char dateSplit = AutoCSer.Date.DefaultDateSplit)
        {
            return value.toString(dateSplit);
        }
        /// <summary>
        /// The integer value of time conversion: Year[23b] + Month[4b] + Day[5b]
        /// 时间转整数值 Year[23b] + Month[4b] + Day[5b]
        /// </summary>
        /// <returns>Year[23b] + Month[4b] + Day[5b]</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public uint ToIntDate()
        {
            return value.toIntDate();
        }
    }
}
