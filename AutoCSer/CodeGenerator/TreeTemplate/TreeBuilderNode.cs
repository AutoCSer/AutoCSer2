using System;
using System.Collections.Generic;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// CSharp 代码树节点
    /// </summary>
    internal sealed class TreeBuilderNode : ITreeTemplateNode<TreeBuilderNode>, ITreeBuilderNode<TreeBuilderNode, TreeBuilderTag>
    {
        /// <summary>
        /// 树节点标识
        /// </summary>
        public TreeBuilderTag Tag { get; set; }
        /// <summary>
        /// 模板命令
        /// </summary>
        public string TemplateCommand
        {
            get { return Tag.Command; }
        }
        /// <summary>
        /// 模板成员名称
        /// </summary>
        public SubString TemplateMemberName
        {
            get { return Tag.Command != null ? Tag.Content : default(SubString); }
        }
        /// <summary>
        /// 模板成员名称
        /// </summary>
        public SubString TemplateMemberNameBeforeAt
        {
            get
            {
                SubString name = TemplateMemberName;
                int index = name.IndexOf('@');
                return index == -1 ? name : name.GetSub(0, index);
            }
        }
        /// <summary>
        /// 模板文本代码
        /// </summary>
        public SubString TemplateCode
        {
            get { return Tag.Command == null ? Tag.Content : default(SubString); }
        }
        /// <summary>
        /// 子节点数量
        /// </summary>
        public int ChildCount
        {
            get { return childs.Length; }
        }
        /// <summary>
        /// 子节点集合
        /// </summary>
        public IEnumerable<TreeBuilderNode> Childs
        {
            get { return childs; }
        }
        /// <summary>
        /// 子节点集合
        /// </summary>
        private LeftArray<TreeBuilderNode> childs = new LeftArray<TreeBuilderNode>(0);
        /// <summary>
        /// 设置子节点集合
        /// </summary>
        /// <param name="childs">子节点集合</param>
        public void SetChilds(TreeBuilderNode[] childs)
        {
            this.childs.Set(childs);
        }
        /// <summary>
        /// 获取第一个匹配的子孙节点
        /// </summary>
        /// <param name="command">模板命令类型</param>
        /// <param name="content">内容</param>
        /// <returns>匹配的CSharp代码树节点</returns>
        internal TreeBuilderNode GetFirstNodeByTag(TreeTemplateCommandEnum command, ref SubString content)
        {
            if (Tag.Command == command.ToString() && content.Equals(ref Tag.Content)) return this;
            foreach (TreeBuilderNode son in childs)
            {
                TreeBuilderNode value = son.GetFirstNodeByTag(command, ref content);
                if (value != null) return value;
            }
            return null;
        }
    }
}
