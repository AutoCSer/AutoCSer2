using AutoCSer.Metadata;
using System;
using System.Reflection;

namespace AutoCSer.CodeGenerator.Metadata
{
    /// <summary>
    /// 成员字段
    /// </summary>
    internal sealed class FieldIndex : MemberIndex
    {
        /// <summary>
        /// 成员字段信息
        /// </summary>
        public FieldInfo Field { get; private set; }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName
        {
            get { return Field.Name; }
        }
        /// <summary>
        /// 成员字段
        /// </summary>
        /// <param name="field">成员字段信息</param>
        internal FieldIndex(AutoCSer.Metadata.FieldIndex field) : base(field.Member, field.MemberFilters, field.MemberIndex)
        {
            Field = field.Member;
        }
        /// <summary>
        /// 获取数据值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object GetValue(object value)
        {
            return Field.GetValue(value);
        }
    }
}
