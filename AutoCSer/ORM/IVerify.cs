using System;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 仅用于快速判断是否可能实现接口 IVerify{T}
    /// </summary>
    public interface IVerify { }
    /// <summary>
    /// 自定义数据验证
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IVerify<T> : IVerify
    {
        /// <summary>
        /// 自定义验证，验证失败需要抛出异常
        /// </summary>
        /// <returns></returns>
        T Verify();
    }
}
