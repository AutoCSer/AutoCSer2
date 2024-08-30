using System;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 数据列自定义配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class MemberAttribute : AutoCSer.Metadata.IgnoreMemberAttribute
    {
        /// <summary>
        /// 关键字类型
        /// </summary>
        public PrimaryKeyTypeEnum PrimaryKeyType;
        /// <summary>
        /// 更新成员位图传参为 null 时，默认为 true 表示更新该成员，否则不更新该成员（关键字不支持更新）
        /// </summary>
        public bool DefaultUpdate = true;
        /// <summary>
        /// 成员排序，值越小越靠前
        /// </summary>
        public int MemberSort = int.MaxValue;
        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue;
        /// <summary>
        /// 备注说明
        /// </summary>
        public string Remark;

        /// <summary>
        /// 自动补全创建数据列时自动更新列值的表达式
        /// </summary>
        public string CreateColumnUpdateValue;
    }
}
