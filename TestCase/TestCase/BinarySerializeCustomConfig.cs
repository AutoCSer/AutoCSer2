using System;

namespace AutoCSer.TestCase
{
    /// <summary>
    /// 二进制序列化自定义全局配置
    /// </summary>
    internal sealed class BinarySerializeCustomConfig : AutoCSer.BinarySerialize.CustomConfig
    {
        /// <summary>
        /// 不输出非泛型反射调用输出异常日志
        /// </summary>
        /// <returns></returns>
        public override bool IsReflectionLog()
        {
            return false;
        }
    }
}
