using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 从服务节点加载数据
    /// </summary>
    internal sealed class SlaveLoader
    {
        /// <summary>
        /// 是否输出反序列化错误日志
        /// </summary>
        private static bool isDeserializeLog = true;

        /// <summary>
        /// 服务端会话绑定日志流持久化内存数据库服务
        /// </summary>
        private readonly StreamPersistenceMemoryDatabaseServiceBase service;
        /// <summary>
        /// 主节点客户端
        /// </summary>
        private readonly IStreamPersistenceMemoryDatabaseClientSocketEvent masterClient;
        /// <summary>
        /// 从服务验证时间戳
        /// </summary>
        private long timestamp;
        /// <summary>
        /// 服务端持久化流已写入位置
        /// </summary>
        private long persistencePosition;
        /// <summary>
        /// 从节点获取修复节点方法信息命令
        /// </summary>
#if NetStandard21
        private CommandKeepCallback? getRepairNodeMethodPositionCommandKeepCallback;
#else
        private CommandKeepCallback getRepairNodeMethodPositionCommandKeepCallback;
#endif
        /// <summary>
        /// 持久化回调异常位置文件流
        /// </summary>
#if NetStandard21
        private FileStream? persistenceCallbackExceptionPositionStream;
#else
        private FileStream persistenceCallbackExceptionPositionStream;
#endif
        /// <summary>
        /// 获取持久化回调异常位置文件数据命令
        /// </summary>
#if NetStandard21
        private CommandKeepCallback? getPersistenceCallbackExceptionPositionFileCommandKeepCallback;
#else
        private CommandKeepCallback getPersistenceCallbackExceptionPositionFileCommandKeepCallback;
#endif
        /// <summary>
        /// 获取持久化回调异常位置数据命令
        /// </summary>
#if NetStandard21
        private CommandKeepCallback? getPersistenceCallbackExceptionPositionCommandKeepCallback;
#else
        private CommandKeepCallback getPersistenceCallbackExceptionPositionCommandKeepCallback;
#endif
        /// <summary>
        /// 持久化文件流
        /// </summary>
#if NetStandard21
        private FileStream? persistenceStream;
#else
        private FileStream persistenceStream;
#endif
        /// <summary>
        /// 获取持久化文件数据命令
        /// </summary>
#if NetStandard21
        private CommandKeepCallback? getPersistenceFileCommandKeepCallback;
#else
        private CommandKeepCallback getPersistenceFileCommandKeepCallback;
#endif
        /// <summary>
        /// 获取持久化回调异常位置文件数据是否已完成
        /// </summary>
        private bool getPersistenceCallbackExceptionPositionFileCompleted;
        /// <summary>
        /// 调用错误状态
        /// </summary>
        private CallStateEnum callState;
        /// <summary>
        /// 服务错误返回值
        /// </summary>
        private CommandClientReturnTypeEnum returnType;
        /// <summary>
        /// 是否备份客户端
        /// </summary>
        private bool isBackup;
        /// <summary>
        /// 从服务节点加载数据
        /// </summary>
        /// <param name="service"></param>
        /// <param name="masterClient"></param>
        internal SlaveLoader(StreamPersistenceMemoryDatabaseServiceBase service, IStreamPersistenceMemoryDatabaseClientSocketEvent masterClient)
        {
            this.service = service;
            this.masterClient = masterClient;
            isBackup = service.IsBackup;
        }
        /// <summary>
        /// 关闭数据加载
        /// </summary>
        internal void Close()
        {
            if (timestamp != 0)
            {
                masterClient.StreamPersistenceMemoryDatabaseClient.RemoveSlave(timestamp);
                timestamp = 0;
                getRepairNodeMethodPositionCommandKeepCallback?.Dispose();
                getPersistenceCallbackExceptionPositionFileCommandKeepCallback?.Dispose();
                getPersistenceCallbackExceptionPositionCommandKeepCallback?.Dispose();
                persistenceCallbackExceptionPositionStream?.Dispose();
                getPersistenceFileCommandKeepCallback?.Dispose();
                persistenceStream?.Dispose();
            }
        }
        /// <summary>
        /// 开始加载数据
        /// </summary>
        /// <returns></returns>
        internal async Task Load()
        {
            bool isLoad = false;
            try
            {
                CommandClientReturnValue<long> timestamp = await masterClient.StreamPersistenceMemoryDatabaseClient.CreateSlave(isBackup);
                if (check(ref timestamp))
                {
                    if (timestamp.Value > 0)
                    {
                        this.timestamp = timestamp.Value;
                        if(!isBackup || await tryGetPersistenceFile()) isLoad = await getPersistenceCallbackExceptionPositionFile() && await getRepairNodeMethodPosition();
                    }
                    else
                    {
                        await AutoCSer.LogHelper.Error($"从节点创建失败 {(CallStateEnum)(byte)-timestamp.Value}");
                        if (-timestamp.Value == (byte)CallStateEnum.CanNotCreateSlave) isLoad = true;
                    }
                }
            }
            catch (Exception exception)
            {
                await AutoCSer.LogHelper.Exception(exception);
            }
            finally
            {
                if (!isLoad) service.CloseLoader(this, true);
            }
        }
        /// <summary>
        /// 获取持久化回调异常位置文件数据
        /// </summary>
        /// <returns></returns>
        private async Task<bool> getPersistenceCallbackExceptionPositionFile()
        {
            uint persistenceFileHeadVersion = 0;
            ulong rebuildPosition = 0;
            if (await AutoCSer.Common.FileExists(service.PersistenceCallbackExceptionPositionFileInfo, true) && service.PersistenceCallbackExceptionPositionFileInfo.Length >= ServiceLoader.ExceptionPositionFileHeadSize)
            {
                persistenceCallbackExceptionPositionStream = await AutoCSer.Common.CreateFileStream(service.PersistenceCallbackExceptionPositionFileInfo.FullName, FileMode.Open, FileAccess.ReadWrite);
                if (await persistenceCallbackExceptionPositionStream.ReadAsync(service.PersistenceDataPositionBuffer, 0, ServiceLoader.ExceptionPositionFileHeadSize) != ServiceLoader.ExceptionPositionFileHeadSize)
                {
                    callState = CallStateEnum.ReadFileSizeError;
                    return false;
                }
                persistenceFileHeadVersion = ServiceLoader.GetPersistenceFileHeadVersion(service.PersistenceDataPositionBuffer, out rebuildPosition);
                CommandClientReturnValue<long> position = await masterClient.StreamPersistenceMemoryDatabaseClient.CheckPersistenceCallbackExceptionPositionFileHead(persistenceFileHeadVersion, rebuildPosition);
                if (!check(ref position)) return false;
                if (position.Value >= persistenceCallbackExceptionPositionStream.Position) await AutoCSer.Common.Seek(persistenceCallbackExceptionPositionStream, 0, SeekOrigin.End);
                else
                {
                    await persistenceCallbackExceptionPositionStream.DisposeAsync();
                    await AutoCSer.Common.FileMove(service.PersistenceCallbackExceptionPositionFileInfo.FullName, service.PersistenceCallbackExceptionPositionFileInfo.FullName + service.Config.GetBackupFileNameSuffix() + ".bak");
                    persistenceCallbackExceptionPositionStream = null;
                    persistenceFileHeadVersion = 0;
                    rebuildPosition = 0;
                }
            }
            if (persistenceCallbackExceptionPositionStream == null) persistenceCallbackExceptionPositionStream = await AutoCSer.Common.CreateFileStream(service.PersistenceCallbackExceptionPositionFileInfo.FullName, FileMode.Create, FileAccess.Write);
            getPersistenceCallbackExceptionPositionCommandKeepCallback = await masterClient.StreamPersistenceMemoryDatabaseClient.GetPersistenceCallbackExceptionPosition(timestamp, getPersistenceCallbackExceptionPosition);
            if (getPersistenceCallbackExceptionPositionCommandKeepCallback != null)
            {
                getPersistenceCallbackExceptionPositionFileCommandKeepCallback = await masterClient.StreamPersistenceMemoryDatabaseClient.GetPersistenceCallbackExceptionPositionFile(timestamp, persistenceFileHeadVersion, rebuildPosition, persistenceCallbackExceptionPositionStream.Position, getPersistenceCallbackExceptionPositionFile);
                if (getPersistenceCallbackExceptionPositionFileCommandKeepCallback != null) return true;
            }
            callState = CallStateEnum.CallFail;
            return false;
        }
        /// <summary>
        /// 获取持久化回调异常位置数据
        /// </summary>
        /// <param name="returnValue"></param>
        /// <param name="keepCallbackCommand"></param>
        private unsafe void getPersistenceCallbackExceptionPosition(CommandClientReturnValue<long> returnValue, KeepCallbackCommand keepCallbackCommand)
        {
            if (check(ref returnValue))
            {
                long position = returnValue.Value;
                try
                {
                    if (position > 0)
                    {
                        fixed (byte* buffer = service.PersistenceDataPositionBuffer) *(long*)buffer = position;
                        var fileStream = persistenceCallbackExceptionPositionStream.notNull();
                        fileStream.Write(service.PersistenceDataPositionBuffer, 0, sizeof(long));
                        fileStream.Flush();
                        if (!isBackup) AutoCSer.Common.EmptyFunction();//添加异常位置数据
                        return;
                    }
                    if (-position == (byte)CallStateEnum.Success)
                    {
                        if (!getPersistenceCallbackExceptionPositionFileCompleted)
                        {
                            persistenceCallbackExceptionPositionStream.notNull().Flush();
                            getPersistenceCallbackExceptionPositionFileCompleted = true;
                            if (!isBackup) getPersistenceFile().NotWait();
                            return;
                        }
                        callState = CallStateEnum.StateNotMatch;
                    }
                    else callState = (CallStateEnum)(byte)(ulong)-position;
                }
                catch (Exception exception)
                {
                    AutoCSer.LogHelper.ExceptionIgnoreException(exception);
                }
            }
            service.CloseLoader(this, true);
        }
        /// <summary>
        /// 获取持久化回调异常位置文件数据
        /// </summary>
        /// <param name="returnValue"></param>
        /// <param name="keepCallbackCommand"></param>
        private void getPersistenceCallbackExceptionPositionFile(CommandClientReturnValue<PersistenceFileBuffer> returnValue, KeepCallbackCommand keepCallbackCommand)
        {
            bool isRetry = true;
            switch (returnValue.ReturnType)
            {
                case CommandClientReturnTypeEnum.Success:
                    switch (returnValue.Value.CallState)
                    {
                        case CallStateEnum.Success: return;
                        case CallStateEnum.NotFoundSessionObject: isRetry = false; break;
                    }
                    callState = returnValue.Value.CallState;
                    break;
                case CommandClientReturnTypeEnum.CancelKeepCallback:
                    if (getPersistenceCallbackExceptionPositionFileCompleted) return;
                    returnType = returnValue.ReturnType;
                    break;
                default: returnType = returnValue.ReturnType; break;
            }
            service.CloseLoader(this, isRetry);
        }
        /// <summary>
        /// 获取持久化回调异常位置文件数据
        /// </summary>
        /// <param name="position"></param>
        /// <param name="buffer"></param>
        internal void GetPersistenceCallbackExceptionPositionFile(long position, ref SubArray<byte> buffer)
        {
            bool isLoad = false;
            try
            {
                var fileStream = persistenceCallbackExceptionPositionStream.notNull();
                if (fileStream.Position == position)
                {
                    fileStream.Write(buffer.Array, buffer.Start, buffer.Length);
                    if (!isBackup) AutoCSer.Common.EmptyFunction();//添加异常位置数据
                    isLoad = true;
                }
                else callState = CallStateEnum.PositionNotMatch;
            }
            catch (Exception exception)
            {
                AutoCSer.LogHelper.ExceptionIgnoreException(exception);
            }
            finally
            {
                if (!isLoad) service.CloseLoader(this, true);
            }
        }
        /// <summary>
        /// 获取持久化文件数据
        /// </summary>
        /// <returns></returns>
        private async Task<bool> tryGetPersistenceFile()
        {
            uint persistenceFileHeadVersion = 0;
            ulong rebuildPosition = 0;
            if (await AutoCSer.Common.FileExists(service.PersistenceFileInfo, true) && service.PersistenceFileInfo.Length >= ServiceLoader.FileHeadSize)
            {
                persistenceStream = await AutoCSer.Common.CreateFileStream(service.PersistenceFileInfo.FullName, FileMode.Open, FileAccess.ReadWrite);
                if (await persistenceStream.ReadAsync(service.PersistenceDataPositionBuffer, 0, ServiceLoader.FileHeadSize) != ServiceLoader.FileHeadSize)
                {
                    callState = CallStateEnum.ReadFileSizeError;
                    return false;
                }
                persistenceFileHeadVersion = ServiceLoader.GetPersistenceFileHeadVersion(service.PersistenceDataPositionBuffer, out rebuildPosition);
                CommandClientReturnValue<long> position = await masterClient.StreamPersistenceMemoryDatabaseClient.CheckPersistenceFileHead(persistenceFileHeadVersion, rebuildPosition);
                if (!check(ref position)) return false;
                if (position.Value >= persistenceStream.Position) await AutoCSer.Common.Seek(persistenceStream, 0, SeekOrigin.End);
                else
                {
                    await persistenceStream.DisposeAsync();
                    await AutoCSer.Common.FileMove(service.PersistenceFileInfo.FullName, service.PersistenceFileInfo.FullName + service.Config.GetBackupFileNameSuffix() + ".bak");
                    persistenceStream = null;
                    persistenceFileHeadVersion = 0;
                    rebuildPosition = 0;
                }
            }
            if (persistenceStream == null) persistenceStream = await AutoCSer.Common.CreateFileStream(service.PersistenceFileInfo.FullName, FileMode.Create, FileAccess.Write);
            getPersistenceFileCommandKeepCallback = await masterClient.StreamPersistenceMemoryDatabaseClient.GetPersistenceFile(timestamp, persistenceFileHeadVersion, rebuildPosition, persistenceStream.Position, getPersistenceFile);
            if (getPersistenceFileCommandKeepCallback != null) return true;
            callState = CallStateEnum.CallFail;
            return false;
        }
        /// <summary>
        /// 获取持久化文件数据
        /// </summary>
        /// <returns></returns>
        private async Task getPersistenceFile()
        {
            bool isLoad = false;
            try
            {
                isLoad =  await tryGetPersistenceFile();
            }
            catch (Exception exception)
            {
                await AutoCSer.LogHelper.Exception(exception);
            }
            finally
            {
                if (!isLoad) service.CloseLoader(this, true);
            }
        }
        /// <summary>
        /// 获取持久化文件数据
        /// </summary>
        /// <param name="returnValue"></param>
        /// <param name="keepCallbackCommand"></param>
        private void getPersistenceFile(CommandClientReturnValue<PersistenceFileBuffer> returnValue, KeepCallbackCommand keepCallbackCommand)
        {
            bool isRetry = true;
            if (check(ref returnValue))
            {
                switch (returnValue.Value.CallState)
                {
                    case CallStateEnum.Success: return;
                    case CallStateEnum.NotFoundSessionObject: isRetry = false; break;
                }
                callState = returnValue.Value.CallState;
            }
            service.CloseLoader(this, isRetry);
        }
        /// <summary>
        /// 获取持久化文件数据
        /// </summary>
        /// <param name="position"></param>
        /// <param name="buffer"></param>
        internal void GetPersistenceFile(long position, ref SubArray<byte> buffer)
        {
            bool isLoad = false;
            try
            {
                var fileStream = persistenceStream.notNull();
                if (fileStream.Position == position)
                {
                    fileStream.Write(buffer.Array, buffer.Start, buffer.Length);
                    fileStream.Flush();
                    if (!isBackup) AutoCSer.Common.EmptyFunction();//尝试触发数据解析
                    isLoad = true;
                }
                else callState = CallStateEnum.PositionNotMatch;
            }
            catch (Exception exception)
            {
                AutoCSer.LogHelper.ExceptionIgnoreException(exception);
            }
            finally
            {
                if (!isLoad) service.CloseLoader(this, true);
            }
        }
        /// <summary>
        /// 从节点获取修复节点方法信息
        /// </summary>
        /// <returns></returns>
        private async Task<bool> getRepairNodeMethodPosition()
        {
            RepairNodeMethodDirectory repairNodeMethodDirectoryKey = default(RepairNodeMethodDirectory);
            DirectoryInfo repairNodeMethodDirectory = new DirectoryInfo(Path.Combine(service.PersistenceFileInfo.Directory.notNull().FullName, service.Config.RepairNodeMethodDirectoryName));
            if (!await AutoCSer.Common.TryCreateDirectory(repairNodeMethodDirectory))
            {
                foreach (DirectoryInfo typeDirectory in await AutoCSer.Common.GetDirectories(repairNodeMethodDirectory))
                {
                    if (typeDirectory.Name.Length == 16 && ServerNodeCreator.GetNodeTypeHashCode(typeDirectory, out repairNodeMethodDirectoryKey.NodeTypeHashCode))
                    {
                        foreach (DirectoryInfo methodDirectory in await AutoCSer.Common.GetDirectories(typeDirectory))
                        {
                            if (methodDirectory.Name.Length == 16 + 14 + 8 && ServerNodeCreator.GetMethodDirectory(methodDirectory, ref repairNodeMethodDirectoryKey))
                            {
                                FileInfo assemblyFile = new FileInfo(Path.Combine(methodDirectory.FullName, service.Config.RepairNodeMethodAssemblyFileName));
                                FileInfo methodNameFile = new FileInfo(Path.Combine(methodDirectory.FullName, service.Config.RepairNodeMethodNameFileName));
                                if (await AutoCSer.Common.FileExists(assemblyFile) && await AutoCSer.Common.FileExists(methodNameFile))
                                {
                                    if (!await masterClient.StreamPersistenceMemoryDatabaseClient.AppendRepairNodeMethodDirectoryFile(timestamp, repairNodeMethodDirectoryKey, new RepairNodeMethodFile(assemblyFile, methodNameFile)))
                                    {
                                        callState = CallStateEnum.CallFail;
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            getRepairNodeMethodPositionCommandKeepCallback = await masterClient.StreamPersistenceMemoryDatabaseClient.GetRepairNodeMethodPosition(timestamp, getRepairNodeMethodPosition);
            if (getRepairNodeMethodPositionCommandKeepCallback != null) return true;
            callState = CallStateEnum.CallFail;
            return false;
        }
        /// <summary>
        /// 从节点获取修复节点方法信息
        /// </summary>
        /// <param name="returnValue"></param>
        /// <param name="keepCallbackCommand"></param>
        private void getRepairNodeMethodPosition(CommandClientReturnValue<RepairNodeMethodPosition> returnValue, KeepCallbackCommand keepCallbackCommand)
        {
            if (check(ref returnValue))
            {
                var repairNodeMethod = returnValue.Value.RepairNodeMethod;
                if (repairNodeMethod == null)
                {
                    persistencePosition = returnValue.Value.Position;
                    if (!isBackup) AutoCSer.Common.EmptyFunction();//尝试触发数据解析
                    return;
                }
                if (repairNodeMethod.CallState == CallStateEnum.Success) return;
                callState = repairNodeMethod.CallState;
            }
            service.CloseLoader(this, true);
        }
        /// <summary>
        /// 检查服务返回值状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private bool check<T>(ref CommandClientReturnValue<T> returnValue)
        {
            if (returnValue.IsSuccess) return true;
            returnType = returnValue.ReturnType;
            return false;
        }

        /// <summary>
        /// 获取从服务节点加载数据接口实例
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns></returns>
#if NetStandard21
        internal static ISlaveLoader? GetSessionObject(AutoCSer.BinaryDeserializer deserializer)
#else
        internal static ISlaveLoader GetSessionObject(AutoCSer.BinaryDeserializer deserializer)
#endif
        {
            CommandClientSocket socket = (CommandClientSocket)deserializer.Context.notNull();
            var loader = socket.SessionObject as ISlaveLoader;
            if (loader != null) return loader;
            else if (isDeserializeLog)
            {
                isDeserializeLog = false;
                if (socket.SessionObject == null) socket.Client.Log.ErrorIgnoreException($"日志流持久化内存数据库从服务客户端缺少套接字自定义会话对象，请在自定义初始化阶段 {typeof(ProcessGuardCommandClientSocketEvent).fullName()}.{nameof(ProcessGuardCommandClientSocketEvent.OnMethodVerified)} 设置 {typeof(CommandClientSocket).fullName()}.{nameof(CommandClientSocket.SessionObject)} = {typeof(ISlaveLoader).fullName()}", LogLevelEnum.Error | LogLevelEnum.Fatal);
                else socket.Client.Log.ErrorIgnoreException($"日志流持久化内存数据库从服务客户端套接字自定义会话对象类型错误 {socket.SessionObject.GetType().fullName()} 未实现接口 {typeof(ISlaveLoader).fullName()}", LogLevelEnum.Error | LogLevelEnum.Fatal);
            }
            return null;
        }
    }
}
