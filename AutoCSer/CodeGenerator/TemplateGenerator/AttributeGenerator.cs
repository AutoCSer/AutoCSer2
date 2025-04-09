using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// 自定义属性模板生成基类
    /// </summary>
    /// <typeparam name="T">自定义属性类型</typeparam>
    internal abstract class AttributeGenerator<T> : Generator, IGenerator
        where T : Attribute
    {
        /// <summary>
        /// 当前处理自定义属性
        /// </summary>
        internal T CurrentAttribute;
        /// <summary>
        /// 是否搜索父类属性
        /// </summary>
        internal virtual bool IsBaseType
        {
            get { return false; }
        }
        /// <summary>
        /// 是否必须配置自定义属性
        /// </summary>
        internal virtual bool IsAttribute
        {
            get { return true; }
        }
        /// <summary>
        /// 获取类型与自定义配置信息
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<KeyValue<Type, T>> GetTypeAttributes()
        {
            foreach (Type type in ProjectParameter.Types)
            {
                T attribute = (T)type.GetCustomAttribute(typeof(T), IsBaseType);
                if ((attribute != null || !IsAttribute) && !type.IsDefined(typeof(AutoCSer.Metadata.IgnoreAttribute), IsBaseType)) yield return new KeyValue<Type, T>(type, attribute);
            }
        }
        /// <summary>
        /// 代码生成入口
        /// </summary>
        /// <param name="parameter">安装参数</param>
        /// <param name="attribute">代码生成器配置</param>
        /// <returns>是否生成成功</returns>
        public async Task<bool> Run(ProjectParameter parameter, GeneratorAttribute attribute)
        {
            ProjectParameter = parameter;
            generatorAttribute = attribute;
            assembly = parameter.Assembly;
            foreach (KeyValue<Type, T> type in GetTypeAttributes())
            {
                CurrentType = type.Key;
                CurrentAttribute = type.Value;
                await nextCreate();
            }
            await onCreated();
            return true;
        }
        /// <summary>
        /// 安装下一个类型
        /// </summary>
        protected abstract Task nextCreate();
        /// <summary>
        /// 安装完成处理
        /// </summary>
        protected abstract Task onCreated();
    }
}
