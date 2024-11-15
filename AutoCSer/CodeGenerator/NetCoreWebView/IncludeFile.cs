using AutoCSer.Extensions;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutoCSer.CodeGenerator.NetCoreWebView
{
    /// <summary>
    /// 嵌入文件信息
    /// </summary>
    internal abstract class IncludeFile
    {
        /// <summary>
        /// 脚本文件嵌入文件前缀
        /// </summary>
        protected const string scriptIncludePrefix = "/*include:";
        /// <summary>
        /// 脚本文件嵌入文件后缀
        /// </summary>
        protected const string scriptIncludeSuffix = "*/";
        /// <summary>
        /// JavaScript 严格模式
        /// </summary>
        private const string useStrict = @"'use strict';
";
        /// <summary>
        /// TypeScript 转 JavaScript 继承代码定义
        /// </summary>
        private const string extendsRegexString = @"var __extends \=[\u0000-\uffff]+?\n((\}\;)|(\}\)\(\)\;))[\r\n]+";
        /// <summary>
        /// TypeScript 引用注释
        /// </summary>
        private const string referencePathRegexString = @"\/\/\/ ?\<reference +path ?\= ?""[^\n]+[\r\n]+";
        /// <summary>
        /// TypeScript 类型转 JavaScript 定义
        /// </summary>
        private const string classFunctionRegexString = @"\/\*\* +@class +\*\/";
        /// <summary>
        /// TypeScript 转 JavaScript 生成 map
        /// </summary>
        private const string mapRegexString = @"[\r|\n]+\/\/[^\n]+\.js\.map$";
        /// <summary>
        /// 多行注释正则
        /// </summary>
        private const string multiLineNodeRegexString = @"\/\*[\u0000-\uffff]*?\*\/[\r|\n]+";
        /// <summary>
        /// 单行注释正则
        /// </summary>
        private const string singleLineNodeRegexString = @"[\r|\n]+[\t ]*\/\/[^\r\n]*";
        /// <summary>
        /// 转换为空字符串
        /// </summary>
        private static readonly Regex emptyStringRegex = new Regex($"({extendsRegexString}|{referencePathRegexString}|{classFunctionRegexString}|{multiLineNodeRegexString}|{singleLineNodeRegexString}|{mapRegexString})", RegexOptions.Compiled);
        /// <summary>
        /// 标记名称替换
        /// </summary>
        private static readonly Regex markNameRegex = new Regex(@"__(JAVASCRIPTPATH|JSONQUERYNAME|CALLBACKQUERYNAME|VERSIONQUERYNAME|VERSION|ERRORPATH|VIEWOVERID)__", RegexOptions.Compiled);
        /// <summary>
        /// 时间版本号
        /// </summary>
        internal static readonly string TimeVersion = ((uint)(AutoCSer.Threading.SecondTimer.Now - new DateTime(2024, 4, 24, 0, 3, 16)).TotalSeconds).toHex().TrimStart('0');

        /// <summary>
        /// HTML 模板展开
        /// </summary>
        protected readonly HtmlGenerator htmlGenerator;
        /// <summary>
        /// 原文件信息
        /// </summary>
        protected readonly FileInfo file;
        /// <summary>
        /// 文件内容
        /// </summary>
        protected string fileContent;
        /// <summary>
        /// 代码片段集合
        /// </summary>
        protected LeftArray<CodeFragment> codes;
        /// <summary>
        /// 创建文件版本，用于循环引用检查与脚本文件去重
        /// </summary>
        protected int createFileVersion;
        /// <summary>
        /// 嵌入文件信息
        /// </summary>
        /// <param name="htmlGenerator">HTML 模板展开</param>
        /// <param name="file">原文件信息</param>
        protected IncludeFile(HtmlGenerator htmlGenerator, FileInfo file)
        {
            this.htmlGenerator = htmlGenerator;
            this.file = file;
            codes = new LeftArray<CodeFragment>(0);
        }
        /// <summary>
        /// 嵌入文件分割代码
        /// </summary>
        protected unsafe void splitScriptInclude()
        {
            if (fileContent.Length >= 13)
            {
                fixed (char* codeFixed = fileContent)
                {
                    char* fragmentStart = codeFixed, end = codeFixed + fileContent.Length;
                    for (char* current = codeFixed, leftEnd = end - 12; current < leftEnd;)
                    {
                        if (*current == '/' && *(current + 1) == '*'
                             && *(ulong*)(current + 2) == 'i' + ('n' << 16) + ((ulong)'c' << 32) + ((ulong)'l' << 48)
                             && *(ulong*)(current + 6) == 'u' + ('d' << 16) + ((ulong)'e' << 32) + ((ulong)':' << 48)
                             )
                        {///*include:
                            if (current != fragmentStart)
                            {
                                codes.Add(new CodeFragment(CodeFragmentTypeEnum.Code, new Range(fragmentStart - codeFixed, current - codeFixed)));
                                fragmentStart = current;
                            }
                            current += scriptIncludePrefix.Length + 1;
                            do
                            {
                                if (*current == '/' && *(current - 1) == '*')
                                {
                                    codes.Add(new CodeFragment(CodeFragmentTypeEnum.Include, new Range(fragmentStart - codeFixed, ++current - codeFixed)));
                                    fragmentStart = current;
                                    break;
                                }
                            }
                            while (++current != end);
                            if (current == end) break;
                        }
                        else ++current;
                    }
                    if (fragmentStart != end) codes.Add(new CodeFragment(CodeFragmentTypeEnum.Code, new Range((int)(fragmentStart - codeFixed), fileContent.Length)));
                }
                return;
            }
            if (fileContent.Length != 0) codes.Add(new CodeFragment(CodeFragmentTypeEnum.Code, new Range(0, fileContent.Length)));
        }
        /// <summary>
        /// 获取脚本代码
        /// </summary>
        /// <param name="scriptCodes">脚本代码</param>
        /// <param name="scriptVersion">脚本文件版本号，用于去重</param>
        internal void GetScriptCode(ListArray<SubString> scriptCodes, int scriptVersion)
        {
            if (createFileVersion == scriptVersion) return;
            createFileVersion = scriptVersion;
            if (file.Name == "loadSendError.js" && string.IsNullOrEmpty(HtmlGenerator.ViewMiddleware.ErrorRequestPath)
                && new DirectoryInfo(Path.Combine(htmlGenerator.ProjectParameter.ProjectPath, HtmlGenerator.ViewMiddleware.AutoCSerScriptPath)).FullName == file.Directory.FullName)
            {
                return;
            }
            foreach (CodeFragment code in codes)
            {
                switch (code.Type)
                {
                    case CodeFragmentTypeEnum.Code:
                        scriptCodes.Add(new SubString(code.CodeRange.StartIndex, code.CodeRange.Length, fileContent));
                        break;
                    case CodeFragmentTypeEnum.Include:
                        code.IncludeFile.GetScriptCode(scriptCodes, scriptVersion);
                        break;
                }
            }
        }
        /// <summary>
        /// 获取代码
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
        protected static unsafe string getCode(ListArray<SubString> codes)
        {
            int length = 0;
            foreach (SubString codeFragment in codes) length += codeFragment.Length;
            string code = AutoCSer.Common.AllocateString(length);
            fixed (char* codeFixed = code)
            {
                char* write = codeFixed;
                foreach (SubString codeFragment in codes)
                {
                    AutoCSer.Common.CopyTo(codeFragment.String, codeFragment.Start, write, codeFragment.Length);
                    write += codeFragment.Length;
                }
            }
            codes.Array.Length = 0;
            return code;
        }
        /// <summary>
        /// 标记名称替换
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static string ReplaceMarkName(string code)
        {
            return markNameRegex.Replace(code, replaceMarkName);
        }
        /// <summary>
        /// 标记名称替换
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        private static string replaceMarkName(Match match)
        {
            string name = match.Groups[1].Value;
            switch (name.Length)
            {
                case 7:
                    if (name == "VERSION") return TimeVersion;
                    break;
                case 9:
                    if (name == "ERRORPATH") return HtmlGenerator.ViewMiddleware.ErrorRequestPath;
                    break;
                case 10:
                    if (name == "VIEWOVERID") return HtmlGenerator.ViewMiddleware.ViewOverId;
                    break;
                case 13:
                    if (name == "JSONQUERYNAME") return HtmlGenerator.ViewMiddleware.JsonQueryName;
                    break;
                case 14:
                    if (name == "JAVASCRIPTPATH") return HtmlGenerator.ViewMiddleware.AutoCSerScriptPath;
                    break;
                case 16:
                    if (name == "VERSIONQUERYNAME") return HtmlGenerator.ViewMiddleware.VersionQueryName;
                    break;
                case 17:
                    if (name == "CALLBACKQUERYNAME") return HtmlGenerator.ViewMiddleware.CallbackQueryName;
                    break;
            }
            return match.Value;
        }
        /// <summary>
        /// 获取 JavaScript 代码
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
        protected static string getJavaScriptCode(ListArray<SubString> codes)
        {
            string javaScriptCode = getCode(codes);
            javaScriptCode = emptyStringRegex.Replace(javaScriptCode, string.Empty);
            int index = javaScriptCode.IndexOf(useStrict);
            if (index != -1) javaScriptCode = javaScriptCode.Substring(0, index += useStrict.Length) + javaScriptCode.Substring(index).Replace(useStrict, string.Empty);
            javaScriptCode = javaScriptCode.Replace(" __extends(", " AutoCSer.Pub.Extends(").Replace("    ", "\t");
            return HtmlGenerator.ViewMiddleware.CodeGeneratorJavaScriptCode(ReplaceMarkName(javaScriptCode));
        }
        /// <summary>
        /// 写入文件内容
        /// </summary>
        /// <param name="file"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        protected static async Task writeFile(FileInfo file, string code)
        {
            if (!await AutoCSer.Common.FileExists(file) || await File.ReadAllTextAsync(file.FullName, HtmlGenerator.ViewMiddleware.ResponseEncoding) != code)
            {
                await File.WriteAllTextAsync(file.FullName, code, HtmlGenerator.ViewMiddleware.ResponseEncoding);
            }
        }
    }
}
