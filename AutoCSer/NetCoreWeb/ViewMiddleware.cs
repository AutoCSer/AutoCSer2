using AutoCSer.Diagnostics;
using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.NetCoreWeb.Metadata;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// 数据视图中间件，用于支持 统一规划全局视图
    /// </summary>
    public abstract class ViewMiddleware : Middleware
    {
        /// <summary>
        /// JavaScript 文件扩展名
        /// </summary>
        internal const string JavaScriptFileExtension = ".js";
        /// <summary>
        /// HTML 文件扩展名
        /// </summary>
        internal const string HtmlFileExtension = ".html";
        /// <summary>
        /// 控制器类型后缀
        /// </summary>
        private const string controllerTypeSuffix = "Controller";
        /// <summary>
        /// JavaScript 序列化参数配置
        /// </summary>
        internal static readonly JsonSerializeConfig JavaScriptSerializeConfig = new JsonSerializeConfig { DateTimeType = AutoCSer.Json.DateTimeTypeEnum.JavaScript, IsBoolToInt = true, IsIntegerToHex = true };
        /// <summary>
        /// JSON API 方法编号
        /// </summary>
        private static int jsonApiMethodIndex;
        /// <summary>
        /// JSON API 方法类型构造函数参数类型集合
        /// </summary>
        private static readonly Type[] jsonApiConstructorTypes = new Type[] { typeof(ViewMiddleware), typeof(Func<JsonApiController>), typeof(MethodInfo), typeof(JsonApiControllerAttribute), typeof(JsonApiAttribute) };
        /// <summary>
        /// JSON API 方法父类型构造函数参数类型集合
        /// </summary>
        private static readonly Type[] jsonApiRequestConstructorTypes = new Type[] { typeof(ViewMiddleware), typeof(Func<JsonApiController>), typeof(MethodInfo), typeof(JsonApiControllerAttribute), typeof(JsonApiAttribute), typeof(JsonApiFlags) };
        /// <summary>
        /// JSON 反序列化参数类型集合
        /// </summary>
        private static readonly Type[] jsonApiRequestJsonDeserializeParameterTypes = new Type[] { typeof(ByteArrayBuffer).MakeByRefType() };
        /// <summary>
        /// JSON 反序列化方法信息
        /// </summary>
        private static readonly MethodInfo jsonApiRequestJsonDeserializeParameterMethod = typeof(JsonApiRequest).GetMethod(nameof(JsonApiRequest.JsonDeserializeParameter), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).notNull();
        /// <summary>
        /// JSON API 检查传参类型集合
        /// </summary>
        private static readonly Type[] jsonApiRequestCheckParameterTypes = new Type[] { typeof(ParameterChecker).MakeByRefType() };
        /// <summary>
        /// 检查参数不允许为空字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <param name="summary"></param>
        /// <param name="checker"></param>
        /// <returns></returns>
        private delegate bool parameterCheckerCheckStringDelegate(string value, string name, string summary, ref ParameterChecker checker);
        /// <summary>
        /// JSON API 获取路由参数类型集合
        /// </summary>
        private static readonly Type[] jsonApiRequestGetRouteParameterTypes = new Type[] { typeof(RouteParameter).MakeByRefType() };
        /// <summary>
        /// JSON API 调用类型集合
        /// </summary>
        private static readonly Type[] jsonApiRequestCallTypes = new Type[] { typeof(JsonApiController) };
        /// <summary>
        /// 路由参数解析方法信息
        /// </summary>
        private static readonly Dictionary<HashObject<Type>, MethodInfo> routeParameterMethods;
        /// <summary>
        /// 路由参数泛型解析方法信息
        /// </summary>
        private static readonly MethodInfo routeParameterMethod;
        /// <summary>
        /// 获取数据视图中间件
        /// </summary>
        private static readonly Func<ViewMiddlewareRequest, ViewMiddleware> getRequestViewMiddleware = ViewMiddlewareRequest.GetViewMiddleware;

        /// <summary>
        /// 命名空间
        /// </summary>
        private readonly string viewNamespace;
        /// <summary>
        /// 服务端静态文件目录
        /// </summary>
        internal readonly string StaticFilePath;
        /// <summary>
        /// 服务端静态文件目录，默认根据中间件命名空间搜索目录名称
        /// </summary>
        protected virtual string staticFilePath
        {
            get
            {
                string directoryName = GetType().Namespace.notNull();
                int startIndex = directoryName.LastIndexOf('.') + 1;
                if (startIndex > 0) directoryName = directoryName.Substring(startIndex);
                string switchDirectoryName = directoryName + ProcessInfo.DefaultSwitchDirectorySuffixName;
                var directory = new FileInfo(ProcessInfo.GetCurrentProcessFileName()).Directory.notNull();
                //var directory = AutoCSer.Common.ApplicationDirectory;
                do
                {
                    if (directory.Name == directoryName || directory.Name == switchDirectoryName) return directory.FullName;
                }
                while ((directory = directory.Parent) != null);
                return AutoCSer.Common.ApplicationDirectory.FullName;
            }
        }
        /// <summary>
        /// 静态文件缓存秒数
        /// </summary>
        internal readonly StringValues StaticFileCacheControl;
        /// <summary>
        /// 静态文件缓存秒数，默认为 13 天
        /// </summary>
        protected virtual int staticFileCacheControl
        {
            get { return 13 * 24 * 60 * 60; }
        }
        /// <summary>
        /// 初始化加载访问锁
        /// </summary>
        internal readonly SemaphoreSlim LoadLock;
        /// <summary>
        /// 输出文本编码，默认为 UTF-8
        /// </summary>
        internal readonly Encoding ResponseEncoding;
        /// <summary>
        /// 输出文本编码，默认为 UTF-8
        /// </summary>
        protected virtual Encoding responseEncoding { get { return Encoding.UTF8; } }
        /// <summary>
        /// 压缩启用最低字节数量
        /// </summary>
        internal readonly int MinCompressSize;
        /// <summary>
        /// 压缩启用最低字节数量，默认为 256B
        /// </summary>
        protected virtual int minCompressSize { get { return 256; } }
        /// <summary>
        /// 输出警告日志最小输出数据字节数
        /// </summary>
        internal readonly int MinResponseLogSize;
        /// <summary>
        /// 输出警告日志最小输出数据字节数，默认为 256KB
        /// </summary>
        protected virtual int minResponseLogSize { get { return 256 << 10; } }
        /// <summary>
        /// 压缩等级
        /// </summary>
        internal readonly CompressionLevel CompressionLevel;
        /// <summary>
        /// 压缩等级，默认为 Fastest
        /// </summary>
        protected virtual CompressionLevel compressionLevel { get { return CompressionLevel.Fastest; } }
        /// <summary>
        /// URL 资源版本查询名称
        /// </summary>
        internal readonly string VersionQueryName;
        /// <summary>
        /// URL 资源版本查询名称，仅支持英文字母与数字不支持符号，默认为 v，在代码生成中替换标记字符串 __VERSIONQUERYNAME__
        /// </summary>
        protected virtual string versionQueryName { get { return "v"; } }
        /// <summary>
        /// GET 查询回调函数参数名称
        /// </summary>
        internal readonly string CallbackQueryName;
        /// <summary>
        /// GET 查询回调函数参数名称，仅支持英文字母与数字不支持符号，默认为 c，在代码生成中替换标记字符串 __CALLBACKQUERYNAME__
        /// </summary>
        protected virtual string callbackQueryName { get { return "c"; } }
        /// <summary>
        /// GET 查询 JSON 参数名称
        /// </summary>
        internal readonly string JsonQueryName;
        /// <summary>
        /// GET 查询 JSON 参数名称，仅支持英文字母与数字不支持符号，默认为 j，在代码生成中替换标记字符串 __JSONQUERYNAME__
        /// </summary>
        protected virtual string jsonQueryName { get { return "j"; } }
        /// <summary>
        /// 是否已经触发加载静态文件与控制器 API
        /// </summary>
        private bool isLoad;
        /// <summary>
        /// 数据视图脚本类型，用于代码生成
        /// </summary>
        public virtual ViewScriptTypeEnum ScriptType { get { return ViewScriptTypeEnum.TypeScript; } }
        /// <summary>
        /// 默认为 false 表示非静态网站（动态请求不添加随机参数），用于代码生成
        /// </summary>
        public virtual bool IsStaticVersion { get { return false; } }
        /// <summary>
        /// 默认为 false 表示文件名称不区分大小写，用于代码生成
        /// </summary>
        public virtual bool FileNameIgnoreCase { get { return false; } }
        /// <summary>
        /// AutoCSer TypeScript / JavaScript 目录相对项目路径，仅支持英文字母与数字不支持符号，默认为 AutoCSerScript，在代码生成中替换标记字符串 __JAVASCRIPTPATH__
        /// </summary>
        public virtual string AutoCSerScriptPath { get { return "AutoCSerScript"; } }
        /// <summary>
        /// 页面初始化等待的默认遮罩层控件 id，默认为 AutoCSerViewOver，仅支持英文字母与数字不支持符号，在代码生成中替换标记字符串 __VIEWOVERID__
        /// </summary>
        public virtual string ViewOverId { get { return AutoCSer.Common.NamePrefix + "ViewOver"; } }
        /// <summary>
        /// 收集客户端错误信息的请求地址，默认为空字符串表示不采集客户端错误，在代码生成中替换标记字符串 __ERRORPATH__
        /// </summary>
        public virtual string ErrorRequestPath { get { return string.Empty; } }
        /// <summary>
        /// 本地 HTML 文件链接是否添加版本号，默认为 false
        /// </summary>
        public virtual bool IsHtmlLinkVersion { get { return false; } }
        /// <summary>
        /// 保存当前版本的文件名称，默认为 AutoCSer.WebViewVersion.txt
        /// </summary>
        public virtual string VersionFileName { get { return AutoCSer.Common.NamePrefix + ".WebViewVersion.txt"; } }
        /// <summary>
        /// 请求实例集合
        /// </summary>
        private readonly Dictionary<string, KeyValue<ViewMiddlewareRequest, ViewRequestTypeEnum>> requests;
        /// <summary>
        /// 数据视图帮助文档类型集合
        /// </summary>
        private readonly Dictionary<string, ViewHelpView> viewHelpTypes;
        /// <summary>
        /// 数据视图帮助文档类型集合
        /// </summary>
        public ICollection<ViewHelpView> ViewHelpTypes { get { return viewHelpTypes.Values; } }
        /// <summary>
        /// JSON API 路由节点
        /// </summary>
        private readonly JsonApiRouteNode routeNode;
        /// <summary>
        /// JSON API 控制器帮助文档视图数据集合
        /// </summary>
        private readonly Dictionary<string, JsonApiControllerHelpView> controllerHelpViews;
        /// <summary>
        /// JSON API 控制器帮助文档视图数据集合
        /// </summary>
        public ICollection<JsonApiControllerHelpView> ControllerHelpViews { get { return controllerHelpViews.Values; } }
        /// <summary>
        /// 帮助文档类型信息集合
        /// </summary>
        private readonly Dictionary<HashObject<Type>, TypeHelpView> typeHelpViews;
        /// <summary>
        /// 帮助文档类型信息集合
        /// </summary>
        private readonly Dictionary<string, TypeHelpView> typeHelpViewNames;
        /// <summary>
        /// 帮助信息访问锁
        /// </summary>
        private readonly object helpLock;
        /// <summary>
        /// 无参构造（避免 WEB 项目错误提示）
        /// </summary>
        protected ViewMiddleware() : this(NullViewMiddleware.NullRequestDelegate) { }
        /// <summary>
        /// 数据视图中间件
        /// </summary>
        /// <param name="nextRequest"></param>
        protected ViewMiddleware(RequestDelegate nextRequest) : base(nextRequest)
        {
            StaticFilePath = staticFilePath;
            ResponseEncoding = responseEncoding;
            JsonQueryName = jsonQueryName;
            CallbackQueryName = callbackQueryName;
            VersionQueryName = versionQueryName;
            CompressionLevel = compressionLevel;
            MinResponseLogSize = Math.Max(minResponseLogSize, 1);
            MinCompressSize = Math.Max(minCompressSize, 1);
            StaticFileCacheControl = "public, max-age=" + staticFileCacheControl.toString();
            viewNamespace = GetType().Namespace.notNull();
            helpLock = new object();
            LoadLock = new SemaphoreSlim(1, 1);
            routeNode = new JsonApiRouteNode(null, 1);
            requests = DictionaryCreator<string>.Create<KeyValue<ViewMiddlewareRequest, ViewRequestTypeEnum>>();
            viewHelpTypes = DictionaryCreator<string>.Create<ViewHelpView>();
            controllerHelpViews = DictionaryCreator<string>.Create<JsonApiControllerHelpView>();
            typeHelpViews = DictionaryCreator.CreateHashObject<Type, TypeHelpView>();
            typeHelpViewNames = DictionaryCreator<string>.Create<TypeHelpView>();
        }
        /// <summary>
        /// 添加数据视图
        /// </summary>
        /// <param name="view"></param>
        protected void appendView(ViewRequest view)
        {
            string path = view.RequestPath;
            if (requests.TryAdd(path, new KeyValue<ViewMiddlewareRequest, ViewRequestTypeEnum>(view, ViewRequestTypeEnum.ViewData)))
            {
                if (view.Attribute.IsHelp) viewHelpTypes.Add(view.Type.fullName().notNull(), new ViewHelpView(view));
                return;
            }
            KeyValue<ViewMiddlewareRequest, ViewRequestTypeEnum> request = requests[path];
            string messgae = $"数据视图 {view.Type.fullName()} 与 {request.Value} {request.Key.RequestInfo} 请求路径冲突 {path}";
            AutoCSer.LogHelper.ErrorIgnoreException(messgae);
            throw new InvalidOperationException(messgae);
        }
        /// <summary>
        /// 初始化加载静态文件与控制器 API
        /// </summary>
        /// <returns></returns>
        protected async Task load()
        {
            await LoadLock.WaitAsync();
            try
            {
                if (!isLoad)
                {
                    isLoad = true;
                    DirectoryInfo directory = new DirectoryInfo(StaticFilePath);
                    int directoryNameLength = directory.FullName.Length;
                    foreach (FileInfo file in directory.GetFiles("*" + HtmlFileExtension, SearchOption.AllDirectories)) appendStaticFile(file, directoryNameLength, ViewRequestTypeEnum.Html);
                    foreach (FileInfo file in directory.GetFiles("*" + JavaScriptFileExtension, SearchOption.AllDirectories)) appendStaticFile(file, directoryNameLength, ViewRequestTypeEnum.JavaScript);
                    LeftArray<SubString> routePaths = new LeftArray<SubString>(0), controllerRoutePaths = new LeftArray<SubString>(0);
                    routePaths.Add(string.Empty);
                    foreach (Type type in GetType().Assembly.GetTypes())
                    {
                        if (!type.IsAbstract && !type.IsGenericType && typeof(JsonApiController).IsAssignableFrom(type))
                        {
                            var constructor = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, EmptyArray<Type>.Array, null);
                            if (constructor != null)
                            {
                                JsonApiControllerAttribute controllerAttribute = type.GetCustomAttribute<JsonApiControllerAttribute>(true) ?? JsonApiControllerAttribute.Default;
                                LeftArray<JsonApiMethod> methods = new LeftArray<JsonApiMethod>(0);
                                foreach (MethodInfo method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public))
                                {
                                    var attribute = method.GetCustomAttribute<JsonApiAttribute>(false);
                                    if (!controllerAttribute.IsMethodAttribute || attribute != null)
                                    {
                                        Type returnType = method.ReturnType;
                                        if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
                                        {
                                            Type jsonType = returnType.GetGenericArguments()[0];
                                            ParameterInfo[] parameters = method.GetParameters();
                                            if (jsonType == typeof(ResponseResult))
                                            {
                                                var parameter = checkRef(parameters);
                                                if (parameter == null) methods.Add(new JsonApiMethod(method, attribute, parameters, JsonApiMethodTypeEnum.ResponseState));
                                                else await AutoCSer.LogHelper.Error($"JSON API {type.fullName()}.{method.Name} 参数 {parameter.Name} 不允许使用 ref / out");
                                            }
                                            else if (jsonType.IsGenericType && jsonType.GetGenericTypeDefinition() == typeof(ResponseResult<>))
                                            {
                                                var parameter = checkRef(parameters);
                                                if (parameter == null) methods.Add(new JsonApiMethod(method, attribute, parameters, jsonType.GetGenericArguments()[0]));
                                                else await AutoCSer.LogHelper.Error($"JSON API {type.fullName()}.{method.Name} 参数 {parameter.Name} 不允许使用 ref / out");
                                            }
                                            else if (attribute != null) await AutoCSer.LogHelper.Error($"JSON API {type.fullName()}.{method.Name} 返回值类型必须为 {typeof(Task<ResponseResult>).fullName()}");
                                        }
                                        else if (attribute != null) await AutoCSer.LogHelper.Error($"JSON API {type.fullName()}.{method.Name} 返回值类型必须为 {typeof(Task<>).fullName()}");
                                    }
                                }
                                var createController = default(Func<JsonApiController>);
                                var controllerRoutePath = default(string);
                                var controllerRouteNode = default(JsonApiRouteNode);
                                foreach (JsonApiMethod method in methods)
                                {
                                    var routeNode = default(JsonApiRouteNode);
                                    LeftArray<ParameterInfo> templateParameters = new LeftArray<ParameterInfo>(0);
                                    var template = method.Attribute.Template;
                                    routePaths.Length = 1;
                                    if (template == null)
                                    {
                                        var route = controllerAttribute.Route;
                                        if (route == null)
                                        {
                                            if (controllerRoutePath == null) controllerRoutePath = type.Namespace.notNull().Substring(viewNamespace.Length).Replace('.', '/') + "/";
                                            method.RoutePath = controllerRoutePath + getControllerTypeRoute(type) + "/" + method.Method.Name;
                                        }
                                        else if (route.Length != 0) method.RoutePath = "/" + route + "/" + getControllerTypeRoute(type) + "/" + method.Method.Name;
                                        else method.RoutePath = "/" + getControllerTypeRoute(type) + "/" + method.Method.Name;
                                    }
                                    else if (template.Length != 0)
                                    {
                                        int parameterIndex = template.Length;
                                        if (template[parameterIndex - 1] == '}')
                                        {
                                            int startIndex = template.IndexOf('{');
                                            if (startIndex >= 0)
                                            {
                                                parameterIndex = startIndex++;
                                                do
                                                {
                                                    int nextIndex = template.IndexOf('}', startIndex), isParameter = 1;
                                                    SubString parameterName = new SubString(startIndex, nextIndex - startIndex, template);
                                                    foreach (ParameterInfo parameter in method.Parameters)
                                                    {
                                                        if (parameterName.Equals(parameter.Name))
                                                        {
                                                            isParameter = 0;
                                                            foreach (ParameterInfo templateParameter in templateParameters)
                                                            {
                                                                if (object.ReferenceEquals(templateParameter, parameter))
                                                                {
                                                                    await AutoCSer.LogHelper.Error($"JSON API {type.fullName()}.{method.Method.Name} 路由模板 {template} 参数名称 {parameterName.ToString()} 重复");
                                                                    isParameter = 2;
                                                                }
                                                            }
                                                            if (isParameter == 0) templateParameters.Add(parameter);
                                                            break;
                                                        }
                                                    }
                                                    if (isParameter == 0) startIndex = template.IndexOf('{', nextIndex + 1) + 1;
                                                    else
                                                    {
                                                        startIndex = 0;
                                                        method.IsApi = false;
                                                        if (isParameter == 1) await AutoCSer.LogHelper.Error($"JSON API {type.fullName()}.{method.Method.Name} 没有找到路由模板 {template} 参数名称 {parameterName.ToString()}");
                                                    }
                                                }
                                                while (startIndex > 0);
                                            }
                                        }
                                        if (method.IsApi)
                                        {
                                            if (templateParameters.Length != 0)
                                            {
                                                if (parameterIndex != 0)
                                                {
                                                    int startIndex;
                                                    if (template[0] != '/')
                                                    {
                                                        if (controllerRouteNode == null) controllerRouteNode = getRouteNode(type, controllerAttribute, ref controllerRoutePaths);
                                                        routePaths.Add(ref controllerRoutePaths);
                                                        routeNode = controllerRouteNode;
                                                        startIndex = 0;
                                                    }
                                                    else
                                                    {
                                                        routeNode = this.routeNode;
                                                        startIndex = 1;
                                                    }
                                                    while (startIndex < parameterIndex)
                                                    {
                                                        int nextIndex = template.IndexOf('/', startIndex, parameterIndex - startIndex);
                                                        if (nextIndex < 0) nextIndex = template.Length;
                                                        SubString path = new SubString(startIndex, nextIndex - startIndex, template);
                                                        routePaths.Add(path);
                                                        routeNode = routeNode.AppendNode(path);
                                                        startIndex = nextIndex + 1;
                                                    }
                                                }
                                                else routeNode = this.routeNode;
                                            }
                                            else if (template[0] != '/')
                                            {
                                                if (controllerRoutePath == null) controllerRoutePath = type.Namespace.notNull().Substring(viewNamespace.Length).Replace('.', '/') + "/";
                                                method.RoutePath = controllerRoutePath + template;
                                            }
                                            else method.RoutePath = template;
                                        }
                                    }
                                    else if (method.Parameters.Length != 0)
                                    {
                                        if (controllerRouteNode == null) controllerRouteNode = getRouteNode(type, controllerAttribute, ref controllerRoutePaths);
                                        routePaths.Add(ref controllerRoutePaths);
                                        routePaths.Add(method.Method.Name);
                                        routeNode = controllerRouteNode.AppendNode(method.Method.Name);
                                        templateParameters = new LeftArray<ParameterInfo>(method.Parameters);
                                    }
                                    else method.RoutePath = "/";
                                    if (method.IsApi)
                                    {
                                        if (method.RoutePath != null)
                                        {
                                            if (requests.TryGetValue(method.RoutePath, out KeyValue<ViewMiddlewareRequest, ViewRequestTypeEnum> request))
                                            {
                                                await AutoCSer.LogHelper.Error($"JSON API {type.fullName()}.{method.Method.Name} 请求路径 {method.RoutePath} 冲突 {request.Value} {request.Key.RequestInfo}");
                                                method.IsApi = false;
                                            }
                                        }
                                        else
                                        {
                                            var request = routeNode.notNull().Request;
                                            if (request != null)
                                            {
                                                await AutoCSer.LogHelper.Error($"JSON API {type.fullName()}.{method.Method.Name} 路由冲突 {request.ControllerType.fullName()}.{request.MethodName}");
                                                method.IsApi = false;
                                            }
                                        }
                                        if (method.IsApi)
                                        {
                                            JsonApiFlags flags = 0;
                                            switch (method.Attribute.CheckRequestEnum)
                                            {
                                                case JsonApiCheckRequestEnum.Controller:
                                                    if (controllerAttribute.IsCheckRequest) flags |= JsonApiFlags.IsCheckRequest;
                                                    break;
                                                case JsonApiCheckRequestEnum.Check: flags |= JsonApiFlags.IsCheckRequest; break;
                                            }
                                            if (method.Parameters.Length > templateParameters.Length) flags |= JsonApiFlags.IsPostParameter;
                                            LeftArray<HttpMethodParameter> httpMethodParameters = new LeftArray<HttpMethodParameter>(method.Parameters.Length);
                                            foreach (ParameterInfo parameter in method.Parameters)
                                            {
                                                bool isTemplateParameter = false;
                                                foreach (ParameterInfo templateParameter in templateParameters)
                                                {
                                                    if (object.ReferenceEquals(templateParameter, parameter))
                                                    {
                                                        isTemplateParameter = true;
                                                        break;
                                                    }
                                                }
                                                HttpMethodParameter httpMethodParameter = new HttpMethodParameter(parameter, controllerAttribute.IsDefaultParameterConstraint, isTemplateParameter);
                                                httpMethodParameters.Add(httpMethodParameter);
                                                if (httpMethodParameter.ConstraintType != ParameterConstraintTypeEnum.None) flags |= JsonApiFlags.IsCheckParameter;
                                            }
                                            method.HttpMethodParameters = httpMethodParameters.ToArray();
                                            var parentType = method.ParentType.notNull();
                                            TypeBuilder typeBuilder = AutoCSer.Reflection.Emit.Module.Builder.DefineType(AutoCSer.Common.NamePrefix + ".NetCoreWeb.JsonApi" + (++jsonApiMethodIndex).toString() + "." + type.Name + "." + method.Method.Name, TypeAttributes.Class | TypeAttributes.Sealed, parentType);
                                            #region 构造函数
                                            ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, jsonApiConstructorTypes);
                                            ILGenerator constructorGenerator = constructorBuilder.GetILGenerator();
                                            #region base()
                                            constructorGenerator.Emit(OpCodes.Ldarg_0);
                                            constructorGenerator.Emit(OpCodes.Ldarg_1);
                                            constructorGenerator.Emit(OpCodes.Ldarg_2);
                                            constructorGenerator.Emit(OpCodes.Ldarg_3);
                                            constructorGenerator.ldarg(4);
                                            constructorGenerator.ldarg(5);
                                            if (method.Parameters.Length != 0 && IsAccessTokenParameter(method.Parameters[0])) flags |= JsonApiFlags.IsAccessTokenParameter;
                                            constructorGenerator.int32((byte)flags);
                                            constructorGenerator.Emit(OpCodes.Call, parentType.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, jsonApiRequestConstructorTypes, null).notNull());
                                            constructorGenerator.ret();
                                            #endregion
                                            #endregion
                                            var parameterType = default(AutoCSer.Net.CommandServer.ServerMethodParameter);
                                            var parameterField = default(FieldBuilder);
                                            if (method.Parameters.Length != 0)
                                            {
                                                parameterType = AutoCSer.Net.CommandServer.ServerMethodParameter.GetOrCreate(method.Parameters.Length, method.Parameters, typeof(void));
                                                parameterField = typeBuilder.DefineField(nameof(method.Parameters), parameterType.Type, FieldAttributes.Public);
                                            }
                                            #region public override Task<ResponseResult> Call(JsonApiController controller)
                                            MethodBuilder methodBuilder = typeBuilder.DefineMethod(nameof(JsonApiResultRequest.Call), MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.ReuseSlot, method.Method.ReturnType, jsonApiRequestCallTypes);
                                            ILGenerator generator = methodBuilder.GetILGenerator();
                                            #region return controller.Call(parameter.value);
                                            generator.Emit(OpCodes.Ldarg_1);
                                            foreach (ParameterInfo parameter in method.Parameters)
                                            {
                                                generator.Emit(OpCodes.Ldarg_0);
                                                generator.Emit(OpCodes.Ldflda, parameterField.notNull());
                                                generator.Emit(OpCodes.Ldfld, parameterType.notNull().GetField(parameter.Name.notNull()).notNull());
                                            }
                                            generator.call(method.Method);
                                            generator.ret();
                                            #endregion
                                            #endregion
                                            if ((flags & JsonApiFlags.IsPostParameter) != 0)
                                            {
                                                #region public override AutoCSer.Json.DeserializeResult JsonDeserialize(ref ByteArrayBuffer postBuffer)
                                                methodBuilder = typeBuilder.DefineMethod(nameof(JsonApiRequest.JsonDeserialize), MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.ReuseSlot, typeof(AutoCSer.Json.DeserializeResult), jsonApiRequestJsonDeserializeParameterTypes);
                                                generator = methodBuilder.GetILGenerator();
                                                #region return JsonApiRequest.JsonDeserializeParameter(ref postBuffer, ref parameter);
                                                generator.Emit(OpCodes.Ldarg_1);
                                                generator.Emit(OpCodes.Ldarg_0);
                                                generator.Emit(OpCodes.Ldflda, parameterField.notNull());
                                                generator.call(jsonApiRequestJsonDeserializeParameterMethod.MakeGenericMethod(parameterType.notNull().Type));
                                                #endregion
                                                generator.ret();
                                                #endregion
                                            }
                                            if ((flags & JsonApiFlags.IsCheckParameter) != 0)
                                            {
                                                #region public override void CheckParameter(ref ParameterChecker checker)
                                                methodBuilder = typeBuilder.DefineMethod(nameof(JsonApiRequest.CheckParameter), MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.ReuseSlot, typeof(void), jsonApiRequestCheckParameterTypes);
                                                generator = methodBuilder.GetILGenerator();
                                                #region if (!AutoCSer.NetCoreWeb.ParameterChecker.CheckEquatable(parameter.value, nameof(parameter.value), "summary", ref checker)) return;
                                                Label end = generator.DefineLabel();
                                                bool isParameter = false;
                                                foreach (HttpMethodParameter parameter in method.HttpMethodParameters)
                                                {
                                                    if (parameter.ConstraintType != ParameterConstraintTypeEnum.None)
                                                    {
                                                        if (isParameter) generator.Emit(OpCodes.Brfalse, end);
                                                        else isParameter = true;
                                                        generator.ldarg(0);
                                                        generator.Emit(OpCodes.Ldflda, parameterField.notNull());
                                                        generator.Emit(OpCodes.Ldfld, parameterType.notNull().GetField(parameter.Parameter.Name.notNull()).notNull());
                                                        generator.ldstr(parameter.Parameter.Name.notNull());
                                                        string summary = AutoCSer.Reflection.XmlDocument.Get(method.Method, parameter.Parameter);
                                                        if (string.IsNullOrEmpty(summary)) AutoCSer.Reflection.XmlDocument.Get(parameter.Parameter.ParameterType);
                                                        generator.ldstr(summary);
                                                        generator.ldarg(1);
                                                        switch (parameter.ConstraintType)
                                                        {
                                                            case ParameterConstraintTypeEnum.NotNull: generator.call(ClassGenericType.Get(parameter.Parameter.ParameterType).ParameterCheckerCheckNullDelegate.Method); break;
                                                            case ParameterConstraintTypeEnum.NotDefault: generator.call(EquatableGenericType.Get(parameter.Parameter.ParameterType).ParameterCheckerCheckEquatableDelegate.Method); break;
                                                            case ParameterConstraintTypeEnum.NotEmptyString: generator.call(((parameterCheckerCheckStringDelegate)ParameterChecker.CheckString).Method); break;
                                                            case ParameterConstraintTypeEnum.NotEmptyCollection: generator.call(CollectionGenericType.Get(parameter.Parameter.ParameterType).ParameterCheckerCheckCollectionDelegate.Method); break;
                                                            case ParameterConstraintTypeEnum.ParameterConstraint: generator.call(ParameterConstraintGenericType.Get(parameter.Parameter.ParameterType).ParameterCheckerCheckConstraintDelegate.Method); break;
                                                        }
                                                    }
                                                }
                                                #endregion
                                                generator.Emit(OpCodes.Pop);
                                                generator.MarkLabel(end);
                                                generator.ret();
                                                #endregion
                                            }
                                            if ((flags & JsonApiFlags.IsAccessTokenParameter) != 0)
                                            {
                                                #region public override Task<ResponseResult> CheckAccessTokenParameter()
                                                methodBuilder = typeBuilder.DefineMethod(nameof(JsonApiRequest.CheckAccessTokenParameter), MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.ReuseSlot, typeof(Task<ResponseResult>), EmptyArray<Type>.Array);
                                                generator = methodBuilder.GetILGenerator();
                                                #region return ViewMiddleware.CheckAccessTokenParameter(ViewMiddleware, parameter.value);
                                                generator.ldarg(0);
                                                generator.call(getRequestViewMiddleware.Method);
                                                generator.ldarg(0);
                                                generator.Emit(OpCodes.Ldflda, parameterField.notNull());
                                                generator.Emit(OpCodes.Ldfld, parameterType.notNull().GetField(method.Parameters[0].Name.notNull()).notNull());
                                                generator.call(GenericType.Get(method.Parameters[0].ParameterType).CheckAccessTokenParameterDelegate.Method);
                                                generator.ret();
                                                #endregion
                                                #endregion
                                            }
                                            if (method.Parameters.Length != 0)
                                            {
                                                #region public override void GetRouteParameter(ref RouteParameter routeParameter)
                                                methodBuilder = typeBuilder.DefineMethod(nameof(JsonApiRequest.GetRouteParameter), MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.ReuseSlot, typeof(void), jsonApiRequestGetRouteParameterTypes);
                                                generator = methodBuilder.GetILGenerator();
                                                Label end = generator.DefineLabel();
                                                bool isParameter = false;
                                                foreach (HttpMethodParameter parameter in method.HttpMethodParameters)
                                                {
                                                    if (parameter.IsTemplateParameter)
                                                    {
                                                        #region if (!routeParameter.Get("value", ref parameter.value)) return;
                                                        if (isParameter) generator.Emit(OpCodes.Brfalse, end);
                                                        else isParameter = true;
                                                        generator.Emit(OpCodes.Ldarg_1);
                                                        generator.ldstr(parameter.Parameter.Name.notNull());
                                                        generator.Emit(OpCodes.Ldarg_0);
                                                        generator.Emit(OpCodes.Ldflda, parameterField.notNull());
                                                        generator.Emit(OpCodes.Ldflda, parameterType.notNull().GetField(parameter.Parameter.Name.notNull()).notNull());
                                                        if (routeParameterMethods.TryGetValue(parameter.Parameter.ParameterType, out var routeParameterMethod)) generator.call(routeParameterMethod);
                                                        else generator.call(ViewMiddleware.routeParameterMethod.MakeGenericMethod(parameter.Parameter.ParameterType));
                                                        #endregion
                                                    }
                                                }
                                                generator.Emit(OpCodes.Pop);
                                                generator.MarkLabel(end);
                                                generator.ret();
                                                #endregion
                                            }
                                            if (createController == null)
                                            {
                                                DynamicMethod dynamicMethod = new DynamicMethod(AutoCSer.Common.NamePrefix + ".NetCoreWeb.JsonApiControllerConstructor", type, EmptyArray<Type>.Array, type, true);
                                                generator = dynamicMethod.GetILGenerator();
                                                generator.Emit(OpCodes.Newobj, constructor);
                                                generator.ret();
                                                createController = (Func<JsonApiController>)dynamicMethod.CreateDelegate(typeof(Func<JsonApiController>));
                                            }
                                            JsonApiRequest jsonApiRequest = (JsonApiRequest)typeBuilder.CreateType().notNull().GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, jsonApiConstructorTypes, null)
                                                .notNull().Invoke(new object[] { this, createController, method.Method, controllerAttribute, method.Attribute });
                                            bool isHelp = controllerAttribute.IsHelp && method.Attribute.IsHelp;
                                            if (method.RoutePath != null) requests.Add(method.RoutePath, new KeyValue<ViewMiddlewareRequest, ViewRequestTypeEnum>(jsonApiRequest, ViewRequestTypeEnum.JsonApi));
                                            else
                                            {
                                                routeNode.notNull().Request = jsonApiRequest;
                                                if (isHelp)
                                                {
                                                    foreach (ParameterInfo parameter in templateParameters) routePaths.Add("{" + parameter.Name + "}");
                                                    method.RoutePath = string.Join('/', routePaths.getArray(p => p.ToString()));
                                                }
                                            }
                                            if (!isHelp) method.IsApi = false;
                                        }
                                    }
                                }
                                int methodCount = methods.Length;
                                if (methodCount != 0 && controllerAttribute.IsHelp)
                                {
                                    JsonApiMethod[] methodArray = methods.Array;
                                    methods.Length = 0;
                                    foreach (JsonApiMethod method in methodArray)
                                    {
                                        if (method.IsApi) methods.Add(method);
                                        if (--methodCount == 0) break;
                                    }
                                    if (methods.Length != 0) controllerHelpViews.Add(type.fullName().notNull(), new JsonApiControllerHelpView(this, type, ref methods));
                                }
                            }
                            else await AutoCSer.LogHelper.Error($"JSON API 代理控制器类型 {type.fullName()} 没有找到无参构造函数");
                        }
                    }
                    AutoCSer.Threading.ThreadPool.TinyBackground.Start(loadTypeView);
                }
            }
            finally { LoadLock.Release(); }
        }
        /// <summary>
        /// 加载类型视图数据
        /// </summary>
        private void loadTypeView()
        {
            foreach (JsonApiControllerHelpView controller in controllerHelpViews.Values) controller.LoadTypeView();
            foreach (ViewHelpView view in viewHelpTypes.Values) view.LoadTypeView();
        }
        /// <summary>
        /// 添加静态文件请求实例
        /// </summary>
        /// <param name="file"></param>
        /// <param name="directoryNameLength"></param>
        /// <param name="type"></param>
        private void appendStaticFile(FileInfo file, int directoryNameLength, ViewRequestTypeEnum type)
        {
            string filePath = file.FullName.Substring(directoryNameLength);
            if (Path.DirectorySeparatorChar != '/') filePath = filePath.Replace(Path.DirectorySeparatorChar, '/');
            requests.TryAdd(filePath, new KeyValue<ViewMiddlewareRequest, ViewRequestTypeEnum>(new ViewStaticFileRequest(this, file), type));
        }
        /// <summary>
        /// ref / out 参数检查
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
#if NetStandard21
        private static ParameterInfo? checkRef(ParameterInfo[] parameters)
#else
        private static ParameterInfo checkRef(ParameterInfo[] parameters)
#endif
        {
            foreach (ParameterInfo parameter in parameters)
            {
                if (parameter.ParameterType.IsByRef) return parameter;
            }
            return null;
        }
        /// <summary>
        /// 根据控制器名称获取路由，默认操作为删除 Controller 后缀
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected virtual string getControllerTypeRoute(Type type)
        {
            string path = type.Name;
            if (path.Length > controllerTypeSuffix.Length && path.EndsWith(controllerTypeSuffix, StringComparison.Ordinal)) return path.Substring(0, path.Length - controllerTypeSuffix.Length);
            return path;
        }
        /// <summary>
        /// 获取 JSON API 代理控制器路由节点
        /// </summary>
        /// <param name="controllerType"></param>
        /// <param name="controllerAttribute"></param>
        /// <param name="controllerRoutePaths"></param>
        /// <returns></returns>
        private JsonApiRouteNode getRouteNode(Type controllerType, JsonApiControllerAttribute controllerAttribute, ref LeftArray<SubString> controllerRoutePaths)
        {
            JsonApiRouteNode routeNode = this.routeNode;
            var route = controllerAttribute.Route;
            controllerRoutePaths.Length = 0;
            if (route == null)
            {
                int startIndex = (route = controllerType.Namespace.notNull()).IndexOf('.', viewNamespace.Length) + 1;
                if (startIndex > 0)
                {
                    do
                    {
                        int nextIndex = route.IndexOf('.', startIndex);
                        if (nextIndex < 0) nextIndex = route.Length;
                        SubString path = new SubString(startIndex, nextIndex - startIndex, route);
                        controllerRoutePaths.Add(path);
                        routeNode = routeNode.AppendNode(path);
                        startIndex = nextIndex + 1;
                    }
                    while (startIndex < route.Length);
                }
                string typePath = getControllerTypeRoute(controllerType);
                controllerRoutePaths.Add(typePath);
                return routeNode.AppendNode(typePath);
            }
            if (route.Length != 0)
            {
                int startIndex = route.IndexOf('/');
                if (startIndex >= 0)
                {
                    if (startIndex > 0)
                    {
                        SubString path = new SubString(0, startIndex, route);
                        controllerRoutePaths.Add(path);
                        routeNode = routeNode.AppendNode(path);
                    }
                    for (++startIndex; startIndex < route.Length;)
                    {
                        int nextIndex = route.IndexOf('/', startIndex);
                        if (nextIndex < 0) nextIndex = route.Length;
                        SubString path = new SubString(startIndex, nextIndex - startIndex, route);
                        controllerRoutePaths.Add(path);
                        routeNode = routeNode.AppendNode(path);
                        startIndex = nextIndex + 1;
                    }
                }
                else
                {
                    controllerRoutePaths.Add(route);
                    return routeNode.AppendNode(route);
                }
            }
            return routeNode;
        }
        /// <summary>
        /// 数据视图处理
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext)
        {
            if (!isLoad) await load();
            string path = httpContext.Request.Path;
            if (requests.TryGetValue(path, out KeyValue<ViewMiddlewareRequest, ViewRequestTypeEnum> request))
            {
                await request.Key.Request(httpContext, request.Value);
                return;
            }
            var routeNode = this.routeNode;
            var requestNode = routeNode.Request == null ? null : routeNode;
            if (routeNode.Nodes != null && path.Length > 1)
            {
                int startIndex = 1;
                do
                {
                    int nextIndex = path.IndexOf('/', startIndex);
                    if (nextIndex < 0) break;
                    SubString key = new SubString(startIndex, nextIndex - startIndex, path);
                    if (!routeNode.Nodes.TryGetValue(key, out routeNode)) break;
                    if (routeNode.Request != null) requestNode = routeNode;
                    if (routeNode.Nodes == null) break;
                    startIndex = nextIndex + 1;
                }
                while (startIndex < path.Length);
            }
            if (requestNode != null)
            {
                await requestNode.LoadRequest(httpContext, path);
                return;
            }
            await nextRequest(httpContext);
        }
        /// <summary>
        /// 获取 POST 数据类型
        /// </summary>
        /// <param name="httpContext">HTTP 上下文</param>
        /// <param name="contentTypeEncoding">POST 数据文本编码类型</param>
        /// <returns>POST 数据类型</returns>
        public virtual PostTypeEnum GetPostType(HttpContext httpContext, out Encoding contentTypeEncoding)
        {
            var contentType = httpContext.Request.ContentType;
            if (contentType != null)
            {
                int contentTypeIndex = contentType.IndexOf(';');
                if (contentTypeIndex <= 0) contentTypeIndex = contentType.Length;
                switch (contentTypeIndex - 8)
                {
                    case 9 - 8:
                        if (new SubString(0, contentTypeIndex, contentType).LowerEquals("text/json"))
                        {
                            contentTypeEncoding = getEncoding(contentType, contentTypeIndex + 1);
                            return PostTypeEnum.Json;
                        }
                        break;
                    case 16 - 8:
                        if (new SubString(0, contentTypeIndex, contentType).LowerEquals("application/json"))
                        {
                            contentTypeEncoding = getEncoding(contentType, contentTypeIndex + 1);
                            return PostTypeEnum.Json;
                        }
                        break;
                    case 33 - 8:
                        if (new SubString(0, contentTypeIndex, contentType).LowerEquals("application/x-www-form-urlencoded"))
                        {
                            contentTypeEncoding = Encoding.UTF8;
                            return PostTypeEnum.Form;
                        }
                        break;
                    case 8 - 8:
                        if (new SubString(0, contentTypeIndex, contentType).LowerEquals("text/xml"))
                        {
                            contentTypeEncoding = getEncoding(contentType, contentTypeIndex + 1);
                            return PostTypeEnum.Xml;
                        }
                        break;
                    case 15 - 8:
                        if (new SubString(0, contentTypeIndex, contentType).LowerEquals("application/xml"))
                        {
                            contentTypeEncoding = getEncoding(contentType, contentTypeIndex + 1);
                            return PostTypeEnum.Xml;
                        }
                        break;
                    case 19 - 8:
                        if (new SubString(0, contentTypeIndex, contentType).LowerEquals("multipart/form-data"))
                        {
                            contentTypeEncoding = Encoding.UTF8;
                            //boundary = getBoundary(contentType, contentTypeIndex + 1);
                            return PostTypeEnum.FormData;
                        }
                        break;
                }
            }
            contentTypeEncoding = Encoding.UTF8;
            return PostTypeEnum.Unknown;
        }
        /// <summary>
        /// 输出数据
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="data"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <param name="checkVersion"></param>
        /// <returns></returns>
        internal async Task Response(HttpContext httpContext, byte[] data, int startIndex, int length, bool checkVersion)
        {
            HttpResponse httpResponse = httpContext.Response;
            if (checkVersion && !string.IsNullOrEmpty(VersionQueryName) && httpContext.Request.Query.ContainsKey(VersionQueryName))
            {
                httpResponse.Headers["Cache-Control"] = StaticFileCacheControl;
            }
            if (length >= MinCompressSize && httpContext.Request.Headers.TryGetValue("Accept-Encoding", out StringValues acceptEncoding)
                && acceptEncoding.ToString().IndexOf("gzip", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                ByteArrayBuffer buffer = default(ByteArrayBuffer);
                try
                {
                    using (MemoryStream bufferStream = AutoCSer.Common.Config.GetExpandableMemoryStream(ref buffer, length))
                    {
                        using (GZipStream compressStream = new GZipStream(bufferStream, CompressionLevel, true)) compressStream.Write(data, startIndex, length);
                        if (bufferStream.Position + 32 < length)
                        {
                            data = bufferStream.GetBuffer();
                            startIndex = buffer.StartIndex;
                            httpResponse.Headers["Content-Encoding"] = "gzip";
                            httpResponse.ContentLength = length = (int)bufferStream.Position;
#if NetStandard21
                            await using (Stream stream = httpResponse.Body) await httpResponse.Body.WriteAsync(data, startIndex, length);
#else
                            using (Stream stream = httpResponse.Body) await httpResponse.Body.WriteAsync(data, startIndex, length);
#endif
                            if (length >= MinResponseLogSize) await responseSizeLog(httpContext, length);
                            return;
                        }
                    }
                }
                finally { buffer.Free(); }
            }
            httpResponse.ContentLength = length;
            if (length != 0)
            {
#if NetStandard21
                await using (Stream stream = httpResponse.Body) await httpResponse.Body.WriteAsync(data, startIndex, length);
#else
                using (Stream stream = httpResponse.Body) await httpResponse.Body.WriteAsync(data, startIndex, length);
#endif
                if (length >= MinResponseLogSize) await responseSizeLog(httpContext, length);
            }
        }
        /// <summary>
        /// 输出错误数据
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="result"></param>
        /// <param name="isResponseJavaScript"></param>
        /// <param name="checkVersion"></param>
        /// <returns></returns>
        internal async Task ResponseError(HttpContext httpContext, ResponseResult result, bool isResponseJavaScript, bool checkVersion)
        {
            var callback = httpContext.Request.Query[CallbackQueryName];
            ViewResponse response = new ViewResponse(View.Null);
            try
            {
                ByteArrayBuffer buffer = response.Error(ref result, callback, ResponseEncoding, isResponseJavaScript);
                try
                {
                    httpContext.Response.ContentType = ResponseContentType.GetJavaScript(ResponseEncoding);
                    await Response(httpContext, buffer.Buffer.notNull().Buffer, buffer.StartIndex, buffer.CurrentIndex, checkVersion);
                }
                finally { buffer.Free(); }
            }
            finally { response.Free(); }
        }
        /// <summary>
        /// 输出成功状态
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="isResponseJavaScript"></param>
        /// <param name="checkVersion"></param>
        /// <returns></returns>
        internal async Task ResponseSuccess(HttpContext httpContext, bool isResponseJavaScript, bool checkVersion)
        {
            var callback = httpContext.Request.Query[CallbackQueryName];
            ViewResponse response = new ViewResponse(View.Null);
            try
            {
                ByteArrayBuffer buffer = response.Success(callback, ResponseEncoding, isResponseJavaScript);
                try
                {
                    httpContext.Response.ContentType = ResponseContentType.GetJavaScript(ResponseEncoding);
                    await Response(httpContext, buffer.Buffer.notNull().Buffer, buffer.StartIndex, buffer.CurrentIndex, checkVersion);
                }
                finally { buffer.Free(); }
            }
            finally { response.Free(); }
        }
        /// <summary>
        /// 输出成功数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpContext"></param>
        /// <param name="result"></param>
        /// <param name="isResponseJavaScript"></param>
        /// <param name="checkVersion"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal Task ResponseSuccess<T>(HttpContext httpContext, T result, bool isResponseJavaScript, bool checkVersion)
        {
            if (result != null) return responseSuccess(httpContext, result, isResponseJavaScript, checkVersion);
            return ResponseSuccess(httpContext, isResponseJavaScript, checkVersion);
        }
        /// <summary>
        /// 输出成功数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpContext"></param>
        /// <param name="result"></param>
        /// <param name="isResponseJavaScript"></param>
        /// <param name="checkVersion"></param>
        /// <returns></returns>
        private async Task responseSuccess<T>(HttpContext httpContext, T result, bool isResponseJavaScript, bool checkVersion)
        {
            var callback = httpContext.Request.Query[CallbackQueryName];
            ViewResponse response = new ViewResponse(View.Null);
            try
            {
                response.Start(callback, isResponseJavaScript);
                response.JsonSerializer.SerializeNext(ref result, isResponseJavaScript ? JavaScriptSerializeConfig : null);
                ByteArrayBuffer buffer = response.End(ResponseEncoding);
                try
                {
                    httpContext.Response.ContentType = ResponseContentType.GetJavaScript(ResponseEncoding);
                    await Response(httpContext, buffer.Buffer.notNull().Buffer, buffer.StartIndex, buffer.CurrentIndex, checkVersion);
                }
                finally { buffer.Free(); }
            }
            finally { response.Free(); }
        }
        /// <summary>
        /// 输出大小超出警告阈值日志
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="size">输出数据字节数</param>
        /// <returns></returns>
        protected virtual Task responseSizeLog(HttpContext httpContext, int size)
        {
            return AutoCSer.LogHelper.Warn($"{httpContext.Request.GetDisplayUrl()} {(size >> 10).toString()}KB");
        }
        /// <summary>
        /// 检查来源页面，用于跨域验证，默认返回 false
        /// </summary>
        /// <param name="referer">来源页面</param>
        /// <returns></returns>
#if NetStandard21
        public virtual ResponseResult CheckReferer(string? referer)
#else
        public virtual ResponseResult CheckReferer(string referer)
#endif
        {
            return ResponseStateEnum.RefererNotMatch;
        }
        /// <summary>
        /// 判断参数是否鉴权参数
        /// </summary>
        /// <param name="type">调用方法第一个参数类型</param>
        /// <param name="name">调用方法第一个参数名称</param>
        /// <returns></returns>
        public virtual bool IsAccessTokenParameter(Type type, string name) { return false; }
        /// <summary>
        /// 判断参数是否鉴权参数
        /// </summary>
        /// <param name="parameter">调用方法第一个参数信息</param>
        /// <returns></returns>
        public virtual bool IsAccessTokenParameter(ParameterInfo parameter) { return false; }
        /// <summary>
        /// 检查数据视图，一般用于 HTTP 头部参数鉴权
        /// </summary>
        /// <param name="httpContext">HTTP 上下文</param>
        /// <param name="view">数据视图</param>
        /// <returns></returns>
        public virtual Task<ResponseResult> Check(HttpContext httpContext, View view) { return View.SuccessResponseResultTask; }
        /// <summary>
        /// 检查 JSON API，一般用于 HTTP 头部参数鉴权
        /// </summary>
        /// <param name="httpContext">HTTP 上下文</param>
        /// <param name="request">JSON API 请求实例</param>
        /// <returns></returns>
        public virtual Task<ResponseResult> Check(HttpContext httpContext, JsonApiRequest request) { return View.SuccessResponseResultTask; }
        /// <summary>
        /// 参数鉴权
        /// </summary>
        /// <typeparam name="T">鉴权参数类型</typeparam>
        /// <param name="parameter">鉴权参数</param>
        /// <returns></returns>
        public virtual Task<ResponseResult> CheckAccessTokenParameter<T>(T parameter) { return View.SuccessResponseResultTask; }
        /// <summary>
        /// 参数鉴权
        /// </summary>
        /// <typeparam name="T">鉴权参数类型</typeparam>
        /// <param name="viewMiddleware">数据视图中间件</param>
        /// <param name="parameter">鉴权参数</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task<ResponseResult> CheckAccessTokenParameter<T>(ViewMiddleware viewMiddleware, T parameter)
        {
            return viewMiddleware.CheckAccessTokenParameter(parameter);
        }
        /// <summary>
        /// 获取调用监视标识
        /// </summary>
        /// <param name="httpContext">HTTP 上下文</param>
        /// <param name="view">数据视图</param>
        /// <returns>调用监视标识，long.MinValue 表示不监视</returns>
        public virtual long GetCallIdentity(HttpContext httpContext, View view) { return long.MinValue; }
        /// <summary>
        /// 获取调用监视标识
        /// </summary>
        /// <param name="httpContext">HTTP 上下文</param>
        /// <param name="request">JSON API 请求实例</param>
        /// <returns>调用监视标识，long.MinValue 表示不监视</returns>
        public virtual long GetCallIdentity(HttpContext httpContext, JsonApiRequest request) { return long.MinValue; }
        /// <summary>
        /// 调用完成
        /// </summary>
        /// <param name="callIdentity">调用监视标识</param>
        /// <param name="isException">接口是否执行异常</param>
        public virtual void OnCallCompleted(long callIdentity, bool isException) { }
        /// <summary>
        /// 获取请求路径，用于代码生成
        /// </summary>
        /// <param name="viewType">数据视图类型名称</param>
        /// <returns>请求路径</returns>
        public virtual string GetRequestPath(Type viewType)
        {
            string typeNamespace = viewType.Namespace.notNull();
            if (typeNamespace.Length <= viewNamespace.Length) return "/" + viewType.Name;
            return typeNamespace.Substring(viewNamespace.Length).Replace('.', '/') + "/" + viewType.Name;
        }
        /// <summary>
        /// 获取命名空间模板文件路径，用于代码生成
        /// </summary>
        /// <param name="viewType">数据视图类型名称</param>
        /// <returns>命名空间模板文件路径</returns>
        public virtual string GetNamespaceTemplateFilePath(Type viewType)
        {
            string typeNamespace = viewType.Namespace.notNull();
            if (typeNamespace.Length <= viewNamespace.Length) return string.Empty;
            return typeNamespace.Substring(viewNamespace.Length + 1).Replace('.', Path.DirectorySeparatorChar);
        }
        /// <summary>
        /// 代码生成 JavaScript 代码转换处理，一般用于替换文件域名标记操作，默认为不处理直接返回原内容
        /// </summary>
        /// <param name="javaScriptCode">JavaScript 代码</param>
        /// <returns></returns>
        public virtual string CodeGeneratorJavaScriptCode(string javaScriptCode)
        {
            return javaScriptCode;
        }
        /// <summary>
        /// 代码生成 HTML 代码转换处理，一般用于替换文件域名标记操作，默认为不处理直接返回原内容
        /// </summary>
        /// <param name="htmlCode">HTML 代码</param>
        /// <returns></returns>
        public virtual string CodeGeneratorHtmlCode(string htmlCode)
        {
            return htmlCode;
        }
        /// <summary>
        /// 代码生成 css 代码生成转换处理，一般用于替换文件域名标记操作，默认为不处理直接返回原内容
        /// </summary>
        /// <param name="cssCode">css 代码</param>
        /// <returns></returns>
        public virtual string CodeGeneratorCssCode(string cssCode)
        {
            return cssCode;
        }
        /// <summary>
        /// 获取帮助文档类型信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal TypeHelpView GetTypeHelpView(Type type)
        {
            HashObject<Type> key = type;
            Monitor.Enter(helpLock);
            if (typeHelpViews.TryGetValue(key, out var view))
            {
                Monitor.Exit(helpLock);
                return view;
            }
            try
            {
                typeHelpViews.Add(key, view = new TypeHelpView(this, type));
                typeHelpViewNames.Add(type.fullName().notNull(), view);
            }
            finally { Monitor.Exit(helpLock); }
            return view;
        }
        /// <summary>
        /// 获取帮助文档类型信息
        /// </summary>
        /// <param name="typeFullName"></param>
        /// <returns></returns>
#if NetStandard21
        public TypeHelpView? GetTypeHelpView(string typeFullName)
#else
        public TypeHelpView GetTypeHelpView(string typeFullName)
#endif
        {
            Monitor.Enter(helpLock);
            typeHelpViewNames.TryGetValue(typeFullName, out var view);
            Monitor.Exit(helpLock);
            return view;
        }
        /// <summary>
        /// 判断类型是否支持类型帮助文档
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual bool IsHelp(Type type)
        {
            return !type.IsPrimitive && type != typeof(string);
        }
        ///// <summary>
        ///// 获取数据视图帮助文档视图数据
        ///// </summary>
        ///// <param name="viewTypeFullName">数据视图类型名称</param>
        ///// <returns></returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //public ViewHelpView GetViewHelpView(string viewTypeFullName)
        //{
        //    return !string.IsNullOrEmpty(viewTypeFullName) && viewHelpTypes.TryGetValue(viewTypeFullName, out ViewHelpView view) ? view : null;
        //}
        /// <summary>
        /// 获取 JSON API 控制器帮助文档视图数据
        /// </summary>
        /// <param name="controllerTypeFullName">控制器类型名称</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public JsonApiControllerHelpView? GetControllerHelpView(string controllerTypeFullName)
#else
        public JsonApiControllerHelpView GetControllerHelpView(string controllerTypeFullName)
#endif
        {
            return !string.IsNullOrEmpty(controllerTypeFullName) && controllerHelpViews.TryGetValue(controllerTypeFullName, out var controllerHelpView) ? controllerHelpView : null;
        }

        /// <summary>
        /// 获取 POST 数据编码类型
        /// </summary>
        /// <param name="contentType"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        private static Encoding getEncoding(string contentType, int startIndex)
        {
            if (startIndex < contentType.Length)
            {
                startIndex = new SubString(startIndex, contentType.Length - startIndex, contentType).IndexLower('c');
                if (startIndex >= 0 && (startIndex + 8) < contentType.Length && new SubString(startIndex, 8, contentType).LowerEquals("charset="))
                {
                    int size = contentType.Length - (startIndex += 8);
                    switch (contentType[startIndex] | 0x20)
                    {
                        case 'g':
                            if (size >= 6 && new SubString(startIndex, 6, contentType).LowerEquals("gb2312")) return Encoding.GetEncoding("GB2312");
                            if (size >= 3 && new SubString(startIndex, 3, contentType).LowerEquals("gbk")) return Encoding.GetEncoding("GBK");
                            if (size >= 7 && new SubString(startIndex, 7, contentType).LowerEquals("gb18030")) return Encoding.GetEncoding("GB18030");
                            break;
                        case 'u':
                            if (size >= 5 && new SubString(startIndex, 5, contentType).LowerEquals("utf-8")) return Encoding.UTF8;
                            if (size >= 7 && new SubString(startIndex, 7, contentType).LowerEquals("unicode")) return Encoding.Unicode;
                            break;
                        case 'b':
                            if (size >= 4 && new SubString(startIndex, 4, contentType).LowerEquals("big5")) return Encoding.GetEncoding("BIG5");
                            break;
                    }
                }
            }
            return Encoding.UTF8;
        }
        /// <summary>
        /// POST 数据转换字符串
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="stringBuffer"></param>
        /// <param name="contentTypeEncoding"></param>
        internal unsafe static void GetPostString(ref ByteArrayBuffer buffer, ref ByteArrayBuffer stringBuffer, Encoding contentTypeEncoding)
        {
            fixed (byte* bufferFixed = buffer.GetFixedBuffer())
            fixed (byte* stringBufferFixed = stringBuffer.GetFixedBuffer())
            {
                contentTypeEncoding.GetChars(bufferFixed + buffer.StartIndex, buffer.CurrentIndex, (char*)(stringBufferFixed + stringBuffer.StartIndex), stringBuffer.CurrentIndex);
            }
        }

        static ViewMiddleware()
        {
            routeParameterMethods = DictionaryCreator.CreateHashObject<Type, MethodInfo>();
            routeParameterMethod = AutoCSer.Common.NullMethodInfo;
            foreach (MethodInfo method in typeof(RouteParameter).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (method.ReturnType == typeof(bool))
                {
                    if (!method.IsGenericMethod)
                    {
                        ParameterInfo[] parameters = method.GetParameters();
                        if (parameters.Length == 2 && parameters[0].ParameterType == typeof(string))
                        {
                            ParameterInfo parameter = parameters[1];
                            if (parameter.ParameterType.IsByRef) routeParameterMethods.Add(parameter.ParameterType.GetElementType().notNull(), method);
                        }
                    }
                    else
                    {
                        ParameterInfo[] parameters = method.GetParameters();
                        if (parameters.Length == 2 && parameters[0].ParameterType == typeof(string))
                        {
                            ParameterInfo parameter = parameters[1];
                            if (parameter.ParameterType.IsByRef && parameter.ParameterType.GetElementType().notNull().IsGenericParameter) routeParameterMethod = method;
                        }
                    }
                }
            }
            if (object.ReferenceEquals(routeParameterMethod, AutoCSer.Common.NullMethodInfo)) throw new EntryPointNotFoundException();
        }
    }
}
