using AutoCSer.CodeGenerator.Metadata;
using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// 命令控制器接口方法序号映射枚举
    /// </summary>
    [Generator(Name = "命令控制器接口方法序号映射枚举", IsAuto = true)]
    internal partial class CommandServerMethodIndexEnumType : AttributeGenerator<AutoCSer.Net.CommandServerControllerInterfaceAttribute>
    {
        /// <summary>
        /// 接口方法与枚举信息
        /// </summary>
        public sealed class MethodInfo
        {
            /// <summary>
            /// 接口方法信息
            /// </summary>
            public MethodIndex Method;
            /// <summary>
            /// 返回值类型
            /// </summary>
            public ExtensionType MethodReturnType;
            /// <summary>
            /// 是否存在返回值
            /// </summary>
            public bool MethodIsReturn { get { return MethodReturnType.Type != typeof(void); } }
            /// <summary>
            /// 枚举名称
            /// </summary>
            public string EnumName;
            /// <summary>
            /// 方法序号
            /// </summary>
            public int MethodIndex;
            /// <summary>
            /// 接口方法与枚举信息
            /// </summary>
            /// <param name="interfaceMethod"></param>
            public MethodInfo(InterfaceMethodBase interfaceMethod)
            {
                if (interfaceMethod != null)
                {
                    Method = new MethodIndex(interfaceMethod.Method, AutoCSer.Metadata.MemberFiltersEnum.Instance, interfaceMethod.MethodIndex);
                    EnumName = Method.MethodName;
                    MethodReturnType = interfaceMethod.ReturnValueType;
                }
            }
        }

        /// <summary>
        /// 接口方法集合
        /// </summary>
        public MethodInfo[] Methods;
        /// <summary>
        /// 生成定义类型
        /// </summary>
        protected override ExtensionType definitionType { get { return CurrentAttribute.MethodIndexEnumType; } }
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <returns></returns>
        protected Task createCode()
        {
            LeftArray<KeyValue<int, string>> enumValues = new LeftArray<KeyValue<int, string>>(0);
            int methodIndex = -1;
            foreach (object value in Enum.GetValues(CurrentAttribute.MethodIndexEnumType))
            {
                int index = ((IConvertible)value).ToInt32(null);
                if (index < Methods.Length) Methods[index].EnumName = value.ToString();
                else
                {
                    if (index > methodIndex) methodIndex = index;
                    enumValues.Add(new KeyValue<int, string>(index, value.ToString()));
                }
            }
            if (methodIndex >= Methods.Length)
            {
                Methods = AutoCSer.Common.GetCopyArray(Methods, methodIndex + 1);
                foreach (KeyValue<int, string> enumValue in enumValues) Methods[enumValue.Key].EnumName = enumValue.Value;
            }
            methodIndex = 0;
            HashSet<string> names = HashSetCreator.CreateAny<string>();
            foreach (MethodInfo methodEnum in Methods)
            {
                if (methodEnum.EnumName != null && !names.Add(methodEnum.EnumName))
                {
                    throw new Exception(Culture.Configuration.Default.GetCommandServerMethodNameConflict(CurrentType.Type, CurrentAttribute.MethodIndexEnumType, methodEnum.EnumName));
                }
                methodEnum.MethodIndex = methodIndex++;
            }
            create(true);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 安装完成处理
        /// </summary>
        protected override Task onCreated() { return AutoCSer.Common.CompletedTask; }

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
