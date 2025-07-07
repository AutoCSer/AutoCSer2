using System;

namespace AutoCSer.CodeGenerator.Template
{
    internal sealed class NetCoreWebViewMiddleware : Pub
    {
        public partial class TypeName : AutoCSer.NetCoreWeb.ViewMiddleware
        {
            #region PART CLASS
            /// <summary>
            /// The default constructor is used for code generation
            /// 默认构造方法，用于代码生成
            /// </summary>
            private @TypeName() { }
            /// <summary>
            /// Data view middleware
            /// 数据视图中间件
            /// </summary>
            /// <param name="nextRequest"></param>
            public @TypeName(Microsoft.AspNetCore.Http.RequestDelegate nextRequest) : base(nextRequest)
            {
                #region LOOP Views
                appendView(new AutoCSer.NetCoreWeb.ViewRequest(this, typeof(@NetCoreWebViewTypeFullName), () => new @NetCoreWebViewTypeFullName()/*PUSH:Parameter*/, typeof(@ParameterType.FullName), "@ParameterName"/*PUSH:Parameter*/));
                #endregion LOOP Views
                AutoCSer.Extensions.TaskExtension.Catch(load());
            }
            #endregion PART CLASS
        }
    }
}
