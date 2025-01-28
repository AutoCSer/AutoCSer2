using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.DeployTask
{
    /// <summary>
    /// 数据库备份模型模式客户端数据库表格
    /// </summary>
    public abstract class DatabaseBackupClientTable
    {
        /// <summary>
        /// 数据库备份客户端
        /// </summary>
        public readonly DatabaseBackupClient Client;
        /// <summary>
        /// 数据库名称
        /// </summary>
        public readonly string Database;
        /// <summary>
        /// 数据库表格名称
        /// </summary>
        public readonly string TableName;
        /// <summary>
        /// 序列化文件保存路径
        /// </summary>
        public readonly string SavePath;
        /// <summary>
        /// 文件编号
        /// </summary>
        protected int fileIndex;
        /// <summary>
        /// 数据库备份模型模式客户端数据库表格
        /// </summary>
        /// <param name="client">数据库备份客户端</param>
        /// <param name="database">数据库名称</param>
        /// <param name="tableName">数据库表格名称</param>
        /// <param name="savePath">序列化文件保存路径</param>
        public DatabaseBackupClientTable(DatabaseBackupClient client, string database, string tableName, string savePath)
        {
            Client = client;
            Database = database;
            TableName = tableName;
            SavePath = savePath;
        }
        /// <summary>
        /// 备份操作
        /// </summary>
        /// <returns></returns>
        public abstract Task Backup();

        /// <summary>
        /// 序列化配置参数
        /// </summary>
        internal static readonly BinarySerializeConfig BinarySerializeConfig;

        static DatabaseBackupClientTable()
        {
            BinarySerializeConfig = AutoCSer.BinarySerializer.CloneDefaultConfig();
            BinarySerializeConfig.StreamSeek = sizeof(int);
        }
    }
    /// <summary>
    /// 数据库备份模型模式客户端数据库表格
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DatabaseBackupClientTable<T> : DatabaseBackupClientTable
        where T : class
    {
        /// <summary>
        /// 获取数据命令
        /// </summary>
        private readonly Func<string, string, EnumeratorCommand<T>> getCommand;
        /// <summary>
        /// 接收数据缓冲区
        /// </summary>
        protected LeftArray<T> values;
        /// <summary>
        /// 数据库备份模型模式客户端数据库表格
        /// </summary>
        /// <param name="client">数据库备份客户端</param>
        /// <param name="database">数据库名称</param>
        /// <param name="tableName">数据库表格名称</param>
        /// <param name="savePath">序列化文件保存路径</param>
        /// <param name="getCommand">获取数据命令</param>
        /// <param name="capacity">缓存区大小，默认为 65536</param>
        public DatabaseBackupClientTable(DatabaseBackupClient client, string database, string tableName, string savePath, Func<string, string, EnumeratorCommand<T>> getCommand, int capacity = 1 << 16)
            : base(client, database, tableName, savePath)
        {
            this.getCommand = getCommand;
            values = new LeftArray<T>(capacity);
        }
        /// <summary>
        /// 备份操作
        /// </summary>
        /// <returns></returns>
        public override async Task Backup()
        {
            Client.OnMessage($"开始备份数据库 {Database}");
            bool isCompleted = false;
            try
            {
                await AutoCSer.Common.TryCreateDirectory(SavePath);

                var command = await getCommand(Database, TableName);
                if (command != null)
                {
                    while (await command.MoveNext())
                    {
                        T value = command.Current;
                        if (value != null)
                        {
                            values.Add(value);
                            if (values.FreeCount == 0)
                            {
                                await save();
                                values.Clear();
                            }
                        }
                        else
                        {
                            if (values.Count != 0) await save();
                            isCompleted = true;
                            return;
                        }
                    }
                }
            }
            finally { await completed(isCompleted); }
        }
        /// <summary>
        /// 保存缓冲区数据
        /// </summary>
        /// <returns></returns>
        protected virtual async Task save()
        {
            byte[] data = AutoCSer.BinarySerializer.Serialize(values, BinarySerializeConfig);
            int fileSize = data.Length, dataSize = fileSize - sizeof(int);
            using (MemoryStream dataStream = new MemoryStream(fileSize))
            {
                dataStream.Seek(sizeof(int), SeekOrigin.Begin);
                using (DeflateStream compressStream = new DeflateStream(dataStream, CompressionLevel.Fastest, true)) await compressStream.WriteAsync(data, sizeof(int), dataSize);
                if (dataStream.Position < fileSize)
                {
                    data = dataStream.GetBuffer();
                    fileSize = (int)dataStream.Position;
                    dataSize = sizeof(int) - fileSize;
                }
            }
            setDataSize(data, dataSize);
#if NetStandard21
            await using (FileStream fileStream = File.Create(Path.Combine(SavePath, $"{(fileIndex++).toString()}.bak")))
#else
            using (FileStream fileStream = File.Create(Path.Combine(SavePath, $"{(fileIndex++).toString()}.bak")))
#endif
            {
                await fileStream.WriteAsync(data, 0, fileSize);
            }
        }
        /// <summary>
        /// 设置数据字节大小
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataSize"></param>
        private static unsafe void setDataSize(byte[] data, int dataSize)
        {
            fixed (byte* dataFixed = data) *(int*)dataFixed = dataSize;
        }
        /// <summary>
        /// 备份完成处理
        /// </summary>
        /// <param name="isCompleted">是否备份成功</param>
        /// <returns></returns>
        protected virtual async Task completed(bool isCompleted)
        {
            if (isCompleted)
            {
                Client.OnMessage($"数据库 {Database} 备份完毕");
                ;
                foreach (DirectoryInfo DirectoryInfo in (await AutoCSer.Common.GetDirectories(new DirectoryInfo(SavePath).Parent.notNull(), "*.bak"))
                    .OrderByDescending(p => p.CreationTime).Skip(2))
                {
                    try
                    {
                        await AutoCSer.Common.TryDeleteDirectory(DirectoryInfo);
                    }
                    catch (Exception exception)
                    {
                        await Client.OnError($"备份目录 {SavePath} 删除失败 {exception.Message}");
                    }
                }
            }
            else
            {
                await Client.OnError($"数据库 {Database} 备份失败");
                try
                {
                    await AutoCSer.Common.TryDeleteDirectory(SavePath);
                }
                catch (Exception exception)
                {
                    await Client.OnError($"备份目录 {SavePath} 删除失败 {exception.Message}");
                }
            }
        }
    }
}
