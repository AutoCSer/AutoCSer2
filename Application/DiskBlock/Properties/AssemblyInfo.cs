using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#if DotNet45
[assembly: AssemblyTitle("AutoCSer.CommandService.DiskBlock")]
[assembly: AssemblyDescription("AutoCSer.CommandService.DiskBlock 是一个日志流磁盘块读写 RPC 服务组件，可用于分布式数据对象的持久化，具有热点数据去重功能。不适合储存大数据对象，如果要储存大数据块，应该在客户端增加分片与重组逻辑组件。")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("AutoCSer.CommandService.DiskBlock")]
[assembly: AssemblyCopyright("Copyright © 肖进 2021")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]
[assembly: AssemblyVersion("2.0.0.0")]
[assembly: AssemblyFileVersion("2.0.0.0")]
#endif
[assembly: Guid("13510310-674e-8c37-4e39-13510c060104")]

[assembly: InternalsVisibleTo("AutoCSer.CommandService.Search")]
