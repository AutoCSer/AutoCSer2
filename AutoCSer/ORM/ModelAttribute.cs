using AutoCSer.Metadata;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 数据表格模型配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ModelAttribute : Attribute
    {
        /// <summary>
        /// 成员选择类型，默认为公开字段 PublicInstanceField
        /// </summary>
        public MemberFiltersEnum MemberFilters = MemberFiltersEnum.PublicInstanceField;
        /// <summary>
        /// 默认表格名称，null 表示数据表格模型类型名称
        /// </summary>
#if NetStandard21
        public string? TableName;
#else
        public string TableName;
#endif
        /// <summary>
        /// 默认为 true 表示自动创建数据库表格
        /// </summary>
        public bool AutoCreateTable = true;
        /// <summary>
        /// 表格记录是否只读，否则写操作抛出异常
        /// </summary>
        public bool IsReadOnly;
        /// <summary>
        /// 获取表格名称
        /// </summary>
        /// <param name="type">持久化表格模型类型</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal string GetTableName(Type type)
        {
            return string.IsNullOrEmpty(TableName) ? type.Name : TableName;
        }
        /// <summary>
        /// 复制配置数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal ModelAttribute Clone()
        {
            return (ModelAttribute)MemberwiseClone();
        }
    }
}
