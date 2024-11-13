using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Reflection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// 类型帮助文档视图数据
    /// </summary>
    public sealed class TypeHelpView
    {
        /// <summary>
        /// 数据视图中间件
        /// </summary>
        internal readonly ViewMiddleware ViewMiddleware;
        /// <summary>
        /// 类型
        /// </summary>
        private readonly Type type;
        /// <summary>
        /// 数据视图显示类型
        /// </summary>
#if NetStandard21
        private Type? viewShowType;
#else
        private Type viewShowType;
#endif
        /// <summary>
        /// 数据视图显示类型
        /// </summary>
        internal Type ViewShowType
        {
            get
            {
                if (viewShowType == null)
                {
                    Type type = GetAwaitResultType(this.type) ?? this.type;
                    if (type != typeof(string))
                    {
                        var enumerableInterfaceType = type.getGenericInterfaceType(typeof(IEnumerable<>));
                        if (enumerableInterfaceType != null)
                        {
                            type = enumerableInterfaceType.GetGenericArguments()[0];
                            IsEnumerableType = true;
                        }
                    }
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        type = type.GetGenericArguments()[0];
                        IsNullableType = true;
                    }
                    IsHelp = ViewMiddleware.IsHelp(type);
                    viewShowType = type;
                }
                return viewShowType;
            }
        }
        /// <summary>
        /// 类型名称
        /// </summary>
        public string Name { get { return ViewShowType.Name; } }
        /// <summary>
        /// 是否 enum 枚举类型
        /// </summary>
        public bool IsEnum { get { return ViewShowType.IsEnum; } }
        /// <summary>
        /// 类型全名（帮助文档识别标识）
        /// </summary>
#if NetStandard21
        private string? fullName;
#else
        private string fullName;
#endif
        /// <summary>
        /// 类型全名（帮助文档识别标识）
        /// </summary>
        public string FullName
        {
            get
            {
                if(fullName == null) fullName = ViewShowType.fullName().notNull();
                return fullName;
            }
        }
        /// <summary>
        /// 类型文档描述
        /// </summary>
#if NetStandard21
        private string? summary;
#else
        private string summary;
#endif
        /// <summary>
        /// 类型文档描述
        /// </summary>
        public string Summary
        {
            get
            {
                if (summary == null) summary = XmlDocument.Get(ViewShowType) ?? string.Empty;
                return summary;
            }
        }
        /// <summary>
        /// 成员集合
        /// </summary>
#if NetStandard21
        private MemberHelpView[]? members;
#else
        private MemberHelpView[] members;
#endif
        /// <summary>
        /// 成员集合
        /// </summary>
#if NetStandard21
        public MemberHelpView[]? Members
#else
        public MemberHelpView[] Members
#endif
        {
            get
            {
                if (IsHelp && !ViewShowType.IsEnum)
                {
                    if (this.members == null)
                    {
                        FieldInfo[] fields = ViewShowType.GetFields(BindingFlags.Public | BindingFlags.Instance);
                        PropertyInfo[] properties = ViewShowType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        LeftArray<MemberHelpView> members = new LeftArray<MemberHelpView>(fields.Length + properties.Length);
                        foreach (FieldInfo field in fields) members.Add(new MemberHelpView(this, field));
                        foreach (PropertyInfo property in properties)
                        {
                            if (property.GetIndexParameters().Length == 0)
                            {
                                bool canRead = property.GetGetMethod() != null, canWrite = property.GetSetMethod(true) != null;
                                if (canRead | canWrite) members.Add(new MemberHelpView(this, property, canRead, canWrite));
                            }
                        }
                        this.members = members.ToArray();
                    }
                    return this.members;
                }
                return null;
            }
        }
        /// <summary>
        /// 枚举值集合
        /// </summary>
#if NetStandard21
        private EnumHelpView[]? enums;
#else
        private EnumHelpView[] enums;
#endif
        /// <summary>
        /// 枚举值集合
        /// </summary>
#if NetStandard21
        public EnumHelpView[]? Enums
#else
        public EnumHelpView[] Enums
#endif
        {
            get
            {
                if (IsHelp && ViewShowType.IsEnum)
                {
                    if (enums == null)
                    {
                        FieldInfo[] fields = ViewShowType.GetFields(BindingFlags.Static | BindingFlags.Public);
                        LeftArray<EnumHelpView> values = new LeftArray<EnumHelpView>(fields.Length);
                        foreach (FieldInfo field in fields) values.Add(new EnumHelpView(field));
                        enums = values.ToArray();
                    }
                    return enums;
                }
                return null;
            }
        }
        /// <summary>
        /// 是否位标记枚举类型
        /// </summary>
        private NullableBoolEnum isEnumFlags;
        /// <summary>
        /// 是否位标记枚举类型
        /// </summary>
        public bool IsEnumFlags
        {
            get
            {
                switch (isEnumFlags)
                {
                    case NullableBoolEnum.Null:
                        if (ViewShowType.IsEnum && ViewShowType.GetCustomAttribute(typeof(FlagsAttribute), false) != null)
                        {
                            isEnumFlags = NullableBoolEnum.True;
                            return true;
                        }
                        isEnumFlags = NullableBoolEnum.False;
                        break;
                    case NullableBoolEnum.True: return true;
                }
                return false;
            }
        }
        /// <summary>
        /// 是否客户端数组类型
        /// </summary>
        public bool IsEnumerableType;
        /// <summary>
        /// 是否可空类型
        /// </summary>
        public bool IsNullableType;
        /// <summary>
        /// 是否支持类型帮助文档
        /// </summary>
        public bool IsHelp;
        /// <summary>
        /// 是否已经触发加载类型视图数据
        /// </summary>
        private bool isLoad;
        /// <summary>
        /// 帮助文档类型信息
        /// </summary>
        /// <param name="viewMiddleware">数据视图中间件</param>
        /// <param name="type">类型</param>
        internal TypeHelpView(ViewMiddleware viewMiddleware, Type type)
        {
            this.ViewMiddleware = viewMiddleware;
            this.type = type;
        }
        /// <summary>
        /// 加载类型视图数据
        /// </summary>
        internal void LoadTypeView()
        {
            if (!isLoad)
            {
                isLoad = true;
                ViewMiddleware.GetTypeHelpView(ViewShowType);
                var members = Members;
                if (members != null)
                {
                    foreach (MemberHelpView member in members) member.Type.LoadTypeView();
                }
            }
        }

        /// <summary>
        /// 空类型
        /// </summary>
        private static readonly TypeHelpView nullType = new TypeHelpView(NullViewMiddleware.Null, typeof(void));
        /// <summary>
        /// 获取 await 结果类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
#if NetStandard21
        internal static Type? GetAwaitResultType(Type type)
#else
        internal static Type GetAwaitResultType(Type type)
#endif
        {
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Task<>)) return type.GetGenericArguments()[0];
                if (genericTypeDefinition == typeof(ReturnCommand<>) || genericTypeDefinition == typeof(ReturnQueueCommand<>)) return typeof(CommandClientReturnValue<>).MakeGenericType(type.GetGenericArguments());
                if(type.GetCustomAttribute(typeof(AutoCSer.CodeGenerator.AwaitResultTypeAttribute), false) != null) return type.GetGenericArguments()[0];
            }
            return null;
        }
    }
}
