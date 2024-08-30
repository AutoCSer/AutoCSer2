using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#if DotNet45
[assembly: AssemblyTitle("AutoCSer.NetCoreWeb")]
[assembly: AssemblyDescription("AutoCSer.NetCoreWeb 是一个基于 .NET Core WEB API 中间件模式的 HTML 模板数据视图支持组件，支持数据 API 帮助文档数据视图，支持统一规划全局数据定义。")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("AutoCSer.NetCoreWeb")]
[assembly: AssemblyCopyright("Copyright © 肖进 2024")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]
[assembly: AssemblyVersion("2.0.0.0")]
[assembly: AssemblyFileVersion("2.0.0.0")]
#endif
[assembly: Guid("13510310-674e-8c37-4e39-13510c060200")]

[assembly: InternalsVisibleTo("AutoCSer.DynamicAssembly")]
[assembly: InternalsVisibleTo("AutoCSer.CodeGenerator")]

[assembly: InternalsVisibleTo("AutoCSer.Custom")]//预留程序集名称，开发者可以自建项目暴露 AutoCSer 的 internal 访问权限
