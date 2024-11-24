using System;

namespace AutoCSer.Document.ServiceDataSerialize.BinarySerialize
{
    /// <summary>
    /// 禁用成员位图 示例
    /// </summary>
    [AutoCSer.BinarySerialize(IsMemberMap = false)]
    class DisabledMemberMap
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
        /// 禁用成员位图 测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            DisabledMemberMap value = new DisabledMemberMap { Value1 = 1, Value2 = 2, Value3 = 3 };

            AutoCSer.Metadata.MemberMap<DisabledMemberMap> serializeMemberMap = AutoCSer.Metadata.MemberMap<DisabledMemberMap>.NewEmpty();
            serializeMemberMap.SetMember(member => member.Value1);//添加成员 Value1
            serializeMemberMap.SetMember(member => member.Value2);//添加成员 Value2
            AutoCSer.BinarySerializeConfig serializeMemberMapConfig = new AutoCSer.BinarySerializeConfig { MemberMap = serializeMemberMap };

            byte[] data = AutoCSer.BinarySerializer.Serialize(value, serializeMemberMapConfig);
            var newValue = AutoCSer.BinaryDeserializer.Deserialize<DisabledMemberMap>(data);

            if (newValue == null || newValue.Value1 != 1 || newValue.Value2 != 2 || newValue.Value3 != 3)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
