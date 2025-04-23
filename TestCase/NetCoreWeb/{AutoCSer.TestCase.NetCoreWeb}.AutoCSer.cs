//本文件由程序自动生成，请不要自行修改
using System;
using System.Numerics;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.TestCase.NetCoreWeb
{
        /// <summary>
        /// 数据视图示例，必须使用 partial 修饰符用于生成静态代码，每一个示例必须对应 类型名称.page.html 页面模板文件
        /// </summary>
    public partial class ExampleView
    {
            /// <summary>
            /// 获取请求路径，默认为 类型命令空间+类型名称，用于代码生成
            /// </summary>
            protected override string defaultRequestPath { get { return "/ExampleView"; } }
            
            /// <summary>
            /// 客户端查询传参
            /// </summary>
            public struct __QUERYPARAMETER__
            {
#pragma warning disable CS0649
                public int left;
                public int right;
#pragma warning restore CS0649
            }
#pragma warning disable CS0649
            /// <summary>
            /// 客户端查询传参
            /// </summary>
            public __QUERYPARAMETER__ Parameters;
#pragma warning restore CS0649
            /// <summary>
            /// 初始化加载数据（基本操作用代码生成组件处理）
            /// </summary>
            /// <param name="httpContext">HTTP 上下文</param>
            /// <param name="viewRequest">数据视图信息</param>
            /// <returns></returns>
            protected override async System.Threading.Tasks.Task<AutoCSer.NetCoreWeb.ResponseResult> load(Microsoft.AspNetCore.Http.HttpContext httpContext, AutoCSer.NetCoreWeb.ViewRequest viewRequest)
            {
                AutoCSer.NetCoreWeb.ResponseResult<__QUERYPARAMETER__> parameter = await getParameter<__QUERYPARAMETER__>(httpContext, viewRequest);
                if (!parameter.IsSuccess) return parameter;
                Parameters = parameter.Result;
                AutoCSer.NetCoreWeb.ParameterChecker checker = default(AutoCSer.NetCoreWeb.ParameterChecker);
                if (!AutoCSer.NetCoreWeb.ParameterChecker.CheckEquatable(Parameters/**/.left, "left", "客户端传参", ref checker)) return checker.ErrorResult;
                if (!AutoCSer.NetCoreWeb.ParameterChecker.CheckEquatable(Parameters/**/.right, "right", "客户端传参", ref checker)) return checker.ErrorResult;
                AutoCSer.NetCoreWeb.ResponseResult loadResult = await LoadView(Parameters/**/.left, Parameters/**/.right);
                if (!loadResult.IsSuccess) return loadResult;
                AutoCSer.NetCoreWeb.ViewResponse response = getResponse();
                try
                {
                    AutoCSer.Memory.CharStream stream = responseStart(httpContext, viewRequest, ref response);
                    stream.Write(@"{DataList:");
                    {
                        System.Collections.Generic.IEnumerable<AutoCSer.TestCase.NetCoreWeb.ExampleData> _value1_ = DataList;
                        if (_value1_ == null) stream.WriteJsonNull();
                        else
                        {
                            stream.Write('[');
                    {
                        int _loopIndex2_ = 0;
                        foreach (AutoCSer.TestCase.NetCoreWeb.ExampleData _value2_ in _value1_)
                        {
                    if (_loopIndex2_++ == 0)
                    {
                        stream.Write('"');
                        stream.Write("IntData,NextData[IntData,StringData]StringData");
                        stream.Write('"');
                    }
                    stream.Write(',');
                    if (_value2_ == null) stream.WriteJsonNull();
                    else
                    {
                    stream.Write('[');
                    {
                        int _value3_ = _value2_.IntData;
                    stream.WriteWebViewJson((System.Int32)_value3_);
                    }
                    stream.Write(',');
                    {
                        AutoCSer.TestCase.NetCoreWeb.ExampleData _value3_ = _value2_.NextData;
                                if (_value3_ == null) stream.WriteJsonNull();
                                else
                                {
                                    stream.Write('[');
                    {
                        int _value4_ = _value3_.IntData;
                    stream.WriteWebViewJson((System.Int32)_value4_);
                    }
                    stream.Write(',');
                    {
                        string _value4_ = _value3_.StringData;
                                if (_value4_ == null) stream.WriteJsonNull();
                                else
                                {
                    stream.WriteWebViewJson(_value4_);
                                }
                    }
                    stream.Write(']');
                                }
                    }
                    stream.Write(',');
                    {
                        string _value3_ = _value2_.StringData;
                                if (_value3_ == null) stream.WriteJsonNull();
                                else
                                {
                    stream.WriteWebViewJson(_value3_);
                                }
                    }
                    stream.Write(']');
                    }
                        }
                    }
                    stream.Write(@"].FormatView()");
                        }
                    }
                    stream.Write(@",Left10:");
                    {
                        System.Collections.Generic.IEnumerable<int> _value1_ = Left10;
                        if (_value1_ == null) stream.WriteJsonNull();
                        else
                        {
                            stream.Write('[');
                    {
                        int _loopIndex2_ = 0;
                        foreach (int _value2_ in _value1_)
                        {
                    if (_loopIndex2_++ != 0) stream.Write(',');
                    stream.WriteWebViewJson((System.Int32)_value2_);
                        }
                    }
                    stream.Write(']');
                        }
                    }
                    stream.Write(@",Parameters:");
                    {
                        AutoCSer.TestCase.NetCoreWeb.ExampleView.__QUERYPARAMETER__ _value1_ = Parameters;
                            stream.Write(@"{left:");
                    {
                        int _value2_ = _value1_.left;
                    stream.WriteWebViewJson((System.Int32)_value2_);
                    }
                    stream.Write(@",right:");
                    {
                        int _value2_ = _value1_.right;
                    stream.WriteWebViewJson((System.Int32)_value2_);
                    }
                    stream.Write('}');
                    }
                    stream.Write(@",Sum:");
                    {
                        int _value1_ = Sum;
                    stream.WriteWebViewJson((System.Int32)_value1_);
                    }
                    stream.Write(@",TaskSum:");
                    {
                        System.Threading.Tasks.Task<int> _awaitValue1_ = TaskSum;
                        int _value1_ = _awaitValue1_ != null ? await _awaitValue1_ : default(int);
                        if (_awaitValue1_ == null) stream.WriteJsonNull();
                        else
                        {
                    stream.WriteWebViewJson((System.Int32)_value1_);
                        }
                    }
                    stream.Write('}');
                    await responseEnd(httpContext, viewRequest, response);
                    return AutoCSer.NetCoreWeb.ResponseStateEnum.Success;
                }
                finally { response.Free(); }
            }
    }
}namespace AutoCSer.TestCase.NetCoreWeb
{
        /// <summary>
        /// 数据视图 API 帮助文档数据视图
        /// </summary>
    public partial class ViewHelp
    {
            /// <summary>
            /// 获取请求路径，默认为 类型命令空间+类型名称，用于代码生成
            /// </summary>
            protected override string defaultRequestPath { get { return "/ViewHelp"; } }
            struct __QUERYPARAMETER__
            {
#pragma warning disable CS0649
                public AutoCSer.TestCase.NetCoreWeb.ViewHelpTypeEnum type;
                public string controllerTypeFullName;
                public string typeFullName;
#pragma warning restore CS0649
            }
            /// <summary>
            /// 初始化加载数据（基本操作用代码生成组件处理）
            /// </summary>
            /// <param name="httpContext">HTTP 上下文</param>
            /// <param name="viewRequest">数据视图信息</param>
            /// <returns></returns>
            protected override async System.Threading.Tasks.Task<AutoCSer.NetCoreWeb.ResponseResult> load(Microsoft.AspNetCore.Http.HttpContext httpContext, AutoCSer.NetCoreWeb.ViewRequest viewRequest)
            {
                AutoCSer.NetCoreWeb.ResponseResult<__QUERYPARAMETER__> parameter = await getParameter<__QUERYPARAMETER__>(httpContext, viewRequest);
                if (!parameter.IsSuccess) return parameter;
                
                __QUERYPARAMETER__ QueryParameterName = parameter.Result;
                AutoCSer.NetCoreWeb.ResponseResult loadResult = await LoadView(viewRequest.ViewMiddleware, QueryParameterName/**/.type, QueryParameterName/**/.controllerTypeFullName, QueryParameterName/**/.typeFullName);
                if (!loadResult.IsSuccess) return loadResult;
                AutoCSer.NetCoreWeb.ViewResponse response = getResponse();
                try
                {
                    AutoCSer.Memory.CharStream stream = responseStart(httpContext, viewRequest, ref response);
                    stream.Write(@"{Controller:");
                    {
                        AutoCSer.NetCoreWeb.JsonApiControllerHelpView _value1_ = Controller;
                        if (_value1_ == null) stream.WriteJsonNull();
                        else
                        {
                            stream.Write(@"{Methods:");
                    {
                        AutoCSer.NetCoreWeb.JsonApiHelpView[] _value2_ = _value1_.Methods;
                        if (_value2_ == null) stream.WriteJsonNull();
                        else
                        {
                            stream.Write('[');
                    {
                        int _loopIndex3_ = 0;
                        foreach (AutoCSer.NetCoreWeb.JsonApiHelpView _value3_ in _value2_)
                        {
                    if (_loopIndex3_++ == 0)
                    {
                        stream.Write('"');
                        stream.Write("Parameters[[Name,Summary,Type[FullName,IsEnumerableType,IsHelp,IsNullableType]]]ReturnSummary,ReturnType[FullName,IsEnumerableType,IsHelp,IsNullableType]RoutePath,Summary");
                        stream.Write('"');
                    }
                    stream.Write(',');
                    if (_value3_ == null) stream.WriteJsonNull();
                    else
                    {
                    stream.Write('[');
                    {
                        AutoCSer.NetCoreWeb.JsonApiParameterHelpView[] _value4_ = _value3_.Parameters;
                                if (_value4_ == null) stream.WriteJsonNull();
                                else
                                {
                                    stream.Write(@"[[");
                    {
                        int _loopIndex5_ = 0;
                        foreach (AutoCSer.NetCoreWeb.JsonApiParameterHelpView _value5_ in _value4_)
                        {
                            if (_loopIndex5_++ != 0) stream.Write(',');
                            if (_value5_ == null) stream.WriteJsonNull();
                            else
                            {
                                stream.Write('[');
                                
                    {
                        string _value6_ = _value5_.Name;
                                if (_value6_ == null) stream.WriteJsonNull();
                                else
                                {
                    stream.WriteWebViewJson(_value6_);
                                }
                    }
                    stream.Write(',');
                    {
                        string _value6_ = _value5_.Summary;
                                if (_value6_ == null) stream.WriteJsonNull();
                                else
                                {
                    stream.WriteWebViewJson(_value6_);
                                }
                    }
                    stream.Write(',');
                    {
                        AutoCSer.NetCoreWeb.TypeHelpView _value6_ = _value5_.Type;
                                if (_value6_ == null) stream.WriteJsonNull();
                                else
                                {
                                    stream.Write('[');
                    {
                        string _value7_ = _value6_.FullName;
                                if (_value7_ == null) stream.WriteJsonNull();
                                else
                                {
                    stream.WriteWebViewJson(_value7_);
                                }
                    }
                    stream.Write(',');
                    {
                        bool _value7_ = _value6_.IsEnumerableType;
                    stream.WriteWebViewJson((System.Boolean)_value7_);
                    }
                    stream.Write(',');
                    {
                        bool _value7_ = _value6_.IsHelp;
                    stream.WriteWebViewJson((System.Boolean)_value7_);
                    }
                    stream.Write(',');
                    {
                        bool _value7_ = _value6_.IsNullableType;
                    stream.WriteWebViewJson((System.Boolean)_value7_);
                    }
                    stream.Write(']');
                                }
                    }
                    stream.Write(']');
                            }
                        }
                    }
                    stream.Write(@"]]");
                                }
                    }
                    stream.Write(',');
                    {
                        string _value4_ = _value3_.ReturnSummary;
                                if (_value4_ == null) stream.WriteJsonNull();
                                else
                                {
                    stream.WriteWebViewJson(_value4_);
                                }
                    }
                    stream.Write(',');
                    {
                        AutoCSer.NetCoreWeb.TypeHelpView _value4_ = _value3_.ReturnType;
                                if (_value4_ == null) stream.WriteJsonNull();
                                else
                                {
                                    stream.Write('[');
                    {
                        string _value5_ = _value4_.FullName;
                                if (_value5_ == null) stream.WriteJsonNull();
                                else
                                {
                    stream.WriteWebViewJson(_value5_);
                                }
                    }
                    stream.Write(',');
                    {
                        bool _value5_ = _value4_.IsEnumerableType;
                    stream.WriteWebViewJson((System.Boolean)_value5_);
                    }
                    stream.Write(',');
                    {
                        bool _value5_ = _value4_.IsHelp;
                    stream.WriteWebViewJson((System.Boolean)_value5_);
                    }
                    stream.Write(',');
                    {
                        bool _value5_ = _value4_.IsNullableType;
                    stream.WriteWebViewJson((System.Boolean)_value5_);
                    }
                    stream.Write(']');
                                }
                    }
                    stream.Write(',');
                    {
                        string _value4_ = _value3_.RoutePath;
                                if (_value4_ == null) stream.WriteJsonNull();
                                else
                                {
                    stream.WriteWebViewJson(_value4_);
                                }
                    }
                    stream.Write(',');
                    {
                        string _value4_ = _value3_.Summary;
                                if (_value4_ == null) stream.WriteJsonNull();
                                else
                                {
                    stream.WriteWebViewJson(_value4_);
                                }
                    }
                    stream.Write(']');
                    }
                        }
                    }
                    stream.Write(@"].FormatView()");
                        }
                    }
                    stream.Write(@",Type:");
                    {
                        AutoCSer.NetCoreWeb.TypeHelpView _value2_ = _value1_.Type;
                        if (_value2_ == null) stream.WriteJsonNull();
                        else
                        {
                            stream.Write(@"{FullName:");
                    {
                        string _value3_ = _value2_.FullName;
                        if (_value3_ == null) stream.WriteJsonNull();
                        else
                        {
                    stream.WriteWebViewJson(_value3_);
                        }
                    }
                    stream.Write(@",Summary:");
                    {
                        string _value3_ = _value2_.Summary;
                        if (_value3_ == null) stream.WriteJsonNull();
                        else
                        {
                    stream.WriteWebViewJson(_value3_);
                        }
                    }
                    stream.Write('}');
                        }
                    }
                    stream.Write('}');
                        }
                    }
                    stream.Write(@",Controllers:");
                    {
                        System.Collections.Generic.ICollection<AutoCSer.NetCoreWeb.JsonApiControllerHelpView> _value1_ = Controllers;
                        if (_value1_ == null) stream.WriteJsonNull();
                        else
                        {
                            stream.Write('[');
                    {
                        int _loopIndex2_ = 0;
                        foreach (AutoCSer.NetCoreWeb.JsonApiControllerHelpView _value2_ in _value1_)
                        {
                    if (_loopIndex2_++ == 0)
                    {
                        stream.Write('"');
                        stream.Write("Type[FullName,Summary]");
                        stream.Write('"');
                    }
                    stream.Write(',');
                    if (_value2_ == null) stream.WriteJsonNull();
                    else
                    {
                    stream.Write('[');
                    {
                        AutoCSer.NetCoreWeb.TypeHelpView _value3_ = _value2_.Type;
                                if (_value3_ == null) stream.WriteJsonNull();
                                else
                                {
                                    stream.Write('[');
                    {
                        string _value4_ = _value3_.FullName;
                                if (_value4_ == null) stream.WriteJsonNull();
                                else
                                {
                    stream.WriteWebViewJson(_value4_);
                                }
                    }
                    stream.Write(',');
                    {
                        string _value4_ = _value3_.Summary;
                                if (_value4_ == null) stream.WriteJsonNull();
                                else
                                {
                    stream.WriteWebViewJson(_value4_);
                                }
                    }
                    stream.Write(']');
                                }
                    }
                    stream.Write(']');
                    }
                        }
                    }
                    stream.Write(@"].FormatView()");
                        }
                    }
                    stream.Write(@",Type:");
                    {
                        AutoCSer.NetCoreWeb.TypeHelpView _value1_ = Type;
                        if (_value1_ == null) stream.WriteJsonNull();
                        else
                        {
                            stream.Write(@"{Enums:");
                    {
                        AutoCSer.NetCoreWeb.EnumHelpView[] _value2_ = _value1_.Enums;
                        if (_value2_ == null) stream.WriteJsonNull();
                        else
                        {
                            stream.Write('[');
                    {
                        int _loopIndex3_ = 0;
                        foreach (AutoCSer.NetCoreWeb.EnumHelpView _value3_ in _value2_)
                        {
                    if (_loopIndex3_++ == 0)
                    {
                        stream.Write('"');
                        stream.Write("Name,Value");
                        stream.Write('"');
                    }
                    stream.Write(',');
                    stream.Write('[');
                    {
                        string _value4_ = _value3_.Name;
                                if (_value4_ == null) stream.WriteJsonNull();
                                else
                                {
                    stream.WriteWebViewJson(_value4_);
                                }
                    }
                    stream.Write(',');
                    {
                        ulong _value4_ = _value3_.Value;
                    stream.WriteWebViewJson((System.UInt64)_value4_);
                    }
                    stream.Write(']');
                        }
                    }
                    stream.Write(@"].FormatView()");
                        }
                    }
                    stream.Write(@",FullName:");
                    {
                        string _value2_ = _value1_.FullName;
                        if (_value2_ == null) stream.WriteJsonNull();
                        else
                        {
                    stream.WriteWebViewJson(_value2_);
                        }
                    }
                    stream.Write(@",IsEnum:");
                    {
                        bool _value2_ = _value1_.IsEnum;
                    stream.WriteWebViewJson((System.Boolean)_value2_);
                    }
                    stream.Write(@",Members:");
                    {
                        AutoCSer.NetCoreWeb.MemberHelpView[] _value2_ = _value1_.Members;
                        if (_value2_ == null) stream.WriteJsonNull();
                        else
                        {
                            stream.Write('[');
                    {
                        int _loopIndex3_ = 0;
                        foreach (AutoCSer.NetCoreWeb.MemberHelpView _value3_ in _value2_)
                        {
                    if (_loopIndex3_++ == 0)
                    {
                        stream.Write('"');
                        stream.Write("CanRead,CanWrite,Name,Summary,Type[FullName,IsEnumerableType,IsHelp,IsNullableType]");
                        stream.Write('"');
                    }
                    stream.Write(',');
                    if (_value3_ == null) stream.WriteJsonNull();
                    else
                    {
                    stream.Write('[');
                    {
                        bool _value4_ = _value3_.CanRead;
                    stream.WriteWebViewJson((System.Boolean)_value4_);
                    }
                    stream.Write(',');
                    {
                        bool _value4_ = _value3_.CanWrite;
                    stream.WriteWebViewJson((System.Boolean)_value4_);
                    }
                    stream.Write(',');
                    {
                        string _value4_ = _value3_.Name;
                                if (_value4_ == null) stream.WriteJsonNull();
                                else
                                {
                    stream.WriteWebViewJson(_value4_);
                                }
                    }
                    stream.Write(',');
                    {
                        string _value4_ = _value3_.Summary;
                                if (_value4_ == null) stream.WriteJsonNull();
                                else
                                {
                    stream.WriteWebViewJson(_value4_);
                                }
                    }
                    stream.Write(',');
                    {
                        AutoCSer.NetCoreWeb.TypeHelpView _value4_ = _value3_.Type;
                                if (_value4_ == null) stream.WriteJsonNull();
                                else
                                {
                                    stream.Write('[');
                    {
                        string _value5_ = _value4_.FullName;
                                if (_value5_ == null) stream.WriteJsonNull();
                                else
                                {
                    stream.WriteWebViewJson(_value5_);
                                }
                    }
                    stream.Write(',');
                    {
                        bool _value5_ = _value4_.IsEnumerableType;
                    stream.WriteWebViewJson((System.Boolean)_value5_);
                    }
                    stream.Write(',');
                    {
                        bool _value5_ = _value4_.IsHelp;
                    stream.WriteWebViewJson((System.Boolean)_value5_);
                    }
                    stream.Write(',');
                    {
                        bool _value5_ = _value4_.IsNullableType;
                    stream.WriteWebViewJson((System.Boolean)_value5_);
                    }
                    stream.Write(']');
                                }
                    }
                    stream.Write(']');
                    }
                        }
                    }
                    stream.Write(@"].FormatView()");
                        }
                    }
                    stream.Write(@",Summary:");
                    {
                        string _value2_ = _value1_.Summary;
                        if (_value2_ == null) stream.WriteJsonNull();
                        else
                        {
                    stream.WriteWebViewJson(_value2_);
                        }
                    }
                    stream.Write('}');
                        }
                    }
                    stream.Write(@",Views:");
                    {
                        System.Collections.Generic.ICollection<AutoCSer.NetCoreWeb.ViewHelpView> _value1_ = Views;
                        if (_value1_ == null) stream.WriteJsonNull();
                        else
                        {
                            stream.Write('[');
                    {
                        int _loopIndex2_ = 0;
                        foreach (AutoCSer.NetCoreWeb.ViewHelpView _value2_ in _value1_)
                        {
                    if (_loopIndex2_++ == 0)
                    {
                        stream.Write('"');
                        stream.Write("Parameters[[Name,Summary,Type[FullName,IsEnumerableType,IsHelp,IsNullableType]]]RequestPath,Type[FullName,IsEnumerableType,IsHelp,IsNullableType,Summary]");
                        stream.Write('"');
                    }
                    stream.Write(',');
                    if (_value2_ == null) stream.WriteJsonNull();
                    else
                    {
                    stream.Write('[');
                    {
                        AutoCSer.NetCoreWeb.ViewLoadParameterHelpView[] _value3_ = _value2_.Parameters;
                                if (_value3_ == null) stream.WriteJsonNull();
                                else
                                {
                                    stream.Write(@"[[");
                    {
                        int _loopIndex4_ = 0;
                        foreach (AutoCSer.NetCoreWeb.ViewLoadParameterHelpView _value4_ in _value3_)
                        {
                            if (_loopIndex4_++ != 0) stream.Write(',');
                            if (_value4_ == null) stream.WriteJsonNull();
                            else
                            {
                                stream.Write('[');
                                
                    {
                        string _value5_ = _value4_.Name;
                                if (_value5_ == null) stream.WriteJsonNull();
                                else
                                {
                    stream.WriteWebViewJson(_value5_);
                                }
                    }
                    stream.Write(',');
                    {
                        string _value5_ = _value4_.Summary;
                                if (_value5_ == null) stream.WriteJsonNull();
                                else
                                {
                    stream.WriteWebViewJson(_value5_);
                                }
                    }
                    stream.Write(',');
                    {
                        AutoCSer.NetCoreWeb.TypeHelpView _value5_ = _value4_.Type;
                                if (_value5_ == null) stream.WriteJsonNull();
                                else
                                {
                                    stream.Write('[');
                    {
                        string _value6_ = _value5_.FullName;
                                if (_value6_ == null) stream.WriteJsonNull();
                                else
                                {
                    stream.WriteWebViewJson(_value6_);
                                }
                    }
                    stream.Write(',');
                    {
                        bool _value6_ = _value5_.IsEnumerableType;
                    stream.WriteWebViewJson((System.Boolean)_value6_);
                    }
                    stream.Write(',');
                    {
                        bool _value6_ = _value5_.IsHelp;
                    stream.WriteWebViewJson((System.Boolean)_value6_);
                    }
                    stream.Write(',');
                    {
                        bool _value6_ = _value5_.IsNullableType;
                    stream.WriteWebViewJson((System.Boolean)_value6_);
                    }
                    stream.Write(']');
                                }
                    }
                    stream.Write(']');
                            }
                        }
                    }
                    stream.Write(@"]]");
                                }
                    }
                    stream.Write(',');
                    {
                        string _value3_ = _value2_.RequestPath;
                                if (_value3_ == null) stream.WriteJsonNull();
                                else
                                {
                    stream.WriteWebViewJson(_value3_);
                                }
                    }
                    stream.Write(',');
                    {
                        AutoCSer.NetCoreWeb.TypeHelpView _value3_ = _value2_.Type;
                                if (_value3_ == null) stream.WriteJsonNull();
                                else
                                {
                                    stream.Write('[');
                    {
                        string _value4_ = _value3_.FullName;
                                if (_value4_ == null) stream.WriteJsonNull();
                                else
                                {
                    stream.WriteWebViewJson(_value4_);
                                }
                    }
                    stream.Write(',');
                    {
                        bool _value4_ = _value3_.IsEnumerableType;
                    stream.WriteWebViewJson((System.Boolean)_value4_);
                    }
                    stream.Write(',');
                    {
                        bool _value4_ = _value3_.IsHelp;
                    stream.WriteWebViewJson((System.Boolean)_value4_);
                    }
                    stream.Write(',');
                    {
                        bool _value4_ = _value3_.IsNullableType;
                    stream.WriteWebViewJson((System.Boolean)_value4_);
                    }
                    stream.Write(',');
                    {
                        string _value4_ = _value3_.Summary;
                                if (_value4_ == null) stream.WriteJsonNull();
                                else
                                {
                    stream.WriteWebViewJson(_value4_);
                                }
                    }
                    stream.Write(']');
                                }
                    }
                    stream.Write(']');
                    }
                        }
                    }
                    stream.Write(@"].FormatView()");
                        }
                    }
                    stream.Write('}');
                    await responseEnd(httpContext, viewRequest, response);
                    return AutoCSer.NetCoreWeb.ResponseStateEnum.Success;
                }
                finally { response.Free(); }
            }
    }
}namespace AutoCSer.TestCase.NetCoreWeb
{
        /// <summary>
        /// 数据视图中间件
        /// </summary>
    public partial class ViewMiddleware
    {
            /// <summary>
            /// 用于代码生成
            /// </summary>
            private ViewMiddleware() { }
            /// <summary>
            /// 数据视图中间件
            /// </summary>
            /// <param name="nextRequest"></param>
            public ViewMiddleware(Microsoft.AspNetCore.Http.RequestDelegate nextRequest) : base(nextRequest)
            {
                appendView(new AutoCSer.NetCoreWeb.ViewRequest(this, typeof(AutoCSer.TestCase.NetCoreWeb.ExampleView), () => new AutoCSer.TestCase.NetCoreWeb.ExampleView(), typeof(int), "left"));
                appendView(new AutoCSer.NetCoreWeb.ViewRequest(this, typeof(AutoCSer.TestCase.NetCoreWeb.ViewHelp), () => new AutoCSer.TestCase.NetCoreWeb.ViewHelp(), typeof(AutoCSer.NetCoreWeb.ViewMiddleware), "viewMiddleware"));
                AutoCSer.Extensions.TaskExtension.Catch(load());
            }
    }
}
#endif