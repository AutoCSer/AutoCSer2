using AutoCSer.TestCase.CommonModel.TableModel.CustomColumn;
using System;

namespace AutoCSer.TestCase.CommonModel.TableModel
{
    /// <summary>
    /// 自定义字段列测试模型（建议使用字段来描述持久化数据，同时也作为内部 RPC 服务接口传参数据配置二进制序列化规则）
    /// </summary>
    [AutoCSer.BinarySerialize]
    public class CustomColumnFieldModel
    {
        /// <summary>
        /// 自定义组合字段列关键字
        /// </summary>
        [AutoCSer.ORM.Member(PrimaryKeyType = AutoCSer.ORM.PrimaryKeyTypeEnum.PrimaryKey)]
        public CustomColumnFieldPrimaryKey Key;

        /// <summary>
        /// 自定义字段列
        /// </summary>
        public CustomColumnField CustomColumnField;
    }
}
