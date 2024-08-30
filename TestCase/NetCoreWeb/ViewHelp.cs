using AutoCSer.NetCoreWeb;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.NetCoreWeb
{
    /// <summary>
    /// 数据视图 API 帮助文档数据视图
    /// </summary>
    [AutoCSer.NetCoreWeb.View(IsStaticVersion = true)]
    public partial class ViewHelp : AutoCSer.NetCoreWeb.View
    {
        /// <summary>
        /// JSON API 控制器帮助文档视图数据集合
        /// </summary>
        public ICollection<JsonApiControllerHelpView> Controllers;
        /// <summary>
        /// 数据视图请求信息帮助类视图数据集合
        /// </summary>
        public ICollection<ViewHelpView> Views;
        /// <summary>
        /// JSON API 控制器帮助文档视图数据
        /// </summary>
        public JsonApiControllerHelpView Controller;
        /// <summary>
        /// 类型帮助文档视图数据
        /// </summary>
        public TypeHelpView Type;
        /// <summary>
        /// 视图数据初始化方法名称必须为 LoadView，返回值类型必须为 Task{AutoCSer.NetCoreWeb.ResponseResult}，参数则根据具体需求而定，如果不需要初始化过程则不需要定义该方法
        /// </summary>
        /// <param name="viewMiddleware">数据视图中间件，必须放在所有数据参数之前</param>
        /// <param name="type">帮助文档类型</param>
        /// <param name="controllerTypeFullName">控制器类型名称</param>
        /// <param name="typeFullName">类型名称</param>
        /// <returns>返回值状态为 Success 则正常执行返回视图数据操作，否则直接将错误信息返回给客户端</returns>
        private Task<ResponseResult> LoadView(AutoCSer.NetCoreWeb.ViewMiddleware viewMiddleware, ViewHelpTypeEnum type
            , [ParameterConstraint(IsDefault = true, IsEmpty = true)] string controllerTypeFullName
            , [ParameterConstraint(IsDefault = true, IsEmpty = true)] string typeFullName)
        {
            switch (type)
            {
                case ViewHelpTypeEnum.Controller: Controller = viewMiddleware.GetControllerHelpView(controllerTypeFullName); break;
                case ViewHelpTypeEnum.Type: Type = viewMiddleware.GetTypeHelpView(typeFullName); break;
                default:
                    Controllers = viewMiddleware.ControllerHelpViews;
                    Views = viewMiddleware.ViewHelpTypes;
                    break;
            }
            return SuccessResponseResultTask;
        }
    }
    /// <summary>
    /// 帮助文档类型
    /// </summary>
    public enum ViewHelpTypeEnum : byte
    {
        /// <summary>
        /// 加载控制器列表视图数据
        /// </summary>
        ControllerList,
        /// <summary>
        /// 加载指定控制器视图数据
        /// </summary>
        Controller,
        /// <summary>
        /// 加载指定类型视图数据
        /// </summary>
        Type,
    }
}
