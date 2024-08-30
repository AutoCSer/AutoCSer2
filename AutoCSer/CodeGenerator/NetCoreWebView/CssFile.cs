using System;
using System.IO;
using System.Threading.Tasks;

namespace AutoCSer.CodeGenerator.NetCoreWebView
{
    /// <summary>
    /// css 文件信息
    /// </summary>
    internal sealed class CssFile : IncludeFile
    {
        /// <summary>
        /// css 页面信息
        /// </summary>
        /// <param name="htmlGenerator">HTML 模板展开</param>
        /// <param name="file">css 文件信息</param>
        internal CssFile(HtmlGenerator htmlGenerator, FileInfo file) : base(htmlGenerator, file) { }
        /// <summary>
        /// 加载 css 代码
        /// </summary>
        /// <returns></returns>
        internal async Task<bool> Load()
        {
            string cssFileName = file.FullName;
            fileContent = await File.ReadAllTextAsync(cssFileName, HtmlGenerator.ViewMiddleware.ResponseEncoding);
            splitScriptInclude();
            CodeFragment[] codeArray = codes.Array;
            int index = codes.Length;
            while (index != 0)
            {
                CodeFragment code = codeArray[--index];
                if (code.Type == CodeFragmentTypeEnum.Include)
                {
                    int startIndex = code.CodeRange.StartIndex + scriptIncludePrefix.Length, length = code.CodeRange.EndIndex - startIndex - scriptIncludeSuffix.Length;
                    if (length <= 0)
                    {
                        Messages.Error($"{cssFileName} 嵌入 css 标记错误 {new SubString(code.CodeRange.StartIndex, code.CodeRange.Length, fileContent)}");
                        return false;
                    }
                    codeArray[index].IncludeFile = await htmlGenerator.LoadIncludeCssFile(file, new SubString(startIndex, length, fileContent));
                    if (codeArray[index].IncludeFile == null) return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 创建目标文件
        /// </summary>
        /// <param name="codes">代码</param>
        /// <param name="cssVersion">css 文件版本号，用于去重</param>
        /// <returns></returns>
        internal async Task CreateFile(ListArray<SubString> codes, int cssVersion)
        {
            GetScriptCode(codes, cssVersion);
            string fileName = file.FullName;
            FileInfo cssFile = new FileInfo(fileName.Substring(0, fileName.Length - HtmlGenerator.PageCssFileExtension.Length) + HtmlGenerator.CssFileExtension);
            string code = getCode(codes);
            code = ReplaceMarkName(code);
            await writeFile(cssFile, HtmlGenerator.ViewMiddleware.CodeGeneratorCssCode(code));
        }
    }
}
