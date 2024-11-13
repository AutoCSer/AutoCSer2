using AutoCSer.Extensions;
using System;
using System.IO;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 加载修复节点方法
    /// </summary>
    internal abstract class RepairNodeMethodLoader : AutoCSer.Threading.Link<RepairNodeMethodLoader>
    {
        /// <summary>
        /// 生成服务端节点
        /// </summary>
        protected readonly ServerNodeCreator nodeCreator;
        /// <summary>
        /// 节点方法目录
        /// </summary>
        protected readonly DirectoryInfo methodDirectory;
        /// <summary>
        /// 修复方法目录信息
        /// </summary>
        protected readonly RepairNodeMethodDirectory repairNodeMethodDirectory;
        /// <summary>
        /// 加载修复节点方法
        /// </summary>
        /// <param name="nodeCreator">生成服务端节点</param>
        /// <param name="methodDirectory">节点方法目录</param>
        /// <param name="repairNodeMethodDirectory">修复方法目录信息</param>
        protected RepairNodeMethodLoader(ServerNodeCreator nodeCreator, DirectoryInfo methodDirectory, ref RepairNodeMethodDirectory repairNodeMethodDirectory)
        {
            this.nodeCreator = nodeCreator;
            this.methodDirectory = methodDirectory;
            this.repairNodeMethodDirectory = repairNodeMethodDirectory;
        }
        /// <summary>
        /// 初始化节点加载修复方法
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        internal abstract RepairNodeMethod? LoadRepair();
#else
        internal abstract RepairNodeMethod LoadRepair();
#endif
    }
    /// <summary>
    /// 加载修复节点方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class RepairNodeMethodLoader<T> : RepairNodeMethodLoader
    {
        /// <summary>
        /// 加载修复节点方法
        /// </summary>
        /// <param name="nodeCreator">生成服务端节点</param>
        /// <param name="methodDirectory">节点方法目录</param>
        /// <param name="repairNodeMethodDirectory">修复方法目录信息</param>
        internal RepairNodeMethodLoader(ServerNodeCreator nodeCreator, DirectoryInfo methodDirectory, ref RepairNodeMethodDirectory repairNodeMethodDirectory) : base(nodeCreator, methodDirectory, ref repairNodeMethodDirectory) { }
        /// <summary>
        /// 初始化节点加载修复方法
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        internal override RepairNodeMethod? LoadRepair()
#else
        internal override RepairNodeMethod LoadRepair()
#endif
        {
            try
            {
                RepairNodeMethod repairNodeMethod = nodeCreator.loadRepairNodeMethod<T>(methodDirectory, typeof(T).fullName().notNull(), null, false).notNull();
                repairNodeMethod.RepairNodeMethodDirectory = repairNodeMethodDirectory;
                return repairNodeMethod;
            }
            catch(Exception exception)
            {
                nodeCreator.State = CallStateEnum.LoadRepairNodeMethodException;
                AutoCSer.LogHelper.ExceptionIgnoreException(exception);
            }
            return null;
        }
    }
}
