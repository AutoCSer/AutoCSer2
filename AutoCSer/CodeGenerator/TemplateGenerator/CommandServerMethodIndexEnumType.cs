using AutoCSer.CodeGenerator.Metadata;
using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// 命令控制器接口方法序号映射枚举
    /// </summary>
    [Generator(Name = "命令控制器接口方法序号映射枚举", IsAuto = true)]
    internal partial class CommandServerMethodIndexEnumType : InterfaceMethodIndexEnumType<AutoCSer.Net.CommandServerControllerInterfaceAttribute>
    {
        /// <summary>
        /// 安装下一个类型
        /// </summary>
        protected override async Task nextCreate()
        {
            if (!CurrentType.Type.IsInterface || CurrentAttribute.MethodIndexEnumTypeCodeGeneratorPath == null || CurrentAttribute.MethodIndexEnumType == null) return;
            ServerInterfaceMethod[] methods = new ServerInterface(CurrentType.Type, null).Methods;
            if (methods == null || methods.Length == 0) return;
            Methods = methods.getArray(p => new MethodInfo(p));

            await createCode();

            if (CurrentAttribute.MethodIndexEnumTypeCodeGeneratorPath.Length != 0 && !string.IsNullOrEmpty(enumCode))
            {
                string filename = Path.Combine(ProjectParameter.ProjectPath, CurrentAttribute.MethodIndexEnumTypeCodeGeneratorPath, $"{CurrentAttribute.MethodIndexEnumType.Name}.cs");
                if (await Coder.WriteFile(filename, enumCode)) Messages.Message($"{filename} 已修改");
            }
        }
        /// <summary>
        /// 当前文件生成代码
        /// </summary>
        private string enumCode;
        /// <summary>
        /// 添加代码
        /// </summary>
        /// <param name="code"></param>
        protected override void addCode(string code)
        {
            if (CurrentAttribute.MethodIndexEnumTypeCodeGeneratorPath.Length == 0) Coder.Add(code, _language_);
            else enumCode = Coder.WarningCode + code + Coder.FileEndCode;
        }
    }
}
