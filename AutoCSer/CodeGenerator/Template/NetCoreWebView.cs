using System;

namespace AutoCSer.CodeGenerator.Template
{
    internal sealed class NetCoreWebView : Pub
    {
        public partial class TypeName : AutoCSer.NetCoreWeb.View
        {
            #region PART CLASS
            /// <summary>
            /// 获取请求路径，默认为 类型命令空间+类型名称，用于代码生成
            /// </summary>
            protected override string defaultRequestPath { get { return "@RequestPath"; } }
            #region PUSH Method
            #region IF ParameterCount
            /*IF:IsQueryName*/
            /// <summary>
            /// 客户端查询传参
            /// </summary>
            public /*IF:IsQueryName*/struct __QUERYPARAMETER__
            {
                #region LOOP LoadParameters
                public @ParameterType.FullName @ParameterName;
                #endregion LOOP LoadParameters
                #region NOTE
                public object ParameterJoinName;
                #endregion NOTE
            }
            #region IF IsQueryName
            /// <summary>
            /// 客户端查询传参
            /// </summary>
            public __QUERYPARAMETER__ @QueryParameterName;
            #endregion IF IsQueryName
            #endregion IF ParameterCount
            #endregion PUSH Method
            /// <summary>
            /// 初始化加载数据（基本操作用代码生成组件处理）
            /// </summary>
            /// <param name="httpContext">HTTP 上下文</param>
            /// <param name="viewRequest">数据视图信息</param>
            /// <returns></returns>
            protected override async System.Threading.Tasks.Task<AutoCSer.NetCoreWeb.ResponseResult> load(Microsoft.AspNetCore.Http.HttpContext httpContext, AutoCSer.NetCoreWeb.ViewRequest viewRequest)
            {
                #region PUSH Method
                #region IF ParameterCount
                AutoCSer.NetCoreWeb.ResponseResult<__QUERYPARAMETER__> parameter = await getParameter<__QUERYPARAMETER__>(httpContext, viewRequest);
                if (!parameter.IsSuccess) return parameter;
                /*NOT:IsQueryName*/
                __QUERYPARAMETER__ /*NOT:IsQueryName*/@QueryParameterName = parameter.Result;
                #region IF CheckParameters
                AutoCSer.NetCoreWeb.ParameterChecker checker = default(AutoCSer.NetCoreWeb.ParameterChecker);
                #region LOOP CheckParameters
                #region IF IsCheckNull
                #region PUSH Parameter
                if (!AutoCSer.NetCoreWeb.ParameterChecker.CheckNull(@QueryParameterName/**/.@ParameterName, "@ParameterName", "@XmlDocumentCodeString", ref checker)) return checker.ErrorResult;
                #endregion PUSH Parameter
                #endregion IF IsCheckNull
                #region IF IsCheckEquatable
                #region PUSH Parameter
                if (!AutoCSer.NetCoreWeb.ParameterChecker.CheckEquatable(@QueryParameterName/**/.@ParameterName, "@ParameterName", "@XmlDocumentCodeString", ref checker)) return checker.ErrorResult;
                #endregion PUSH Parameter
                #endregion IF IsCheckEquatable
                #region IF IsCheckCollection
                #region PUSH Parameter
                if (!AutoCSer.NetCoreWeb.ParameterChecker.CheckCollection(/*NOTE*/(System.Collections.ICollection)(object)/*NOTE*/@QueryParameterName/**/.@ParameterName, "@ParameterName", "@XmlDocumentCodeString", ref checker)) return checker.ErrorResult;
                #endregion PUSH Parameter
                #endregion IF IsCheckCollection
                #region IF IsCheckString
                #region PUSH Parameter
                if (!AutoCSer.NetCoreWeb.ParameterChecker.CheckString(/*NOTE*/(string)(object)/*NOTE*/@QueryParameterName/**/.@ParameterName, "@ParameterName", "@XmlDocumentCodeString", ref checker)) return checker.ErrorResult;
                #endregion PUSH Parameter
                #endregion IF IsCheckString
                #region IF IsCheckConstraint
                #region PUSH Parameter
                if (!AutoCSer.NetCoreWeb.ParameterChecker.CheckConstraint(/*NOTE*/(AutoCSer.NetCoreWeb.IParameterConstraint)(object)/*NOTE*/@QueryParameterName/**/.@ParameterName, "@ParameterName", "@XmlDocumentCodeString", ref checker)) return checker.ErrorResult;
                #endregion PUSH Parameter
                #endregion IF IsCheckConstraint
                #endregion LOOP CheckParameters
                #endregion IF CheckParameters
                #region PUSH AccessTokenParameter
                if (viewRequest.IsAccessTokenParameter)
                {
                    AutoCSer.NetCoreWeb.ResponseResult checkResult = await viewRequest.ViewMiddleware.CheckAccessTokenParameter(@QueryParameterName/**/.@ParameterName);
                    if (!checkResult.IsSuccess) return checkResult;
                }
                #endregion PUSH AccessTokenParameter
                #endregion IF ParameterCount
                AutoCSer.NetCoreWeb.ResponseResult loadResult = await LoadView(/*IF:IsHttpContextParameter*/httpContext/*IF:IsHttpContextParameter*//*IF:IsHttpContextParameterJoin*/, /*IF:IsHttpContextParameterJoin*//*IF:IsViewMiddlewareParameter*/viewRequest.ViewMiddleware/*IF:IsViewMiddlewareParameter*//*IF:IsViewMiddlewareParameterJoin*/, /*IF:IsViewMiddlewareParameterJoin*//*LOOP:LoadParameters*/@QueryParameterName/**/.@ParameterJoinName/*LOOP:LoadParameters*/);
                if (!loadResult.IsSuccess) return loadResult;
                #endregion PUSH Method
                AutoCSer.NetCoreWeb.ViewResponse response = getResponse();
                try
                {
                    AutoCSer.Memory.CharStream stream = responseStart(httpContext, viewRequest, ref response);
                    /*AT:ViewCode*/
                    await responseEnd(httpContext, viewRequest, response);
                    return AutoCSer.NetCoreWeb.ResponseStateEnum.Success;
                }
                finally { response.Free(); }
            }
            #region NOTE
            internal System.Threading.Tasks.Task<AutoCSer.NetCoreWeb.ResponseResult> LoadView(params object[] parameters) { return null; }
            #endregion NOTE
            #endregion PART CLASS
        }
    }
}
