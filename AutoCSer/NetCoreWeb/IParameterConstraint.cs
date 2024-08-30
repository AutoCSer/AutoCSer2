using System;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// 代理控制器方法参数自定义约束接口
    /// </summary>
    public interface IParameterConstraint
    {
        /// <summary>
        /// 检查参数数据
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="summary">参数描述</param>
        /// <returns>错误信息，返回 null 表示参数正常</returns>
        string Check(string name, string summary);
    }
}
