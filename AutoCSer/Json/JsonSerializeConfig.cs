using AutoCSer.Metadata;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// JSON serialization configuration parameters
    /// JSON 序列化配置参数
    /// </summary>
    public class JsonSerializeConfig : AutoCSer.TextSerialize.SerializeConfig
    {
        /// <summary>
        /// Customize the ToString("xxx") format
        /// 自定义 ToString("xxx") 格式
        /// </summary>
#if NetStandard21
        public string? DateTimeCustomFormat;
#else
        public string DateTimeCustomFormat;
#endif
        /// <summary>
        /// Time output type
        /// 时间输出类型
        /// </summary>
        public Json.DateTimeTypeEnum DateTimeType;
        /// <summary>
        /// The default is true, indicating that the minimum time output is null
        /// 默认为 true 表示最小时间输出为 null
        /// </summary>
        public bool IsDateTimeMinNull = true;
        ///// <summary>
        ///// 字符 0
        ///// </summary>
        //public char NullChar = (char)0;
        /// <summary>
        /// By default, true indicates that IDictionary{string,T} is converted to object output; otherwise, the output is an array of {"Key":x,"Value":y}
        /// 默认为 true 表示 IDictionary{string,T} 转换成对象输出，否则输出为 {"Key":x,"Value":y} 数组
        /// </summary>
        public bool IsStringDictionaryToObject = true;
        /// <summary>
        /// By default, false indicates that IDictionary{KT,VT} outputs an array of {"Key":x,"Value":y}, and setting it to true replaces it with object output
        /// 默认为 false 表示 IDictionary{KT,VT} 输出 {"Key":x,"Value":y} 数组，设置为 true 则换成对象输出
        /// </summary>
        public bool IsDictionaryToObject;
        /// <summary>
        /// Whether integers are allowed to be converted to hexadecimal strings (for WEB API output)
        /// 整数是否允许转换为 16 进制字符串（用于 WEB API 输出）
        /// </summary>
        public bool IsIntegerToHex;
        /// <summary>
        /// Whether long/ulong that exceeds the maximum effective precision is converted to a string (for WEB API output)
        /// 超出最大有效精度的 long / ulong 是否转换成字符串（用于 WEB API 输出）
        /// </summary>
        public bool IsMaxNumberToString;
        /// <summary>
        /// Whether the logical value is converted to 1/0 for output (for WEB API output)
        /// 逻辑值是否转换成 1/0 输出（用于 WEB API 输出）
        /// </summary>
        public bool IsBoolToInt;

        ///// <summary>
        ///// 创建内部配置参数
        ///// </summary>
        ///// <returns></returns>
        //internal static JsonSerializeConfig CreateInternal()
        //{
        //    return new JsonSerializeConfig { DateTimeType = Json.DateTimeTypeEnum.JavaScript, IsIntegerToHex = true, IsBoolToInt = true };
        //}
    }
}
