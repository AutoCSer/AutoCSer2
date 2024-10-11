using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#if DotNet45
[assembly: AssemblyTitle("AutoCSer.Search")]
[assembly: AssemblyDescription("AutoCSer.Search 是一个基于 Trie 图查找关键字的高性能内存数据搜索组件，主要用于镜像数据库数据提供关键字查询功能，降低数据库关键字查询压力。由于采用内存数据同步模式，单节点数据量不宜过大，一般不要超过 100W 量级。")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("AutoCSer.Search")]
[assembly: AssemblyCopyright("Copyright © 肖进 2021")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]
[assembly: AssemblyVersion("2.0.0.0")]
[assembly: AssemblyFileVersion("2.0.0.0")]
#endif
[assembly: Guid("13510310-674e-8c37-4e39-13510c060005")]

[assembly: InternalsVisibleTo("AutoCSer.CommandService.Search")]
