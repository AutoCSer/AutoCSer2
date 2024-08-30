using AutoCSer.CodeGenerator.Metadata;
using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AutoCSer.CodeGenerator.NetCoreWebView
{
    /// <summary>
    /// 输出 JavaScript 的 HTML 模板解析
    /// </summary>
    internal sealed class JavaScriptTreeTemplate : TreeTemplate<TreeNode>
    {
        /// <summary>
        /// 视图 JavaScript 成员节点有序名称排序
        /// </summary>
        private static readonly Func<KeyValue<SubString, TreeTemplateMemberNode>, KeyValue<SubString, TreeTemplateMemberNode>, int> nameSortHandle = nameSort;
        /// <summary>
        /// JavaScript 流写入字符前缀
        /// </summary>
        private const string streamWriteCharPrefix = "stream.Write('";
        /// <summary>
        /// JavaScript 流写入字符后缀
        /// </summary>
        private const string streamWriteCharSuffix = "');";
        /// <summary>
        /// JavaScript 引号替换
        /// </summary>
        private static readonly Regex quoteRegex = new Regex(@"(stream\.Write\(@"".?""\);)", RegexOptions.Compiled);

        /// <summary>
        /// 是否 JavaScript
        /// </summary>
        internal override bool IsJavaScript { get { return true; } }
        /// <summary>
        /// HTML 模板文件名称
        /// </summary>
        private readonly string fileName;
        /// <summary>
        /// 当前解析表达式字符串
        /// </summary>
        private SubString currentExpression;
        /// <summary>
        /// 当前解析 HTML 模板指令类型
        /// </summary>
        private TreeTemplateCommandEnum currentCommandType;
        /// <summary>
        /// 视图 JavaScript 输出
        /// </summary>
        private LeftArray<string> codes;
        /// <summary>
        /// HTML 模板解析
        /// </summary>
        /// <param name="type">模板关联视图类型</param>
        /// <param name="fileName">HTML 模板文件名称</param>
        /// <param name="html">HTML 模板</param>
        unsafe internal JavaScriptTreeTemplate(Type type, string fileName, string html)
            : base(type, Messages.Error, Messages.Message)
        {
            this.fileName = fileName;
            creators[TreeTemplateCommandEnum.CLINET.ToString()] = note;
            creators[TreeTemplateCommandEnum.LOOP.ToString()] = loop;
            creators[TreeTemplateCommandEnum.PUSH.ToString()] = push;
            creators[TreeTemplateCommandEnum.GET.ToString()] = at;
            creators[TreeTemplateCommandEnum.IF.ToString()] = not;
            creators[TreeTemplateCommandEnum.NOT.ToString()] = not;

            TreeBuilderTag checkTag = new TreeBuilderTag();
            TreeBuilder<TreeNode, TreeBuilderTag> tree = new TreeBuilder<TreeNode, TreeBuilderTag>();
            fixed (char* htmlFixed = html)
            {
                foreach (Range range in splitTemplate(htmlFixed, htmlFixed + html.Length))
                {
                    int startIndex = range.StartIndex;
                    if (startIndex != 0)
                    {
                        int noteEndIndex = getTemplateTagEnd(htmlFixed, range);
                        if (noteEndIndex != -1)
                        {
                            SubString note = new SubString(startIndex, noteEndIndex - startIndex, html), command = note;
                            int expressionIndex = command.IndexOf(':');
                            if (expressionIndex >= 0)
                            {
                                currentExpression = command.GetSub(expressionIndex + 1);
                                command.Length = expressionIndex;
                            }
                            else currentExpression.SetEmpty();
                            int nameIndex = command.IndexOf('#');
                            if (nameIndex >= 0) command.Length = nameIndex;
                            if (commands.TryGetValue(command, out currentCommandType) && currentCommandType != TreeTemplateCommandEnum.GET)
                            {
                                checkTag.Content = note;
                                TreeBuilderTag tag = null;
                                if (!tree.IsRound(checkTag))
                                {
                                    switch (currentCommandType)
                                    {
                                        case TreeTemplateCommandEnum.IF:
                                        case TreeTemplateCommandEnum.NOT:
                                        case TreeTemplateCommandEnum.PUSH:
                                        case TreeTemplateCommandEnum.LOOP:
                                            AutoCSer.Expression.ValueNode node = getExpression();
                                            if (node != null) tag = new TreeBuilderTag(ref note, currentCommandType, node);
                                            break;
                                    }
                                    if (tag == null)
                                    {
                                        switch (currentCommandType)
                                        {
                                            case TreeTemplateCommandEnum.PUSH:
                                            case TreeTemplateCommandEnum.LOOP:
                                                tag = new TreeBuilderTag(ref note, TreeTemplateCommandEnum.CLINET);
                                                break;
                                            default: tag = new TreeBuilderTag(ref note, currentCommandType); break;
                                        }
                                    }
                                    tree.Append(new TreeNode { Tag = tag }, currentCommandType != TreeTemplateCommandEnum.GET);
                                }
                                startIndex = noteEndIndex + 3;
                            }
                        }
                    }
                    if (range.EndIndex - startIndex >= 5)
                    {
                        char* start = htmlFixed + startIndex, end = htmlFixed + range.EndIndex, getStart = null;
                        char getChar = '{';
                        do
                        {
                            if (*start == getChar && *(start + 1) == getChar)
                            {
                                if (getChar == '{')
                                {
                                    getChar = '}';
                                    getStart = (start += 2);
                                }
                                else
                                {
                                    if (start != getStart)
                                    {
                                        currentExpression.Set(html, (int)(getStart - htmlFixed), (int)(start - getStart));
                                        currentCommandType = TreeTemplateCommandEnum.GET;
                                        AutoCSer.Expression.ValueNode node = getExpression();
                                        if (node != null) tree.Append(new TreeNode { Tag = new TreeBuilderTag(node) }, false);
                                    }
                                    getChar = '{';
                                    start += 2;
                                }
                            }
                            else ++start;
                        }
                        while (start <= end);
                    }
                }
            }
            foreach (TreeNode node in tree.End()) skin(node);
        }
        /// <summary>
        /// 获取表达式
        /// </summary>
        /// <returns>null 表示失败，ValueExpression.Null 表示返回 null</returns>
        private AutoCSer.Expression.ValueNode getExpression()
        {
            if (currentExpression.Length != 0)
            {
                AutoCSer.Expression.Builder builder = new AutoCSer.Expression.Builder(ref currentExpression, true);
                if (builder.Expression != null)
                {
                    check(builder.Expression);
                    return builder.Expression;
                }
                else Messages.Error($"HTML 页面文件 {fileName} 表达式 {currentExpression} 解析失败位置 {builder.Index}");
            }
            else Messages.Error($"HTML 页面文件 {fileName} 模板 {currentCommandType} 缺少表达式");
            return null;
        }
        /// <summary>
        /// 检查节点表达式
        /// </summary>
        /// <param name="node"></param>
        private void check(AutoCSer.Expression.ValueNode node)
        {
            bool isClient = false;
            do
            {
                switch (node.ValueType)
                {
                    case Expression.ValueTypeEnum.Client:
                    case Expression.ValueTypeEnum.ClientEncode:
                        isClient = true;
                        break;
                    case Expression.ValueTypeEnum.Index:
                    case Expression.ValueTypeEnum.IfNotNullIndex:
                    case Expression.ValueTypeEnum.Call:
                        if (!isClient) Messages.Error($"HTML 页面文件 {fileName} 指令 {currentCommandType} 不支持服务端 {node.ValueType} 表达式 {currentExpression}");
                        foreach (AutoCSer.Expression.ValueNode parameter in ((AutoCSer.Expression.CallNode)node).Parameters) check(parameter);
                        break;
                    case Expression.ValueTypeEnum.Parenthesis:
                        if (!isClient && node.Next != null) Messages.Error($"HTML 页面文件 {fileName} 指令 {currentCommandType} 不支持服务端 ({node.ValueType}).Next 表达式 {currentExpression}");
                        check(((AutoCSer.Expression.ParenthesisNode)node).Parenthesis);
                        break;
                    case Expression.ValueTypeEnum.IfElse:
                        check(((AutoCSer.Expression.IfElseNode)node).Else);
                        isClient = false;
                        break;
                    case Expression.ValueTypeEnum.LessOrEqual:
                    case Expression.ValueTypeEnum.Less:
                    case Expression.ValueTypeEnum.NotEqual:
                    case Expression.ValueTypeEnum.Equal:
                    case Expression.ValueTypeEnum.GreaterOrEqual:
                    case Expression.ValueTypeEnum.Greater:
                    case Expression.ValueTypeEnum.And:
                    case Expression.ValueTypeEnum.Or:
                    case Expression.ValueTypeEnum.Not:
                    case Expression.ValueTypeEnum.Add:
                    case Expression.ValueTypeEnum.Subtract:
                    case Expression.ValueTypeEnum.Multiply:
                    case Expression.ValueTypeEnum.Divide:
                    case Expression.ValueTypeEnum.Mod:
                    case Expression.ValueTypeEnum.LeftShift:
                    case Expression.ValueTypeEnum.RightShift:
                    case Expression.ValueTypeEnum.BitAnd:
                    case Expression.ValueTypeEnum.BitOr:
                    case Expression.ValueTypeEnum.Xor:
                    case Expression.ValueTypeEnum.IfNullThen:
                        isClient = false;
                        break;
                }
            }
            while ((node = node.Next) != null);
        }
        /// <summary>
        /// 根据表达式获取成员树节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="isEnumerableType"></param>
        /// <returns>成员树节点</returns>
        private TreeTemplateMemberNode getMember(TreeNode node, bool isEnumerableType)
        {
            AutoCSer.Expression.ValueNode expression = node.Tag.Expression;
            if (expression != null)
            {
                int memberIndex = currentMembers.Length - expression.MemberDepth - 1;
                if (memberIndex >= 0)
                {
                    TreeTemplateMemberNode member = currentMembers[memberIndex];
                    do
                    {
                        switch (expression.ValueType)
                        {
                            case Expression.ValueTypeEnum.Member:
                                member = member.Get(ref ((AutoCSer.Expression.ContentNode)expression).Content);
                                if (member == null)
                                {
                                    Messages.Error($"{viewType.fullName()} 成员 {currentMembers[memberIndex].FullPath} 没有找到表达式成员 {node.Tag.Content} [{((AutoCSer.Expression.ContentNode)expression).Content}]");
                                    expression = null;
                                }
                                else expression = expression.Next;
                                break;
                            case Expression.ValueTypeEnum.NextMember:
                            case Expression.ValueTypeEnum.IfNotNullMember:
                                expression = expression.Next;
                                break;
                            case Expression.ValueTypeEnum.Client:
                            case Expression.ValueTypeEnum.ClientEncode:
                                member = null;
                                expression = null;
                                break;
                            default:
                                Messages.Error($"{viewType.fullName()} 指令 {node.Tag.Command} 仅支持成员表达式，不支持 {expression.ValueType} 表达式 {node.Tag.Content}");
                                member = null;
                                expression = null;
                                break;
                        }
                    }
                    while (expression != null);
                    if (member != null)
                    {
                        if (!isEnumerableType || member.Type.EnumerableType != null) return member;
                        Messages.Error($"{viewType.fullName()} 成员 {currentMembers[memberIndex].FullPath} 表达式 {node.Tag.Content} 类型不可枚举 {member.Type.FullName}");
                        return null;
                    }
                }
            }
            checkMember(node);
            return null;
        }
        /// <summary>
        /// 检查表达式
        /// </summary>
        /// <param name="node"></param>
        private void checkMember(TreeNode node)
        {
            AutoCSer.Expression.ValueNode expression = node.Tag.Expression;
            if (expression != null)
            {
                LeftArray<AutoCSer.Expression.ValueNode> expressions = new LeftArray<Expression.ValueNode>(0);
                do
                {
                    int memberIndex = currentMembers.Length - expression.MemberDepth - 1;
                    if (memberIndex >= 0)
                    {
                        TreeTemplateMemberNode member = currentMembers[memberIndex];
                        do
                        {
                            switch (expression.ValueType)
                            {
                                case Expression.ValueTypeEnum.Member:
                                    if (member != null)
                                    {
                                        member = member.Get(ref ((AutoCSer.Expression.ContentNode)expression).Content);
                                        if (member == null)
                                        {
                                            Messages.Error($"{viewType.fullName()} 成员 {currentMembers[memberIndex].FullPath} 没有找到表达式成员 {node.Tag.Content} [{((AutoCSer.Expression.ContentNode)expression).Content}]");
                                        }
                                    }
                                    break;
                                case Expression.ValueTypeEnum.NextMember:
                                case Expression.ValueTypeEnum.IfNotNullMember:
                                    break;
                                case Expression.ValueTypeEnum.Call:
                                case Expression.ValueTypeEnum.Index:
                                case Expression.ValueTypeEnum.IfNotNullIndex:
                                    expressions.Add(((AutoCSer.Expression.CallNode)expression).Parameters);
                                    member = null;
                                    break;
                                case Expression.ValueTypeEnum.Client:
                                case Expression.ValueTypeEnum.ClientEncode:
                                    member = null;
                                    break;
                                case Expression.ValueTypeEnum.Parenthesis:
                                    expressions.Add(((AutoCSer.Expression.ParenthesisNode)expression).Parenthesis);
                                    member = null;
                                    break;
                                case Expression.ValueTypeEnum.IfElse:
                                    expressions.Add(((AutoCSer.Expression.IfElseNode)expression).Else);
                                    expressions.Add(expression.Next);
                                    expression = AutoCSer.Expression.ValueNode.Null;
                                    break;
                                case Expression.ValueTypeEnum.String:
                                case Expression.ValueTypeEnum.Decimalism:
                                case Expression.ValueTypeEnum.Hex:
                                case Expression.ValueTypeEnum.SignedDecimalism:
                                case Expression.ValueTypeEnum.SignedHex:
                                case Expression.ValueTypeEnum.Decimal:
                                case Expression.ValueTypeEnum.SignedDecimal:
                                case Expression.ValueTypeEnum.True:
                                case Expression.ValueTypeEnum.False:
                                    member = null;
                                    break;
                                case Expression.ValueTypeEnum.LessOrEqual:
                                case Expression.ValueTypeEnum.Less:
                                case Expression.ValueTypeEnum.NotEqual:
                                case Expression.ValueTypeEnum.Equal:
                                case Expression.ValueTypeEnum.GreaterOrEqual:
                                case Expression.ValueTypeEnum.Greater:
                                case Expression.ValueTypeEnum.And:
                                case Expression.ValueTypeEnum.Or:
                                case Expression.ValueTypeEnum.Not:
                                case Expression.ValueTypeEnum.Add:
                                case Expression.ValueTypeEnum.Subtract:
                                case Expression.ValueTypeEnum.Multiply:
                                case Expression.ValueTypeEnum.Divide:
                                case Expression.ValueTypeEnum.Mod:
                                case Expression.ValueTypeEnum.LeftShift:
                                case Expression.ValueTypeEnum.RightShift:
                                case Expression.ValueTypeEnum.BitAnd:
                                case Expression.ValueTypeEnum.BitOr:
                                case Expression.ValueTypeEnum.Xor:
                                case Expression.ValueTypeEnum.IfNullThen:
                                    expressions.Add(expression.Next);
                                    expression = AutoCSer.Expression.ValueNode.Null;
                                    break;
                            }
                        }
                        while ((expression = expression.Next) != null);
                    }
                    else Messages.Error($"{viewType.fullName()} 成员 {currentMembers.LastOrDefault().FullPath} 表达式 {node.Tag.Content} 回溯深度 {expression.MemberDepth} 超出范围 {currentMembers.Length - 1}");
                }
                while (expressions.TryPop(out expression));
            }
        }
        /// <summary>
        /// 添加代码树节点
        /// </summary>
        /// <param name="boot">代码树节点</param>
        protected override void skin(TreeNode node)
        {
            string command = node.TemplateCommand;
            if (command != null)
            {
                if (creators.TryGetValue(command, out Action<TreeNode> creator)) creator(node);
                else onError(viewType.fullName() + " 未找到指令处理函数 " + command + " : " + node.TemplateMemberName.ToString());
            }
        }
        /// <summary>
        /// 循环处理
        /// </summary>
        /// <param name="node">代码树节点</param>
        protected override void loop(TreeNode node)
        {
            TreeTemplateMemberNode member = getMember(node, true);
            if (member != null)
            {
                currentMembers.Add(member.Get(string.Empty));
                foreach (TreeNode nextNode in node.Childs) skin(nextNode);
                --currentMembers.Length;
            }
        }
        /// <summary>
        /// 子代码段处理
        /// </summary>
        /// <param name="node">代码树节点</param>
        protected override void push(TreeNode node)
        {
            TreeTemplateMemberNode member = getMember(node, false);
            if (member != null)
            {
                currentMembers.Add(member);
                foreach (TreeNode nextNode in node.Childs) skin(nextNode);
                --currentMembers.Length;
            }
        }
        /// <summary>
        /// 输出绑定的数据
        /// </summary>
        /// <param name="node">代码树节点</param>
        protected override void at(TreeNode node)
        {
            checkMember(node);
        }
        /// <summary>
        /// 子代码段处理
        /// </summary>
        /// <param name="node"></param>
        protected override void not(TreeNode node)
        {
            checkMember(node);
            foreach (TreeNode nextNode in node.Childs) skin(nextNode);
        }
        /// <summary>
        /// 视图 JavaScript 输出
        /// </summary>
        /// <returns></returns>
        internal string GetCode()
        {
            if (currentMembers.Length == 0) return @"stream.WriteJsonObject();";
            codes = new LeftArray<string>(1 << 10);
            codes.Add(@"stream.Write(@""");
            code(currentMembers[0], null);
            codes.Add(@""");");
            return quoteRegex.Replace(string.Concat(codes.ToArray()), replaceQuote);
        }
        /// <summary>
        /// 添加成员节点并返回临时变量名称
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        private string pushMemberPath(TreeTemplateMemberNode member)
        {
            currentMembers.Add(member);
            return path(0);
        }
        /// <summary>
        /// 视图 JavaScript 成员节点代码
        /// </summary>
        /// <param name="node">成员节点</param>
        /// <param name="parentPath">父节点路径</param>
        private void code(TreeTemplateMemberNode node, string parentPath)
        {
            TreeTemplateMemberNode loopMember = node.GetOnly(string.Empty);
            if (loopMember == null)
            {
                AutoCSer.NetCoreWeb.ViewClientTypeAttribute viewClientTypeAttribute = node.Type.NetCoreWebViewClientTypeAttribute;
                if (!string.IsNullOrEmpty(viewClientTypeAttribute?.Name))
                {
                    if (string.IsNullOrEmpty(viewClientTypeAttribute.PrimaryKeyMemberName)) codes.Append("new ", viewClientTypeAttribute.Name, "(");
                    else codes.Append(viewClientTypeAttribute.Name, ".Get(");
                }
                codes.Add("{");
                KeyValue<SubString, TreeTemplateMemberNode>[] names = getNames(node);
                int memberIndex = 0;
                foreach (KeyValue<SubString, TreeTemplateMemberNode> name in names)
                {
                    TreeTemplateMemberNode member = name.Value;
                    if (!member.IsIgnoreDefault)
                    {
                        string pathName = pushMemberPath(member), memberName = name.Key;
                        if (memberIndex++ != 0) codes.Add(",");
                        codes.Append(memberName, @":"");
                    {
                        ");
                        string awaitPathName = getAwaitPathName(member, parentPath, memberName, pathName);
                        if (member.Type.IsNull || awaitPathName != null)
                        {
                            codes.Add(@"
                        if (");
                            if (awaitPathName != null)
                            {
                                codes.Append(awaitPathName, " == null");
                                if (member.Type.IsNull) codes.Add(" || ");
                            }
                            if(member.Type.IsNull) codes.Append(pathName, " == null");
                            codes.Add(@") stream.WriteJsonNull();
                        else
                        {");
                        }
                        if (member.IsNextPath)
                        {
                            codes.Add(@"
                            stream.Write(@""");
                            code(member, pathName);
                            codes.Add(@""");");
                        }
                        else value(member.Type, pathName);
                        if (member.Type.IsNull || awaitPathName != null)
                        {
                            codes.Add(@"
                        }");
                        }
                        codes.Add(@"
                    }
                    stream.Write(@""");
                        --currentMembers.Length;
                    }
                }
                if (memberIndex == 0)
                {
                    string memberIgnoreName = memberIgnore(0);
                    codes.Append(@""");
                    bool ", memberIgnoreName, @" = false;");
                    foreach (KeyValue<SubString, TreeTemplateMemberNode> name in names)
                    {
                        TreeTemplateMemberNode member = name.Value;
                        if (member.IsIgnoreDefault)
                        {
                            string pathName = pushMemberPath(member), memberName = name.Key;
                            codes.Add(@"
                    {
                        ");
                            string awaitPathName = getAwaitPathName(member, parentPath, memberName, pathName);
                            codes.Add(@"
                        if (");
                            if (awaitPathName != null) codes.Append(awaitPathName, @" != null && ");
                            codes.Append("!", pathName, @".Equals(default(", member.Type.FullName, @")))
                        {
                            if (", memberIgnoreName, @") stream.Write(',');
                            else ", memberIgnoreName, @" = true;
                            stream.Write(@""", memberName, ":");
                            if (member.IsNextPath)
                            {
                                code(member, pathName);
                                codes.Add(@""");");
                            }
                            else
                            {
                                codes.Add(@""");");
                                value(member.Type, pathName);
                            }
                            codes.Add(@"
                        }
                    }");
                            --currentMembers.Length;
                        }
                    }
                    codes.Add(@"
                    stream.Write(@""");
                }
                else
                {
                    foreach (KeyValue<SubString, TreeTemplateMemberNode> name in names)
                    {
                        TreeTemplateMemberNode member = name.Value;
                        if (member.IsIgnoreDefault)
                        {
                            string pathName = pushMemberPath(member), memberName = name.Key;
                            codes.Add(@""");
                    {
                        ");
                            string awaitPathName = getAwaitPathName(member, parentPath, memberName, pathName);
                            codes.Add(@"
                        if (");
                            if (awaitPathName != null) codes.Append(awaitPathName, @" != null && ");
                            codes.Append("!", pathName, @".Equals(default(", member.Type.FullName, @")))
                        {
                            stream.Write(@"",", memberName, ":");
                            if (member.IsNextPath)
                            {
                                code(member, pathName);
                                codes.Add(@""");");
                            }
                            else
                            {
                                codes.Add(@""");");
                                value(member.Type, pathName);
                            }
                            codes.Add(@"
                        }
                    }
                    stream.Write(@""");
                            --currentMembers.Length;
                        }
                    }
                }
                codes.Add("}");
                if (!string.IsNullOrEmpty(viewClientTypeAttribute?.Name)) codes.Add(")");
                return;
            }
            ExtensionType type = loopMember.Type;
            string loopPathName = pushMemberPath(loopMember), loopIndexName = loopIndex(0);
            codes.Append(@"["");
                    {
                        int ", loopIndexName, @" = 0;
                        foreach (", type.FullName, " ", loopPathName + " in ", parentPath, @")
                        {");
            if (loopMember.IsNextPath)
            {
                codes.Append(@"
                    if (", loopIndexName, @"++ == 0)
                    {
                        stream.Write('""');
                        stream.Write(""");
                loopNames(loopMember);
                codes.Add(@""");
                        stream.Write('""');
                    }
                    stream.Write(',');");
                if (type.IsNull)
                {
                    codes.Append(@"
                    if (", loopPathName, @" == null) stream.WriteJsonNull();
                    else
                    {");
                }
                codes.Add(@"
                    stream.Write(@""[");
                loop(loopMember, loopPathName);
                codes.Add(@"]"");");
                if (type.IsNull)
                {
                    codes.Add(@"
                    }");
                }
                codes.Append(@"
                        }
                    }
                    stream.Write(@""].FormatView()");
            }
            else
            {
                codes.Append(@"
                    if (", loopIndexName, @"++ != 0) stream.Write(',');");
                if (type.IsNull)
                {
                    codes.Append(@"
                    if (", loopPathName, @" == null) stream.WriteJsonNull();
                    else
                    {");
                }
                value(type, loopPathName);
                if (type.IsNull)
                {
                    codes.Add(@"
                    }");
                }
                codes.Append(@"
                        }
                    }
                    stream.Write(@""]");
            }
            --currentMembers.Length;
        }
        /// <summary>
        /// 视图 JavaScript 成员节点有序名称集合
        /// </summary>
        /// <param name="node">成员节点</param>
        /// <returns>视图 JavaScript 成员节点有序名称集合</returns>
        private KeyValue<SubString, TreeTemplateMemberNode>[] getNames(TreeTemplateMemberNode node)
        {
            Dictionary<HashSubString, TreeTemplateMemberNode> members;
            return memberPaths.TryGetValue(node, out members) && members.Count != 0 ? members.getArray(value => new KeyValue<SubString, TreeTemplateMemberNode>(value.Key.String, value.Value)).sort(nameSortHandle) : EmptyArray<KeyValue<SubString, TreeTemplateMemberNode>>.Array;
        }
        /// <summary>
        /// 获取成员并返回 await 变量名称
        /// </summary>
        /// <param name="member"></param>
        /// <param name="parentPath"></param>
        /// <param name="memberName"></param>
        /// <param name="pathName"></param>
        /// <returns></returns>
        private string getAwaitPathName(TreeTemplateMemberNode member, string parentPath, string memberName, string pathName)
        {
            if (member.AwaitType != null && member.AwaitType.IsNull)
            {
                string awaitPathName = awaitPath();
                codes.Append(member.AwaitType.FullName, " ", awaitPathName, " = ");
                if (!string.IsNullOrEmpty(parentPath)) codes.Append(parentPath, ".");
                codes.Append(memberName, @";
                        ", member.Type.FullName, " ", pathName, " = ", awaitPathName, " != null ? await ", awaitPathName, " : default(", member.Type.FullName, ");");
                return awaitPathName;
            }
            codes.Append(member.Type.FullName, " ", pathName, " = ");
            if (member.AwaitType != null) codes.Add("await ");
            if (!string.IsNullOrEmpty(parentPath)) codes.Append(parentPath, ".");
            codes.Append(memberName, ";");
            return null;
        }
        /// <summary>
        /// 视图 JavaScript 循环成员节点名称
        /// </summary>
        /// <param name="node">成员节点</param>
        private void loopNames(TreeTemplateMemberNode node)
        {
            TreeTemplateMemberNode loopMember = node.GetOnly(string.Empty);
            if(loopMember == null)
            {
                int memberIndex = 0;
                AutoCSer.NetCoreWeb.ViewClientTypeAttribute viewClientTypeAttribute = node.Type.NetCoreWebViewClientTypeAttribute;
                if (!string.IsNullOrEmpty(viewClientTypeAttribute?.Name))
                {
                    memberIndex = 1;
                    codes.Add("@");//视图类型名称
                    if (!string.IsNullOrEmpty(viewClientTypeAttribute.PrimaryKeyMemberName)) codes.Add(".");//视图类型成员标识
                    codes.Append(viewClientTypeAttribute.Name, ",");
                }
                foreach (KeyValue<SubString, TreeTemplateMemberNode> name in getNames(node))
                {
                    if (memberIndex != 0) codes.Add(",");
                    codes.Add(name.Key);
                    if (name.Value.IsNextPath)
                    {
                        codes.Add("[");
                        loopNames(name.Value);
                        codes.Add("]");
                        memberIndex = 0;
                    }
                    else memberIndex = 1;
                }
                return;
            }
            codes.Add("[");
            if (loopMember.IsNextPath) loopNames(loopMember);
            codes.Add("]");
        }
        /// <summary>
        /// 视图 JavaScript 循环成员节点数据
        /// </summary>
        /// <param name="node">成员节点</param>
        /// <param name="parentPath">父节点路径</param>
        private void loop(TreeTemplateMemberNode node, string parentPath)
        {
            TreeTemplateMemberNode loopMember = node.GetOnly(string.Empty);
            if (loopMember == null)
            {
                int memberIndex = 0;
                foreach (KeyValue<SubString, TreeTemplateMemberNode> name in getNames(node))
                {
                    if (memberIndex++ != 0) codes.Add(",");
                    TreeTemplateMemberNode member = name.Value;
                    string pathName = pushMemberPath(member), memberName = name.Key;
                    codes.Add(@""");
                    {
                        ");
                    string awaitPathName = getAwaitPathName(member, parentPath, memberName, pathName);
                    if (member.Type.IsNull || awaitPathName != null)
                    {
                        codes.Add(@"
                                if (");
                        if (awaitPathName != null)
                        {
                            codes.Append(awaitPathName, " == null");
                            if (member.Type.IsNull) codes.Add(" || ");
                        }
                        if (member.Type.IsNull) codes.Append(pathName, " == null");
                        codes.Add(@") stream.WriteJsonNull();
                                else
                                {");
                    }
                    if (member.IsNextPath)
                    {
                        codes.Add(@"
                                    stream.Write(@""[");
                        loop(member, pathName);
                        codes.Add(@"]"");");
                    }
                    else value(member.Type, pathName);
                    if (member.Type.IsNull || awaitPathName != null)
                    {
                        codes.Add(@"
                                }");
                    }
                    codes.Add(@"
                    }
                    stream.Write(@""");
                    --currentMembers.Length;
                }
                return;
            }
            ExtensionType type = loopMember.Type;
            string loopPathName = pushMemberPath(loopMember), loopIndexName = loopIndex(0);
            codes.Append(@"["");
                    {
                        int ", loopIndexName, @" = 0;
                        foreach (", type.FullName, " " + loopPathName + " in ", parentPath, @")
                        {
                            if (", loopIndexName, @"++ != 0) stream.Write(',');");
            if (type.IsNull)
            {
                codes.Append(@"
                            if (", loopPathName, @" == null) stream.WriteJsonNull();
                            else
                            {");
            }
            if (loopMember.IsNextPath)
            {
                codes.Add(@"
                                stream.Write('[');");
                codes.Add(@"
                                stream.Write(@""");
                loop(loopMember, loopPathName);
                codes.Add(@"]"");");
            }
            else value(type, loopPathName);
            if (type.IsNull)
            {
                codes.Add(@"
                            }");
            }
            codes.Append(@"
                        }
                    }
                    stream.Write(@""]");
            --currentMembers.Length;
        }
        /// <summary>
        /// 视图 JavaScript 叶子成员节点代码
        /// </summary>
        /// <param name="type">成员类型</param>
        /// <param name="name">成员名称</param>
        private void value(ExtensionType type, string name)
        {
            if (type.IsJavaScriptToString || type.IsDateTime)
            {
                codes.Append(@"
                    stream.WriteWebViewJson((", (type.NullableType?.Type ?? type.Type).FullName, ")", name, ");");
            }
            else if (type.IsString || type.IsSubString)
            {
                codes.Append(@"
                    stream.WriteWebViewJson(", name, ");");
            }
            else if (type.Type.IsEnum)
            {
                codes.Append(@"
                    stream.Write(", name, @".ToString(), '""');");
            }
            else
            {
                AutoCSer.NetCoreWeb.ViewClientTypeAttribute viewClientTypeAttribute = type.NetCoreWebViewClientTypeAttribute;
                if (!string.IsNullOrEmpty(viewClientTypeAttribute?.Name))
                {
                    codes.Add(@"
                    stream.Write(@""");
                    if (string.IsNullOrEmpty(viewClientTypeAttribute.PrimaryKeyMemberName)) codes.Append("new ", viewClientTypeAttribute.Name, "(");
                    else codes.Append(viewClientTypeAttribute.Name, ".Get(");
                    codes.Add(@"{})"");");
                }
                else
                {
                    codes.Add(@"
                    stream.WriteJsonObject();");
                }
            }
        }
        /// <summary>
        /// 获取忽略输出变量名称
        /// </summary>
        /// <param name="index">忽略输出变量层次</param>
        /// <returns>忽略输出变量名称</returns>
        private string memberIgnore(int index)
        {
            return "_memberIgnore" + (index == 0 ? (currentMembers.Length - 1) : index).toString() + "_";
        }

        /// <summary>
        /// 视图 JavaScript 成员节点有序名称排序
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int nameSort(KeyValue<SubString, TreeTemplateMemberNode> left, KeyValue<SubString, TreeTemplateMemberNode> right)
        {
            return left.Key.CompareTo(ref right.Key);
        }
        /// <summary>
        /// JavaScript 引号替换
        /// </summary>
        private static string replaceQuote(Match match)
        {
            if (match.Length != streamWriteCharPrefix.Length + streamWriteCharSuffix.Length + 2) return string.Empty;
            return streamWriteCharPrefix + match.Value[streamWriteCharPrefix.Length + 1] + streamWriteCharSuffix;
        }
        /// <summary>
        /// HTML 模板分割
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private unsafe static LeftArray<Range> splitTemplate(char* html, char* end)
        {
            LeftArray<Range> htmls = new LeftArray<Range>(0);
            char* last = html;
            for (char* start = html; start < end;)
            {
                if (*start == '<' && *(ulong*)start == '<' + ('!' << 16) + ((ulong)'-' << 32) + ((ulong)'-' << 48))
                {
                    if (start - last >= 5) htmls.Add(new Range(last - html, start - html));
                    last = (start += 4);
                }
                else ++start;
            }
            if (end - last >= 5) htmls.Add(new Range(last - html, end - html));
            return htmls;
        }
        /// <summary>
        /// 获取模板标签结束位置
        /// </summary>
        /// <param name="html"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        private unsafe static int getTemplateTagEnd(char* html, Range range)
        {
            for (char* start = html + (range.StartIndex + 2), end = html + range.EndIndex; start != end; ++start)
            {
                if (*start == '>' && *(uint*)(start - 2) == '-' + ('-' << 16))
                {
                    int index = (int)(start - html) - 2;
                    return (index - range.StartIndex) >= 4 ? index : -1;
                }
            }
            return -1;
        }
        /// <summary>
        /// HTML 模板指令类型集合
        /// </summary>
        private static readonly Dictionary<HashSubString, TreeTemplateCommandEnum> commands;

        static JavaScriptTreeTemplate()
        {
            commands = AutoCSer.Extensions.DictionaryCreator.CreateHashSubString<TreeTemplateCommandEnum>();
            commands.Add(TreeTemplateCommandEnum.CLINET.ToString(), TreeTemplateCommandEnum.CLINET);
            commands.Add(TreeTemplateCommandEnum.PUSH.ToString(), TreeTemplateCommandEnum.PUSH);
            commands.Add(TreeTemplateCommandEnum.LOOP.ToString(), TreeTemplateCommandEnum.LOOP);
            commands.Add(TreeTemplateCommandEnum.IF.ToString(), TreeTemplateCommandEnum.IF);
            commands.Add(TreeTemplateCommandEnum.NOT.ToString(), TreeTemplateCommandEnum.NOT);
        }
    }
}
