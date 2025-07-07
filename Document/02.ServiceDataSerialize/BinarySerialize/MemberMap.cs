using System;

namespace AutoCSer.Document.ServiceDataSerialize.BinarySerialize
{
    /// <summary>
    /// Example of member bitmap selection
    /// 成员位图选择 示例
    /// </summary>
    [AutoCSer.BinarySerialize(IsMemberMap = true)]
    class MemberMap
    {
        /// <summary>
        /// Field member
        /// </summary>
        public int Value1;
        /// <summary>
        /// Automatically implementing property
        /// 自动属性
        /// </summary>
        public int Value2 { get; set; }
        /// <summary>
        /// Field member
        /// </summary>
        public int Value3;

        /// <summary>
        /// Member bitmap selection test
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            MemberMap value = new MemberMap { Value1 = 1, Value2 = 2, Value3 = 3 };

            //The initialization cost of the member bitmap is relatively high, and this object should be reused according to the requirements.
            //成员位图初始化代价比较大，应该根据需求重用该对象
            AutoCSer.Metadata.MemberMap<MemberMap> serializeMemberMap = AutoCSer.Metadata.MemberMap<MemberMap>.NewEmpty();
            serializeMemberMap.SetMember(member => member.Value1);//Add the member Value1
            serializeMemberMap.SetMember(member => member.Value2);//Add the member Value2

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
