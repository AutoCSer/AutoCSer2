using AutoCSer.CodeGenerator.Metadata;
using AutoCSer.CodeGenerator.NetCoreWebView;
using AutoCSer.Extensions;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// WEB 数据视图中间件
    /// </summary>
    [Generator(Name = "WEB 数据视图中间件", DependType = typeof(NetCoreWebView), IsAuto = true, CheckDotNet45 = true)]
    internal partial class NetCoreWebViewMiddleware : Generator, IGenerator
    {
        /// <summary>
        /// 数据视图加载方法信息
        /// </summary>
        public sealed class ViewLoadMethod
        {
            /// <summary>
            /// 数据视图类型
            /// </summary>
            private readonly ExtensionType type;
            /// <summary>
            /// 数据视图类型名称
            /// </summary>
            public string NetCoreWebViewTypeFullName
            {
                get { return type.FullName; }
            }
            /// <summary>
            /// 数据视图实例
            /// </summary>
            private readonly AutoCSer.NetCoreWeb.View view;
            /// <summary>
            /// 数据视图初始化加载方法信息
            /// </summary>
            private readonly MethodIndex method;
            /// <summary>
            /// 第一个参数
            /// </summary>
            public MethodParameter Parameter
            {
                get
                {
                    if (method != null && method.Parameters.Length != 0) return method.Parameters[0];
                    return null;
                }
            }
            /// <summary>
            /// 数据视图加载方法信息
            /// </summary>
            /// <param name="type">数据视图类型</param>
            /// <param name="view">数据视图实例</param>
            /// <param name="method">数据视图初始化加载方法信息</param>
            internal ViewLoadMethod(ExtensionType type, AutoCSer.NetCoreWeb.View view, MethodIndex method)
            {
                this.type = type;
                this.view = view;
                this.method = method;
            }
        }
        /// <summary>
        /// 数据视图加载方法信息集合
        /// </summary>
        public ViewLoadMethod[] Views;
        /// <summary>
        /// 代码生成入口
        /// </summary>
        /// <param name="parameter">安装参数</param>
        /// <returns>是否生成成功</returns>
        public Task<bool> Run(ProjectParameter parameter)
        {
            if (HtmlGenerator.ViewMiddleware != null)
            {
                ProjectParameter = parameter;
                assembly = parameter.Assembly;
                CurrentType = HtmlGenerator.ViewMiddleware.GetType();
                Views = NetCoreWebView.Views.Values.getArray();
                create(true);
            }
            return AutoCSer.Common.TrueCompletedTask;
        }
    }
}
