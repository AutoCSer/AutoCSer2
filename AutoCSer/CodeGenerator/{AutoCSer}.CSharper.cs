﻿//本文件由程序自动生成，请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    internal partial class CommandServerMethodIndexEnumType
    {
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="isOut">是否输出类定义代码</param>
        protected override void create(bool _isOut_)
        {
            if (outStart(AutoCSer.CodeGenerator.CodeLanguageEnum.CSharp, _isOut_))
            {
                
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.InterfaceMethodIndexEnumType<AutoCSer.Net.CommandServerControllerInterfaceAttribute>.MethodInfo[] _value1_;
                    _value1_ = Methods;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.InterfaceMethodIndexEnumType<AutoCSer.Net.CommandServerControllerInterfaceAttribute>.MethodInfo _value2_ in _value1_)
                        {
            _if_ = false;
                    if (_value2_.EnumName != default(string))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value2_.Method != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            /// <summary>
            /// [");
            _code_.Add(_value2_.MethodIndex.ToString());
            _code_.Add(@"] ");
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value3_ = _value2_.Method;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
            _code_.Add(_value3_.XmlDocument);
                    }
                }
                {
                    AutoCSer.CodeGenerator.Metadata.MethodParameter[] _value3_ = default(AutoCSer.CodeGenerator.Metadata.MethodParameter[]);
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value4_ = _value2_.Method;
                    if (_value4_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
                    _value3_ = _value4_.Parameters;
                    }
                }
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.Metadata.MethodParameter _value4_ in _value3_)
                        {
            _code_.Add(@"
            /// ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value5_ = _value4_.ParameterType;
                    if (_value5_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value5_.XmlFullName);
                    }
                }
            _code_.Add(@" ");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@" ");
            _code_.Add(_value4_.XmlDocument);
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                    }
                }
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            /// 返回值 ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.XmlFullName);
                    }
                }
            _code_.Add(@" ");
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value3_ = _value2_.Method;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
            _code_.Add(_value3_.ReturnXmlDocument);
                    }
                }
            }
            _code_.Add(@"
            /// </summary>");
            }
            _code_.Add(@"
            ");
            _code_.Add(_value2_.EnumName);
            _code_.Add(@" = ");
            _code_.Add(_value2_.MethodIndex.ToString());
            _code_.Add(@",");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
                if (_isOut_) outEnd();
            }
        }
    }
}
#if !DotNet45
namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    internal partial class NetCoreWebView
    {
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="isOut">是否输出类定义代码</param>
        protected override void create(bool _isOut_)
        {
            if (outStart(AutoCSer.CodeGenerator.CodeLanguageEnum.CSharp, _isOut_))
            {
                
            _code_.Add(@"
            /// <summary>
            /// 获取请求路径，默认为 类型命令空间+类型名称，用于代码生成
            /// </summary>
            protected override string defaultRequestPath { get { return """);
            _code_.Add(RequestPath);
            _code_.Add(@"""; } }");
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value1_ = default(AutoCSer.CodeGenerator.Metadata.MethodIndex);
                    _value1_ = Method;
            _if_ = false;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value1_.ParameterCount != default(int))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            ");
            _if_ = false;
                    if (IsQueryName)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            /// <summary>
            /// 客户端查询传参
            /// </summary>
            public ");
            }
            _code_.Add(@"struct __QUERYPARAMETER__
            {");
                {
                    System.Collections.Generic.IEnumerable<AutoCSer.CodeGenerator.Metadata.MethodParameter> _value2_;
                    _value2_ = LoadParameters;
                    if (_value2_ != null)
                    {
                        int _loopIndex2_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.Metadata.MethodParameter _value3_ in _value2_)
                        {
            _code_.Add(@"
                public ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value4_ = _value3_.ParameterType;
                    if (_value4_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
            _code_.Add(@" ");
            _code_.Add(_value3_.ParameterName);
            _code_.Add(@";");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex2_;
                    }
                }
            _code_.Add(@"
            }");
            _if_ = false;
                    if (IsQueryName)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            /// <summary>
            /// 客户端查询传参
            /// </summary>
            public __QUERYPARAMETER__ ");
            _code_.Add(QueryParameterName);
            _code_.Add(@";");
            }
            }
            }
                }
            _code_.Add(@"
            /// <summary>
            /// 初始化加载数据（基本操作用代码生成组件处理）
            /// </summary>
            /// <param name=""httpContext"">HTTP 上下文</param>
            /// <param name=""viewRequest"">数据视图信息</param>
            /// <returns></returns>
            protected override async System.Threading.Tasks.Task<AutoCSer.NetCoreWeb.ResponseResult> load(Microsoft.AspNetCore.Http.HttpContext httpContext, AutoCSer.NetCoreWeb.ViewRequest viewRequest)
            {");
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value1_ = default(AutoCSer.CodeGenerator.Metadata.MethodIndex);
                    _value1_ = Method;
            _if_ = false;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value1_.ParameterCount != default(int))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                AutoCSer.NetCoreWeb.ResponseResult<__QUERYPARAMETER__> parameter = await getParameter<__QUERYPARAMETER__>(httpContext, viewRequest);
                if (!parameter.IsSuccess) return parameter;
                ");
            _if_ = false;
                if (!(bool)IsQueryName)
                {
                    _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                __QUERYPARAMETER__ ");
            }
            _code_.Add(QueryParameterName);
            _code_.Add(@" = parameter.Result;");
            _if_ = false;
                    if (CheckParameters != default(AutoCSer.CodeGenerator.TemplateGenerator.NetCoreWebView.CheckParameter[]))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                AutoCSer.NetCoreWeb.ParameterChecker checker = default(AutoCSer.NetCoreWeb.ParameterChecker);");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.NetCoreWebView.CheckParameter[] _value2_;
                    _value2_ = CheckParameters;
                    if (_value2_ != null)
                    {
                        int _loopIndex2_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.NetCoreWebView.CheckParameter _value3_ in _value2_)
                        {
            _if_ = false;
                    if (_value3_.IsCheckNull)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
                {
                    AutoCSer.CodeGenerator.Metadata.MethodParameter _value4_ = default(AutoCSer.CodeGenerator.Metadata.MethodParameter);
                    _value4_ = _value3_.Parameter;
            _if_ = false;
                    if (_value4_ != default(AutoCSer.CodeGenerator.Metadata.MethodParameter))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                if (!AutoCSer.NetCoreWeb.ParameterChecker.CheckNull(");
            _code_.Add(QueryParameterName);
            _code_.Add(@"/**/.");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@", """);
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@""", """);
            _code_.Add(_value4_.XmlDocumentCodeString);
            _code_.Add(@""", ref checker)) return checker.ErrorResult;");
            }
                }
            }
            _if_ = false;
                    if (_value3_.IsCheckEquatable)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
                {
                    AutoCSer.CodeGenerator.Metadata.MethodParameter _value4_ = default(AutoCSer.CodeGenerator.Metadata.MethodParameter);
                    _value4_ = _value3_.Parameter;
            _if_ = false;
                    if (_value4_ != default(AutoCSer.CodeGenerator.Metadata.MethodParameter))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                if (!AutoCSer.NetCoreWeb.ParameterChecker.CheckEquatable(");
            _code_.Add(QueryParameterName);
            _code_.Add(@"/**/.");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@", """);
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@""", """);
            _code_.Add(_value4_.XmlDocumentCodeString);
            _code_.Add(@""", ref checker)) return checker.ErrorResult;");
            }
                }
            }
            _if_ = false;
                    if (_value3_.IsCheckCollection)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
                {
                    AutoCSer.CodeGenerator.Metadata.MethodParameter _value4_ = default(AutoCSer.CodeGenerator.Metadata.MethodParameter);
                    _value4_ = _value3_.Parameter;
            _if_ = false;
                    if (_value4_ != default(AutoCSer.CodeGenerator.Metadata.MethodParameter))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                if (!AutoCSer.NetCoreWeb.ParameterChecker.CheckCollection(");
            _code_.Add(QueryParameterName);
            _code_.Add(@"/**/.");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@", """);
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@""", """);
            _code_.Add(_value4_.XmlDocumentCodeString);
            _code_.Add(@""", ref checker)) return checker.ErrorResult;");
            }
                }
            }
            _if_ = false;
                    if (_value3_.IsCheckString)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
                {
                    AutoCSer.CodeGenerator.Metadata.MethodParameter _value4_ = default(AutoCSer.CodeGenerator.Metadata.MethodParameter);
                    _value4_ = _value3_.Parameter;
            _if_ = false;
                    if (_value4_ != default(AutoCSer.CodeGenerator.Metadata.MethodParameter))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                if (!AutoCSer.NetCoreWeb.ParameterChecker.CheckString(");
            _code_.Add(QueryParameterName);
            _code_.Add(@"/**/.");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@", """);
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@""", """);
            _code_.Add(_value4_.XmlDocumentCodeString);
            _code_.Add(@""", ref checker)) return checker.ErrorResult;");
            }
                }
            }
            _if_ = false;
                    if (_value3_.IsCheckConstraint)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
                {
                    AutoCSer.CodeGenerator.Metadata.MethodParameter _value4_ = default(AutoCSer.CodeGenerator.Metadata.MethodParameter);
                    _value4_ = _value3_.Parameter;
            _if_ = false;
                    if (_value4_ != default(AutoCSer.CodeGenerator.Metadata.MethodParameter))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                if (!AutoCSer.NetCoreWeb.ParameterChecker.CheckConstraint(");
            _code_.Add(QueryParameterName);
            _code_.Add(@"/**/.");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@", """);
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@""", """);
            _code_.Add(_value4_.XmlDocumentCodeString);
            _code_.Add(@""", ref checker)) return checker.ErrorResult;");
            }
                }
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex2_;
                    }
                }
            }
                {
                    AutoCSer.CodeGenerator.Metadata.MethodParameter _value2_ = default(AutoCSer.CodeGenerator.Metadata.MethodParameter);
                    _value2_ = AccessTokenParameter;
            _if_ = false;
                    if (_value2_ != default(AutoCSer.CodeGenerator.Metadata.MethodParameter))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
                if (viewRequest.IsAccessTokenParameter)
                {
                    AutoCSer.NetCoreWeb.ResponseResult checkResult = await viewRequest.ViewMiddleware.CheckAccessTokenParameter(");
            _code_.Add(QueryParameterName);
            _code_.Add(@"/**/.");
            _code_.Add(_value2_.ParameterName);
            _code_.Add(@");
                    if (!checkResult.IsSuccess) return checkResult;
                }");
            }
                }
            }
            _code_.Add(@"
                AutoCSer.NetCoreWeb.ResponseResult loadResult = await LoadView(");
            _if_ = false;
                    if (IsHttpContextParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"httpContext");
            }
            _if_ = false;
                    if (IsHttpContextParameterJoin)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@", ");
            }
            _if_ = false;
                    if (IsViewMiddlewareParameter)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"viewRequest.ViewMiddleware");
            }
            _if_ = false;
                    if (IsViewMiddlewareParameterJoin)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@", ");
            }
                {
                    System.Collections.Generic.IEnumerable<AutoCSer.CodeGenerator.Metadata.MethodParameter> _value2_;
                    _value2_ = LoadParameters;
                    if (_value2_ != null)
                    {
                        int _loopIndex2_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.Metadata.MethodParameter _value3_ in _value2_)
                        {
            _code_.Add(QueryParameterName);
            _code_.Add(@"/**/.");
            _code_.Add(_value3_.ParameterJoinName);
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex2_;
                    }
                }
            _code_.Add(@");
                if (!loadResult.IsSuccess) return loadResult;");
            }
                }
            _code_.Add(@"
                AutoCSer.NetCoreWeb.ViewResponse response = getResponse();
                try
                {
                    AutoCSer.Memory.CharStream stream = responseStart(httpContext, viewRequest, ref response);
                    ");
            _code_.Add(ViewCode);
            _code_.Add(@"
                    await responseEnd(httpContext, viewRequest, response);
                    return AutoCSer.NetCoreWeb.ResponseStateEnum.Success;
                }
                finally { response.Free(); }
            }");
                if (_isOut_) outEnd();
            }
        }
    }
}
#endif
#if !DotNet45
namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    internal partial class NetCoreWebViewMiddleware
    {
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="isOut">是否输出类定义代码</param>
        protected override void create(bool _isOut_)
        {
            if (outStart(AutoCSer.CodeGenerator.CodeLanguageEnum.CSharp, _isOut_))
            {
                
            _code_.Add(@"
            /// <summary>
            /// 用于代码生成
            /// </summary>
            private ");
            _code_.Add(TypeName);
            _code_.Add(@"() { }
            /// <summary>
            /// 数据视图中间件
            /// </summary>
            /// <param name=""nextRequest""></param>
            public ");
            _code_.Add(TypeName);
            _code_.Add(@"(Microsoft.AspNetCore.Http.RequestDelegate nextRequest) : base(nextRequest)
            {");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.NetCoreWebViewMiddleware.ViewLoadMethod[] _value1_;
                    _value1_ = Views;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.NetCoreWebViewMiddleware.ViewLoadMethod _value2_ in _value1_)
                        {
            _code_.Add(@"
                appendView(new AutoCSer.NetCoreWeb.ViewRequest(this, typeof(");
            _code_.Add(_value2_.NetCoreWebViewTypeFullName);
            _code_.Add(@"), () => new ");
            _code_.Add(_value2_.NetCoreWebViewTypeFullName);
            _code_.Add(@"()");
                {
                    AutoCSer.CodeGenerator.Metadata.MethodParameter _value3_ = default(AutoCSer.CodeGenerator.Metadata.MethodParameter);
                    _value3_ = _value2_.Parameter;
            _if_ = false;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.MethodParameter))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@", typeof(");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value4_ = _value3_.ParameterType;
                    if (_value4_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value4_.FullName);
                    }
                }
            _code_.Add(@"), """);
            _code_.Add(_value3_.ParameterName);
            _code_.Add(@"""");
            }
                }
            _code_.Add(@"));");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
                AutoCSer.Threading.CatchTask.Add(load());
            }");
                if (_isOut_) outEnd();
            }
        }
    }
}
#endif
namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    internal partial class StreamPersistenceMemoryDatabaseLocalClientNode
    {
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="isOut">是否输出类定义代码</param>
        protected override void create(bool _isOut_)
        {
            if (outStart(AutoCSer.CodeGenerator.CodeLanguageEnum.CSharp, _isOut_))
            {
                
            _code_.Add(@"
        /// <summary>
        /// ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.XmlDocument);
                    }
                }
            _code_.Add(@" 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.GenericDefinitionFullName);
                    }
                }
            _code_.Add(@"))]
        ");
            _code_.Add(TypeNameDefinition);
            _code_.Add(@"
        {");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.StreamPersistenceMemoryDatabaseLocalClientNode.NodeMethod[] _value1_;
                    _value1_ = Methods;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.StreamPersistenceMemoryDatabaseLocalClientNode.NodeMethod _value2_ in _value1_)
                        {
            _if_ = false;
                    if (_value2_.Method != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            /// <summary>
            /// ");
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value3_ = _value2_.Method;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
            _code_.Add(_value3_.XmlDocument);
                    }
                }
            _code_.Add(@"
            /// </summary>");
                {
                    AutoCSer.CodeGenerator.Metadata.MethodParameter[] _value3_ = default(AutoCSer.CodeGenerator.Metadata.MethodParameter[]);
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value4_ = _value2_.Method;
                    if (_value4_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
                    _value3_ = _value4_.Parameters;
                    }
                }
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.Metadata.MethodParameter _value4_ in _value3_)
                        {
            _code_.Add(@"
            /// <param name=""");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@""">");
            _code_.Add(_value4_.XmlDocument);
            _code_.Add(@"</param>");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                    }
                }
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            /// <returns>");
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value3_ = _value2_.Method;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
            _code_.Add(_value3_.ReturnXmlDocument);
                    }
                }
            _code_.Add(@"</returns>");
            }
            _code_.Add(@"
            ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@" ");
            _code_.Add(_value2_.MethodName);
            _code_.Add(@"(");
                {
                    AutoCSer.CodeGenerator.Metadata.MethodParameter[] _value3_ = default(AutoCSer.CodeGenerator.Metadata.MethodParameter[]);
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value4_ = _value2_.Method;
                    if (_value4_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
                    _value3_ = _value4_.Parameters;
                    }
                }
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.Metadata.MethodParameter _value4_ in _value3_)
                        {
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value5_ = _value4_.ParameterType;
                    if (_value5_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value5_.FullName);
                    }
                }
            _code_.Add(@" ");
            _code_.Add(_value4_.ParameterJoinName);
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                    }
                }
            _code_.Add(@");");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
        }");
                if (_isOut_) outEnd();
            }
        }
    }
}
namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    internal partial class StreamPersistenceMemoryDatabaseClientNode
    {
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="isOut">是否输出类定义代码</param>
        protected override void create(bool _isOut_)
        {
            if (outStart(AutoCSer.CodeGenerator.CodeLanguageEnum.CSharp, _isOut_))
            {
                
            _code_.Add(@"
        /// <summary>
        /// ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.XmlDocument);
                    }
                }
            _code_.Add(@" 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = CurrentType;
                    if (_value1_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value1_.GenericDefinitionFullName);
                    }
                }
            _code_.Add(@"))]
        ");
            _code_.Add(TypeNameDefinition);
            _code_.Add(@"
        {");
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.StreamPersistenceMemoryDatabaseClientNode.NodeMethod[] _value1_;
                    _value1_ = Methods;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.StreamPersistenceMemoryDatabaseClientNode.NodeMethod _value2_ in _value1_)
                        {
            _if_ = false;
                    if (_value2_.Method != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            /// <summary>
            /// ");
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value3_ = _value2_.Method;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
            _code_.Add(_value3_.XmlDocument);
                    }
                }
            _code_.Add(@"
            /// </summary>");
                {
                    AutoCSer.CodeGenerator.Metadata.MethodParameter[] _value3_ = default(AutoCSer.CodeGenerator.Metadata.MethodParameter[]);
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value4_ = _value2_.Method;
                    if (_value4_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
                    _value3_ = _value4_.Parameters;
                    }
                }
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.Metadata.MethodParameter _value4_ in _value3_)
                        {
            _code_.Add(@"
            /// <param name=""");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@""">");
            _code_.Add(_value4_.XmlDocument);
            _code_.Add(@"</param>");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                    }
                }
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            /// <returns>");
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value3_ = _value2_.Method;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
            _code_.Add(_value3_.ReturnXmlDocument);
                    }
                }
            _code_.Add(@"</returns>");
            }
            _code_.Add(@"
            ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@" ");
            _code_.Add(_value2_.MethodName);
            _code_.Add(@"(");
                {
                    AutoCSer.CodeGenerator.Metadata.MethodParameter[] _value3_ = default(AutoCSer.CodeGenerator.Metadata.MethodParameter[]);
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value4_ = _value2_.Method;
                    if (_value4_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
                    _value3_ = _value4_.Parameters;
                    }
                }
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.Metadata.MethodParameter _value4_ in _value3_)
                        {
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value5_ = _value4_.ParameterType;
                    if (_value5_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value5_.FullName);
                    }
                }
            _code_.Add(@" ");
            _code_.Add(_value4_.ParameterJoinName);
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                    }
                }
            _code_.Add(@");");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
            _code_.Add(@"
        }");
                if (_isOut_) outEnd();
            }
        }
    }
}
namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    internal partial class StreamPersistenceMemoryDatabaseMethodIndexEnumType
    {
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="isOut">是否输出类定义代码</param>
        protected override void create(bool _isOut_)
        {
            if (outStart(AutoCSer.CodeGenerator.CodeLanguageEnum.CSharp, _isOut_))
            {
                
                {
                    AutoCSer.CodeGenerator.TemplateGenerator.InterfaceMethodIndexEnumType<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeAttribute>.MethodInfo[] _value1_;
                    _value1_ = Methods;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.TemplateGenerator.InterfaceMethodIndexEnumType<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeAttribute>.MethodInfo _value2_ in _value1_)
                        {
            _if_ = false;
                    if (_value2_.EnumName != default(string))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _if_ = false;
                    if (_value2_.Method != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            /// <summary>
            /// [");
            _code_.Add(_value2_.MethodIndex.ToString());
            _code_.Add(@"] ");
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value3_ = _value2_.Method;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
            _code_.Add(_value3_.XmlDocument);
                    }
                }
                {
                    AutoCSer.CodeGenerator.Metadata.MethodParameter[] _value3_ = default(AutoCSer.CodeGenerator.Metadata.MethodParameter[]);
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value4_ = _value2_.Method;
                    if (_value4_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
                    _value3_ = _value4_.Parameters;
                    }
                }
                    if (_value3_ != null)
                    {
                        int _loopIndex3_ = _loopIndex_;
                        _loopIndex_ = 0;
                        foreach (AutoCSer.CodeGenerator.Metadata.MethodParameter _value4_ in _value3_)
                        {
            _code_.Add(@"
            /// ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value5_ = _value4_.ParameterType;
                    if (_value5_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value5_.XmlFullName);
                    }
                }
            _code_.Add(@" ");
            _code_.Add(_value4_.ParameterName);
            _code_.Add(@" ");
            _code_.Add(_value4_.XmlDocument);
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex3_;
                    }
                }
            _if_ = false;
                    if (_value2_.MethodIsReturn)
                    {
                        _if_ = true;
                }
            if (_if_)
            {
            _code_.Add(@"
            /// 返回值 ");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MethodReturnType;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.ExtensionType))
                    {
            _code_.Add(_value3_.XmlFullName);
                    }
                }
            _code_.Add(@" ");
                {
                    AutoCSer.CodeGenerator.Metadata.MethodIndex _value3_ = _value2_.Method;
                    if (_value3_ != default(AutoCSer.CodeGenerator.Metadata.MethodIndex))
                    {
            _code_.Add(_value3_.ReturnXmlDocument);
                    }
                }
            }
            _code_.Add(@"
            /// </summary>");
            }
            _code_.Add(@"
            ");
            _code_.Add(_value2_.EnumName);
            _code_.Add(@" = ");
            _code_.Add(_value2_.MethodIndex.ToString());
            _code_.Add(@",");
            }
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                    }
                }
                if (_isOut_) outEnd();
            }
        }
    }
}
#endif