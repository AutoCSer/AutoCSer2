using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// CSharp 代码建树器
    /// </summary>
    internal sealed class TreeBuilder
    {
        /// <summary>
        /// #region代码段分组名称
        /// </summary>
        public const string RegionGroupName = "region";
        /// <summary>
        /// command代码段分组名称
        /// </summary>
        public const string CommandGroupName = "command";
        /// <summary>
        /// 内容分组名称
        /// </summary>
        public const string ContentGroupName = "content";
        /// <summary>
        /// 所有command
        /// </summary>
        public readonly static string Commands = ((TreeTemplateCommandEnum[])System.Enum.GetValues(typeof(TreeTemplateCommandEnum))).joinString('|', value => value.ToString());
        /// <summary>
        /// command后续取值范围
        /// </summary>
        public const string CommandContent = @"[0-9A-Za-z\.\=]+";
        /// <summary>
        /// #region代码段
        /// </summary>
        private static readonly Regex regionTag = new Regex(@"\r?\n *#(?<" + RegionGroupName + @">" + RegionGroupName + @"|endregion) +(?<" + CommandGroupName + @">" + Commands + @")( +(?<" + ContentGroupName + @">" + CommandContent + @"))? *(?=\r?\n)", RegexOptions.Compiled);
        /// <summary>
        /// /**/注释代码段
        /// </summary>
        private static readonly Regex noteTag = new Regex(@"\/\*(?<" + CommandGroupName + @">" + Commands + @")(\:(?<" + ContentGroupName + @">" + CommandContent + @"))?\*\/", RegexOptions.Compiled);
        /// <summary>
        /// @取值
        /// </summary>
        private static readonly Regex atTag = new Regex(@"@(?<" + ContentGroupName + @">[0-9A-Za-z\.]+)", RegexOptions.Compiled);

        /// <summary>
        /// 建树器
        /// </summary>
        private TreeBuilder<TreeBuilderNode, TreeBuilderTag> tree = new TreeBuilder<TreeBuilderNode, TreeBuilderTag>();
        /// <summary>
        /// 根据代码获取代码树
        /// </summary>
        /// <param name="code">代码</param>
        /// <returns>代码树</returns>
        internal TreeBuilderNode Create(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                try
                {
                    tree.Empty();
                    region(code);
                    TreeBuilderNode boot = new TreeBuilderNode();
                    boot.SetChilds(tree.End());
                    return boot;
                }
                catch (Exception error)
                {
                    Messages.Exception(error);
                }
            }
            return null;
        }
        /// <summary>
        /// 解析#region代码段
        /// </summary>
        /// <param name="code">代码</param>
        private void region(string code)
        {
            int index = 0;
            foreach (Match match in regionTag.Matches(code))
            {
                int length = match.Index - index;
                if (length != 0) note(code.Substring(index, length));
                Group name = match.Groups[ContentGroupName];
                TreeBuilderTag tag = new TreeBuilderTag
                {
                    TagType = TreeBuilderTagTypeEnum.Region,
                    Command = match.Groups[CommandGroupName].Value,
                    Content = name?.Value
                };
                if (match.Groups[RegionGroupName].Value == RegionGroupName) tree.Append(new TreeBuilderNode { Tag = tag });
                else tree.Round(tag);
                index = match.Index + match.Length;
            }
            if (index != code.Length) note(code.Substring(index));
        }
        /// <summary>
        /// 解析/**/注释代码段
        /// </summary>
        /// <param name="code">代码</param>
        private void note(string code)
        {
            int index = 0;
            foreach (Match match in noteTag.Matches(code))
            {
                int length = match.Index - index;
                if (length != 0) at(code.Substring(index, length));
                Group name = match.Groups[ContentGroupName];
                TreeBuilderTag tag = new TreeBuilderTag
                {
                    TagType = TreeBuilderTagTypeEnum.Note,
                    Command = match.Groups[CommandGroupName].Value,
                    Content = name?.Value
                };
                bool isAt = tag.Command == TreeTemplateCommandEnum.AT.ToString();
                if (isAt || !tree.IsRound(tag)) tree.Append(new TreeBuilderNode { Tag = tag }, !isAt);
                index = match.Index + match.Length;
            }
            if (index != code.Length) at(code.Substring(index));
        }
        /// <summary>
        /// 解析@取值
        /// </summary>
        /// <param name="code">代码</param>
        private void at(string code)
        {
            int index = 0;
            foreach (Match match in atTag.Matches(code))
            {
                int length = match.Index - index;
                if (length != 0) this.code(new SubString { String = code, Start = index, Length = length });
                tree.Append(new TreeBuilderNode
                {
                    Tag = new TreeBuilderTag
                    {
                        TagType = TreeBuilderTagTypeEnum.At,
                        Command = TreeTemplateCommandEnum.AT.ToString(),
                        Content = match.Groups[ContentGroupName].Value
                    }
                }, false);
                index = match.Index + match.Length;
            }
            this.code(new SubString { String = code, Start = index, Length = code.Length - index });
        }
        /// <summary>
        /// 普通代码段
        /// </summary>
        /// <param name="code">代码</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void code(SubString code)
        {
            tree.Append(new TreeBuilderNode
            {
                Tag = new TreeBuilderTag
                {
                    TagType = TreeBuilderTagTypeEnum.Code,
                    Content = code
                }
            }, false);
        }
    }
    /// <summary>
    /// 建树器
    /// </summary>
    /// <typeparam name="NT">树节点类型</typeparam>
    /// <typeparam name="TT">树节点标识类型</typeparam>
    internal sealed class TreeBuilder<NT, TT>
        where NT : ITreeBuilderNode<NT, TT>
        where TT : IEquatable<TT>
    {
        /// <summary>
        /// 当前节点集合
        /// </summary>
        private LeftArray<KeyValue<NT, bool>> nodes = new LeftArray<KeyValue<NT, bool>>(0);
        /// <summary>
        /// 清除节点
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Empty()
        {
            nodes.Length = 0;
        }
        /// <summary>
        /// 追加新节点
        /// </summary>
        /// <param name="node">新节点</param>
        /// <param name="isRound">是否需要判断回合</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Append(NT node, bool isRound = true)
        {
            nodes.Add(new KeyValue<NT, bool>(node, isRound));
        }
        /// <summary>
        /// 节点回合
        /// </summary>
        /// <param name="tagName">树节点标识</param>
        public void Round(TT tagName)
        {
            TreeBuilderCheckRoundTypeEnum check = round(tagName);
            switch (check)
            {
                case TreeBuilderCheckRoundTypeEnum.LessRound:
                    throw new OverflowException(Culture.Configuration.Default.GetTreeTemplateNotFoundRoundNode + " : " + tagName.ToString() + @"
" + nodes.JoinString(@"
", value => value.Key.Tag.ToString()));
                case TreeBuilderCheckRoundTypeEnum.UnknownRound:
                    throw new OverflowException(Culture.Configuration.Default.GetTreeTemplateUnknownRoundNode + " : " + tagName.ToString() + @"
" + nodes.JoinString(@"
", value => value.Key.Tag.ToString()));
            }
        }
        /// <summary>
        /// 节点回合
        /// </summary>
        /// <param name="tagName">树节点标识</param>
        /// <returns>节点回合是否成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool IsRound(TT tagName)
        {
            return round(tagName) == TreeBuilderCheckRoundTypeEnum.Ok;
        }
        /// <summary>
        /// 节点回合
        /// </summary>
        /// <param name="tagName">树节点标识</param>
        /// <returns>节点回合状态</returns>
        private TreeBuilderCheckRoundTypeEnum round(TT tagName)
        {
            KeyValue<NT, bool>[] array = nodes.Array;
            for (int index = nodes.Length; index != 0;)
            {
                if (array[--index].Value)
                {
                    NT node = array[index].Key;
                    if (tagName.Equals(node.Tag))
                    {
                        array[index].Set(node, false);
                        node.SetChilds(new SubArray<KeyValue<NT, bool>>(++index, nodes.Length - index, array).GetArray(value => value.Key));
                        nodes.Length = index;
                        return TreeBuilderCheckRoundTypeEnum.Ok;
                    }
                    return TreeBuilderCheckRoundTypeEnum.LessRound;
                    //if (!isAny) return TreeBuilderCheckRoundTypeEnum.LessRound;
                    //if (checkRound != null && checkRound(node)) return TreeBuilderCheckRoundTypeEnum.LessRound;
                }
            }
            return TreeBuilderCheckRoundTypeEnum.UnknownRound;
        }
        /// <summary>
        /// 建树结束
        /// </summary>
        /// <returns>根节点集合</returns>
        public NT[] End()
        {
            //if (checkRound != null)
            //{
            KeyValue<NT, bool>[] array = nodes.Array;
            for (int index = nodes.Length; index != 0;)
            {
                //if (array[--index].Value && checkRound(array[index].Key))
                if (array[--index].Value)
                {
                    throw new OverflowException(Culture.Configuration.Default.GetTreeTemplateNotFoundRoundNode + @"
" + nodes.JoinString(@"
", value => value.Key.Tag.ToString()));
                }
            }
            //}
            return nodes.GetArray(value => value.Key);
        }
    }
}
