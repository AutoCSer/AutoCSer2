using AutoCSer.NetCoreWeb;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AutoCSer.CodeGenerator.NetCoreWebView
{
    /// <summary>
    /// JavaScript 文件信息
    /// </summary>
    internal sealed class JavaScriptFile : IncludeFile
    {
        /// <summary>
        /// JavaScript 文件信息
        /// </summary>
        /// <param name="htmlGenerator">HTML 模板展开</param>
        /// <param name="file">JavaScript 文件信息</param>
        internal JavaScriptFile(HtmlGenerator htmlGenerator, FileInfo file) : base(htmlGenerator, file) { }
        /// <summary>
        /// 加载 JavaScript 代码
        /// </summary>
        /// <returns></returns>
        internal async Task<bool> Load()
        {
            string javaScriptFileName = file.FullName;
            fileContent = await File.ReadAllTextAsync(javaScriptFileName, HtmlGenerator.ViewMiddleware.ResponseEncoding);
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
                        Messages.Error($"{javaScriptFileName} 嵌入 JavaScript 标记错误 {new SubString(code.CodeRange.StartIndex, code.CodeRange.Length, fileContent)}");
                        return false;
                    }
                    codeArray[index].IncludeFile = await htmlGenerator.LoadIncludeJavaScriptFile(file, new SubString(startIndex, length, fileContent));
                    if (codeArray[index].IncludeFile == null) return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 创建目标文件
        /// </summary>
        /// <param name="codes">代码</param>
        /// <param name="javaScriptVersion">脚本文件版本号，用于去重</param>
        /// <returns></returns>
        internal Task CreateFile(ListArray<SubString> codes, int javaScriptVersion)
        {
            GetScriptCode(codes, javaScriptVersion);
            string fileName = file.FullName;
            fileName = fileName.Substring(0, fileName.Length - HtmlGenerator.PageJavaScriptFileExtension.Length);
            FileInfo javaScriptFile = new FileInfo(fileName + ViewMiddleware.JavaScriptFileExtension);
            return writeFile(javaScriptFile, getJavaScriptCode(codes));
        }
    }
}
