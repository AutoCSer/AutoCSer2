﻿using System;

namespace AutoCSer.Document.ServiceDataSerialize.JsonSerialize
{
    /// <summary>
    /// 忽略成员 示例
    /// </summary>
    class IgnoreMember
    {
        /// <summary>
        /// 公共字段成员
        /// </summary>
        public int Value;
        /// <summary>
        /// 忽略当前成员
        /// </summary>
        [AutoCSer.JsonSerializeMember(IsIgnoreCurrent = true)]
        public int Ignore;

        /// <summary>
        /// 忽略成员 测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            var value = new IgnoreMember { Value = 1, Ignore = 2 };
            string json = AutoCSer.JsonSerializer.Serialize(value);
            var newValue = AutoCSer.JsonDeserializer.Deserialize<NoIgnoreMember>(json);
            if (newValue == null || newValue.Value != 1 || newValue.Ignore != 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            newValue = new NoIgnoreMember { Value = 1, Ignore = 2 };
            json = AutoCSer.JsonSerializer.Serialize(newValue);
            value = AutoCSer.JsonDeserializer.Deserialize<IgnoreMember>(json);
            if (value == null || value.Value != 1 || value.Ignore != 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
    /// <summary>
    /// 非忽略成员数据定义，用于验证忽略成员测试
    /// </summary>
    class NoIgnoreMember
    {
        /// <summary>
        /// 公共字段成员
        /// </summary>
        public int Value;
        /// <summary>
        /// 公共字段成员
        /// </summary>
        public int Ignore;
    }
}