using AutoCSer.CodeGenerator.Metadata;
using AutoCSer.Extensions;
using System;
using System.Collections.Generic;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 树节点模板
    /// </summary>
    internal abstract class TreeTemplate
    {
        /// <summary>
        /// 模板数据视图类型
        /// </summary>
        protected Type viewType;
        /// <summary>
        /// 成员树
        /// </summary>
        internal readonly Dictionary<TreeTemplateMemberNode, Dictionary<HashSubString, TreeTemplateMemberNode>> memberPaths = AutoCSer.Extensions.DictionaryCreator.CreateOnly<TreeTemplateMemberNode, Dictionary<HashSubString, TreeTemplateMemberNode>>();
        /// <summary>
        /// 当前成员节点集合
        /// </summary>
        protected LeftArray<TreeTemplateMemberNode> currentMembers = new LeftArray<TreeTemplateMemberNode>(0);
        /// <summary>
        /// 是否 JavaScript
        /// </summary>
        internal virtual bool IsJavaScript { get { return false; } }
        /// <summary>
        /// 临时逻辑变量名称
        /// </summary>
        protected string ifName = "_if_";
        /// <summary>
        /// 错误处理委托
        /// </summary>
        protected readonly Action<string> onError;
        /// <summary>
        /// 信息处理委托
        /// </summary>
        protected readonly Action<string> onMessage;
        /// <summary>
        /// 忽略代码
        /// </summary>
        protected int ignoreCode;
        /// <summary>
        /// 忽略成员错误
        /// </summary>
        protected int ignoreMemberError;
        /// <summary>
        /// 树节点模板
        /// </summary>
        /// <param name="onError">错误处理委托</param>
        /// <param name="onMessage">消息处理委托</param>
        protected TreeTemplate(Action<string> onError, Action<string> onMessage)
        {
            this.onError = onError;
            this.onMessage = onMessage;
        }
        /// <summary>
        /// 当前代码字符串
        /// </summary>
        internal ListArray<string> Code = new ListArray<string>();
        /// <summary>
        /// 当前代码字符串常量
        /// </summary>
        protected ListArray<string> pushCodes = new ListArray<string>();
        /// <summary>
        /// 子段程序代码集合
        /// </summary>
        internal Dictionary<string, string> PartCodes = DictionaryCreator.CreateAny<string, string>();
        /// <summary>
        /// 截断代码字符串
        /// </summary>
        protected virtual void pushCode()
        {
            if (ignoreCode != 0 || pushCodes.Count == 0) return;
            string code = string.Concat(pushCodes.Array.ToArray());
            if (code.Length != 0)
            {
                Code.Append(@"
            _code_.Add(@""", code.Replace(@"""", @""""""), @""");");
            }
            pushCodes.Array.Length = 0;
        }
        /// <summary>
        /// 根据成员名称获取成员树节点
        /// </summary>
        /// <param name="memberName">成员名称</param>
        /// <param name="isDepth">是否深度搜索,false表示当前节点子节点</param>
        /// <returns>成员树节点</returns>
        protected TreeTemplateMemberNode getMember(ref SubString memberName, out bool isDepth)
        {
            int memberIndex = 0;
            while (memberIndex != memberName.Length && memberName[memberIndex] == '.') ++memberIndex;
            memberName.MoveStart(memberIndex);
            memberIndex = currentMembers.Length - memberIndex - 1;
            if (memberIndex < 0) memberIndex = 0;
            TreeTemplateMemberNode value = currentMembers[memberIndex];
            isDepth = false;
            if (memberName.Length == 0) return value;
            LeftArray<SubString> names = memberName.Split('.');
            SubString[] nameArray = names.Array;
            for (int lastIndex = names.Length - 1; memberIndex >= 0; --memberIndex)
            {
                if ((value = currentMembers[memberIndex].Get(ref nameArray[0])) != null)
                {
                    value.Path = memberIndex == 0 ? nameArray[0].ToString() : path(memberIndex, ref nameArray[0]);
                    if (names.Length != 1) isDepth = true;
                    for (int nameIndex = 1; nameIndex != names.Length; ++nameIndex)
                    {
                        if ((value = value.Get(ref nameArray[nameIndex])) == null) break;
                        value.Path = value.Parent.AwaitPath + "." + nameArray[nameIndex].ToString();
                    }
                    if (value == null) break;
                    else return value;
                }
            }
            string message = viewType.fullName() + " 未找到属性 " + currentMembers.LastOrDefault().FullPath + " . " + memberName.ToString() + " [" + memberName.String + @"]
" + new System.Diagnostics.StackTrace().ToString();
            if (checkErrorMemberName(ref memberName))
            {
                if (ignoreMemberError == 0) onMessage(message);
            }
            else if (ignoreMemberError == 0) onError(message);
            return null;
        }
        /// <summary>
        /// 检测错误成员名称
        /// </summary>
        /// <param name="memberName">成员名称</param>
        /// <returns>是否忽略错误</returns>
        protected virtual bool checkErrorMemberName(ref SubString memberName)
        {
            return false;
        }
        /// <summary>
        /// 检测成员名称
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="isClient"></param>
        protected virtual void checkMemberName(ref SubString memberName, ref bool isClient) { }
        /// <summary>
        /// 获取临时变量名称
        /// </summary>
        /// <param name="index">临时变量层次</param>
        /// <returns>变量名称</returns>
        protected string path(int index)
        {
            return "_value" + (index == 0 ? (currentMembers.Length - 1) : index).toString() + "_";
        }
        /// <summary>
        /// 获取 await 临时变量名称
        /// </summary>
        /// <returns>变量名称</returns>
        protected string awaitPath()
        {
            return "_awaitValue" + (currentMembers.Length - 1).toString() + "_";
        }
        /// <summary>
        /// 获取临时变量名称
        /// </summary>
        /// <param name="index">临时变量层次</param>
        /// <param name="name"></param>
        /// <returns>变量名称</returns>
        private string path(int index, ref SubString name)
        {
            return "_value" + (index == 0 ? (currentMembers.Length - 1) : index).toString() + "_." + name.ToString();
        }
        /// <summary>
        /// 获取循环索引临时变量名称
        /// </summary>
        /// <param name="index">临时变量层次</param>
        /// <returns>循环索引变量名称</returns>
        protected string loopIndex(int index)
        {
            return "_loopIndex" + (index == 0 ? (currentMembers.Length - 1) : index).toString() + "_";
        }
        /// <summary>
        /// 添加代码
        /// </summary>
        /// <param name="code">代码</param>
        protected virtual void pushCode(SubString code)
        {
            if (ignoreCode == 0) pushCodes.Add(code);
        }
        /// <summary>
        /// 输出绑定的数据
        /// </summary>
        /// <param name="member">成员节点</param>
        protected void at(TreeTemplateMemberNode member)
        {
            pushCode();
            if (ignoreCode != 0) return;
            if (member.Type.IsString)
            {
                Code.Append(@"
            _code_.Add(", member.AwaitPath, ");");
                return;
            }
            if (member.Type.IsBool && member.Type.IsStruct)
            {
                Code.Append(@"
            _code_.Add(", member.AwaitPath, @" ? ""true"" : ""false"");");
                return;
            }
            Code.Append(@"
            _code_.Add(", member.AwaitPath, ".ToString());");
        }
        /// <summary>
        /// if代码段处理
        /// </summary>
        /// <param name="member">成员节点</param>
        /// <param name="memberName">成员名称</param>
        /// <param name="isDepth">是否深度搜索</param>
        /// <param name="doMember">成员处理函数</param>
        protected void ifThen(TreeTemplateMemberNode member, ref SubString memberName, bool isDepth, Action<TreeTemplateMemberNode> doMember)
        {
            if (isDepth)
            {
                pushCode();
                LeftArray<SubString> names = splitMemberName(ref memberName);
                for (int index = 0; index != names.Length - 1; ++index) ifStart(ref names.Array[index], false);
                doMember(getMember(ref names.Array[names.Length - 1], out isDepth));
                pushCode();
                for (int index = 0; index != names.Length - 1; ++index) ifEnd(true);
            }
            else doMember(member);
        }
        /// <summary>
        /// if开始代码段
        /// </summary>
        /// <param name="memberName">成员名称</param>
        /// <param name="isSkip">是否跳跃层次</param>
        protected void ifStart(ref SubString memberName, bool isSkip)
        {
            bool isDepth;
            TreeTemplateMemberNode member = getMember(ref memberName, out isDepth);
            currentMembers.Add(member);
            if (isSkip) currentMembers.Add(member);
            string name = path(0);
            if (ignoreCode == 0)
            {
                Code.Append(@"
                {
                    ", member.Type.FullName, " ", name, " = ", member.AwaitPath, ";");
            }
            ifStart(member.Type, name, null);
        }
        /// <summary>
        /// if开始代码段
        /// </summary>
        /// <param name="type">成员类型</param>
        /// <param name="name">成员路径名称</param>
        /// <param name="ifName">if临时变量名称</param>
        internal void ifStart(ExtensionType type, string name, string ifName)
        {
            if (ignoreCode != 0) return;
            Code.Append(@"
                    if (", name);
            if (!type.IsBool) Code.Append(" != default(", type.FullName, ")");
            Code.Add(@")
                    {");
            if (ifName == null) return;
            Code.Append(@"
                        ", ifName, " = true;");
        }
        /// <summary>
        /// if结束代码段
        /// </summary>
        /// <param name="isMember">是否删除成员节点</param>
        protected void ifEnd(bool isMember)
        {
            if (isMember) --currentMembers.Length;
            if (ignoreCode != 0) return;
            Code.Append(@"
                    }
                }");
        }

        /// <summary>
        /// 分解成员名称
        /// </summary>
        /// <param name="memberName">成员名称</param>
        /// <returns>成员名称集合</returns>
        protected static LeftArray<SubString> splitMemberName(ref SubString memberName)
        {
            int memberIndex = 0;
            while (memberIndex != memberName.Length && memberName[memberIndex] == '.') ++memberIndex;
            LeftArray<SubString> names = memberName.GetSub(memberIndex).Split('.');
            names.Array[0].MoveStart(-memberIndex);
            return names;
        }
    }
    /// <summary>
    /// 树节点模板
    /// </summary>
    /// <typeparam name="T">树节点类型</typeparam>
    internal abstract class TreeTemplate<T> : TreeTemplate
        where T : ITreeTemplateNode<T>
    {
        /// <summary>
        /// 模板command+解析器
        /// </summary>
        protected readonly Dictionary<string, Action<T>> creators = DictionaryCreator.CreateAny<string, Action<T>>();
        /// <summary>
        /// 引用代码树节点
        /// </summary>
        protected readonly Dictionary<HashSubString, T> nameNodes = AutoCSer.Extensions.DictionaryCreator.CreateHashSubString<T>();
        /// <summary>
        /// 树节点模板
        /// </summary>
        /// <param name="type">模板数据视图</param>
        /// <param name="onError">错误处理委托</param>
        /// <param name="onMessage">消息处理委托</param>
        protected TreeTemplate(Type type, Action<string> onError, Action<string> onMessage) : base(onError, onMessage)
        {
            SubString name = default(SubString);
            currentMembers.Add(new TreeTemplateMemberNode(this, viewType = type ?? GetType(), ref name, TreeTemplateMemberNode.ThisPath));
        }
        /// <summary>
        /// 添加代码树节点
        /// </summary>
        /// <param name="boot">代码树节点</param>
        protected virtual void skin(T boot)
        {
            Action<T> creator;
            foreach (T node in boot.Childs)
            {
                string command = node.TemplateCommand;
                if (command == null) pushCode(node.TemplateCode);
                else if (creators.TryGetValue(command, out creator)) creator(node);
                else onError(viewType.fullName() + " 未找到命名处理函数 " + command + " : " + node.TemplateMemberName.ToString());
            }
        }
        /// <summary>
        /// 注释处理
        /// </summary>
        /// <param name="node">代码树节点</param>
        protected void note(T node) { }
        /// <summary>
        /// 输出绑定的数据
        /// </summary>
        /// <param name="node">代码树节点</param>
        protected virtual void at(T node)
        {
            bool isDepth;
            SubString memberName = node.TemplateMemberName;
            TreeTemplateMemberNode member = getMember(ref memberName, out isDepth);
            if (member != null) ifThen(member, ref memberName, isDepth, value => at(value));
        }
        /// <summary>
        /// 子代码段处理
        /// </summary>
        /// <param name="node">代码树节点</param>
        protected virtual void push(T node)
        {
            bool isDepth = false, isClient = false;
            TreeTemplateMemberNode member = null;
            SubString memberName = node.TemplateMemberNameBeforeAt;
            checkMemberName(ref memberName, ref isClient);
            if (isClient) ++ignoreMemberError;
            if ((member = getMember(ref memberName, out isDepth)) != null && node.ChildCount != 0)
            {
                if (isClient) ++ignoreCode;
                pushCode();
                string name = path(currentMembers.Length);
                if (ignoreCode == 0)
                {
                    Code.Append(@"
                {
                    ", member.Type.FullName, " ", name, " = default(", member.Type.FullName, ");");
                }
                if (isDepth)
                {
                    LeftArray<SubString> names = splitMemberName(ref memberName);
                    ifStart(ref names.Array[0], true);
                    for (int index = 1; index != names.Length - 1; ++index) ifStart(ref names.Array[index], false);
                    push(getMember(ref names.Array[names.Length - 1], out isDepth), node, name, names.Length - 1);
                }
                else push(member, node, name, 0);
                if (ignoreCode == 0)
                {
                    Code.Append(@"
                }");
                }
                if (isClient) --ignoreCode;
            }
            if (isClient) --ignoreMemberError;
        }
        /// <summary>
        /// 子代码段处理
        /// </summary>
        /// <param name="member">成员节点</param>
        /// <param name="node">代码树节点</param>
        /// <param name="name">成员路径名称</param>
        /// <param name="popCount">删除成员节点数量</param>
        protected void push(TreeTemplateMemberNode member, T node, string name, int popCount)
        {
            if (ignoreCode == 0)
            {
                Code.Append(@"
                    ", name, " = ", member.AwaitPath, ";");
            }
            if (popCount != 0) --currentMembers.Length;
            while (popCount != 0)
            {
                ifEnd(true);
                --popCount;
            }
            currentMembers.Add(member);
            if (ignoreCode == 0)
            {
                Code.Append(@"
            ", ifName, " = false;");
            }
            ifThen(node, member.Type, name, ifName, false, 0, false);
            --currentMembers.Length;
        }
        /// <summary>
        /// if代码段处理
        /// </summary>
        /// <param name="node">代码树节点</param>
        /// <param name="type">成员类型</param>
        /// <param name="name">成员路径名称</param>
        /// <param name="ifName">逻辑变量名称</param>
        /// <param name="isMember">是否删除当前成员节点</param>
        /// <param name="popCount">删除成员节点数量</param>
        /// <param name="isNot"></param>
        internal void ifThen(T node, ExtensionType type, string name, string ifName, bool isMember, int popCount, bool isNot)
        {
            ifOr(type, name, ifName, isMember, popCount);
            ifEnd(node, isNot);
        }
        /// <summary>
        /// if代码段处理
        /// </summary>
        /// <param name="type">成员类型</param>
        /// <param name="name">成员路径名称</param>
        /// <param name="ifName">逻辑变量名称</param>
        /// <param name="isMember">是否删除当前成员节点</param>
        /// <param name="popCount">删除成员节点数量</param>
        private void ifOr(ExtensionType type, string name, string ifName, bool isMember, int popCount)
        {
            ifStart(type, name, ifName);
            while (popCount != 0)
            {
                ifEnd(true);
                --popCount;
            }
            if (isMember) --currentMembers.Length;
            if (ignoreCode != 0) return;
            Code.Append(@"
                }");
        }
        /// <summary>
        /// if条件判断结束
        /// </summary>
        /// <param name="node"></param>
        /// <param name="isNot"></param>
        private void ifEnd(T node, bool isNot)
        {
            if (ignoreCode == 0)
            {
                Code.Append(@"
            if (", isNot ? "!" : string.Empty, ifName, @")
            {");
            }
            skin(node);
            pushCode();
            if (ignoreCode != 0) return;
            Code.Append(@"
            }");
        }
        /// <summary>
        /// 绑定的数据为true非0非null时输出代码
        /// </summary>
        /// <param name="node">代码树节点</param>
        protected void ifThen(T node)
        {
            SubString memberName = node.TemplateMemberNameBeforeAt;
            if (memberName.IndexOf('|') == -1)
            {
                if (memberName.IndexOf('&') == -1)
                {
                    string value = null;
                    int valueIndex = memberName.IndexOf('=');
                    if (valueIndex != -1)
                    {
                        value = memberName.GetSub(valueIndex + 1);
                        memberName.Sub(0, valueIndex);
                    }
                    TreeTemplateMemberNode member = null;
                    bool isDepth = false, isClient = false, isNot = false;
                    if (memberName.Length != 0 && memberName[0] == '!')
                    {
                        isNot = true;
                        memberName.MoveStart(1);
                    }
                    checkMemberName(ref memberName, ref isClient);
                    //if (isClient) ++ignoreMemberError;
                    if ((member = getMember(ref memberName, out isDepth)) == null)
                    {
                        //if (isClient) --ignoreMemberError;
                        return;
                    }
                    if (isClient) ++ignoreCode;
                    pushCode();
                    if (ignoreCode == 0)
                    {
                        Code.Append(@"
            ", ifName, " = false;");
                    }
                    if (isDepth)
                    {
                        LeftArray<SubString> names = splitMemberName(ref memberName);
                        for (int index = 0; index != names.Length - 1; ++index) ifStart(ref names.Array[index], false);
                        ifThen(getMember(ref names.Array[names.Length - 1], out isDepth), node, value, ifName, names.Length - 1, isNot);
                    }
                    else ifThen(member, node, value, ifName, 0, isNot);
                    if (isClient)
                    {
                        --ignoreCode;
                        //--ignoreMemberError;
                    }
                }
                else ifThen(node, ref memberName, true);
            }
            else ifThen(node, ref memberName, false);
        }
        /// <summary>
        /// if代码段处理
        /// </summary>
        /// <param name="member">成员节点</param>
        /// <param name="node">代码树节点</param>
        /// <param name="value">匹配值</param>
        /// <param name="ifName">逻辑变量名称</param>
        /// <param name="popCount">删除成员节点数量</param>
        /// <param name="isNot"></param>
        protected void ifThen(TreeTemplateMemberNode member, T node, string value, string ifName, int popCount, bool isNot)
        {
            if (value == null) ifThen(node, member.Type, member.AwaitPath, ifName, false, popCount, isNot);
            else ifThen(node, member.Type, member.AwaitPath, value, ifName, popCount, isNot);
        }
        /// <summary>
        /// if代码段处理
        /// </summary>
        /// <param name="node">代码树节点</param>
        /// <param name="type">成员类型</param>
        /// <param name="name">成员路径名称</param>
        /// <param name="value">匹配值</param>
        /// <param name="ifName">逻辑变量名称</param>
        /// <param name="popCount">删除成员节点数量</param>
        /// <param name="isNot"></param>
        internal void ifThen(T node, ExtensionType type, string name, string value, string ifName, int popCount, bool isNot)
        {
            ifOr(type, name, value, ifName, popCount);
            ifEnd(node, isNot);
        }
        /// <summary>
        /// if代码段处理
        /// </summary>
        /// <param name="type">成员类型</param>
        /// <param name="name">成员路径名称</param>
        /// <param name="value">匹配值</param>
        /// <param name="ifName">逻辑变量名称</param>
        /// <param name="popCount">删除成员节点数量</param>
        private void ifOr(ExtensionType type, string name, string value, string ifName, int popCount)
        {
            if (ignoreCode == 0)
            {
                if (type.IsStruct || type.Type.IsEnum)
                {
                    Code.Append(@"
                if (", name, @".ToString() == @""", value.Replace(@"""", @""""""), @""")");
                }
                else
                {
                    Code.Append(@"
                if (", name, @" != null && ", name, @".ToString() == @""", value.Replace(@"""", @""""""), @""")");
                }
                Code.Append(@"
                {
                    ", ifName, @" = true;
                }");
            }
            while (popCount != 0)
            {
                ifEnd(true);
                --popCount;
            }
        }
        /// <summary>
        /// if代码段处理
        /// </summary>
        /// <param name="node"></param>
        /// <param name="memberName"></param>
        /// <param name="isAnd"></param>
        private void ifThen(T node, ref SubString memberName, bool isAnd)
        {
            pushCode();
            if (ignoreCode == 0)
            {
                Code.Append(@"
                ", ifName, " = false;");
            }
            byte isNext = 0;
            bool isNot = false;
            foreach (SubString subMemberName in memberName.Split(isAnd ? '&' : '|'))
            {
                if (isNext == 0)
                {
                    isNot = ifOr(subMemberName);
                    isNext = 1;
                }
                else
                {
                    if (ignoreCode == 0)
                    {
                        if (isAnd ^ isNot)
                        {
                            Code.Append(@"
            if (", ifName, @")
            {
                ", ifName, " = false;");
                        }
                        else
                        {
                            Code.Append(@"
            if (!", ifName, @")
            {");
                        }
                    }
                    isNot = ifOr(subMemberName);
                    if (ignoreCode == 0)
                    {
                        Code.Append(@"
            }");
                    }
                }
            }
            ifEnd(node, isNot);
        }
        /// <summary>
        /// if多条件OR
        /// </summary>
        /// <param name="subMemberName"></param>
        /// <returns>是否需要取反</returns>
        private bool ifOr(SubString subMemberName)
        {
            string value = null;
            SubString memberName;
            bool isDepth = false, isClient = false, isNot = false;
            if (subMemberName.Length != 0 && subMemberName[0] == '!')
            {
                subMemberName.MoveStart(1);
                isNot = true;
            }
            int valueIndex = subMemberName.IndexOf('=');
            if (valueIndex != -1)
            {
                value = subMemberName.GetSub(valueIndex + 1);
                memberName = subMemberName.GetSub(0, valueIndex);
            }
            else memberName = subMemberName;
            TreeTemplateMemberNode member = null;
            checkMemberName(ref memberName, ref isClient);
            if (isClient) ++ignoreMemberError;
            if ((member = getMember(ref memberName, out isDepth)) == null)
            {
                if (isClient) --ignoreMemberError;
                return isNot;
            }
            if (isClient) ++ignoreCode;
            if (isDepth)
            {
                LeftArray<SubString> names = splitMemberName(ref memberName);
                for (int index = 0; index != names.Length - 1; ++index) ifStart(ref names.Array[index], false);
                ifOr(getMember(ref names.Array[names.Length - 1], out isDepth), value, ifName, names.Length - 1);
            }
            else ifOr(member, value, ifName, 0);
            if (isClient)
            {
                --ignoreCode;
                --ignoreMemberError;
            }
            return isNot;
        }
        /// <summary>
        /// if代码段处理
        /// </summary>
        /// <param name="member">成员节点</param>
        /// <param name="value">匹配值</param>
        /// <param name="ifName">逻辑变量名称</param>
        /// <param name="popCount">删除成员节点数量</param>
        private void ifOr(TreeTemplateMemberNode member, string value, string ifName, int popCount)
        {
            if (value == null) ifOr(member.Type, member.AwaitPath, ifName, false, popCount);
            else ifOr(member.Type, member.AwaitPath, value, ifName, popCount);
        }
        /// <summary>
        /// 绑定的数据为false或者0或者null时输出代码
        /// </summary>
        /// <param name="node">代码树节点</param>
        protected virtual void not(T node)
        {
            SubString memberName = node.TemplateMemberNameBeforeAt;
            if (memberName.IndexOf('|') == -1)
            {
                if (memberName.IndexOf('&') == -1)
                {
                    string value = null;
                    int valueIndex = memberName.IndexOf('=');
                    if (valueIndex != -1)
                    {
                        value = memberName.GetSub(valueIndex + 1);
                        memberName.Sub(0, valueIndex);
                    }
                    TreeTemplateMemberNode member = null;
                    bool isDepth = false, isClient = false, isNot = false;
                    if (memberName.Length != 0 && memberName[0] == '!')
                    {
                        isNot = true;
                        memberName.MoveStart(1);
                    }
                    checkMemberName(ref memberName, ref isClient);
                    //if (isClient) ++ignoreMemberError;
                    if ((member = getMember(ref memberName, out isDepth)) == null)
                    {
                        //if (isClient) --ignoreMemberError;
                        return;
                    }
                    if (isClient) ++ignoreCode;
                    pushCode();
                    if (ignoreCode == 0)
                    {
                        Code.Append(@"
            ", ifName, " = false;");

                    }
                    if (isDepth)
                    {
                        LeftArray<SubString> names = splitMemberName(ref memberName);
                        for (int index = 0; index != names.Length - 1; ++index) ifStart(ref names.Array[index], false);
                        not(getMember(ref names.Array[names.Length - 1], out isDepth), node, value, ifName, names.Length - 1, isNot);
                    }
                    else not(member, node, value, ifName, 0, isNot);
                    if (isClient)
                    {
                        --ignoreCode;
                        //--ignoreMemberError;
                    }
                }
                else not(node, ref memberName, true);
            }
            else not(node, ref memberName, false);
        }
        /// <summary>
        /// not代码段处理
        /// </summary>
        /// <param name="member">成员节点</param>
        /// <param name="node">代码树节点</param>
        /// <param name="value">匹配值</param>
        /// <param name="ifName">逻辑变量名称</param>
        /// <param name="popCount">删除成员节点数量</param>
        /// <param name="isNot"></param>
        protected void not(TreeTemplateMemberNode member, T node, string value, string ifName, int popCount, bool isNot)
        {
            notOr(member, value, ifName, popCount);
            ifEnd(node, isNot);
        }
        /// <summary>
        /// not代码段处理
        /// </summary>
        /// <param name="member">成员节点</param>
        /// <param name="value">匹配值</param>
        /// <param name="ifName">逻辑变量名称</param>
        /// <param name="popCount">删除成员节点数量</param>
        private void notOr(TreeTemplateMemberNode member, string value, string ifName, int popCount)
        {
            if (ignoreCode == 0)
            {
                if (member.Type.IsBool)
                {
                    Code.Append(@"
                if (!(bool)", member.AwaitPath, ")");
                }
                else
                {
                    if (member.Type.IsStruct || member.Type.Type.IsEnum)
                    {
                        if (value != null)
                        {
                            Code.Append(@"
                if (", member.AwaitPath, @".ToString() != @""", value.Replace(@"""", @""""""), @""")");
                        }
                        else
                        {
                            Code.Append(@"
                if (", member.AwaitPath, " == default(", member.Type.FullName, "))");
                        }
                    }
                    else if (value != null)
                    {
                        string memberName = path(0);
                        Code.Append(@"
                ", member.Type.FullName, " ", memberName, " = ", member.AwaitPath, @";
                if (", memberName, @" == default(", member.Type.FullName, ") || ", memberName, @".ToString() != @""", value.Replace(@"""", @""""""), @""")");
                    }
                    else
                    {
                        Code.Append(@"
                if (", member.AwaitPath, " == default(", member.Type.FullName, "))");
                    }
                }
                Code.Append(@"
                {
                    ", ifName, @" = true;
                }");
            }
            while (popCount != 0)
            {
                ifEnd(true);
                --popCount;
            }
        }
        /// <summary>
        /// not代码段处理
        /// </summary>
        /// <param name="node"></param>
        /// <param name="memberName"></param>
        /// <param name="isAnd"></param>
        private void not(T node, ref SubString memberName, bool isAnd)
        {
            pushCode();
            if (ignoreCode == 0)
            {
                Code.Append(@"
                ", ifName, " = false;");
            }
            byte isNext = 0;
            bool isNot = false;
            foreach (SubString subMemberName in memberName.Split(isAnd ? '&' : '|'))
            {
                if (isNext == 0)
                {
                    isNot = notOr(subMemberName);
                    isNext = 1;
                }
                else
                {
                    if (ignoreCode == 0)
                    {
                        if (isAnd ^ isNot)
                        {
                            Code.Append(@"
            if (", ifName, @")
            {
                ", ifName, " = false;");
                        }
                        else
                        {
                            Code.Append(@"
            if (!", ifName, @")
            {");
                        }
                    }
                    isNot = notOr(subMemberName);
                    if (ignoreCode == 0)
                    {
                        Code.Append(@"
            }");
                    }
                }
            }
            ifEnd(node, isNot);
        }
        /// <summary>
        /// not多条件OR
        /// </summary>
        /// <param name="subMemberName"></param>
        private bool notOr(SubString subMemberName)
        {
            string value = null;
            SubString memberName;
            bool isDepth = false, isClient = false, isNot = false;
            if (subMemberName.Length != 0 && subMemberName[0] == '!')
            {
                subMemberName.MoveStart(1);
                isNot = true;
            }
            int valueIndex = subMemberName.IndexOf('=');
            if (valueIndex != -1)
            {
                value = subMemberName.GetSub(valueIndex + 1);
                memberName = subMemberName.GetSub(0, valueIndex);
            }
            else memberName = subMemberName;
            TreeTemplateMemberNode member = null;
            checkMemberName(ref memberName, ref isClient);
            if (isClient) ++ignoreMemberError;
            if ((member = getMember(ref memberName, out isDepth)) == null)
            {
                if (isClient) --ignoreMemberError;
                return isNot;
            }
            if (isClient) ++ignoreCode;
            if (isDepth)
            {
                LeftArray<SubString> names = splitMemberName(ref memberName);
                for (int index = 0; index != names.Length - 1; ++index) ifStart(ref names.Array[index], false);
                notOr(getMember(ref names.Array[names.Length - 1], out isDepth), value, ifName, names.Length - 1);
            }
            else notOr(member, value, ifName, 0);
            if (isClient)
            {
                --ignoreCode;
                --ignoreMemberError;
            }
            return isNot;
        }
        /// <summary>
        /// 循环处理
        /// </summary>
        /// <param name="node">代码树节点</param>
        protected virtual void loop(T node)
        {
            bool isDepth = false, isClient = false;
            TreeTemplateMemberNode member = null;
            SubString memberName = node.TemplateMemberNameBeforeAt;
            checkMemberName(ref memberName, ref isClient);
            if (isClient) ++ignoreMemberError;
            member = getMember(ref memberName, out isDepth);
            if (member == null)
            {
                if (isClient) --ignoreMemberError;
                return;
            }
            pushCode();
            string name = path(currentMembers.Length);
            int codeCount = Code.Array.Length;
            bool isLoop;
            if (isDepth)
            {
                if (ignoreCode == 0)
                {
                    Code.Append(@"
                {
                    ", member.Type.FullName, " ", name, " = default(", member.Type.FullName, ");");
                }
                LeftArray<SubString> names = splitMemberName(ref memberName);
                ifStart(ref names.Array[0], true);
                for (int index = 1; index != names.Length - 1; ++index) ifStart(ref names.Array[index], false);
                isLoop = loop(getMember(ref names.Array[names.Length - 1], out isDepth), node, name, names.Length - 1);
            }
            else
            {
                if (ignoreCode == 0)
                {
                    Code.Append(@"
                {
                    ", member.Type.FullName, " ", name, ";");
                }
                isLoop = loop(member, node, name, 0);
            }
            if (ignoreCode == 0)
            {
                if (isLoop)
                {
                    Code.Append(@"
                }");
                }
                else
                {
                    Code.Array.Length = codeCount;
                    pushCodes.Array.Length = 0;
                }
            }
            if (isClient) --ignoreMemberError;
        }
        /// <summary>
        /// 循环处理
        /// </summary>
        /// <param name="member">成员节点</param>
        /// <param name="node">代码树节点</param>
        /// <param name="name">成员路径名称</param>
        /// <param name="popCount">删除成员节点数量</param>
        /// <returns>是否正常生成代码</returns>
        protected bool loop(TreeTemplateMemberNode member, T node, string name, int popCount)
        {
            ExtensionType enumerableType = member.Type.EnumerableType;
            if (enumerableType != null && ignoreCode == 0)
            {
                Code.Append(@"
                    ", name, " = ", member.AwaitPath, ";");
            }
            if (popCount != 0) --currentMembers.Length;
            while (popCount != 0)
            {
                ifEnd(true);
                --popCount;
            }
            if (enumerableType == null)
            {
                if (ignoreMemberError == 0) onError(viewType.fullName() + " 属性不可枚举 " + currentMembers.LastOrDefault().FullPath);
                return false;
            }
            currentMembers.Add(member);
            string valueName = path(currentMembers.Length);
            if (ignoreCode == 0)
            {
                if (!member.Type.Type.IsValueType)
                {
                    Code.Append(@"
                    if (", name, @" != null)");
                }
                Code.Append(@"
                    {
                        int ", loopIndex(0), @" = _loopIndex_;");
                Code.Append(@"
                        _loopIndex_ = 0;
                        foreach (", member.Type.EnumerableArgumentType.FullName, " " + valueName + " in ", name, @")
                        {");
            }
            TreeTemplateMemberNode loopMember = member.Get(string.Empty);
            loopMember.Path = valueName;
            currentMembers.Add(loopMember);
            skin(node);
            --currentMembers.Length;
            pushCode();
            if (ignoreCode == 0)
            {
                Code.Append(@"
                            ++_loopIndex_;
                        }
                        _loopIndex_ = ", loopIndex(0), @";");
                Code.Append(@"
                    }");
            }
            --currentMembers.Length;
            return true;
        }
        /// <summary>
        /// 子段模板处理
        /// </summary>
        /// <param name="node">代码树节点</param>
        protected void name(T node)
        {
            HashSubString nameKey = node.TemplateMemberName;
            if (nameNodes.ContainsKey(nameKey)) onError(viewType.fullName() + " NAME " + nameKey.ToString() + " 重复定义");
            nameNodes[nameKey] = node;
            if (node.ChildCount != 0) skin(node);
        }
        /// <summary>
        /// 引用子段模板处理
        /// </summary>
        /// <param name="node">代码树节点</param>
        protected void fromName(T node)
        {
            SubString memberName = node.TemplateMemberName;
            int typeIndex = memberName.IndexOf('.');
            if (typeIndex == -1)
            {
                if (!nameNodes.TryGetValue(memberName, out node)) onError(viewType.fullName() + " NAME " + memberName.ToString() + " 未定义");
            }
            else
            {
                SubString name = memberName.GetSub(++typeIndex);
                node = fromNameNode(memberName.GetSub(0, typeIndex), ref name);
            }
            if (node != null && node.ChildCount != 0) skin(node);
        }
        /// <summary>
        /// 根据类型名称获取子段模板
        /// </summary>
        /// <param name="fileName">模板文件名称</param>
        /// <param name="name">子段模板名称</param>
        /// <returns>子段模板</returns>
        protected virtual T fromNameNode(string fileName, ref SubString name)
        {
            return default(T);
        }
        /// <summary>
        /// 子段程序代码处理
        /// </summary>
        /// <param name="node">代码树节点</param>
        protected void part(T node)
        {
            string memberName = node.TemplateMemberName;
            pushCode();
            Code.Add(@"
            ListArray<string> _PART_" + memberName + @"_ = _code_;
            _code_ = new ListArray<string>();");
            ListArray<string> historyCode = Code;
            Code = new ListArray<string>();
            skin(node);
            pushCode();
            string partCode = string.Concat(Code.Array.ToArray());
            PartCodes[memberName] = partCode;
            Code = historyCode;
            Code.Add(partCode);
            Code.Add(@"
            _partCodes_[""" + memberName + @"""] = string.Concat(_code_.Array.ToArray());
            _code_ = _PART_" + memberName + @"_;
            _code_.Add(_partCodes_[""" + memberName + @"""]);");
        }
    }
}
