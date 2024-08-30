using AutoCSer.TestCase.CommonModel.TableModel.CustomColumn;
using System;

namespace AutoCSer.TestCase.CommonModel.TableModel
{
    /// <summary>
    /// 自增ID与其它混合测试模型（建议使用字段来描述持久化数据，同时也作为内部 RPC 服务接口传参数据配置二进制序列化规则）
    /// </summary>
    [AutoCSer.BinarySerialize]
    public class AutoIdentityModel
    {
        /// <summary>
        /// 64位自增ID 关键字
        /// </summary>
        [AutoCSer.ORM.Member(PrimaryKeyType = AutoCSer.ORM.PrimaryKeyTypeEnum.AutoIdentity)]
        [AutoCSer.RandomObject.Ignore]
        public long Identity;

        /// <summary>
        /// 时间范围
        /// </summary>
        public DateTimeRange RangeTime;

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public Email Email;

        /// <summary>
        /// 不可识别类型持久化为 JSON 字符串
        /// </summary>
        [AutoCSer.ORM.String(IsAscii = true, Size = 1024)]
        [AutoCSer.RandomObject.Member(IsNullable = false)]
        public Email[] EmailArray;
    }
}
