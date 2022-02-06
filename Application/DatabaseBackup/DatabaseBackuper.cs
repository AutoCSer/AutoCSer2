using AutoCSer.Net;
using System;
using System.IO;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 数据库备份器
    /// </summary>
    public abstract class DatabaseBackuper : CommandServerCallQueueNode
    {
        /// <summary>
        /// 数据库备份服务
        /// </summary>
        protected readonly DatabaseBackup databaseBackup;
        /// <summary>
        /// 服务端执行队列
        /// </summary>
        protected readonly CommandServerCallQueue queue;
        /// <summary>
        /// 数据库名称
        /// </summary>
        protected readonly string database;
        /// <summary>
        /// 数据库备份文件名称
        /// </summary>
        protected readonly string backupFullName;
        /// <summary>
        /// 压缩数据库备份文件名称
        /// </summary>
        protected readonly string compressionFullName;
        /// <summary>
        /// 备份异常信息
        /// </summary>
        protected Exception exception;
        /// <summary>
        /// 是否已经完成
        /// </summary>
        private bool isCompleted;
        /// <summary>
        /// 回调集合，重写必须保证触发回调
        /// </summary>
        private LeftArray<CommandServerCallback<string>> callbacks = new LeftArray<CommandServerCallback<string>>(1);
        /// <summary>
        /// 默认为 3600 秒
        /// </summary>
        protected virtual int commandTimeout { get { return 60 * 60; } }
        /// <summary>
        /// 数据库备份器
        /// </summary>
        /// <param name="databaseBackup">数据库备份服务</param>
        /// <param name="queue">服务端执行队列</param>
        /// <param name="database">数据库名称</param>
        /// <param name="backupFullName">数据库备份文件名称</param>
        /// <param name="compressionFullName">压缩数据库备份文件名称</param>
        public DatabaseBackuper(DatabaseBackup databaseBackup, CommandServerCallQueue queue, string database, string backupFullName, string compressionFullName = null)
        {
            this.databaseBackup = databaseBackup;
            this.queue = queue;
            this.database = database;
            this.backupFullName = backupFullName;
            this.compressionFullName = compressionFullName ?? backupFullName;
        }
        /// <summary>
        /// 回调数量
        /// </summary>
        public int CallbackCount { get { return callbacks.Count; } }
        /// <summary>
        /// 尝试添加回调委托
        /// </summary>
        /// <param name="callback"></param>
        public virtual void Callback(ref CommandServerCallback<string> callback)
        {
            if (isCompleted) this.callback(callback);
            else
            {
                callbacks.Add(callback);
                callback = null;
            }
        }
        /// <summary>
        /// 开始备份数据
        /// </summary>
        /// <param name="callback"></param>
        public virtual void Start(ref CommandServerCallback<string> callback)
        {
            callbacks.Add(callback);
            if (AutoCSer.Threading.ThreadPool.TinyBackground.Start(backupThread)) callback = null;
            else
            {
                exception = new Exception("备份线程启动失败");
                RunTask();
            }
        }
        /// <summary>
        /// 备份数据库
        /// </summary>
        private void backupThread()
        {
            Exception exception = null;
            try
            {
                DirectoryInfo Directory = new FileInfo(backupFullName).Directory;
                if (!Directory.Exists) Directory.Create();
                databaseBackup.OnMessage($"开始备份数据库 {database} 到 {backupFullName}");
                backup();
                if (backupFullName != compressionFullName)
                {
                    databaseBackup.OnMessage($"开始压缩备份数据库 {database} 到 {compressionFullName}");
                    compression();
                }
            }
            catch (Exception error)
            {
                AutoCSer.Threading.CatchTask.AddIgnoreException(databaseBackup.OnException(exception = error));
                if (File.Exists(backupFullName)) File.Delete(backupFullName);
                if (File.Exists(compressionFullName)) File.Delete(compressionFullName);
            }
            finally
            {
                completed(exception);
                if (exception == null) databaseBackup.OnMessage($"数据库 {database} 备份完毕 {compressionFullName}");
                else databaseBackup.OnMessage($"数据库 {database} 备份失败");
            }
        }
        /// <summary>
        /// 备份数据库，失败要抛出异常
        /// </summary>
        protected abstract void backup();
        /// <summary>
        /// 压缩数据库备份文件，比如可以引用 SevenZipSharp 压缩成 7z 文件
        /// </summary>
        protected virtual void compression() { }
        /// <summary>
        /// 返回 SQL Server 2016 备份数据库 SQL 语句
        /// </summary>
        /// <returns></returns>
        protected string getBackupSqlServer()
        {
            return $"BACKUP DATABASE [{database}] TO DISK='{backupFullName}' WITH FORMAT,INIT,COMPRESSION";
        }
        /// <summary>
        /// 数据库备份完成
        /// </summary>
        /// <param name="exception">存在异常信息表示备份失败</param>
        protected void completed(Exception exception)
        {
            this.exception = exception;
            queue.Add(this);
        }
        /// <summary>
        /// 执行回调
        /// </summary>
        public override void RunTask()
        {
            try
            {
                foreach (CommandServerCallback<string> callback in callbacks) this.callback(callback);
            }
            finally { databaseBackup.RemoveDatabaseBackuper(database); }
        }
        /// <summary>
        /// 数据库备份完成回调
        /// </summary>
        /// <param name="callback"></param>
        private void callback(CommandServerCallback<string> callback)
        {
            if (exception == null) callback.Callback(compressionFullName);
            else callback.Callback(CommandClientReturnType.ServerException, exception);
        }
    }
}
