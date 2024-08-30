using AutoCSer.CodeGenerator.Metadata;
using AutoCSer.Extensions;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// 流序列化缓存节点接口方法序号映射枚举
    /// </summary>
    [Generator(Name = "流序列化缓存节点接口方法序号映射枚举", IsAuto = true)]
    internal partial class StreamPersistenceMemoryDatabaseMethodIndexEnumType : InterfaceMethodIndexEnumType<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeAttribute>
    {
        /// <summary>
        /// 安装下一个类型
        /// </summary>
        protected override async Task nextCreate()
        {
            if (!CurrentType.Type.IsInterface || CurrentAttribute.MethodIndexEnumType == null) return;
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethod[] methods = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeType(CurrentType.Type).Methods;
            if (methods == null || methods.Length == 0) return;
            Methods = methods.getArray(p => new MethodInfo(p));

            await createCode();
        }
    }
}
