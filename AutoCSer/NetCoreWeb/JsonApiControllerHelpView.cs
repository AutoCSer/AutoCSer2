using System;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// JSON API 控制器帮助文档视图数据
    /// </summary>
    public sealed class JsonApiControllerHelpView
    {
        /// <summary>
        /// 数据视图中间件
        /// </summary>
        internal readonly ViewMiddleware ViewMiddleware;
        /// <summary>
        /// 控制器类型
        /// </summary>
        private readonly Type controllerType;
        /// <summary>
        /// 控制器类型
        /// </summary>
        private TypeHelpView type;
        /// <summary>
        /// 控制器类型
        /// </summary>
        public TypeHelpView Type
        {
            get
            {
                if (type == null) type = ViewMiddleware.GetTypeHelpView(controllerType);
                return type;
            }
        }
        /// <summary>
        /// API 方法集合
        /// </summary>
        private LeftArray<JsonApiMethod> apiMethods;
        /// <summary>
        /// API 方法集合
        /// </summary>
        private JsonApiHelpView[] methods;
        /// <summary>
        /// API 方法集合
        /// </summary>
        public JsonApiHelpView[] Methods
        {
            get
            {
                if (this.methods == null)
                {
                    LeftArray<JsonApiHelpView> methods = new LeftArray<JsonApiHelpView>(apiMethods.Length);
                    foreach (JsonApiMethod method in apiMethods) methods.Add(new JsonApiHelpView(this, method));
                    this.methods = methods.ToArray();
                }
                return this.methods;
            }
        }
        /// <summary>
        /// JSON API 控制器帮助文档视图数据
        /// </summary>
        /// <param name="viewMiddleware">数据视图中间件</param>
        /// <param name="controllerType">控制器类型</param>
        /// <param name="methods">API 方法集合</param>
        internal JsonApiControllerHelpView(ViewMiddleware viewMiddleware, Type controllerType, ref LeftArray<JsonApiMethod> methods)
        {
            this.ViewMiddleware = viewMiddleware;
            this.controllerType = controllerType;
            this.apiMethods = methods;
        }
        /// <summary>
        /// 加载类型视图数据
        /// </summary>
        internal void LoadTypeView()
        {
            Type.LoadTypeView();
            foreach(JsonApiHelpView method in Methods) method.LoadTypeView();
        }
    }
}
