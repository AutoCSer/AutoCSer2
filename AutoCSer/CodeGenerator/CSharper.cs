using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// CSharp代码生成数据视图生成
    /// </summary>
    [Generator(Name = "C#", IsTemplate = false)]
    internal sealed class CSharper : IGenerator
    {
        /// <summary>
        /// 类定义生成
        /// </summary>
        private sealed class Definition
        {
            /// <summary>
            /// 类型
            /// </summary>
            internal readonly Type Type;
            /// <summary>
            /// 代码生成器配置
            /// </summary>
            internal readonly GeneratorAttribute Attribute;
            /// <summary>
            /// 安装参数
            /// </summary>
            internal readonly ProjectParameter Parameter;
            /// <summary>
            /// 类定义生成
            /// </summary>
            /// <param name="type"></param>
            /// <param name="attribute"></param>
            /// <param name="parameter"></param>
            internal Definition(Type type, GeneratorAttribute attribute, ProjectParameter parameter)
            {
                Type = type;
                Attribute = attribute;
                Parameter = parameter;
            }
            /// <summary>
            /// 类定义生成
            /// </summary>
            private TemplateGenerator.CSharpTypeDefinition typeDefinition;
            /// <summary>
            /// 模板代码生成器
            /// </summary>
            private Coder coder;
            /// <summary>
            /// 模板代码
            /// </summary>
            private ListArray<string> codeBuilder;
            /// <summary>
            /// 生成类定义字符串
            /// </summary>
            /// <returns>类定义字符串</returns>
            public override string ToString()
            {
                typeDefinition = new TemplateGenerator.CSharpTypeDefinition(Type, true);
                coder = new Coder(Parameter, Type, Attribute.Language);
                coder.SkinEnd(coder.GetNode(Attribute.GetFileName(Type)));
                codeBuilder = new ListArray<string>(0);
#if !DotNet45
                if (Attribute.CheckDotNet45) codeBuilder.Append(@"
", "#if !DotNet45");
#endif
                codeBuilder.Append(@"
", typeDefinition.Start, @"
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name=""isOut"">是否输出类定义代码</param>
        protected override void create(bool _isOut_)
        {
            if (outStart(_isOut_))
            {
                ");
                switch (Attribute.Language)
                {
                    case CodeLanguageEnum.JavaScript:
                    case CodeLanguageEnum.TypeScript:
                        return javaScript();
                    default: return cSharp();
                }
            }
            /// <summary>
            /// 模板结束
            /// </summary>
            /// <returns></returns>
            private string end()
            {
                codeBuilder.Append(@"
                if (_isOut_) outEnd();
            }
        }", typeDefinition.End);
#if !DotNet45
                if (Attribute.CheckDotNet45) codeBuilder.Append(@"
", "#endif");
#endif
                return string.Concat(codeBuilder.Array.ToArray());
            }
            /// <summary>
            /// 生成C#模板代码
            /// </summary>
            /// <returns></returns>
            private string cSharp()
            {
                codeBuilder.Append(coder.PartCodes["CLASS"]);
                return end();
            }
            /// <summary>
            /// 生成JavaScript模板代码
            /// </summary>
            /// <returns></returns>
            private string javaScript()
            {
                codeBuilder.Add(coder.Code);
                return end();
            }
        }
        /// <summary>
        /// 获取类定义生成
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private IEnumerable<Definition> getDefinition(ProjectParameter parameter)
        {
            foreach (Type type in (parameter.IsCustomCodeGenerator ? parameter.Assembly : ProjectParameter.CurrentAssembly).GetTypes())
            {
                GeneratorAttribute attribute = (GeneratorAttribute)type.GetCustomAttribute(typeof(GeneratorAttribute), false);
                if (attribute != null && attribute.IsTemplate) yield return new Definition(type, attribute, parameter);
            }
        }
        /// <summary>
        /// 安装入口
        /// </summary>
        /// <param name="parameter">安装参数</param>
        /// <param name="attribute">代码生成器配置</param>
        /// <returns>是否安装成功</returns>
        public async Task<bool> Run(ProjectParameter parameter, GeneratorAttribute attribute)
        {
            if (parameter.IsAutoCSerCodeGenerator || parameter.IsCustomCodeGenerator)
            {
                LeftArray<string> codes = new LeftArray<string>(0);
                foreach (Definition definition in getDefinition(parameter))
                {
                    codes.Add(definition.ToString());
                    if (Messages.IsError) return false;
                }
#if AOT
                string fileName = parameter.ProjectPath + @"{AutoCSer}.AOT.CSharper.cs";
#else
                string fileName = parameter.ProjectPath + @"{AutoCSer}.CSharper.cs";
#endif
                if (await Coder.WriteFile(fileName, Coder.WarningCode + string.Concat(codes.ToArray()) + Coder.FileEndCode))
                {
                    Messages.Error(fileName + " 被修改");
                    return false;
                }
            }
            return true;
        }
    }
}
