using System;

namespace AutoCSer.Document.ServiceDataSerialize.JsonSerialize
{
    /// <summary>
    /// 成员位图选择 示例
    /// </summary>
    class MemberMap
    {
        /// <summary>
        /// 字段成员
        /// </summary>
        public int Value1;
        /// <summary>
        /// 属性成员
        /// </summary>
        public int Value2 { get; set; }
        /// <summary>
        /// 字段成员
        /// </summary>
        public int Value3;

        /// <summary>
        /// 成员位图选择 测试
        /// </summary>
        /// <returns></returns>
        [AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            MemberMap value = new MemberMap { Value1 = 1, Value2 = 2, Value3 = 3 };

            //成员位图初始化代价比较大，应该根据需求重用该对象
            AutoCSer.Metadata.MemberMap<MemberMap> serializeMemberMap = AutoCSer.Metadata.MemberMap<MemberMap>.NewEmpty();
            serializeMemberMap.SetMember(member => member.Value1);//添加成员 Value1
            serializeMemberMap.SetMember(member => member.Value2);//添加成员 Value2
            AutoCSer.JsonSerializeConfig serializeMemberMapConfig = new AutoCSer.JsonSerializeConfig { MemberMap = serializeMemberMap };

            string json = AutoCSer.JsonSerializer.Serialize(value, serializeMemberMapConfig);
            var newValue = AutoCSer.JsonDeserializer.Deserialize<MemberMap>(json);

            if (newValue == null || newValue.Value1 != 1 || newValue.Value2 != 2 || newValue.Value3 != 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }


            AutoCSer.Metadata.MemberMapValue<MemberMap> memberMapValue = new Metadata.MemberMapValue<MemberMap> { Value = value, MemberMap = serializeMemberMap };
            json = AutoCSer.JsonSerializer.Serialize(memberMapValue);
            var newMemberMapValue = AutoCSer.JsonDeserializer.Deserialize<AutoCSer.Metadata.MemberMapValue<MemberMap>>(json);
            if (newMemberMapValue.Value == null || newMemberMapValue.Value.Value1 != 1 || newMemberMapValue.Value.Value2 != 2 || newMemberMapValue.Value.Value3 != 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            return true;
        }
    }
}
