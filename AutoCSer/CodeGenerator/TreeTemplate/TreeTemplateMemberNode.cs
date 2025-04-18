using AutoCSer.CodeGenerator.Metadata;
using AutoCSer.Extensions;
using AutoCSer.Metadata;
using AutoCSer.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
#if !DotNet45 && !AOT
using AutoCSer.NetCoreWeb;
#endif

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 成员树节点
    /// </summary>
    internal sealed class TreeTemplateMemberNode
    {
        /// <summary>
        /// 成员信息缓存集合
        /// </summary>
        private static Dictionary<HashObject<System.Type>, Dictionary<HashSubString, AutoCSer.Metadata.MemberIndexInfo>> memberCache = DictionaryCreator.CreateHashObject<System.Type, Dictionary<HashSubString, AutoCSer.Metadata.MemberIndexInfo>>();
        /// <summary>
        /// 当前对象节点路径
        /// </summary>
        internal const string ThisPath = "this";

        /// <summary>
        /// 树节点模板
        /// </summary>
        private readonly TreeTemplate template;
        /// <summary>
        /// 父节点成员
        /// </summary>
        internal TreeTemplateMemberNode Parent;
        /// <summary>
        /// 成员类型
        /// </summary>
        internal ExtensionType Type { get; private set; }
#if !DotNet45 && !AOT
        /// <summary>
        /// await 成员原类型
        /// </summary>
        internal readonly ExtensionType AwaitType;
        /// <summary>
        /// 默认为 false 表示不忽略默认值输出，基于 IEquatable{T}
        /// </summary>
        internal readonly bool IsIgnoreDefault;
#endif
        /// <summary>
        /// 当前节点成员名称
        /// </summary>
        private readonly SubString name;
        /// <summary>
        /// 节点路径
        /// </summary>
        internal string Path;
        /// <summary>
        /// 节点路径
        /// </summary>
        internal string AwaitPath
        {
            get
            {
#if DotNet45 || AOT
                return Path;
#else
                return AwaitType != null ? "(await " + Path + ")" : Path;
#endif
            }
        }
        /// <summary>
        /// 节点路径全称
        /// </summary>
        internal string FullPath
        {
            get
            {
                if (Parent != null)
                {
                    LeftArray<string> path = new LeftArray<string>(0);
                    for (TreeTemplateMemberNode member = this; member.Parent != null; member = member.Parent) path.Add(member.name);
                    path.Reverse();
                    return string.Join(".", path);
                }
                return null;
            }
        }
        /// <summary>
        /// 节点路径上是否有下级路径
        /// </summary>
        internal bool IsNextPath
        {
            get
            {
                Dictionary<HashSubString, TreeTemplateMemberNode> paths;
                return template.memberPaths.TryGetValue(this, out paths) && paths.Count != 0;
            }
        }
        /// <summary>
        /// 成员树节点
        /// </summary>
        /// <param name="template">树节点模板</param>
        /// <param name="type">成员类型</param>
        /// <param name="name">当前节点成员名称</param>
        /// <param name="path">当前节点成员名称</param>
#if !DotNet45 && !AOT
        /// <param name="viewMemberAttribute">数据视图成员自定义属性</param>
#endif
        internal TreeTemplateMemberNode(TreeTemplate template, ExtensionType type, ref SubString name, string path
#if !DotNet45 && !AOT
        , ViewMemberAttribute viewMemberAttribute = null
#endif
            )
        {
            this.template = template;
            this.Type = type;
            this.name = name;
            Path = path;
#if !DotNet45 && !AOT
            if (template.IsJavaScript)
            {
                Type awaitResultType = TypeHelpView.GetAwaitResultType(type.Type);
                if (awaitResultType != null)
                {
                    AwaitType = type;
                    Type = awaitResultType;
                }
                if (viewMemberAttribute == null) viewMemberAttribute = ViewMemberAttribute.Default;
                IsIgnoreDefault = viewMemberAttribute.IsIgnoreDefault && typeof(IEquatable<>).MakeGenericType(type.Type).IsAssignableFrom(type.Type);
                if (viewMemberAttribute.IsAllMember)
                {
                    foreach (MemberIndexInfo member in getMembers(Type.Type).Values)
                    {
                        if (!member.IsIgnore)
                        {
                            viewMemberAttribute = member.GetAttribute<ViewMemberAttribute>(true);
                            if (viewMemberAttribute == null || !viewMemberAttribute.GetIsIgnoreCurrent)
                            {
                                SubString memberName = member.Member.Name;
                                Get(ref memberName);
                            }
                        }
                    }
                }
                else
                {
                    foreach (MemberIndexInfo member in getMembers(Type.Type).Values)
                    {
                        if (!member.IsIgnore)
                        {
                            viewMemberAttribute = member.GetAttribute<ViewMemberAttribute>(true);
                            if (viewMemberAttribute != null && viewMemberAttribute.IsAuto && !viewMemberAttribute.GetIsIgnoreCurrent)
                            {
                                SubString memberName = member.Member.Name;
                                Get(ref memberName);
                                if (!string.IsNullOrEmpty(viewMemberAttribute.BindingName))
                                {
                                    memberName = viewMemberAttribute.BindingName;
                                    Get(ref memberName);
                                }
                            }
                        }
                    }
                }
            }
#endif
        }
        /// <summary>
        /// 根据成员名称获取子节点成员
        /// </summary>
        /// <param name="name">成员名称</param>
        /// <returns>子节点成员</returns>
        internal TreeTemplateMemberNode Get(ref SubString name)
        {
            Dictionary<HashSubString, TreeTemplateMemberNode> paths;
            if (!template.memberPaths.TryGetValue(this, out paths))
            {
                template.memberPaths[this] = paths = AutoCSer.Extensions.DictionaryCreator.CreateHashSubString<TreeTemplateMemberNode>();
            }
            TreeTemplateMemberNode value;
            HashSubString hashName = name;
            if (paths.TryGetValue(hashName, out value)) return value;
            bool isPath = true;
            if (name.Length != 0)
            {
                AutoCSer.Metadata.MemberIndexInfo member;
                if (getMembers(Type.Type).TryGetValue(name, out member))
                {
                    if (member.IsIgnore) isPath = false;
#if DotNet45 || AOT
                    value = new TreeTemplateMemberNode(template, member.TemplateMemberType, ref name, null);
#else
                    ViewMemberAttribute viewMemberAttribute = null;
                    if (template.IsJavaScript)
                    {
                        viewMemberAttribute = member.GetAttribute<ViewMemberAttribute>(false) ?? ViewMemberAttribute.Default;
                        if (!string.IsNullOrEmpty(viewMemberAttribute.BindingName))
                        {
                            SubString bindingName = viewMemberAttribute.BindingName;
                            Get(ref bindingName);
                        }
                        if (viewMemberAttribute.GetIsIgnoreCurrent) isPath = false;
                    }
                    value = new TreeTemplateMemberNode(template, member.TemplateMemberType, ref name, null, viewMemberAttribute);
#endif
                }
                //if (value == null && isLast && template.IsJavaScript && name.Equals("length"))
                //{
                //    if (Type.Type.IsArray) name = nameof(Array.Length);
                //    else if (typeof(ICollection).IsAssignableFrom(Type.Type)) name = nameof(ICollection.Count);
                //    else name.SetEmpty();
                //    if (name.Length != 0 && getMembers(Type.Type).TryGetValue(name, out member)) value = new TreeTemplateMemberNode(template, member.MemberSystemType, ref name, null);
                //}
            }
            else
            {
                SubString nullName = default(SubString);
                value = new TreeTemplateMemberNode(template, Type.EnumerableArgumentType, ref nullName, null);
            }
            if (value != null)
            {
                value.Parent = this;
                if (isPath) paths[hashName] = value;
            }
            return value;
        }
        /// <summary>
        /// 根据成员名称获取子节点成员
        /// </summary>
        /// <param name="name">成员名称</param>
        /// <returns>子节点成员</returns>
        internal TreeTemplateMemberNode Get(string name)
        {
            SubString subName = name;
            return Get(ref subName);
        }
        /// <summary>
        /// 根据成员名称获取子节点成员
        /// </summary>
        /// <param name="name">成员名称</param>
        /// <returns>子节点成员</returns>
        internal TreeTemplateMemberNode GetOnly(ref SubString name)
        {
            Dictionary<HashSubString, TreeTemplateMemberNode> paths;
            if (template.memberPaths.TryGetValue(this, out paths))
            {
                TreeTemplateMemberNode value;
                if (paths.TryGetValue(name, out value)) return value;
            }
            return null;
        }
        /// <summary>
        /// 根据成员名称获取子节点成员
        /// </summary>
        /// <param name="name">成员名称</param>
        /// <returns>子节点成员</returns>
        internal TreeTemplateMemberNode GetOnly(string name)
        {
            SubString subName = name;
            return GetOnly(ref subName);
        }

        /// <summary>
        /// 获取成员集合
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Dictionary<HashSubString, AutoCSer.Metadata.MemberIndexInfo> getMembers(Type type)
        {
            Dictionary<HashSubString, AutoCSer.Metadata.MemberIndexInfo> values;
            if (memberCache.TryGetValue(type, out values)) return values;
            memberCache[type] = values = AutoCSer.Extensions.DictionaryCreator.CreateHashSubString<AutoCSer.Metadata.MemberIndexInfo>();
            foreach (AutoCSer.Metadata.MemberIndexInfo member in MemberIndexGroup.GetGroup(type).Find(MemberFiltersEnum.PublicInstance))
            {
                HashSubString name = member.Member.Name;
                if (!values.ContainsKey(name)) values.Add(name, member);
            }
            return values;
        }
    }
}
