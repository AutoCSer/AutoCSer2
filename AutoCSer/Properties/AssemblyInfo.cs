using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#if DotNet45
[assembly: AssemblyTitle("AutoCSer 2.0")]
[assembly: AssemblyDescription("AutoCSer 2.0 是一个支持 Actor 模式的高性能 RPC 框架，在高并发环境中可通用支持 100W+/s 量级异步调用吞吐，用于构造分布式网络基础架构。框架内置配套高性能组件，主要包括 JSON 序列化组件、二进制数据序列化组件。")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("AutoCSer 2.0")]
[assembly: AssemblyCopyright("Copyright © 肖进 2021")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]
[assembly: AssemblyVersion("2.0.0.0")]
[assembly: AssemblyFileVersion("2.0.0.0")]
#endif
[assembly: Guid("13510310-674e-8c37-4e39-13510c060000")]
#if AOT
[assembly: InternalsVisibleTo("AutoCSer.CodeGenerator.AOT")]
[assembly: InternalsVisibleTo("AutoCSer.Extensions.AOT")]
[assembly: InternalsVisibleTo("AutoCSer.Drawing.AOT")]
[assembly: InternalsVisibleTo("AutoCSer.RandomObject.AOT")]
[assembly: InternalsVisibleTo("AutoCSer.FieldEquals.AOT")]

[assembly: InternalsVisibleTo("AutoCSer.CommandService.StreamPersistenceMemoryDatabase.AOT")]
[assembly: InternalsVisibleTo("AutoCSer.CommandService.DiskBlock.AOT")]
[assembly: InternalsVisibleTo("AutoCSer.CommandService.DeployTask.AOT")]
#else
[assembly: InternalsVisibleTo("AutoCSer.DynamicAssembly")]
[assembly: InternalsVisibleTo("AutoCSer.CodeGenerator")]
[assembly: InternalsVisibleTo("AutoCSer.Extensions")]
[assembly: InternalsVisibleTo("AutoCSer.NetCoreWeb")]
[assembly: InternalsVisibleTo("AutoCSer.ORM")]
[assembly: InternalsVisibleTo("AutoCSer.Drawing")]
[assembly: InternalsVisibleTo("AutoCSer.RandomObject")]
[assembly: InternalsVisibleTo("AutoCSer.FieldEquals")]

[assembly: InternalsVisibleTo("AutoCSer.CommandService.TimestampVerify")]
[assembly: InternalsVisibleTo("AutoCSer.CommandService.ServiceRegistry")]
[assembly: InternalsVisibleTo("AutoCSer.CommandService.StreamPersistenceMemoryDatabase")]
[assembly: InternalsVisibleTo("AutoCSer.CommandService.DeployTask")]
[assembly: InternalsVisibleTo("AutoCSer.CommandService.DiskBlock")]
[assembly: InternalsVisibleTo("AutoCSer.CommandService.Search")]
[assembly: InternalsVisibleTo("AutoCSer.CommandService.InterfaceRealTimeCallMonitor")]
[assembly: InternalsVisibleTo("AutoCSer.CommandService.ReverseLogCollection")]
#endif
[assembly: InternalsVisibleTo("AutoCSer.Custom")]//预留程序集名称，开发者可以自建项目暴露 AutoCSer 的 internal 访问权限
