﻿using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
#if !DotNet45
using AutoCSer.CodeGenerator.NetCoreWebView;
using AutoCSer.NetCoreWeb;
#endif

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 模板代码生成器
    /// </summary>
    internal sealed class Coder : TreeTemplate<TreeBuilderNode>
    {
        /// <summary>
        /// 安装参数
        /// </summary>
        private readonly ProjectParameter parameter;
        /// <summary>
        /// 模板文件扩展名
        /// </summary>
        private readonly string extensionName;
        /// <summary>
        /// CSharp代码生成器
        /// </summary>
        /// <param name="parameter">安装参数</param>
        /// <param name="type">模板数据视图</param>
        /// <param name="language">语言</param>
        internal Coder(ProjectParameter parameter, Type type, CodeLanguageEnum language)
            : base(type, Messages.Error, Messages.Message)
        {
            this.parameter = parameter;
            switch (language)
            {
                case CodeLanguageEnum.TypeScript: extensionName = ".ts.txt"; break;
#if !DotNet45
                case CodeLanguageEnum.JavaScript: extensionName = ViewMiddleware.JavaScriptFileExtension; break;
#endif
                default: extensionName = ".cs"; break;
            }
            creators[TreeTemplateCommandEnum.NOTE.ToString()] = note;
            creators[TreeTemplateCommandEnum.LOOP.ToString()] = creators[TreeTemplateCommandEnum.FOR.ToString()] = loop;
            creators[TreeTemplateCommandEnum.AT.ToString()] = at;
            creators[TreeTemplateCommandEnum.PUSH.ToString()] = push;
            creators[TreeTemplateCommandEnum.IF.ToString()] = ifThen;
            creators[TreeTemplateCommandEnum.NOT.ToString()] = not;
            creators[TreeTemplateCommandEnum.NAME.ToString()] = name;
            creators[TreeTemplateCommandEnum.FROMNAME.ToString()] = fromName;
            creators[TreeTemplateCommandEnum.PART.ToString()] = part;
        }
        /// <summary>
        /// 根据类型名称获取子段模板
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <param name="name">子段模板名称</param>
        /// <returns>子段模板</returns>
        protected override TreeBuilderNode fromNameNode(string typeName, ref SubString name)
        {
            TreeBuilderNode node = GetNode(typeName);
            if (node != null)
            {
                node = node.GetFirstNodeByTag(TreeTemplateCommandEnum.NAME, ref name);
                if (node == null) Messages.Error("模板文件 " + getTemplateFileName(typeName) + " 未找到NAME " + name.ToString());
            }
            return node;
        }
        /// <summary>
        /// 根据类型名称获取CSharp代码树节点
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>CSharp代码树节点</returns>
        internal TreeBuilderNode GetNode(string fileName)
        {
            TreeBuilderNode node;
            if (!nodeCache.TryGetValue(fileName + extensionName, out node))
            {
                fileName = getTemplateFileName(fileName);
                if (File.Exists(fileName))
                {
                    nodeCache.Add(fileName, node = new TreeBuilder().Create(File.ReadAllText(fileName)));
                }
                else Messages.Error("未找到模板文件 " + fileName);
            }
            return node;
        }
        /// <summary>
        /// 根据类型名称获取模板文件名
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <returns>模板文件名</returns>
        private string getTemplateFileName(string typeName)
        {
            return Path.Combine(parameter.ProjectPath, "Template", typeName + extensionName);
        }
        /// <summary>
        /// 添加代码树节点
        /// </summary>
        /// <param name="node">代码树节点</param>
        internal void SkinEnd(TreeBuilderNode node)
        {
            skin(node);
            pushCode();
        }

        /// <summary>
        /// 声明与警告+文件头
        /// </summary>
        public const string WarningCode = @"//本文件由程序自动生成，请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
";
        /// <summary>
        /// 文件结束
        /// </summary>
        public const string FileEndCode = @"
#endif";

        /// <summary>
        /// 已经生成的代码
        /// </summary>
        private static readonly ListArray<string>[] codes;
        /// <summary>
        /// 已经生成代码的类型
        /// </summary>
        private static readonly HashSet<HashKey<HashObject<System.Type>, HashObject<System.Type>>> codeTypes = HashSetCreator<HashKey<HashObject<System.Type>, HashObject<System.Type>>>.Create();
        /// <summary>
        /// CSharp代码树节点缓存
        /// </summary>
        private static readonly Dictionary<HashString, TreeBuilderNode> nodeCache = DictionaryCreator.CreateHashString<TreeBuilderNode>();
        /// <summary>
        /// 添加代码
        /// </summary>
        /// <param name="cSharperType">模板类型</param>
        /// <param name="type">实例类型</param>
        /// <returns>锁定是否成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool Add(Type cSharperType, Type type)
        {
            HashKey<HashObject<Type>, HashObject<Type>> hashType =  new HashKey<HashObject<Type>, HashObject<Type>>(cSharperType, type);
            if (codeTypes.Contains(hashType)) return false;
            codeTypes.Add(hashType);
            return true;
        }
        /// <summary>
        /// 添加代码
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="language">代码生成语言</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Add(string code, CodeLanguageEnum language = CodeLanguageEnum.CSharp)
        {
            codes[(int)(byte)language].Add(code);
        }
        /// <summary>
        /// 输出代码
        /// </summary>
        internal static async Task Output(ProjectParameter parameter)
        {
            ListArray<string>[] builders = new ListArray<string>[codes.Length];
            for (int index = codes.Length; index != 0;)
            {
                ListArray<string> builder = codes[--index];
                if (builder.Array.Length != 0)
                {
                    builders[index] = builder;
                    codes[index] = new ListArray<string>();
                }
                CodeLanguageEnum language = (CodeLanguageEnum)(byte)index;
                switch (language)
                {
                    case CodeLanguageEnum.JavaScript:
                    case CodeLanguageEnum.TypeScript:
                        if (builders[index] != null) Messages.Error($"生成了未知的 {language} 代码。");
                        break;
                }
            }

            codeTypes.Clear();
            if (Messages.IsError) return;
            for (int index = builders.Length; index != 0;)
            {
                ListArray<string> builder = builders[--index];
                if (builder != null)
                {
                    switch (index)
                    {
                        case (int)CodeLanguageEnum.CSharp:
                            string code = string.Concat(builder.Array);
                            if (!string.IsNullOrEmpty(code))
                            {
                                string fileName = Path.Combine(parameter.ProjectPath, "{" + parameter.DefaultNamespace + "}.AutoCSer.cs");
                                if (await WriteFile(fileName, WarningCode + code + FileEndCode))
                                {
                                    Messages.Message($"{fileName} 被修改");
                                }
                            }
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// 输出代码
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="content">文件内容</param>
        /// <returns>是否写入新内容</returns>
        public static async Task<bool> WriteFile(string fileName, string content)
        {
            try
            {
                if (!await AutoCSer.Common.Config.FileExists(fileName))
                {
                    await AutoCSer.Common.Config.WriteFileAllText(fileName, content, Encoding.UTF8);
                    return true;
                }
                if (await AutoCSer.Common.Config.ReadFileAllText(fileName, Encoding.UTF8) != content) return await MoveFile(fileName, content);
            }
            catch (Exception exception)
            {
                await AutoCSer.LogHelper.Exception(exception, $"文件创建失败 : {fileName}", LogLevelEnum.All);
            }
            return false;
        }
        /// <summary>
        /// 输出代码
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="content">文件内容</param>
        /// <returns>是否写入新内容</returns>
        public static async Task<bool> MoveFile(string fileName, string content)
        {
            try
            {
                await AutoCSer.IO.File.MoveBak(fileName);
                await AutoCSer.Common.Config.WriteFileAllText(fileName, content, Encoding.UTF8);
                return true;
            }
            catch (Exception exception)
            {
                await AutoCSer.LogHelper.Exception(exception, $"文件创建失败 : {fileName}", LogLevelEnum.All);
                await AutoCSer.LogHelper.Flush();
                throw exception;
            }
        }

        static Coder()
        {
            codes = new ListArray<string>[(byte)CodeLanguageEnum.COUNT];
            for (int index = codes.Length; index != 0; codes[--index] = new ListArray<string>()) ;
        }
    }
}