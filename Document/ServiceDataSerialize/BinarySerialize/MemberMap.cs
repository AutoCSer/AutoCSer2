using System;

namespace AutoCSer.Document.ServiceDataSerialize.BinarySerialize
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
        /// 字段成员
        /// </summary>
        public int Value2;
        /// <summary>
        /// 字段成员
        /// </summary>
        public int Value3;

        /// <summary>
        /// 成员位图选择 测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            MemberMap value = new MemberMap { Value1 = 1, Value2 = 2, Value3 = 3 };

            AutoCSer.Metadata.MemberMap<MemberMap> serializeMemberMap = AutoCSer.Metadata.MemberMap<MemberMap>.NewEmpty();
            serializeMemberMap.SetMember(member => member.Value1);//添加成员 Value1
            serializeMemberMap.SetMember(member => member.Value2);//添加成员 Value2

            AutoCSer.BinarySerializeConfig serializeMemberMapConfig = new AutoCSer.BinarySerializeConfig { MemberMap = serializeMemberMap };

            byte[] data = AutoCSer.BinarySerializer.Serialize(value, serializeMemberMapConfig);
            var newValue = AutoCSer.BinaryDeserializer.Deserialize<MemberMap>(data);

            if (newValue == null || newValue.Value1 != 1 || newValue.Value2 != 2 || newValue.Value3 != 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            AutoCSer.Metadata.MemberMapValue<MemberMap> memberMapValue = new Metadata.MemberMapValue<MemberMap> { Value = value, MemberMap = serializeMemberMap };
            data = AutoCSer.BinarySerializer.Serialize(memberMapValue);
            var newMemberMapValue = AutoCSer.BinaryDeserializer.Deserialize<AutoCSer.Metadata.MemberMapValue<MemberMap>>(data);
            if (newMemberMapValue.Value == null || newMemberMapValue.Value.Value1 != 1 || newMemberMapValue.Value.Value2 != 2 || newMemberMapValue.Value.Value3 != 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            return true;
        }
    }
}
