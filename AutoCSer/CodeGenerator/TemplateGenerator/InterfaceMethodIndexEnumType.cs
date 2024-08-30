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
    /// 接口方法序号映射枚举
    /// </summary>
    /// <typeparam name="T">自定义属性类型</typeparam>
    internal abstract class InterfaceMethodIndexEnumType<T> : AttributeGenerator<T>
        where T : InterfaceMethodIndexAttribute
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
                Methods = AutoCSer.Common.Config.GetCopyArray(Methods, methodIndex + 1);
                foreach (KeyValue<int, string> enumValue in enumValues) Methods[enumValue.Key].EnumName = enumValue.Value;
            }
            methodIndex = 0;
            HashSet<HashString> names = HashSetCreator.CreateHashString();
            foreach (MethodInfo methodEnum in Methods)
            {
                if (methodEnum.EnumName != null && !names.Add(methodEnum.EnumName))
                {
                    throw new Exception($"{CurrentType.Type.fullName()} 生成 {CurrentAttribute.MethodIndexEnumType.fullName()}.{methodEnum.EnumName} 名称冲突");
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
    }
}
