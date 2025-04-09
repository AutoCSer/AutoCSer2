using AutoCSer.CodeGenerator.Metadata;
using AutoCSer.CodeGenerator.NetCoreWebView;
using AutoCSer.NetCoreWeb;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// WEB 数据视图
    /// </summary>
    [Generator(Name = "WEB 数据视图", DependType = typeof(AutoCSer.CodeGenerator.NetCoreWebView.HtmlGenerator), IsAuto = true, CheckDotNet45 = true)]
    internal partial class NetCoreWebView : Generator, IGenerator
    {
        /// <summary>
        /// WEB 数据视图集合
        /// </summary>
        internal static readonly Dictionary<HashObject<Type>, NetCoreWebViewMiddleware.ViewLoadMethod> Views = DictionaryCreator.CreateHashObject<Type, NetCoreWebViewMiddleware.ViewLoadMethod>();

        /// <summary>
        /// 检查参数信息
        /// </summary>
        public sealed class CheckParameter
        {
            /// <summary>
            /// 检查参数信息
            /// </summary>
            public MethodParameter Parameter;
            /// <summary>
            /// 参数约束类型
            /// </summary>
            private readonly ParameterConstraintTypeEnum constraintType;
            /// <summary>
            /// 自定义接口约束
            /// </summary>
            public bool IsCheckNull { get { return constraintType == ParameterConstraintTypeEnum.NotNull; } }
            /// <summary>
            /// 自定义接口约束
            /// </summary>
            public bool IsCheckEquatable { get { return constraintType == ParameterConstraintTypeEnum.NotDefault; } }
            /// <summary>
            /// 非空集合
            /// </summary>
            public bool IsCheckCollection { get { return constraintType == ParameterConstraintTypeEnum.NotEmptyCollection; } }
            /// <summary>
            /// 非空字符串
            /// </summary>
            public bool IsCheckString { get { return constraintType == ParameterConstraintTypeEnum.NotEmptyString; } }
            /// <summary>
            /// 自定义接口约束
            /// </summary>
            public bool IsCheckConstraint { get { return constraintType == ParameterConstraintTypeEnum.ParameterConstraint; } }
            /// <summary>
            /// 检查参数信息
            /// </summary>
            /// <param name="parameter">检查参数信息</param>
            /// <param name="constraintType">参数约束类型</param>
            internal CheckParameter(MethodParameter parameter, ParameterConstraintTypeEnum constraintType)
            {
                Parameter = parameter;
                this.constraintType = constraintType;
            }
        }
        /// <summary>
        /// 请求路径
        /// </summary>
        public string RequestPath
        {
            get { return HtmlGenerator.ViewMiddleware.GetRequestPath(CurrentType.Type); }
        }
        /// <summary>
        /// 数据视图初始化加载方法信息
        /// </summary>
        public MethodIndex Method;
        /// <summary>
        /// 检查参数信息集合
        /// </summary>
        public CheckParameter[] CheckParameters;
        /// <summary>
        /// 鉴权参数
        /// </summary>
        public MethodParameter AccessTokenParameter;
        /// <summary>
        /// 数据视图
        /// </summary>
        private View view;
        /// <summary>
        /// 起始参数位置
        /// </summary>
        private int parameterIndex;
        /// <summary>
        /// 是否存在 HTTP 上下文参数
        /// </summary>
        public bool IsHttpContextParameter;
        /// <summary>
        /// HTTP 上下文参数之后是否存在逗号
        /// </summary>
        public bool IsHttpContextParameterJoin { get { return IsHttpContextParameter&& (IsViewMiddlewareParameter || Method.Parameters.Length != parameterIndex); } }
        /// <summary>
        /// 是否存在数据视图中间件参数
        /// </summary>
        public bool IsViewMiddlewareParameter;
        /// <summary>
        /// 数据视图中间件参数之后是否存在逗号
        /// </summary>
        public bool IsViewMiddlewareParameterJoin { get { return IsViewMiddlewareParameter && Method.Parameters.Length != parameterIndex; } }
        /// <summary>
        /// 加载视图数据参数集合
        /// </summary>
        public IEnumerable<MethodParameter> LoadParameters
        {
            get
            {
                for (int index = parameterIndex; index != Method.Parameters.Length;) yield return Method.Parameters[index++];
            }
        }
        /// <summary>
        /// 是否生成生成参数成员
        /// </summary>
        public bool IsQueryName { get { return !string.IsNullOrEmpty(view.QueryName); } }
        /// <summary>
        /// 参数成员名称
        /// </summary>
        public string QueryParameterName { get { return IsQueryName ? view.QueryName: nameof(QueryParameterName); } }
        /// <summary>
        /// 视图代码
        /// </summary>
        public string ViewCode;
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
            foreach (View view in HtmlGenerator.Views.Values)
            {
                CurrentType = view.GetType();
                string filePath = HtmlGenerator.ViewMiddleware.GetNamespaceTemplateFilePath(CurrentType.Type);
                FileInfo file = new FileInfo(string.IsNullOrEmpty(filePath) ? Path.Combine(ProjectParameter.ProjectPath, $"{CurrentType.Type.Name}.html") : Path.Combine(ProjectParameter.ProjectPath, filePath, $"{CurrentType.Type.Name}.html"));
                if (!await AutoCSer.Common.FileExists(file))
                {
                    Messages.Error($"没有找到 HTML 页面文件 {file.FullName}");
                    return false;
                }
                JavaScriptTreeTemplate template = new JavaScriptTreeTemplate(CurrentType.Type, file.FullName, await File.ReadAllTextAsync(file.FullName, view.ResponseEncoding ?? HtmlGenerator.ViewMiddleware.ResponseEncoding));
                ViewCode = template.GetCode();

                Method = null;
                CheckParameters = null;
                AccessTokenParameter = null;
                parameterIndex = 0;
                IsHttpContextParameter = IsViewMiddlewareParameter = false;
                MethodInfo method = View.GetLoadMethod(CurrentType.Type);
                if (method != null)
                {
                    Method = new MethodIndex(method);
                    if (Method.Parameters.Length != parameterIndex && Method.Parameters[parameterIndex].ParameterType.Type == typeof(HttpContext))
                    {
                        IsHttpContextParameter = true;
                        ++parameterIndex;
                    }
                    if (Method.Parameters.Length != parameterIndex && Method.Parameters[parameterIndex].ParameterType.Type == typeof(ViewMiddleware))
                    {
                        IsViewMiddlewareParameter = true;
                        ++parameterIndex;
                    }
                    if (Method.Parameters.Length != parameterIndex)
                    {
                        LeftArray<CheckParameter> checkParameters = new LeftArray<CheckParameter>(0);
                        for (int index = parameterIndex; index != Method.Parameters.Length;) 
                        {
                            MethodParameter methodParameter = Method.Parameters[index++];
                            HttpMethodParameter httpMethodParameter = new HttpMethodParameter(methodParameter.Parameter, view.IsDefaultParameterConstraint, false);
                            if (httpMethodParameter.ConstraintType != ParameterConstraintTypeEnum.None) checkParameters.Add(new CheckParameter(methodParameter, httpMethodParameter.ConstraintType));
                        }
                        if (checkParameters.Length != 0) CheckParameters = checkParameters.ToArray();
                        if(HtmlGenerator.ViewMiddleware.IsAccessTokenParameter(Method.Parameters[parameterIndex].Parameter)) AccessTokenParameter = Method.Parameters[parameterIndex];
                    }
                }
                Views.Add(CurrentType.Type, new NetCoreWebViewMiddleware.ViewLoadMethod(CurrentType, view, Method));
                this.view = view;
                create(true);
            }
            return true;
        }
    }
}
