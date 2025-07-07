using System;

namespace AutoCSer.Document.ServiceDataSerialize.JsonSerialize
{
    /// <summary>
    /// Only select the examples of public field members
    /// 仅选择公共字段成员 示例
    /// </summary>
    [AutoCSer.JsonSerialize(Filter = AutoCSer.Metadata.MemberFiltersEnum.PublicInstanceField)]
    class PublicInstanceField
    {
        /// <summary>
        /// Public field member
        /// </summary>
        public int Public;
        /// <summary>
        /// Public property member (ignored)
        /// 公共属性成员（被忽略）
        /// </summary>
        public int Property { get; set; }
        /// <summary>
        /// Non-public field member (ignored)
        /// 非公共字段成员（被忽略）
        /// </summary>
        private int Private;
        /// <summary>
        /// Non-public field member (ignored)
        /// 非公共字段成员（被忽略）
        /// </summary>
        protected int Protected;
        /// <summary>
        /// Non-public field member (ignored)
        /// 非公共字段成员（被忽略）
        /// </summary>
        internal int Internal;

        /// <summary>
        /// Only select the public field members for testing
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            PublicInstanceField value = new PublicInstanceField { Public = 1, Private = 2, Protected = 3, Internal = 4, Property = 5 };

            string json = AutoCSer.JsonSerializer.Serialize(value);
            var newValue = AutoCSer.JsonDeserializer.Deserialize<PublicInstanceField>(json);

            if (newValue == null || newValue.Public != 1 || newValue.Private != 0 || newValue.Protected != 0 || newValue.Internal != 0 || newValue.Property != 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
