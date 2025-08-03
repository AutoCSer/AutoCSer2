using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoCSer.Extensions;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 项目参数
    /// </summary>
    internal sealed class ProjectParameter
    {
        /// <summary>
        /// 当前程序集
        /// </summary>
        internal static readonly Assembly CurrentAssembly = Assembly.GetExecutingAssembly();

        /// <summary>
        /// 项目名称
        /// </summary>
        internal readonly string ProjectName;
        /// <summary>
        /// 项目路径
        /// </summary>
        internal readonly string ProjectPath;
        /// <summary>
        /// 程序集文件名全称
        /// </summary>
        internal readonly string AssemblyPath;
        /// <summary>
        /// 项目默认命名空间
        /// </summary>
        public readonly string DefaultNamespace;
        /// <summary>
        /// 是否自动更新项目文件
        /// </summary>
        internal readonly bool IsProjectFile;
        /// <summary>
        /// 是否 AutoCSer 项目
        /// </summary>
        internal readonly bool IsAutoCSerCodeGenerator;
        /// <summary>
        /// 是否自定义代码生成项目
        /// </summary>
        internal readonly bool IsCustomCodeGenerator;
        /// <summary>
        /// 程序集
        /// </summary>
        internal Assembly Assembly;
        /// <summary>
        /// 类型集合
        /// </summary>
        private Type[] types;
        /// <summary>
        /// 类型集合
        /// </summary>
        internal Type[] Types
        {
            get
            {
                if (types == null && Assembly != null)
                {
                    try
                    {
                        types = Assembly.GetTypes();
                    }
                    catch (System.Reflection.ReflectionTypeLoadException reflectionTypeLoadException)
                    {
                        types = reflectionTypeLoadException.Types.Where(p => p != null).ToArray();
                    }
                    catch (Exception exception)
                    {
                        types = EmptyArray<Type>.Array;
                        Messages.Exception(exception);
                    }
                    if (types.Length != 0)
                    {
                        types = types.sort((left, right) => string.CompareOrdinal(left.FullName, right.FullName));
                    }
                }
                return types;
            }
        }
        ///// <summary>
        ///// 网站生成配置
        ///// </summary>
        //private WebViewConfig webConfig;
        ///// <summary>
        ///// 网站生成配置
        ///// </summary>
        //public WebViewConfig WebConfig
        //{
        //    get
        //    {
        //        if (webConfig == null)
        //        {
        //            foreach (Type type in Types)
        //            {
        //                if (!type.IsAbstract && typeof(AutoCSer.WebViewConfig).IsAssignableFrom(type))
        //                {
        //                    if (type.IsDefined(typeof(WebViewConfigAttribute), false))
        //                    {
        //                        webConfig = Activator.CreateInstance(type) as AutoCSer.WebViewConfig;
        //                        if (webConfig != null) break;
        //                    }
        //                }
        //            }
        //            if (webConfig == null) webConfig = AutoCSer.WebViewConfig.Null.Default;
        //        }
        //        return object.ReferenceEquals(webConfig, AutoCSer.WebViewConfig.Null.Default) ? null : webConfig;
        //    }
        //}
        /// <summary>
        /// 自动安装参数
        /// </summary>
        /// <param name="projectName">项目名称</param>
        /// <param name="projectPath">项目路径</param>
        /// <param name="assemblyPath">程序集文件名全称</param>
        /// <param name="defaultNamespace">项目默认命名空间</param>
        /// <param name="isProjectFile">是否自动更新项目文件</param>
        internal ProjectParameter(string projectName, string projectPath, string assemblyPath, string defaultNamespace, bool isProjectFile)
        {
            ProjectName = projectName;
            ProjectPath = new DirectoryInfo(projectPath).FullName;
            AssemblyPath = assemblyPath;
            DefaultNamespace = defaultNamespace;
            IsProjectFile = isProjectFile;
            string assemblyFile = new FileInfo(assemblyPath).Name;
#if AOT
            if (assemblyFile == AutoCSer.Common.NamePrefix + ".CodeGenerator.AOT.exe" || assemblyFile == AutoCSer.Common.NamePrefix + ".CodeGenerator.AOT.dll") IsAutoCSerCodeGenerator = true;
#else
            if (assemblyFile == AutoCSer.Common.NamePrefix + ".CodeGenerator.exe" || assemblyFile == AutoCSer.Common.NamePrefix + ".CodeGenerator.dll") IsAutoCSerCodeGenerator = true;
#endif
            else if (assemblyFile == CustomConfig.CustomAssemblyName + ".dll") IsCustomCodeGenerator = true;
        }
        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <returns></returns>
        internal async Task<KeyValue<Exception, bool>> LoadAssembly()
        {
            bool isAssemblyPath = false;
            try
            {
                string assemblyFile = new FileInfo(AssemblyPath).Name;
                foreach (Assembly value in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (value.ManifestModule.Name == assemblyFile)
                    {
                        Assembly = value;
                        return new KeyValue<Exception, bool>(null, true);
                    }
                }
                if (isAssemblyPath = await AutoCSer.Common.FileExists(AssemblyPath))
                {
                    Assembly = Assembly.LoadFrom(AssemblyPath);
                }
            }
            catch (Exception exception)
            {
                return new KeyValue<Exception, bool>(exception, isAssemblyPath);
            }
            return new KeyValue<Exception, bool>(null, isAssemblyPath);
        }
        /// <summary>
        /// 运行代码生成
        /// </summary>
        /// <param name="generator">代码生成接口</param>
        /// <param name="attribute">代码生成器配置</param>
        private async Task run(IGenerator generator, GeneratorAttribute attribute)
        {
            if (generator != null && !await generator.Run(this, attribute)) Messages.Error($"{generator.GetType().fullName()} 生成代码失败");
        }
        /// <summary>
        /// 启动代码生成
        /// </summary>
        internal async Task Start()
        {
            if (string.IsNullOrEmpty(ProjectPath) || !await AutoCSer.Common.DirectoryExists(ProjectPath))
            {
                Messages.Error($"项目路径不存在 : {ProjectPath}");
                return;
            }
            try
            {
                if (IsAutoCSerCodeGenerator || IsCustomCodeGenerator) await run(new CSharper(), null);
                else
                {
                    KeyValue<Type, GeneratorAttribute>[] generators = (CustomConfig.Default.IsAutoCSer ? CurrentAssembly.GetTypes() : EmptyArray<Type>.Array)
                        .Concat(CustomConfig.Assembly == null ? EmptyArray<Type>.Array : CustomConfig.Assembly.GetTypes())
                        .Where(type => !type.IsInterface && !type.IsAbstract && typeof(IGenerator).IsAssignableFrom(type))
                        .Select(type => new KeyValue<Type, GeneratorAttribute>(type, (GeneratorAttribute)type.GetCustomAttribute(typeof(GeneratorAttribute))))
                        .Where(value => value.Value != null && value.Value.IsAuto)
                        .Select(value => new KeyValue<Type, GeneratorAttribute>(value.Key, value.Value))
                        .ToArray();
                    if (generators.Length != 0)
                    {
                        generators = generators.sort((left, right) => string.CompareOrdinal(left.Key.FullName, right.Key.FullName));
                        HashSet<HashObject<Type>> types = new HashSet<HashObject<Type>>(generators.Select(value => (HashObject<Type>)value.Key));
                        KeyValue<HashObject<Type>, HashObject<Type>>[] depends = generators
                            .Where(value => value.Value.DependType != null && types.Contains(value.Value.DependType))
                            .Select(value => new KeyValue<HashObject<Type>, HashObject<Type>>(value.Key, value.Value.DependType))
                            .ToArray();
                        foreach (HashObject<Type> type in AutoCSer.Algorithm.TopologySort.Sort(depends, types, true))
                        {
                            foreach(KeyValue<Type, GeneratorAttribute> generator in generators)
                            {
                                if (type.Value == generator.Key)
                                {
                                    await run(type.Value.Assembly.CreateInstance(type.Value.FullName) as IGenerator, generator.Value);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                if (CustomConfig.Assembly == null) Messages.Exception(exception);
                else Messages.Error(CustomConfig.Assembly.FullName + "\r\n" + exception.ToString());
            }
            finally { await Coder.Output(this); }
        }
        /// <summary>
        /// 启动代码生成
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        internal static async Task Start(string[] args)
        {
            try
            {
                await AutoCSer.LogHelper.Info(string.Join(@""", @""", args), LogLevelEnum.Info | LogLevelEnum.AutoCSer);
                //args = new string[] { @"AutoCSer.CommandService.TimestampVerify.NET8", @"C:\AutoCSer2\Application\TimestampVerify\ ", @"C:\AutoCSer2\Application\TimestampVerify\bin\Release\net8.0\AutoCSer.CommandService.TimestampVerify.dll ", @"AutoCSer.CommandService.TimestampVerify" };
                if (args.Length >= 4)
                {
                    ProjectParameter parameter = new ProjectParameter(args[0].TrimEnd(' '), args[1].TrimEnd(' '), args[2].TrimEnd(' '), args[3].TrimEnd(' '), args.Length > 4);
                    KeyValue<Exception, bool> exception = await parameter.LoadAssembly();
                    if (exception.Value)
                    {
                        if (exception.Key == null) await parameter.Start();
                        else Messages.Exception(exception.Key);
                    }
                    else Messages.Error("未找到程序集文件 : " + parameter.AssemblyPath);
                }
            }
            catch (Exception exception)
            {
                Messages.Exception(exception);
            }
            finally
            {
                await Messages.Open();
                await AutoCSer.LogHelper.Flush();
            }
        }
    }
}
