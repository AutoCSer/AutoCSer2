﻿using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 从节点客户端信息
    /// </summary>
    internal sealed class ServiceSlave : AutoCSer.Threading.Link<ServiceSlave>
    {
        /// <summary>
        /// 日志流持久化内存数据库服务端
        /// </summary>
        private readonly StreamPersistenceMemoryDatabaseService service;
        /// <summary>
        /// 修复方法目录与文件信息集合
        /// </summary>
        private readonly Dictionary<RepairNodeMethodDirectory, RepairNodeMethodFile> repairNodeMethodDirectoryFiles;
        /// <summary>
        /// 异常移除客户端回调
        /// </summary>
        private ServiceSlaveCallback removeCallback;
        /// <summary>
        /// 获取修复节点方法信息委托
        /// </summary>
        private CommandServerKeepCallback<RepairNodeMethodPosition> repairNodeMethodPositionCallback;
        /// <summary>
        /// 获取持久化文件数据委托
        /// </summary>
        private CommandServerKeepCallback<PersistenceFileBuffer> persistenceFileCallback;
        /// <summary>
        /// 获取持久化回调异常位置文件数据委托
        /// </summary>
        private CommandServerKeepCallback<PersistenceFileBuffer> persistenceCallbackExceptionPositionFileCallback;
        /// <summary>
        /// 获取持久化回调异常位置信息委托
        /// </summary>
        internal CommandServerKeepCallback<long> PersistenceCallbackExceptionPositionCallback;
        /// <summary>
        /// 持久化文件读取操作等待锁
        /// </summary>
        private ManualResetEvent readPersistenceWaitLock;
        /// <summary>
        /// 持久化文件流
        /// </summary>
        private FileStream persistenceFileStream;
        /// <summary>
        /// 持久化异常位置文件流
        /// </summary>
        private FileStream persistenceCallbackExceptionPositionFileStream;
        /// <summary>
        /// 创建从节点客户端信息时间戳
        /// </summary>
        internal readonly long Timestamp;
        /// <summary>
        /// 读取文件缓冲区大小
        /// </summary>
        private readonly int bufferSize;
        /// <summary>
        /// 是否直接获取持久化回调异常位置信息，否则正在读取文件信息
        /// </summary>
        private bool isPersistenceCallbackExceptionPosition;
        /// <summary>
        /// 是否已释放资源
        /// </summary>
        private bool isClosed;
        /// <summary>
        /// 是否备份客户端
        /// </summary>
        private readonly bool isBackup;
        /// <summary>
        /// 读取文件位置
        /// </summary>
        private long persistencePosition;
        /// <summary>
        /// 读取持久化回调异常位置文件位置
        /// </summary>
        private long persistenceCallbackExceptionFilePosition;
        /// <summary>
        /// 从节点客户端信息
        /// </summary>
        /// <param name="service"></param>
        /// <param name="socket"></param>
        /// <param name="isBackup">是否备份客户端</param>
        internal ServiceSlave(StreamPersistenceMemoryDatabaseService service, CommandServerSocket socket, bool isBackup)
        {
            this.service = service;
            LinkNext = service.Slave;
            this.isBackup = isBackup;
            removeCallback = new ServiceSlaveCallback(this, ServiceSlaveCallbackTypeEnum.Remove);
            bufferSize = socket.Server.SendBufferPool.Size;
            repairNodeMethodDirectoryFiles = DictionaryCreator<RepairNodeMethodDirectory>.Create<RepairNodeMethodFile>();
            Timestamp = service.GetSlaveClientTimestamp();
            service.Slave = this;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        internal void Close()
        {
            if (!isClosed)
            {
                isClosed = true;
                readPersistenceWaitLock?.Set();
                repairNodeMethodPositionCallback?.CancelKeep();
                persistenceCallbackExceptionPositionFileCallback?.CancelKeep();
                PersistenceCallbackExceptionPositionCallback?.CancelKeep();
                persistenceCallbackExceptionPositionFileStream?.Dispose();
                persistenceFileStream?.Dispose();
                persistenceFileCallback?.CancelKeep();
            }
        }
        /// <summary>
        /// 移除从节点
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void RemoveCallback()
        {
            service.RemoveSlave(Timestamp);
        }
        /// <summary>
        /// 移除从节点
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void remove()
        {
            if (!isClosed)
            {
                ServiceSlaveCallback removeCallback = System.Threading.Interlocked.Exchange(ref this.removeCallback, null);
                if (removeCallback != null) service.CommandServerCallQueue.AddOnly(removeCallback);
            }
        }
        /// <summary>
        /// 添加修复方法目录与文件信息
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        internal bool AppendRepairNodeMethodDirectoryFile(RepairNodeMethodDirectory directory, RepairNodeMethodFile file)
        {
            bool isAppend = false;
            try
            {
                repairNodeMethodDirectoryFiles.Add(directory, file);
                return isAppend = true;
            }
            catch (Exception exception)
            {
                AutoCSer.LogHelper.ExceptionIgnoreException(exception);
            }
            finally
            {
                if (!isAppend) Close();
            }
            return false;
        }
        /// <summary>
        /// 获取修复节点方法信息
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        internal bool GetRepairNodeMethodPosition(ref CommandServerKeepCallback<RepairNodeMethodPosition> callback)
        {
            try
            {
                RepairNodeMethodFile file;
                for (RepairNodeMethod head = service.LoadedRepairNodeMethod; head != null; head = head.LinkNext)
                {
                    if (!repairNodeMethodDirectoryFiles.Remove(head.RepairNodeMethodDirectory, out file) || !head.RepairNodeMethodFile.Equals(file))
                    {
                        if (!callback.Callback(new RepairNodeMethodPosition(head))) return false;
                    }
                }
                if (!isBackup && !callback.Callback(new RepairNodeMethodPosition(service.PersistencePosition))) return false;
                repairNodeMethodPositionCallback = callback;
                return true;
            }
            catch(Exception exception)
            {
                AutoCSer.LogHelper.ExceptionIgnoreException(exception);
            }
            finally
            {
                callback = null;
                if (repairNodeMethodPositionCallback == null) Close();
            }
            return false;
        }
        /// <summary>
        /// 修复节点方法信息回调
        /// </summary>
        /// <param name="repairNodeMethod"></param>
        /// <returns></returns>
        internal bool AppendRepairNodeMethod(RepairNodeMethod repairNodeMethod)
        {
            bool isCallback = false;
            try
            {
                RepairNodeMethodFile file;
                if (repairNodeMethodDirectoryFiles.Remove(repairNodeMethod.RepairNodeMethodDirectory, out file) && repairNodeMethod.RepairNodeMethodFile.Equals(file))
                {
                    return isCallback = true;
                }
                if (repairNodeMethodPositionCallback.Callback(new RepairNodeMethodPosition(repairNodeMethod))) return isCallback = true;
                repairNodeMethodPositionCallback = null;
            }
            catch (Exception exception)
            {
                AutoCSer.LogHelper.ExceptionIgnoreException(exception);
            }
            finally
            {
                if (!isCallback) Close();
            }
            return false;
        }
        /// <summary>
        /// 持久化流已写入位置回调
        /// </summary>
        /// <param name="persistencePosition">持久化流已写入位置</param>
        /// <returns></returns>
        internal bool SetPersistencePosition(long persistencePosition)
        {
            if (isBackup) readPersistenceWaitLock?.Set();
            else
            {
                bool isCallback = false;
                try
                {
                    if (repairNodeMethodPositionCallback.Callback(new RepairNodeMethodPosition(persistencePosition)))
                    {
                        readPersistenceWaitLock?.Set();
                        return isCallback = true;
                    }
                    repairNodeMethodPositionCallback = null;
                }
                catch (Exception exception)
                {
                    AutoCSer.LogHelper.ExceptionIgnoreException(exception);
                }
                finally
                {
                    if (!isCallback) Close();
                }
                return false;
            }
            return true;
        }
        /// <summary>
        /// 获取持久化文件数据
        /// </summary>
        /// <param name="position"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        internal bool GetPersistenceFile(long position, ref CommandServerKeepCallback<PersistenceFileBuffer> callback)
        {
            try
            {
                readPersistenceWaitLock = new ManualResetEvent(false);
                persistencePosition = position;
                persistenceFileCallback = callback;
                AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(getPersistenceFile);
                callback = null;
                return true;
            }
            catch (Exception exception)
            {
                AutoCSer.LogHelper.ExceptionIgnoreException(exception);
            }
            finally
            {
                if (callback != null)
                {
                    persistenceFileCallback = null;
                    Close();
                }
            }
            return false;
        }
        /// <summary>
        /// 获取持久化文件数据
        /// </summary>
        private void getPersistenceFile()
        {
            ByteArrayBuffer buffer = default(ByteArrayBuffer);
            try
            {
                buffer = ByteArrayPool.GetBuffer(this.bufferSize);
                PersistenceFileBuffer fileBuffer = new PersistenceFileBuffer(ref buffer, false);
                byte[] bufferArray = buffer.Buffer.Buffer;
                int bufferSize = buffer.Buffer.BufferSize;
                Action onFree = fileBuffer.SetSerializeWaitLock;
                using (persistenceFileStream = new FileStream(service.PersistenceFileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, bufferSize, FileOptions.SequentialScan))
                {
                    if (isClosed) return;
                    persistenceFileStream.Seek(persistencePosition, SeekOrigin.Begin);
                    while (readPersistenceWaitLock.Reset() && !isClosed)
                    {
                        int readIndex = 0;
                        while (persistencePosition < service.PersistencePosition)
                        {
                            int readSize = persistenceFileStream.Read(bufferArray, buffer.StartIndex + readIndex, bufferSize - readIndex);
                            long position = persistencePosition - readIndex;
                            fileBuffer.SetBuffer(readIndex += readSize, position);
                            if (!persistenceFileCallback.Callback(fileBuffer, onFree))
                            {
                                persistenceFileCallback = null;
                                return;
                            }
                            if (isClosed) return;
                            fileBuffer.SerializeWaitLock.Wait();
                            if (isClosed) return;
                            int serializeSize = fileBuffer.Buffer.Length;
                            if (serializeSize < 0)
                            {
                                persistenceFileCallback = null;
                                return;
                            }
                            if ((readIndex -= serializeSize) != 0) AutoCSer.Common.Config.CopyTo(bufferArray, buffer.StartIndex + serializeSize, bufferArray, buffer.StartIndex, readIndex);
                            persistencePosition += readSize;
                        }
                        while (readIndex > 0)
                        {
                            fileBuffer.SetBuffer(readIndex, persistencePosition - readIndex);
                            if (!persistenceFileCallback.Callback(fileBuffer, onFree))
                            {
                                persistenceFileCallback = null;
                                return;
                            }
                            fileBuffer.SerializeWaitLock.Wait();
                            if (isClosed) return;
                            int serializeSize = fileBuffer.Buffer.Length;
                            if (serializeSize < 0)
                            {
                                persistenceFileCallback = null;
                                return;
                            }
                            if ((readIndex -= serializeSize) != 0) AutoCSer.Common.Config.CopyTo(bufferArray, buffer.StartIndex + serializeSize, bufferArray, buffer.StartIndex, readIndex);
                        }
                        if (!readPersistenceWaitLock.WaitOne()) return;
                    }
                }
                persistenceFileStream = null;
            }
            catch (Exception exception)
            {
                if (!isClosed) AutoCSer.LogHelper.ExceptionIgnoreException(exception);
            }
            finally
            {
                buffer.Free();
                remove();
            }
        }
        /// <summary>
        /// 获取持久化回调异常位置文件数据
        /// </summary>
        /// <param name="position"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        internal bool GetPersistenceCallbackExceptionPositionFile(long position, ref CommandServerKeepCallback<PersistenceFileBuffer> callback)
        {
            try
            {
                if (position == service.PersistenceCallbackExceptionFilePosition)
                {
                    isPersistenceCallbackExceptionPosition = true;
                    if (PersistenceCallbackExceptionPositionCallback.Callback(-(long)(ulong)(byte)CallStateEnum.Success))
                    {
                        callback.CancelKeep();
                        callback = null;
                        return true;
                    }
                    PersistenceCallbackExceptionPositionCallback = null;
                }
                else
                {
                    persistenceCallbackExceptionFilePosition = position;
                    persistenceCallbackExceptionPositionFileCallback = callback;
                    AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(getPersistenceCallbackExceptionPositionFile);
                    callback = null;
                    return true;
                }
            }
            catch (Exception exception)
            {
                AutoCSer.LogHelper.ExceptionIgnoreException(exception);
            }
            finally
            {
                if (callback != null)
                {
                    persistenceCallbackExceptionPositionFileCallback = null;
                    Close();
                }
            }
            return false;
        }
        /// <summary>
        /// 获取持久化回调异常位置文件数据
        /// </summary>
        private void getPersistenceCallbackExceptionPositionFile()
        {
            bool isFile = false;
            ByteArrayBuffer buffer = default(ByteArrayBuffer);
            try
            {
                int bufferSize = Math.Max((int)Math.Min(this.bufferSize, service.PersistenceCallbackExceptionFilePosition - persistenceCallbackExceptionFilePosition), 4 << 10);
                buffer = ByteArrayPool.GetBuffer(bufferSize);
                PersistenceFileBuffer fileBuffer = new PersistenceFileBuffer(ref buffer, true);
                byte[] bufferArray = buffer.Buffer.Buffer;
                bufferSize = buffer.Buffer.BufferSize;
                Action onFree = fileBuffer.SetSerializeWaitLock;
                int readIndex = 0;
                do
                {
                    while (persistenceCallbackExceptionFilePosition < service.PersistenceCallbackExceptionFilePosition)
                    {
                        using (persistenceCallbackExceptionPositionFileStream = new FileStream(service.PersistenceCallbackExceptionPositionFileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, bufferSize, FileOptions.SequentialScan))
                        {
                            persistenceCallbackExceptionPositionFileStream.Seek(persistenceCallbackExceptionFilePosition, SeekOrigin.Begin);
                            if (isClosed) return;
                            do
                            {
                                int readSize = persistenceCallbackExceptionPositionFileStream.Read(bufferArray, buffer.StartIndex + readIndex, bufferSize - readIndex);
                                long position = persistenceCallbackExceptionFilePosition - readIndex;
                                fileBuffer.SetBuffer(readIndex += readSize, position);
                                if (!persistenceCallbackExceptionPositionFileCallback.Callback(fileBuffer, onFree))
                                {
                                    persistenceCallbackExceptionPositionFileCallback = null;
                                    return;
                                }
                                if (isClosed) return;
                                fileBuffer.SerializeWaitLock.Wait();
                                if (isClosed) return;
                                int serializeSize = fileBuffer.Buffer.Length;
                                if (serializeSize < 0)
                                {
                                    persistenceCallbackExceptionPositionFileCallback = null;
                                    return;
                                }
                                if ((readIndex -= serializeSize) != 0) AutoCSer.Common.Config.CopyTo(bufferArray, buffer.StartIndex + serializeSize, bufferArray, buffer.StartIndex, readIndex);
                                persistenceCallbackExceptionFilePosition += readSize;
                            }
                            while (persistenceCallbackExceptionFilePosition < service.PersistenceCallbackExceptionFilePosition);
                        }
                        persistenceCallbackExceptionPositionFileStream = null;
                    }
                    while (readIndex > 0)
                    {
                        fileBuffer.SetBuffer(readIndex, persistenceCallbackExceptionFilePosition - readIndex);
                        if (!persistenceCallbackExceptionPositionFileCallback.Callback(fileBuffer, onFree))
                        {
                            persistenceCallbackExceptionPositionFileCallback = null;
                            return;
                        }
                        fileBuffer.SerializeWaitLock.Wait();
                        if (isClosed) return;
                        int serializeSize = fileBuffer.Buffer.Length;
                        if (serializeSize < 0)
                        {
                            persistenceCallbackExceptionPositionFileCallback = null;
                            return;
                        }
                        if ((readIndex -= serializeSize) != 0) AutoCSer.Common.Config.CopyTo(bufferArray, buffer.StartIndex + serializeSize, bufferArray, buffer.StartIndex, readIndex);
                    }
                }
                while (persistenceCallbackExceptionFilePosition < service.PersistenceCallbackExceptionFilePosition);
                service.CommandServerCallQueue.AddOnly(new ServiceSlaveCallback(this, ServiceSlaveCallbackTypeEnum.CheckPersistenceCallbackExceptionPosition));
                isFile = true;
            }
            catch (Exception exception)
            {
                if (!isClosed) AutoCSer.LogHelper.ExceptionIgnoreException(exception);
            }
            finally
            {
                buffer.Free();
                if (!isFile) remove();
            }
        }
        /// <summary>
        /// 检查持久化回调异常位置文件已写入位置
        /// </summary>
        internal void CheckPersistenceCallbackExceptionPositionCallback()
        {
            if (!isClosed)
            {
                bool isFile = false;
                try
                {
                    if (persistenceCallbackExceptionFilePosition == service.PersistenceCallbackExceptionFilePosition)
                    {
                        isPersistenceCallbackExceptionPosition = true;
                        if (PersistenceCallbackExceptionPositionCallback.Callback(-(long)(ulong)(byte)CallStateEnum.Success))
                        {
                            persistenceCallbackExceptionPositionFileCallback.CancelKeep();
                            persistenceCallbackExceptionPositionFileCallback = null;
                            isFile = true;
                            return;
                        }
                        PersistenceCallbackExceptionPositionCallback = null;
                    }
                    else
                    {
                        AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(getPersistenceCallbackExceptionPositionFile);
                        isFile = true;
                    }
                }
                catch (Exception exception)
                {
                    AutoCSer.LogHelper.ExceptionIgnoreException(exception);
                }
                finally
                {
                    if (!isFile) Close();
                }
            }
        }
        /// <summary>
        /// 持久化回调异常位置信息回调
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        internal bool AppendPersistenceCallbackExceptionPosition(long position)
        {
            if (!isPersistenceCallbackExceptionPosition) return true;
            bool isCallback = false;
            try
            {
                if (PersistenceCallbackExceptionPositionCallback.Callback(position)) return isCallback = true;
                PersistenceCallbackExceptionPositionCallback = null;
            }
            catch (Exception exception)
            {
                AutoCSer.LogHelper.ExceptionIgnoreException(exception);
            }
            finally
            {
                if (!isCallback) Close();
            }
            return false;
        }
    }
}