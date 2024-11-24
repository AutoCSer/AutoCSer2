using System;

namespace AutoCSer.Document.ServiceDataSerialize.JsonSerialize
{
    /// <summary>
    /// 入侵派生类型 示例
    /// </summary>
    [AutoCSer.JsonSerialize(IsBaseType = true)]
    class BaseType
    {
        /// <summary>
        /// 基类数据字段
        /// </summary>
        public int Value;

        /// <summary>
        /// 入侵派生类型 测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            SonType value = new SonType { Value = 1, SonValue = 2 };
            string json = AutoCSer.JsonSerializer.Serialize(value);

            var newValue = AutoCSer.JsonDeserializer.Deserialize<SonType>(json);
            if (newValue == null || newValue.Value != 1 || newValue.SonValue != 0)
            {
                return false;
            }
            return true;
        }
    }
    /// <summary>
    /// 派生类型
    /// </summary>
    class SonType : BaseType
    {
        /// <summary>
        /// 派生类型数据字段
        /// </summary>
        public int SonValue;
    }
}
