using System;

namespace AutoCSer.TestCase.SearchQueryService
{
    /// <summary>
    /// 用户搜索查询参数
    /// </summary>
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    public sealed class UserQueryParameter : SearchUserQueryParameter
    {
        /// <summary>
        /// 搜索用户名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 搜索用户备注
        /// </summary>
        public string Remark;
    }
}
