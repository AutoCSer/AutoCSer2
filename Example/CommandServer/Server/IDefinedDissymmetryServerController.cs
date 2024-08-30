using System;

namespace AutoCSer.Example.CommandServer.Server
{
    /// <summary>
    /// 服务端定义非对称测试接口
    /// </summary>
    [CommandServerControllerInterface(MethodIndexEnumType = typeof(DefinedDissymmetryMethodEnum), IsAutoMethodIndex = false)]
    public interface IDefinedDissymmetryServerController
    {
    }
    /// <summary>
    /// 服务端定义非对称测试接口实例
    /// </summary>
    internal sealed class DefinedDissymmetryServerController : IDefinedDissymmetryServerController
    {
    }
}
