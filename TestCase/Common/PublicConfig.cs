using System;

namespace AutoCSer.TestCase.Common
{
    /// <summary>
    /// AutoCSer 公共配置
    /// </summary>
    public sealed class PublicConfig
    {
        ///// <summary>
        ///// 二进制数据序列化类型全局配置（如果项目组习惯使用属性定义数据成员，为了支持 RPC 传参可以全局配置序列化匿名字段）
        ///// </summary>
        //[AutoCSer.Configuration.Member]
        //internal static AutoCSer.BinarySerializeAttribute BinarySerializeAttribute
        //{
        //    get { return new AutoCSer.BinarySerializeAttribute { IsBaseType = false, IsAnonymousFields = true }; }
        //}
    }
}
