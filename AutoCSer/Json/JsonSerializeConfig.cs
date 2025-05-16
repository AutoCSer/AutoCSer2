using AutoCSer.Metadata;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化配置参数
    /// </summary>
    public class JsonSerializeConfig : AutoCSer.TextSerialize.SerializeConfig
    {
        /// <summary>
        /// 自定义 ToString("xxx") 格式
        /// </summary>
#if NetStandard21
        public string? DateTimeCustomFormat;
#else
        public string DateTimeCustomFormat;
#endif
        /// <summary>
        /// 时间输出类型
        /// </summary>
        public Json.DateTimeTypeEnum DateTimeType;
        /// <summary>
        /// 最小时间是否输出为 null，默认为 true
        /// </summary>
        public bool IsDateTimeMinNull = true;
        ///// <summary>
        ///// 字符 0
        ///// </summary>
        //public char NullChar = (char)0;
        /// <summary>
        /// Dictionary[string,] 是否转换成对象输出，默认为 true
        /// </summary>
        public bool IsStringDictionaryToObject = true;
        /// <summary>
        /// Dictionary 是否转换成对象模式输出
        /// </summary>
        public bool IsDictionaryToObject;
        /// <summary>
        /// 整数是否允许转换为 16 进制字符串
        /// </summary>
        public bool IsIntegerToHex;
        /// <summary>
        /// 超出最大有效精度的 long / ulong 是否转换成字符串
        /// </summary>
        public bool IsMaxNumberToString;
        /// <summary>
        /// 逻辑值是否转换成 1/0 输出
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
