using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.CodeGenerator.NetCoreWebView
{
    /// <summary>
    /// HTML 模板树节点
    /// </summary>
    internal sealed class TreeNode : ITreeTemplateNode<TreeNode>, ITreeBuilderNode<TreeNode, TreeBuilderTag>
    {
        /// <summary>
        /// 树节点标识
        /// </summary>
        public TreeBuilderTag Tag { get; internal set; }
        /// <summary>
        /// 子节点集合
        /// </summary>
        private LeftArray<TreeNode> childs = new LeftArray<TreeNode>(0);
        /// <summary>
        /// 模板命令
        /// </summary>
        public string TemplateCommand { get { return Tag.Command.ToString(); } }
        /// <summary>
        /// 模板成员名称
        /// </summary>
        public SubString TemplateMemberName
        {
            get { return default(SubString); }
            //get { return Tag.Command.String != null ? Tag.Content : default(SubString); }
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
            get { return default(SubString); }
            //get { return Tag.Command.String == null ? Tag.Content : default(SubString); }
        }
        /// <summary>
        /// 子节点数量
        /// </summary>
        public int ChildCount { get { return childs.Length; } }
        /// <summary>
        /// 子节点集合
        /// </summary>
        public IEnumerable<TreeNode> Childs
        {
            get { return childs; }
        }
        /// <summary>
        /// 设置子节点集合
        /// </summary>
        /// <param name="childs">子节点集合</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void SetChilds(TreeNode[] childs)
        {
            if (Tag != null) Tag.IsRound = true;
            this.childs = new LeftArray<TreeNode>(childs);
        }
    }
}
