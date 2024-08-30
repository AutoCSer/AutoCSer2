using System;
using System.Threading.Tasks;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// 派生类型模板生成基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal abstract class BaseClassGenerator<T> : Generator, IGenerator
        where T : class
    {
        /// <summary>
        /// 是否调用代码生成
        /// </summary>
        protected virtual bool isRun { get { return true; } }
        /// <summary>
        /// 代码生成入口
        /// </summary>
        /// <param name="parameter">安装参数</param>
        /// <returns>是否生成成功</returns>
        public async Task<bool> Run(ProjectParameter parameter)
        {
            if (isRun)
            {
                ProjectParameter = parameter;
                assembly = parameter.Assembly;
                foreach (Type type in parameter.Types)
                {
                    if (typeof(T).IsAssignableFrom(type))
                    {
                        CurrentType = type;
                        await nextCreate();
                    }
                }
                await onCreated();
            }
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
