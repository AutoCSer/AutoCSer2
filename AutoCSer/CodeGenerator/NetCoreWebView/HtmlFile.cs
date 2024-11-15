using AutoCSer.Html;
using AutoCSer.NetCoreWeb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutoCSer.CodeGenerator.NetCoreWebView
{
    /// <summary>
    /// HTML 文件信息
    /// </summary>
    internal sealed class HtmlFile : IncludeFile
    {
        /// <summary>
        /// 嵌入文件前缀
        /// </summary>
        private const string includePrefix = "<!--include:";
        /// <summary>
        /// 嵌入文件后缀
        /// </summary>
        private const string includeSuffix = "-->";
        /// <summary>
        /// 换行符
        /// </summary>
        private static readonly Regex enterRegex = new Regex(@"[\r\n]+", RegexOptions.Compiled);

        /// <summary>
        /// JavaScript 文件信息
        /// </summary>
        private JavaScriptFile javaScriptFile;
        /// <summary>
        /// 是否模板页面
        /// </summary>
        private readonly bool isPage;
        /// <summary>
        /// HTML 页面信息
        /// </summary>
        /// <param name="htmlGenerator">HTML 模板展开</param>
        /// <param name="file">HTML 文件信息</param>
        internal HtmlFile(HtmlGenerator htmlGenerator, FileInfo file, bool isPage) : base(htmlGenerator, file)
        {
            this.isPage = isPage;
        }
        /// <summary>
        /// 加载 HTML 代码
        /// </summary>
        /// <returns></returns>
        internal async Task<bool> Load()
        {
            string htmlFileName = file.FullName;
            fileContent = await File.ReadAllTextAsync(htmlFileName, HtmlGenerator.ViewMiddleware.ResponseEncoding);
            splitInclude();
            CodeFragment[] codeArray = codes.Array;
            int index = codes.Length;
            while (index != 0)
            {
                CodeFragment code = codeArray[--index];
                if (code.Type == CodeFragmentTypeEnum.Include)
                {
                    int startIndex = code.CodeRange.StartIndex + includePrefix.Length, length = code.CodeRange.EndIndex - startIndex - includeSuffix.Length;
                    if (length <= 0)
                    {
                        Messages.Error($"{htmlFileName} 嵌入 HTML 标记错误 {new SubString(code.CodeRange.StartIndex, code.CodeRange.Length, fileContent)}");
                        return false;
                    }
                    SubString include = new SubString(startIndex, length, fileContent);
                    int endIndex = include.IndexOf('[');
                    if (endIndex >= 0)
                    {
                        if (include[include.Length - 1] != ']')
                        {
                            Messages.Error($"{htmlFileName} 嵌入 HTML 没有找到传参结束标记 {new SubString(code.CodeRange.StartIndex, code.CodeRange.Length, fileContent)}");
                            return false;
                        }
                        include.Length = endIndex;
                    }
                    if (include.Length <= 0)
                    {
                        Messages.Error($"{htmlFileName} 嵌入 HTML 标记错误 {new SubString(code.CodeRange.StartIndex, code.CodeRange.Length, fileContent)}");
                        return false;
                    }
                    codeArray[index].IncludeFile = await htmlGenerator.LoadIncludeHtmlFile(file, include);
                    if (codeArray[index].IncludeFile == null) return false;
                }
            }
            string javaScriptFileName = htmlFileName.Substring(0, htmlFileName.Length - (isPage ? HtmlGenerator.PageHtmlFileExtension.Length : ViewMiddleware.HtmlFileExtension.Length)) + ViewMiddleware.JavaScriptFileExtension;
            FileInfo javaScriptFile = new FileInfo(javaScriptFileName);
            if (await AutoCSer.Common.FileExists(javaScriptFile))
            {
                this.javaScriptFile = await htmlGenerator.LoadJavaScriptFile(javaScriptFile);
                if (this.javaScriptFile == null) return false;
            }
            return true;
        }
        /// <summary>
        /// 嵌入文件分割代码
        /// </summary>
        private unsafe void splitInclude()
        {
            if (fileContent.Length >= 5)
            {
                fixed (char* htmlFixed = fileContent)
                {
                    char* fragmentStart = htmlFixed, end = htmlFixed + fileContent.Length;
                    for (char* current = htmlFixed, leftEnd = end - 15; current < leftEnd;)
                    {
                        if (*current == '<' && *(ulong*)current == '<' + ('!' << 16) + ((ulong)'-' << 32) + ((ulong)'-' << 48)
                             && *(ulong*)(current + 4) == 'i' + ('n' << 16) + ((ulong)'c' << 32) + ((ulong)'l' << 48)
                             && *(ulong*)(current + 8) == 'u' + ('d' << 16) + ((ulong)'e' << 32) + ((ulong)':' << 48)
                             )
                        {//<!--include:
                            if (current != fragmentStart)
                            {
                                appendCode(htmlFixed, fragmentStart, current);
                                fragmentStart = current;
                            }
                            current += includePrefix.Length + 2;
                            do
                            {
                                if (*current == '>' && *(uint*)(current - 2) == '-' + ('-' << 16))
                                {
                                    codes.Add(new CodeFragment(CodeFragmentTypeEnum.Include, new Range(fragmentStart - htmlFixed, ++current - htmlFixed)));
                                    fragmentStart = current;
                                    break;
                                }
                            }
                            while (++current != end);
                            if (current == end) break;
                        }
                        else ++current;
                    }
                    if (fragmentStart != end) appendCode(htmlFixed, fragmentStart, end);
                }
                return;
            }
            if (fileContent.Length != 0) codes.Add(new CodeFragment(CodeFragmentTypeEnum.Code, new Range(0, fileContent.Length)));
        }
        /// <summary>
        /// 添加代码
        /// </summary>
        /// <param name="htmlFixed"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private unsafe void appendCode(char* htmlFixed, char* start, char* end)
        {
            char* fragmentStart = start, parameterStart = null;
            for (char* leftEnd = end - 4; start < leftEnd;)
            {
                if (*start == '{' && (*(ulong*)(start + 1) & 0xfffffffffff0ffffUL) == '{' + (0x30 << 16) + ((ulong)'}' << 32) + ((ulong)'}' << 48) && (uint)(*(start + 2) - '0') < 10)
                {
                    if (fragmentStart != start)
                    {
                        codes.Add(new CodeFragment(CodeFragmentTypeEnum.Code, new Range(fragmentStart - htmlFixed, start - htmlFixed)));
                        fragmentStart = start;
                    }
                    codes.Add(new CodeFragment(CodeFragmentTypeEnum.Parameter, new Range(fragmentStart - htmlFixed, (start += 5) - htmlFixed)));
                    fragmentStart = start;
                }
                else ++start;
            }
            if (fragmentStart != end) codes.Add(new CodeFragment(CodeFragmentTypeEnum.Code, new Range(fragmentStart - htmlFixed, end - htmlFixed)));
        }
        /// <summary>
        /// 创建目标文件
        /// </summary>
        /// <param name="codes">代码</param>
        /// <param name="javaScriptVersion">脚本文件版本号，用于去重</param>
        /// <returns></returns>
        internal async Task<bool> CreateFile(ListArray<SubString> codes, int javaScriptVersion)
        {
            if (!getJavaScriptCode(codes, javaScriptVersion)) return false;
            string javaScriptCode = codes.Count != 0 ? getJavaScriptCode(codes) : string.Empty;
            if (!getHtmlCode(codes, this, new LeftArray<SubString>(0))) return false;
            string htmlCode = getCode(codes);
            htmlCode = ReplaceMarkName(htmlCode);
            if (htmlCode.IndexOf("<!--NOBR-->") >= 0)
            {
                string[] htmlCodes = htmlCode.Split(new string[] { "<!--NOBR-->" }, StringSplitOptions.None);
                for (int bodyIndex = 1; bodyIndex < htmlCodes.Length; bodyIndex += 2) htmlCodes[bodyIndex] = enterRegex.Replace(htmlCodes[bodyIndex], string.Empty);
                htmlCode = string.Concat(htmlCodes);
            }
            if (htmlCode.IndexOf("__LOADPARAMETER__") >= 0)
            {
                string loadParameter = HtmlGenerator.ViewMiddleware.IsStaticVersion ? "S" : string.Empty;
                View view = HtmlGenerator.GetView(file);
                if (view != null)
                {
                    if(loadParameter.Length == 0)
                    {
                        ViewAttribute attribute = (ViewAttribute)view.GetType().GetCustomAttribute(typeof(ViewAttribute), false) ?? ViewAttribute.Default;
                        if (attribute.IsStaticVersion) loadParameter = "S";
                    }
                    var requestPath = view.RequestPath;
                    loadParameter += !string.IsNullOrEmpty(requestPath) ? requestPath : HtmlGenerator.ViewMiddleware.GetRequestPath(view.GetType());
                }
                htmlCode = htmlCode.Replace("__LOADPARAMETER__", loadParameter);
            }
            htmlCode = getRawHtml(htmlCode, codes);
            if (htmlCode.IndexOf("<VIEWJAVASCRIPT />") >= 0)
            {
                if (javaScriptCode.Length != 0)
                {
                    javaScriptCode = @"<script language=""javascript"" type=""text/javascript"">
" + javaScriptCode + @"
</script>";
                }
                htmlCode = htmlCode.Replace("<VIEWJAVASCRIPT />", javaScriptCode);
            }
            string fileName = file.FullName;
            await writeFile(new FileInfo(fileName.Substring(0, fileName.Length - HtmlGenerator.PageHtmlFileExtension.Length) + ViewMiddleware.HtmlFileExtension), HtmlGenerator.ViewMiddleware.CodeGeneratorHtmlCode(htmlCode));
            return true;
        }
        /// <summary>
        /// 获取 JavaScript 代码
        /// </summary>
        /// <param name="javaScriptCodes">JavaScript 代码</param>
        /// <param name="javaScriptVersion">脚本文件版本号，用于去重</param>
        /// <returns>返回 false 表示产生循环引用</returns>
        private bool getJavaScriptCode(ListArray<SubString> javaScriptCodes, int javaScriptVersion)
        {
            if (createFileVersion != 0)
            {
                Messages.Error($"{file.FullName} 展开 HTML 循环引用错误");
                return false;
            }
            createFileVersion = 1;
            foreach (CodeFragment code in codes)
            {
                if (code.Type == CodeFragmentTypeEnum.Include)
                {
                    if (!((HtmlFile)code.IncludeFile).getJavaScriptCode(javaScriptCodes, javaScriptVersion)) return false;
                }
            }
            createFileVersion = 0;
            javaScriptFile?.GetScriptCode(javaScriptCodes, javaScriptVersion);
            return true;
        }
        /// <summary>
        /// 获取 HTML 代码
        /// </summary>
        /// <param name="htmlCodes">HTML 代码</param>
        /// <param name="parameters">嵌入传参数组</param>
        /// <returns>返回 false 表示产生循环引用</returns>
        private bool getHtmlCode(ListArray<SubString> htmlCodes, HtmlFile parameterHtmlFile, LeftArray<SubString> parameters)
        {
            if (createFileVersion != 0)
            {
                Messages.Error($"{file.FullName} 展开 HTML 循环引用错误");
                return false;
            }
            createFileVersion = 1;
            foreach (CodeFragment code in codes)
            {
                switch (code.Type)
                {
                    case CodeFragmentTypeEnum.Code:
                        htmlCodes.Add(new SubString(code.CodeRange.StartIndex, code.CodeRange.Length, fileContent));
                        break;
                    case CodeFragmentTypeEnum.Include:
                        LeftArray<SubString> includeParameters = new LeftArray<SubString>(0);
                        int startIndex = code.CodeRange.StartIndex + includePrefix.Length, length = code.CodeRange.EndIndex - startIndex - includeSuffix.Length - 1;
                        SubString include = new SubString(startIndex, length, fileContent);
                        int parameterIndex = include.IndexOf('[');
                        if (parameterIndex >= 0)
                        {
                            include.MoveStart(parameterIndex + 1);
                            includeParameters = include.Split(',');
                        }
                        if (!((HtmlFile)code.IncludeFile).getHtmlCode(htmlCodes, this, includeParameters)) return false;
                        break;
                    case CodeFragmentTypeEnum.Parameter:
                        int index = fileContent[code.CodeRange.StartIndex + 2] - '0';
                        if ((uint)index < parameters.Length)
                        {
                            SubString parameter = parameters.Array[index];
                            if (parameter.Length != 0) htmlCodes.Add(parameter);
                        }
                        else
                        {
                            Messages.Error($"{file.FullName} 展开 HTML 参数索引位置 {code.CodeRange.StartIndex}[{index}]  超出 {parameterHtmlFile.file.FullName} 传参范围 [{parameters.Length}] {string.Join(" , ", parameters)}");
                            return false;
                        }
                        break;
                }
            }
            createFileVersion = 0;
            return true;
        }
        /// <summary>
        /// 原始 HTML 解析处理
        /// </summary>
        /// <param name="html"></param>
        /// <param name="codes"></param>
        /// <returns></returns>
        private unsafe string getRawHtml(string html, ListArray<SubString> codes)
        {
            int startIndex = 0, length;
            fixed (char* htmlFixed = html)
            {
                foreach (RawNode node in RawNode.GetNodes(html))
                {
                    switch (node.NodeType)
                    {
                        case RawNodeTypeEnum.StartTag:
                        case RawNodeTypeEnum.Tag:
                            Range tag = node.GetTag(htmlFixed);
                            if ((length = tag.Length) > 0)
                            {
                                if (isSelectTag(tag, htmlFixed))
                                {
                                    codes.Add(new SubString(startIndex, tag.StartIndex += 6, html));
                                    codes.Add("@");
                                    startIndex = tag.StartIndex;
                                }
                                Range value = tag;
                                do
                                {
                                    Range name = node.GetAttributeName(htmlFixed, value.EndIndex);
                                    if (name.EndIndex == 0) break;
                                    value = node.GetAttributeValue(htmlFixed, name.EndIndex);
                                    if (value.EndIndex == 0) break;
                                    if (value.Length >= 5)
                                    {
                                        NullableBoolEnum isHref = NullableBoolEnum.Null;
                                        char* start = htmlFixed + name.StartIndex;
                                        switch (name.Length)
                                        {
                                            case 3 - 3://src
                                                if ((*(ulong*)start | 0xffff002000200020UL) == 's' + ('r' << 16) + ((ulong)'c' << 32) + 0xffff000000000000UL) isHref = NullableBoolEnum.False;
                                                break;
                                            case 4 - 3://href
                                                if(HtmlGenerator.ViewMiddleware.IsHtmlLinkVersion && (*(ulong*)start | 0x20002000200020UL) == 'h' + ('r' << 16) + ((ulong)'e' << 32) + ((ulong)'f' << 48)) isHref = NullableBoolEnum.True;
                                                break;
                                            case 5 - 3:
                                                if ((*(ulong*)start | 0x20002000200020UL) == 's' + ('t' << 16) + ((ulong)'y' << 32) + ((ulong)'l' << 48))
                                                {//style
                                                    if (*(start + 4) == 'e') isHref = NullableBoolEnum.False;
                                                }
                                                break;
                                            case 7 - 3:
                                                if ((*(ulong*)start | 0x20002000200020UL) == 's' + ('t' << 16) + ((ulong)'y' << 32) + ((ulong)'l' << 48))
                                                {//checked
                                                    if ((*(ulong*)(start + 4) | 0xffff002000200020UL) == 'k' + ('e' << 16) + ((ulong)'d' << 32) + 0xffff000000000000UL) isHref = NullableBoolEnum.False;
                                                }
                                                break;
                                        }
                                        switch (isHref)
                                        {
                                            case NullableBoolEnum.False:
                                                if (isTemplateValue(value, htmlFixed))
                                                {
                                                    if (startIndex != name.StartIndex)
                                                    {
                                                        codes.Add(new SubString(startIndex, name.StartIndex, html));
                                                        startIndex = name.StartIndex;
                                                    }
                                                    codes.Add("@");
                                                }
                                                break;
                                            case NullableBoolEnum.True:
                                                start = htmlFixed + value.StartIndex;
                                                if (*start == '/' && *(start + 1) != '/')
                                                {///xxx.html?
                                                    bool isQuery = false;
                                                    start = htmlFixed + value.EndIndex;
                                                    if (*(start - 1) == '?')
                                                    {
                                                        --start;
                                                        isQuery = true;
                                                    }
                                                    if ((*(ulong*)(start - 4) | 0x20002000200020UL) == 'h' + ('t' << 16) + ((ulong)'m' << 32) + ((ulong)'l' << 48) && *(start - 5) == '.')
                                                    {
                                                        codes.Add(new SubString(startIndex, value.EndIndex, html));
                                                        startIndex = value.EndIndex;
                                                        if (isQuery) codes.Add($"{HtmlGenerator.ViewMiddleware.VersionQueryName}=__VERSION__&");
                                                        else codes.Add($"?{HtmlGenerator.ViewMiddleware.VersionQueryName}=__VERSION__");
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                }
                                while (true);
                            }
                            break;
                        case RawNodeTypeEnum.RoundTag:
                            tag = node.GetRoundTag(htmlFixed);
                            if (isSelectTag(tag, htmlFixed))
                            {
                                codes.Add(new SubString(startIndex, tag.StartIndex += 6, html));
                                codes.Add("@");
                                startIndex = tag.StartIndex;
                            }
                            break;
                    }
                }
            }
            if ((length = html.Length - startIndex) != 0) codes.Add(new SubString(startIndex, length, html));
            return getCode(codes);
        }
        /// <summary>
        /// 是否 select 标签
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="htmlFixed"></param>
        /// <returns></returns>
        private unsafe static bool isSelectTag(Range tag, char* htmlFixed)
        {
            if (tag.Length == 6)
            {
                char* start = htmlFixed + tag.StartIndex;
                return (*(ulong*)start | 0x20002000200020UL) == 's' + ('e' << 16) + ((ulong)'l' << 32) + ((ulong)'e' << 48) && (*(uint*)(start + 4) | 0x200020UL) == 'c' + ('t' << 16);
            }
            return false;
        }
        /// <summary>
        /// 判断属性值是否包含模板取值 {{xxx}}
        /// </summary>
        /// <param name="value"></param>
        /// <param name="htmlFixed"></param>
        /// <returns></returns>
        private unsafe static bool isTemplateValue(Range value, char* htmlFixed)
        {
            char* start = htmlFixed + value.StartIndex, end = htmlFixed + value.EndIndex - 1;
            do
            {
                if (*start == '{' && *(start + 1) == '{')
                {
                    for (start += 2; start < end; ++start)
                    {
                        if (*start == '}' && *(start + 1) == '}') return true;
                    }
                    return false;
                }
            }
            while (++start != end);
            return false;
        }
    }
}
