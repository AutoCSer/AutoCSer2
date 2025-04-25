using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#if DotNet45
[assembly: AssemblyTitle("AutoCSer.Extensions")]
[assembly: AssemblyDescription("AutoCSer.Extensions 是一个杂乱工具帮助类库集合，主要包括 XML 序列化、项目 XML 文档解析、取值表达式解析、原始套接字监听、平衡二叉树、基数排序 等 API。")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("AutoCSer.Extensions")]
[assembly: AssemblyCopyright("Copyright © 肖进 2021")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]
[assembly: AssemblyVersion("2.0.0.0")]
[assembly: AssemblyFileVersion("2.0.0.0")]
#endif
[assembly: Guid("13510310-674e-8c37-4e39-13510c060001")]
#if AOT
[assembly: InternalsVisibleTo("AutoCSer.CodeGenerator.AOT")]
[assembly: InternalsVisibleTo("AutoCSer.Drawing.AOT")]
#else
[assembly: InternalsVisibleTo("AutoCSer.DynamicAssembly")]
[assembly: InternalsVisibleTo("AutoCSer.CodeGenerator")]
[assembly: InternalsVisibleTo("AutoCSer.NetCoreWeb")]
[assembly: InternalsVisibleTo("AutoCSer.Drawing")]
[assembly: InternalsVisibleTo("AutoCSer.CommandService.StreamPersistenceMemoryDatabase")]
[assembly: InternalsVisibleTo("AutoCSer.CommandService.DiskBlock")]
[assembly: InternalsVisibleTo("AutoCSer.CommandService.Search")]

[assembly: InternalsVisibleTo("AutoCSer.Custom")]//预留程序集名称，开发者可以自建项目暴露 AutoCSer 的 internal 访问权限
#endif
