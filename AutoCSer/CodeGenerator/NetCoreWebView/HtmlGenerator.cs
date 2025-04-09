using AutoCSer.CodeGenerator.Metadata;
using AutoCSer.CodeGenerator.TemplateGenerator;
using AutoCSer.Extensions;
using AutoCSer.NetCoreWeb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoCSer.CodeGenerator.NetCoreWebView
{
    /// <summary>
    /// HTML 模板展开
    /// </summary>
    [Generator(Name = "HTML 模板展开", IsAuto = true, IsTemplate = false)]
    internal sealed class HtmlGenerator : IGenerator
    {
        /// <summary>
        /// TypeScript 文件扩展名
        /// </summary>
        internal const string TypeScriptFileExtension = ".ts";
        /// <summary>
        /// JavaScript 模板文件扩展名
        /// </summary>
        internal const string PageJavaScriptFileExtension = ".page.js";
        /// <summary>
        /// HTML 模板页面文件扩展名
        /// </summary>
        internal const string PageHtmlFileExtension = ".page.html";
        /// <summary>
        /// css 文件扩展名
        /// </summary>
        internal const string CssFileExtension = ".css";
        /// <summary>
        /// css 模板文件扩展名
        /// </summary>
        internal const string PageCssFileExtension = ".page.css";
        /// <summary>
        /// 数据视图中间件
        /// </summary>
        internal static ViewMiddleware ViewMiddleware;
        /// <summary>
        /// 数据视图集合
        /// </summary>
        internal static Dictionary<string, View> Views = DictionaryCreator<string>.Create<View>();

        /// <summary>
        /// 安装参数
        /// </summary>
        internal ProjectParameter ProjectParameter;
        /// <summary>
        /// 嵌入 HTML 文件集合
        /// </summary>
        private Dictionary<string, HtmlFile> includeHtmlFiles;
        /// <summary>
        /// JavaScript 文件集合
        /// </summary>
        private Dictionary<string, JavaScriptFile> javaScriptFiles;
        /// <summary>
        /// 嵌入 Css 文件集合
        /// </summary>
        private Dictionary<string, CssFile> includeCssFiles;
        /// <summary>
        /// 代码生成入口
        /// </summary>
        /// <param name="parameter">安装参数</param>
        /// <param name="attribute">代码生成器配置</param>
        /// <returns>是否生成成功</returns>
        public async Task<bool> Run(ProjectParameter parameter, GeneratorAttribute attribute)
        {
            foreach (Type type in parameter.Types)
            {
                if (!type.IsAbstract && typeof(ViewMiddleware).IsAssignableFrom(type) && type != typeof(NullViewMiddleware))
                {
                    ViewMiddleware = GenericType.Get(type).CallNetCoreWebViewMiddlewareDefaultConstructor;
                    break;
                }
            }
            if (ViewMiddleware != null)
            {
                ProjectParameter = parameter;
                foreach (Type type in parameter.Types)
                {
                    if (!type.IsAbstract && typeof(View).IsAssignableFrom(type) && type != typeof(View))
                    {
                        View view = GenericType.Get(type).CallNetCoreWebViewDefaultConstructor;
                        string filePath = ViewMiddleware.GetNamespaceTemplateFilePath(type);
                        FileInfo pageFile = new FileInfo(string.IsNullOrEmpty(filePath) ? Path.Combine(parameter.ProjectPath, $"{type.Name}{PageHtmlFileExtension}") : Path.Combine(parameter.ProjectPath, filePath, $"{type.Name}{PageHtmlFileExtension}"));
                        if (!await AutoCSer.Common.FileExists(pageFile))
                        {
                            Messages.Error(Culture.Configuration.Default.GetWebViewNotFoundTemplateFile(type, pageFile));
                            return false;
                        }
                        string pageFileName = pageFile.FullName;
                        string viewKey = ViewMiddleware.FileNameIgnoreCase ? pageFileName.ToLower() : pageFileName;
                        if (Views.TryGetValue(viewKey, out View keyView))
                        {
                            Messages.Error(Culture.Configuration.Default.GetWebViewTemplateFileNameConflict(type, keyView.GetType(), pageFile));
                            return false;
                        }
                        Views.Add(viewKey, view);
                    }
                }
                DirectoryInfo directory = AutoCSer.Common.ApplicationDirectory;
                do
                {
                    if (directory.Name == "CodeGenerator")
                    {
                        directory = new DirectoryInfo(Path.Combine(directory.FullName, "Script"));
                        break;
                    }
                }
                while ((directory = directory.Parent) != null);
                if (directory != null && await AutoCSer.Common.DirectoryExists(directory)) await copyAutoCSerScript(directory);
                else Messages.Message($"没有找到 AutoCSer TypeScript / JavaScript 文件目录 {directory?.FullName ?? "Script"}");

                includeHtmlFiles = DictionaryCreator<string>.Create<HtmlFile>();
                javaScriptFiles = DictionaryCreator<string>.Create<JavaScriptFile>();
                FileInfo[] files = await AutoCSer.Common.DirectoryGetFiles(new DirectoryInfo(parameter.ProjectPath), "*" + PageHtmlFileExtension, SearchOption.AllDirectories);
                LeftArray<HtmlFile> pages = new LeftArray<HtmlFile>(files.Length);
                foreach (FileInfo htmlPageFile in files)
                {
                    HtmlFile page = new HtmlFile(this, htmlPageFile, true);
                    pages.Add(page);
                    if (!await page.Load()) return false;
                }
                ListArray<SubString> codes = new ListArray<SubString>();
                int scriptVersion = 0;
                foreach (HtmlFile page in pages)
                {
                    if(!await page.CreateFile(codes, ++scriptVersion)) return false;
                }

                files = await AutoCSer.Common.DirectoryGetFiles(new DirectoryInfo(Path.Combine(parameter.ProjectPath, ViewMiddleware.AutoCSerScriptPath)), "*" + PageJavaScriptFileExtension, SearchOption.AllDirectories);
                LeftArray<JavaScriptFile> javaScriptPages = new LeftArray<JavaScriptFile>(files.Length);
                foreach (FileInfo javaScriptFile in files)
                {
                    JavaScriptFile page = new JavaScriptFile(this, javaScriptFile);
                    javaScriptPages.Add(page);
                    if (!await page.Load()) return false;
                }
                foreach (JavaScriptFile page in javaScriptPages)
                {
                    await page.CreateFile(codes, ++scriptVersion);
                }

                includeCssFiles = DictionaryCreator<string>.Create<CssFile>();
                files = await AutoCSer.Common.DirectoryGetFiles(new DirectoryInfo(parameter.ProjectPath), "*" + PageCssFileExtension, SearchOption.AllDirectories);
                LeftArray<CssFile> cssFiles = new LeftArray<CssFile>(files.Length);
                foreach (FileInfo cssFile in files)
                {
                    CssFile page = new CssFile(this, cssFile);
                    cssFiles.Add(page);
                    if (!await page.Load()) return false;
                }
                foreach (CssFile page in cssFiles) await page.CreateFile(codes, ++scriptVersion);

                string versionFileName = ViewMiddleware.VersionFileName;
                if (!string.IsNullOrEmpty(versionFileName)) await File.WriteAllTextAsync(Path.Combine(parameter.ProjectPath, ViewMiddleware.VersionFileName), HtmlFile.TimeVersion, Encoding.ASCII);
            }
            return true;
        }
        /// <summary>
        /// 复制 AutoCSer TypeScript / JavaScript 文件
        /// </summary>
        /// <param name="scriptDirectory">AutoCSer TypeScript / JavaScript 文件目录</param>
        /// <returns></returns>
        private Task copyAutoCSerScript(DirectoryInfo scriptDirectory)
        {
            string scriptPath = Path.Combine(ProjectParameter.ProjectPath, ViewMiddleware.AutoCSerScriptPath);
            return copyAutoCSerScript(scriptDirectory, scriptPath);
            //foreach (DirectoryInfo directory in await AutoCSer.Common.GetDirectories(scriptDirectory))
            //{
            //    switch (directory.Name)
            //    {
            //        case "ace": await copyAutoCSerScript(directory, Path.Combine(scriptPath, directory.Name), true); break;
            //        case "MathJax": await copyAutoCSerScript(directory, Path.Combine(scriptPath, directory.Name), ViewMiddleware.ScriptType == ViewScriptTypeEnum.TypeScript ? "load.ts" : "load.js", "MathJax.js"); break;
            //        case "Highcharts": await copyAutoCSerScript(directory, Path.Combine(scriptPath, directory.Name), true); break;
            //        default: Messages.Error($"未知的 JavaScript 文件夹 {directory.FullName}"); break;
            //    }
            //}
        }
        /// <summary>
        /// 复制 AutoCSer TypeScript / JavaScript 文件
        /// </summary>
        /// <param name="scriptDirectory">AutoCSer TypeScript / JavaScript 文件目录</param>
        /// <param name="projectPath">项目文件目录</param>
        private async Task copyAutoCSerScript(DirectoryInfo scriptDirectory, string projectPath)
        {
            await AutoCSer.Common.TryCreateDirectory(projectPath);
            foreach (FileInfo file in await AutoCSer.Common.DirectoryGetFiles(scriptDirectory))
            {
                string fileExtension = file.Extension;
                if (!fileExtension.EndsWith(".map", StringComparison.OrdinalIgnoreCase) && !file.Name.EndsWith(".js.map", StringComparison.OrdinalIgnoreCase))
                {
                    switch (ViewMiddleware.ScriptType)
                    {
                        case ViewScriptTypeEnum.TypeScript:
                            if (fileExtension.EndsWith(ViewMiddleware.JavaScriptFileExtension, StringComparison.OrdinalIgnoreCase))
                            {
                                string fileName = file.FullName;
                                if (!await AutoCSer.Common.FileExists(fileName.Substring(0, fileName.Length - 3) + TypeScriptFileExtension)) await copyAutoCSerScript(file, Path.Combine(projectPath, file.Name));
                            }
                            else await copyAutoCSerScript(file, Path.Combine(projectPath, file.Name));
                            break;
                        case ViewScriptTypeEnum.JavaScript:
                            if (!fileExtension.EndsWith(TypeScriptFileExtension, StringComparison.OrdinalIgnoreCase)) await copyAutoCSerScript(file, Path.Combine(projectPath, file.Name));
                            break;
                    }
                }
            }
            foreach(DirectoryInfo directory in await AutoCSer.Common.GetDirectories(scriptDirectory))
            {
                await copyAutoCSerScript(directory, Path.Combine(projectPath, directory.Name));
            }
        }
        /// <summary>
        /// 复制 AutoCSer TypeScript / JavaScript 文件
        /// </summary>
        /// <param name="scriptDirectory">AutoCSer TypeScript / JavaScript 文件目录</param>
        /// <param name="projectPath">项目文件目录</param>
        /// <param name="fileNames">文件名称集合</param>
        private async Task copyAutoCSerScript(DirectoryInfo scriptDirectory, string projectPath, params string[] fileNames)
        {
            await AutoCSer.Common.TryCreateDirectory(projectPath);
            string path = scriptDirectory.FullName;
            foreach (string fileName in fileNames)
            {
                FileInfo file = new FileInfo(Path.Combine(path, fileName));
                if (await AutoCSer.Common.FileExists(file)) await copyAutoCSerScript(file, Path.Combine(projectPath, file.Name));
            }
        }
        /// <summary>
        /// 复制 AutoCSer TypeScript / JavaScript 文件
        /// </summary>
        /// <param name="file">AutoCSer TypeScript / JavaScript 文件</param>
        /// <param name="fileName">项目文件名称</param>
        private async Task copyAutoCSerScript(FileInfo file, string fileName)
        {
            FileInfo projectFile = new FileInfo(fileName);
            if (await AutoCSer.Common.FileExists(projectFile))
            {
                if (projectFile.LastWriteTimeUtc == file.LastWriteTimeUtc && projectFile.Length == file.Length) return;
                if ((projectFile.Attributes & FileAttributes.ReadOnly) != 0) await AutoCSer.Common.SetFileAttributes(projectFile, projectFile.Attributes & ~FileAttributes.ReadOnly);
            }
            await copyAutoCSerScript(file, projectFile);
        }
        /// <summary>
        /// 复制 AutoCSer JavaScript 文件
        /// </summary>
        /// <param name="file">AutoCSer TypeScript / JavaScript 文件</param>
        /// <param name="projectFile">项目文件</param>
        private async Task copyAutoCSerScript(FileInfo file, FileInfo projectFile)
        {
            if (ViewMiddleware.ResponseEncoding.CodePage != Encoding.UTF8.CodePage)
            {
                if (file.Extension.EndsWith(TypeScriptFileExtension, StringComparison.OrdinalIgnoreCase)
                    || file.Extension.EndsWith(ViewMiddleware.JavaScriptFileExtension, StringComparison.OrdinalIgnoreCase))
                {
                    await File.WriteAllTextAsync(projectFile.FullName, await File.ReadAllTextAsync(file.FullName, Encoding.UTF8), ViewMiddleware.ResponseEncoding);
                    projectFile.LastWriteTimeUtc = file.LastWriteTimeUtc;
                    return;
                }
            }
            await AutoCSer.Common.FileCopyTo(file, projectFile.FullName);
            if ((file.Attributes & FileAttributes.ReadOnly) != 0) await AutoCSer.Common.SetFileAttributes(new FileInfo(projectFile.FullName), file.Attributes & ~FileAttributes.ReadOnly);
        }
        /// <summary>
        /// 加载嵌入 HTML 文件
        /// </summary>
        /// <param name="htmlFile">原 HTML 文件信息</param>
        /// <param name="include">嵌入文件名称字符串</param>
        /// <returns></returns>
        internal async Task<HtmlFile> LoadIncludeHtmlFile(FileInfo htmlFile, SubString include)
        {
            FileInfo includeFile = new FileInfo(include[0] == '\\' ? Path.Combine(ProjectParameter.ProjectPath, IncludeFile.ReplaceMarkName(include.GetSub(1)) + ViewMiddleware.HtmlFileExtension) : Path.Combine(htmlFile.Directory.FullName, IncludeFile.ReplaceMarkName(include) + ViewMiddleware.HtmlFileExtension));
            string fileKey = ViewMiddleware.FileNameIgnoreCase ? includeFile.FullName.ToLower() : includeFile.FullName;
            if (includeHtmlFiles.TryGetValue(fileKey, out HtmlFile includeHtmlFile)) return includeHtmlFile;
            if (await AutoCSer.Common.FileExists(includeFile))
            {
                includeHtmlFiles.Add(fileKey, includeHtmlFile = new HtmlFile(this, includeFile, false));
                if (await includeHtmlFile.Load()) return includeHtmlFile;
            }
            else Messages.Error($"{htmlFile.FullName} 没有找到嵌入 HTML 文件 {include} => {includeFile.FullName}");
            return null;
        }
        /// <summary>
        /// 加载 JavaScript 脚本
        /// </summary>
        /// <param name="file">JavaScript 脚本文件信息</param>
        /// <returns></returns>
        internal async Task<JavaScriptFile> LoadJavaScriptFile(FileInfo file)
        {
            string fileKey = ViewMiddleware.FileNameIgnoreCase ? file.FullName.ToLower() : file.FullName;
            if (javaScriptFiles.TryGetValue(fileKey, out JavaScriptFile javaScriptFile)) return javaScriptFile;
            javaScriptFiles.Add(fileKey, javaScriptFile = new JavaScriptFile(this, file));
            if (await javaScriptFile.Load()) return javaScriptFile;
            return null;
        }
        /// <summary>
        /// 加载嵌入 JavaScript 脚本
        /// </summary>
        /// <param name="htmlFile">原 JavaScript 文件信息</param>
        /// <param name="include">嵌入文件名称字符串</param>
        /// <returns></returns>
        internal async Task<JavaScriptFile> LoadIncludeJavaScriptFile(FileInfo file, SubString include)
        {
            FileInfo includeFile = new FileInfo(include[0] == '\\' ? Path.Combine(ProjectParameter.ProjectPath, IncludeFile.ReplaceMarkName(include.GetSub(1)) + ViewMiddleware.JavaScriptFileExtension) : Path.Combine(file.Directory.FullName, IncludeFile.ReplaceMarkName(include) + ViewMiddleware.JavaScriptFileExtension));
            string fileKey = ViewMiddleware.FileNameIgnoreCase ? includeFile.FullName.ToLower() : includeFile.FullName;
            if (javaScriptFiles.TryGetValue(fileKey, out JavaScriptFile javaScriptFile)) return javaScriptFile;
            if (await AutoCSer.Common.FileExists(includeFile))
            {
                javaScriptFiles.Add(fileKey, javaScriptFile = new JavaScriptFile(this, includeFile));
                if (await javaScriptFile.Load()) return javaScriptFile;
            }
            else Messages.Error($"{file.FullName} 没有找到嵌入 JavaScript 文件 {include} => {includeFile.FullName}");
            return null;
        }
        /// <summary>
        /// 加载嵌入 css 文件
        /// </summary>
        /// <param name="htmlFile">原 css 文件信息</param>
        /// <param name="include">嵌入文件名称字符串</param>
        /// <returns></returns>
        internal async Task<CssFile> LoadIncludeCssFile(FileInfo file, SubString include)
        {
            FileInfo includeFile = new FileInfo(Path.Combine(include[0] == '/' ? ProjectParameter.ProjectPath : file.Directory.FullName, include + CssFileExtension));
            string fileKey = ViewMiddleware.FileNameIgnoreCase ? includeFile.FullName.ToLower() : includeFile.FullName;
            if (includeCssFiles.TryGetValue(fileKey, out CssFile cssFile)) return cssFile;
            if (await AutoCSer.Common.FileExists(includeFile))
            {
                includeCssFiles.Add(fileKey, cssFile = new CssFile(this, includeFile));
                if (await cssFile.Load()) return cssFile;
            }
            else Messages.Error($"{file.FullName} 没有找到嵌入 css 文件 {include} => {includeFile.FullName}");
            return null;
        }
        /// <summary>
        /// 根据 HTML 模板页面文件信息获取对应数据视图
        /// </summary>
        /// <param name="pageFile"></param>
        /// <returns></returns>
        internal static View GetView(FileInfo pageFile)
        {
            string pageFileName = pageFile.FullName;
            string viewKey = ViewMiddleware.FileNameIgnoreCase ? pageFileName.ToLower() : pageFileName;
            return Views.TryGetValue(viewKey, out View view) ? view : null;
        }
    }
}
