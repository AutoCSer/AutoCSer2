using System;

namespace AutoCSer.CodeGenerator.Template
{
    internal sealed class NetCoreWebViewMiddleware : Pub
    {
        public partial class TypeName : AutoCSer.NetCoreWeb.ViewMiddleware
        {
            #region PART CLASS
            /// <summary>
            /// 用于代码生成
            /// </summary>
            private @TypeName() { }
            /// <summary>
            /// 数据视图中间件
            /// </summary>
            /// <param name="nextRequest"></param>
            public @TypeName(Microsoft.AspNetCore.Http.RequestDelegate nextRequest) : base(nextRequest)
            {
                #region LOOP Views
                appendView(new AutoCSer.NetCoreWeb.ViewRequest(this, typeof(@NetCoreWebViewTypeFullName), () => new @NetCoreWebViewTypeFullName()/*PUSH:Parameter*/, typeof(@ParameterType.FullName), "@ParameterName"/*PUSH:Parameter*/));
                #endregion LOOP Views
                AutoCSer.Threading.CatchTask.Add(load());
            }
            #endregion PART CLASS
        }
    }
    #region NOTE
    /// <summary>
    /// CSharp 模板公用模糊类型
    /// </summary>
    internal partial class Pub
    {
        public partial class NetCoreWebViewTypeFullName : AutoCSer.NetCoreWeb.View
        {
        }
        public static readonly string ParameterName = null;
    }
    #endregion NOTE
}
