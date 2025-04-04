using AutoCSer.TestCase.CommonModel.TableModel.CustomColumn;
using System;

namespace AutoCSer.TestCase.CommonModel.TableModel
{
    /// <summary>
    /// 自定义属性列测试模型（二进制序列化不支持属性，需要配置序列化匿名字段以支持 RPC 传参）
    /// </summary>
    [AutoCSer.ORM.Model(MemberFilters = Metadata.MemberFiltersEnum.PublicInstance)]
    public class CustomColumnPropertyModel
    {
        /// <summary>
        /// 自定义组合属性列关键字
        /// </summary>
        [AutoCSer.ORM.Member(PrimaryKeyType = AutoCSer.ORM.PrimaryKeyTypeEnum.PrimaryKey)]
        public CustomColumnPropertyPrimaryKey Key { get; set; }

        /// <summary>
        /// 自定义属性列
        /// </summary>
        public CustomColumnProperty CustomColumnProperty { get; set; }
    }
}
