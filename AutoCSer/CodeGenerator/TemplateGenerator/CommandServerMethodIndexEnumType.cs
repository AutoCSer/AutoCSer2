using AutoCSer.CodeGenerator.Metadata;
using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
            public Metadata.MethodIndex Method;
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
                    Method = new Metadata.MethodIndex(interfaceMethod.Method, AutoCSer.Metadata.MemberFiltersEnum.Instance, interfaceMethod.MethodIndex);
                    EnumName = Method.MethodName;
                    MethodReturnType = interfaceMethod.ReturnValueType;
                }
            }
        }

        /// <summary>
        /// 输出类定义开始段代码是否包含当前类型
        /// </summary>
        protected override bool isStartClass { get { return false; } }
        /// <summary>
        /// 是否输出生成配置代码
        /// </summary>
        public bool IsGeneratorAttribute;
        /// <summary>
        /// 是否输出枚举代码
        /// </summary>
        public bool IsGeneratorEnum;
        /// <summary>
        /// The command service controller interface generates configuration
        /// 命令服务控制器接口生成配置
        /// </summary>
        private AutoCSer.Net.CommandServer.ServerControllerInterfaceAttribute generatorControllerAttribute;
        /// <summary>
        /// 接口方法集合
        /// </summary>
        public MethodInfo[] Methods;
        /// <summary>
        /// 节点方法序号映射枚举类型
        /// </summary>
        public string MethodIndexEnumTypeName
        {
            get
            {
                if (generatorControllerAttribute.MethodIndexEnumType != null) return generatorControllerAttribute.MethodIndexEnumType.Name;
                return CurrentType.TypeOnlyName + "MethodEnum";
            }
        }
        ///// <summary>
        ///// 生成定义类型
        ///// </summary>
        //protected override ExtensionType definitionType 
        //{
        //    get
        //    {
        //        if (IsGeneratorAttribute) return base.definitionType;
        //        return generatorControllerAttribute.MethodIndexEnumType ?? base.definitionType; 
        //    }
        //}
        /// <summary>
        /// 是否检查添加代码类型
        /// </summary>
        /// <returns></returns>
        protected override bool checkAddType()
        {
            return IsGeneratorAttribute || !IsGeneratorEnum;
        }
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <returns></returns>
        protected Task createCode()
        {
            int methodIndex = -1;
            Type methodIndexEnumType = generatorControllerAttribute.MethodIndexEnumType;
            if (methodIndexEnumType != null && methodIndexEnumType.IsEnum)
            {
                LeftArray<KeyValue<int, string>> enumValues = new LeftArray<KeyValue<int, string>>(0);
                foreach (object value in Enum.GetValues(methodIndexEnumType))
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
            }
            methodIndex = 0;
            HashSet<string> names = HashSetCreator<string>.Create();
            foreach (MethodInfo method in Methods)
            {
                //if (method.EnumName == null) method.EnumName = method.Method?.MethodName;
                if (method.EnumName != null && !names.Add(method.EnumName))
                {
                    throw new Exception(Culture.Configuration.Default.GetCommandServerMethodNameConflict(CurrentType.Type, generatorControllerAttribute.MethodIndexEnumType, method.EnumName));
                }
                method.MethodIndex = methodIndex++;
            }
            IsGeneratorAttribute = true;
            if (string.IsNullOrEmpty(CurrentAttribute.MethodIndexEnumTypeCodeGeneratorPath))
            {
                IsGeneratorEnum = true;
                create(true);
            }
            else
            {
                IsGeneratorEnum = false;
                create(true);
                IsGeneratorAttribute = false;
                IsGeneratorEnum = true;
                create(true);
            }
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
            if (!CurrentType.Type.IsInterface || !CurrentAttribute.IsCodeGeneratorMethodEnum) return;
            generatorControllerAttribute = CurrentType.Type.GetCustomAttribute<AutoCSer.Net.CommandServer.ServerControllerInterfaceAttribute>(false) ?? AutoCSer.Net.CommandServer.ServerControllerInterfaceAttribute.Default;
            ServerInterfaceMethod[] methods = new ServerInterface(CurrentType.Type, null, null, true).Methods;
            if (methods == null || methods.Length == 0) return;
            Methods = methods.getArray(p => new MethodInfo(p));

            enumCode = string.Empty;
            await createCode();

            if (!string.IsNullOrEmpty(enumCode))
            {
                string filename = Path.Combine(ProjectParameter.ProjectPath, CurrentAttribute.MethodIndexEnumTypeCodeGeneratorPath, $"{MethodIndexEnumTypeName}.cs");
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
            if (IsGeneratorAttribute || !IsGeneratorEnum) Coder.Add(code, generatorAttribute.Language);
            else enumCode = Coder.WarningCode + code + Coder.FileEndCode;
        }
    }
}
