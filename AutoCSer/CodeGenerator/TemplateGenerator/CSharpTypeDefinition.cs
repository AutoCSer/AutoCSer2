using AutoCSer.CodeGenerator.Extensions;
using AutoCSer.Extensions;
using AutoCSer.Reflection;
using System;
using System.Linq;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// C# 类定义生成
    /// </summary>
    internal sealed class CSharpTypeDefinition : TypeDefinition
    {
        /// <summary>
        /// 类定义生成
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="isClass"></param>
        /// <param name="typeNamespace"></param>
        /// <param name="typeNameSuffix">类名称后缀</param>
        internal CSharpTypeDefinition(Type type, bool isClass, string typeNamespace = null, string typeNameSuffix = null)
        {
            create(type, isClass, typeNamespace, typeNameSuffix);
            end.Reverse();
        }
        /// <summary>
        /// 生成类定义
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="isClass">是否建立类定义</param>
        /// <param name="typeNameSuffix">类名称后缀</param>
        /// <param name="typeNamespace"></param>
        private void create(Type type, bool isClass, string typeNamespace, string typeNameSuffix)
        {
            if (type.ReflectedType == null)
            {
                start.Append("namespace ", typeNamespace ?? type.Namespace, @"
{");
                end.Add(@"
}");
            }
            else
            {
                create(type.ReflectedType.IsGenericType ? type.ReflectedType.MakeGenericType(type.GetGenericArguments()) : type.ReflectedType, true, null, null);
            }
            if (isClass)
            {
                string xmlDocument = XmlDocument.Get(type);
                if (!string.IsNullOrEmpty(xmlDocument))
                {
                    start.Append(@"
        /// <summary>
        /// ", XmlDocument.CodeGeneratorFormat(xmlDocument), @"
        /// </summary>");
                }
                start.Append(@"
    ", type.getDefinitionString(typeNameSuffix), @"
    {");
                end.Add(@"
    }");
            }
        }
    }
}
