using System;
using System.Collections.Generic;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 模板代码节点接口
    /// </summary>
    /// <typeparam name="T">树节点类型</typeparam>
    internal interface ITreeTemplateNode<T>
    {
        /// <summary>
        /// 模板命令
        /// </summary>
        string TemplateCommand { get; }
        /// <summary>
        /// 模板成员名称
        /// </summary>
        SubString TemplateMemberName { get; }
        /// <summary>
        /// 模板成员名称
        /// </summary>
        SubString TemplateMemberNameBeforeAt { get; }
        /// <summary>
        /// 模板文本代码
        /// </summary>
        SubString TemplateCode { get; }
        /// <summary>
        /// 子节点数量
        /// </summary>
        int ChildCount { get; }
        /// <summary>
        /// 子节点集合
        /// </summary>
        IEnumerable<T> Childs { get; }
    }
}
