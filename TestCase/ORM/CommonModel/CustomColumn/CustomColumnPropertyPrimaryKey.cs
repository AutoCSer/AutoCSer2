using AutoCSer.TestCase.Common.Data;
using System;

#pragma warning disable
namespace AutoCSer.TestCase.CommonModel.TableModel.CustomColumn
{
    /// <summary>
    /// 自定义组合属性列关键字，需要实现接口 IEquatable{T}，如果有缓存关键字需求需要同时重写 Equals 与 GetHashCode（二进制序列化不支持属性，需要配置序列化匿名字段以支持 RPC 传参）
    /// </summary>
    [AutoCSer.ORM.CustomColumn(MemberFilters = Metadata.MemberFiltersEnum.Instance, NameConcatType = AutoCSer.ORM.CustomColumnNameConcatTypeEnum.Node)]
    public struct CustomColumnPropertyPrimaryKey : IEquatable<CustomColumnPropertyPrimaryKey>
    {
        public int IntKey { get; set; }
        public ByteEnum ByteEnumKey { get; set; }
        public bool BoolKey { get; set; }

        public static bool operator ==(CustomColumnPropertyPrimaryKey left, CustomColumnPropertyPrimaryKey right) { return left.Equals(right); }
        public static bool operator !=(CustomColumnPropertyPrimaryKey left, CustomColumnPropertyPrimaryKey right) { return !left.Equals(right); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(CustomColumnPropertyPrimaryKey other)
        {
            return IntKey == other.IntKey && ByteEnumKey == other.ByteEnumKey && BoolKey == other.BoolKey;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals((CustomColumnPropertyPrimaryKey)obj);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return IntKey ^ ((byte)ByteEnumKey << 19) ^ (BoolKey ? (1 << 29) : 0);
        }
    }
}
