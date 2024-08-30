using AutoCSer.TestCase.Common.Data;
using System;

#pragma warning disable
namespace AutoCSer.TestCase.CommonModel.TableModel.CustomColumn
{
    /// <summary>
    /// 自定义组合字段列关键字，需要实现接口 IEquatable{T}，如果有缓存关键字需求需要同时重写 Equals 与 GetHashCode（建议使用字段来描述持久化数据，同时也作为内部 RPC 服务接口传参）
    /// </summary>
    [AutoCSer.ORM.CustomColumn(NameConcatType = AutoCSer.ORM.CustomColumnNameConcatTypeEnum.Node)]
    public struct CustomColumnFieldPrimaryKey : IEquatable<CustomColumnFieldPrimaryKey>
    {
        public int IntKey;
        public ByteEnum ByteEnumKey;
        public bool BoolKey;

        public static bool operator ==(CustomColumnFieldPrimaryKey left, CustomColumnFieldPrimaryKey right) { return left.Equals(right); }
        public static bool operator !=(CustomColumnFieldPrimaryKey left, CustomColumnFieldPrimaryKey right) { return !left.Equals(right); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(CustomColumnFieldPrimaryKey other)
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
            return Equals((CustomColumnFieldPrimaryKey)obj);
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
