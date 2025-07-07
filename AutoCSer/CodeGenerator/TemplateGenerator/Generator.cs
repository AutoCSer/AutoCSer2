using AutoCSer.CodeGenerator.Extensions;
using AutoCSer.CodeGenerator.Metadata;
using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// 模板生成基类
    /// </summary>
    internal abstract class Generator
    {
        /// <summary>
        /// 项目参数
        /// </summary>
        internal ProjectParameter ProjectParameter;
        /// <summary>
        /// 代码生成器配置
        /// </summary>
        protected GeneratorAttribute generatorAttribute;
        /// <summary>
        /// 程序集
        /// </summary>
        protected Assembly assembly;
        /// <summary>
        /// 当成处理类型
        /// </summary>
        public ExtensionType CurrentType;
        /// <summary>
        /// 生成的代码
        /// </summary>
        protected ListArray<string> _code_ = new ListArray<string>();
        ///// <summary>
        ///// 代码段
        ///// </summary>
        //protected Dictionary<string, string> _partCodes_ = DictionaryCreator.CreateOnly<string, string>();
        /// <summary>
        /// 生成定义类型
        /// </summary>
        protected virtual ExtensionType definitionType { get { return CurrentType; } }
        /// <summary>
        /// 是否 AOT 环境
        /// </summary>
        public bool IsAOT
        {
            get
            {
#if AOT
                return true;
#else
                return false;
#endif
            }
        }
        /// <summary>
        /// 命名空间
        /// </summary>
        public string Namespace { get { return CurrentType.Type.Namespace; } }
        /// <summary>
        /// 类名称定义
        /// </summary>
        public virtual string TypeNameDefinition { get { return definitionType.GetTypeNameDefinition(typeNameSuffix); } }
        /// <summary>
        /// 类名称
        /// </summary>
        public string TypeName
        {
            get { return CurrentType.Type.Name; }
        }
        /// <summary>
        /// 生成类型名称后缀
        /// </summary>
        protected virtual string typeNameSuffix { get { return null; } }
        /// <summary>
        /// 输出类定义开始段代码是否包含当前类型
        /// </summary>
        protected virtual bool isStartClass { get { return true; } }
        /// <summary>
        /// 类定义生成
        /// </summary>
        protected TypeDefinition _definition_;
        /// <summary>
        /// 临时逻辑变量
        /// </summary>
        protected bool _if_;
        /// <summary>
        /// 当前循环索引
        /// </summary>
        protected int _loopIndex_;
        /// <summary>
        /// 异步关键字
        /// </summary>
        public string Async
        {
            get { return "async"; }
        }
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="isOut">是否输出类定义代码</param>
        protected virtual void create(bool isOut)
        {
            throw new InvalidOperationException(GetType().fullName());
        }
        /// <summary>
        /// 输出类定义开始段代码
        /// </summary>
        /// <param name="isOutDefinition">是否输出类定义</param>
        /// <returns>类定义</returns>
        protected bool outStart(bool isOutDefinition)
        {
            _definition_ = null;
            if (isOutDefinition)
            {
                _code_.Array.Length = 0;
                if (!checkAddType() || Coder.Add(GetType(), CurrentType.Type))
                {
                    switch (generatorAttribute.Language)
                    {
                        case CodeLanguageEnum.JavaScript:
                        case CodeLanguageEnum.TypeScript:
                            _definition_ = new JavaScriptTypeDefinition(definitionType.Type);
                            break;
                        default: _definition_ = new CSharpTypeDefinition(definitionType.Type, isStartClass, null, typeNameSuffix); break;
                    }
                    _code_.Add(_definition_.Start);
                    return true;
                }
                return false;
            }
            return true;
        }
        /// <summary>
        /// 是否检查添加代码类型
        /// </summary>
        /// <returns></returns>
        protected virtual bool checkAddType()
        {
            return true;
        }
        /// <summary>
        /// 输出类定义结束段代码
        /// </summary>
        protected void outEnd()
        {
            _code_.Add(_definition_.End);
            switch (generatorAttribute.Language)
            {
                case CodeLanguageEnum.JavaScript:
                case CodeLanguageEnum.TypeScript:
                    break;
                default:
                    addCode(string.Concat(_code_.Array.ToArray()));
                    return;
            }
        }
        /// <summary>
        /// 添加代码
        /// </summary>
        /// <param name="code"></param>
        protected virtual void addCode(string code)
        {
            Coder.Add(code, generatorAttribute.Language);
        }
        /// <summary>
        /// Get the quantity of data
        /// 获取数据数量
        /// </summary>
        /// <param name="value">数据集合</param>
        /// <returns>null为0</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static int _getCount_<valueType>(ICollection<valueType> value)
        {
            return value != null ? value.Count : 0;
        }
    }
}
