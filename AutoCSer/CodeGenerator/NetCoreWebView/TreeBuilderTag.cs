using System;

namespace AutoCSer.CodeGenerator.NetCoreWebView
{
    /// <summary>
    /// HTML 模板树节点标识
    /// </summary>
    internal sealed class TreeBuilderTag : IEquatable<TreeBuilderTag>
    {
        /// <summary>
        /// 模板标签内容
        /// </summary>
        internal SubString Content;
        /// <summary>
        /// HTML 模板命令类型
        /// </summary>
        internal readonly TreeTemplateCommandEnum Command;
        /// <summary>
        /// 树节点标识类型
        /// </summary>
        internal readonly TreeBuilderTagTypeEnum Type;
        /// <summary>
        /// 是否已经回合
        /// </summary>
        internal bool IsRound;
        /// <summary>
        /// 表达式
        /// </summary>
        internal AutoCSer.Expression.ValueNode Expression;
        /// <summary>
        /// 空 HTML 模板树节点标识，用于检查回合
        /// </summary>
        internal TreeBuilderTag() { }
        /// <summary>
        /// HTML 模板树节点标识
        /// </summary>
        /// <param name="content"></param>
        /// <param name="command">标识 command</param>
        internal TreeBuilderTag(ref SubString content, TreeTemplateCommandEnum command)
        {
            Content = content;
            Command = command;
            Type = TreeBuilderTagTypeEnum.Note;
        }
        /// <summary>
        /// HTML 模板树节点标识
        /// </summary>
        /// <param name="content"></param>
        /// <param name="command">标识 command</param>
        /// <param name="expression"></param>
        internal TreeBuilderTag(ref SubString content, TreeTemplateCommandEnum command, AutoCSer.Expression.ValueNode expression) : this(ref content, command)
        {
            Expression = expression;
        }
        /// <summary>
        /// HTML 模板树节点标识
        /// </summary>
        /// <param name="expression"></param>
        internal TreeBuilderTag(AutoCSer.Expression.ValueNode expression)
        {
            Command = TreeTemplateCommandEnum.GET;
            Type = TreeBuilderTagTypeEnum.Get;
            Expression = expression;
        }
        /// <summary>
        /// 判断树节点标识是否相等
        /// </summary>
        /// <param name="other">树节点标识</param>
        /// <returns>Is it equal
        /// 是否相等</returns>
        public bool Equals(TreeBuilderTag other)
        {
            return other != null && other.Content.Equals(Content);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Content;
        }
    }
}
