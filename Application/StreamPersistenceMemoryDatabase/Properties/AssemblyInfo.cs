using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#if DotNet45
[assembly: AssemblyTitle("AutoCSer.CommandService.StreamPersistenceMemoryDatabase")]
[assembly: AssemblyDescription("AutoCSer.CommandService.StreamPersistenceMemoryDatabase 是一个基于文件流可靠持久化的高性能内存数据库 RPC 服务组件，高并发环境中可通用支持接近 100W/s 量级调用吞吐，支持异地实时备份。基于接口 API 模式，持久化操作以 API 调用为单位，每一次 API 调用都可以当成是一次事务操作。服务端支持动态更新 API 用于修复 API 异常错误或者动态新增 API 操作，避免不必要的服务端冷启动操作。支持自定义任意数据结构，组件内包含一个消息组件与一些简单的常用数据结构实现，非持久化模式则相当于分布式缓存服务组件。单节点数据量不宜过大，适合数据更新频繁的场景，或者储存生存周期较小的数据。")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("AutoCSer.CommandService.StreamPersistenceMemoryDatabase")]
[assembly: AssemblyCopyright("Copyright © 肖进 2024")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]
[assembly: AssemblyVersion("2.0.0.0")]
[assembly: AssemblyFileVersion("2.0.0.0")]
#endif
[assembly: Guid("13510310-674e-8c37-4e39-13510c060105")]

[assembly: InternalsVisibleTo("AutoCSer.DynamicAssembly")]
[assembly: InternalsVisibleTo("AutoCSer.CodeGenerator")]

[assembly: InternalsVisibleTo("AutoCSer.Custom")]//预留程序集名称，开发者可以自建项目暴露 AutoCSer 的 internal 访问权限
