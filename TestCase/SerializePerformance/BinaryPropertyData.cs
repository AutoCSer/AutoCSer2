using System;

namespace AutoCSer.TestCase.SerializePerformance
{
    /// <summary>
    /// 二进制序列化属性测试数据（二进制序列化默认仅操作普通字段，如果需要操作属性需要做如下配置增加匿名字段）
    /// </summary>
    [AutoCSer.BinarySerialize(IsAnonymousFields = true, IsBaseType = false, IsMemberMap = false)]
    class BinaryPropertyData : FloatPropertyData { }
}
