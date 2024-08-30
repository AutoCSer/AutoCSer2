using System;

namespace AutoCSer.Example.CommandServer.Client
{
    /// <summary>
    /// 客户端定义非对称测试接口
    /// </summary>
    [CommandServerControllerInterface(MethodIndexEnumType = typeof(DefinedDissymmetryMethodEnum), IsAutoMethodIndex = false)]
    public interface IDefinedDissymmetryClientController
    {
    }
}
