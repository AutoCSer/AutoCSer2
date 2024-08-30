using AutoCSer.Metadata;
using System;
using System.Reflection;

namespace AutoCSer.CodeGenerator.Metadata
{
    /// <summary>
    /// 成员信息
    /// </summary>
    internal abstract class MemberIndex : MemberIndexInfo
    {
        /// <summary>
        /// XML文档注释
        /// </summary>
        protected string xmlDocument;
        /// <summary>
        /// 成员信息
        /// </summary>
        /// <param name="method">成员方法信息</param>
        /// <param name="filter">选择类型</param>
        /// <param name="index">成员编号</param>
        protected MemberIndex(MethodInfo method, MemberFiltersEnum filter, int index)
            : base(method, method.ReturnType, filter, index)
        {
        }
    }
}
