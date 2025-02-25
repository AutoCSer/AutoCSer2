﻿using System;

namespace AutoCSer.Document.ServiceDataSerialize.JsonSerialize
{
    /// <summary>
    /// 匿名类型序列化 示例
    /// </summary>
    class AnonymousType
    {
        /// <summary>
        /// 匿名类型序列化 测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            string json = AutoCSer.JsonSerializer.Serialize(new { Value = 1 });
            var newValue = new { Value = 0 };
            AutoCSer.JsonDeserializer.Deserialize(json, ref newValue);
            if (newValue == null || newValue.Value != 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
