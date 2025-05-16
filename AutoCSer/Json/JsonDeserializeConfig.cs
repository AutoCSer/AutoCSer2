using System;

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化配置参数
    /// </summary>
    public class JsonDeserializeConfig : TextDeserializeConfig
    {
        /// <summary>
        /// 对象解析结束后是否检测最后的空格符，默认为 true
        /// </summary>
        public bool IsEndSpace = true;
    }
}
