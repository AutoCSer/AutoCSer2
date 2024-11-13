using AutoCSer.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 文件块服务配置
    /// </summary>
    public class FileBlockServiceConfig : DiskBlockServiceConfig
    {
        /// <summary>
        /// 文件扩展名 AutoCSer File Block
        /// </summary>
        public const string ExtensionName = ".afb";

        /// <summary>
        /// 文件存储路径
        /// </summary>
#if NetStandard21
        public string? Path;
#else
        public string Path;
#endif
        /// <summary>
        /// 写入文件流缓存区字节数，默认为 1MB，最小值为 4KB
        /// </summary>
        public int WriteBufferSize = 1 << 20;
        /// <summary>
        /// 读取文件缓存区字节数，默认为 4KB，最小值为 4B
        /// </summary>
        public int ReadBufferSize = 4 << 10;

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private unsafe static KeyValue<FileInfo, long> getFileStartIndex(FileInfo file)
        {
            string name = file.Name;
            if (name.Length == 28)
            {
                ulong index;
                fixed (char* nameFixed = name)
                {
                    if (AutoCSer.Extensions.NumberExtension.FromHex(nameFixed + 8, out index)) return new KeyValue<FileInfo, long>(file, (long)index);
                }
            }
            return default(KeyValue<FileInfo, long>);
        }
        /// <summary>
        /// 创建磁盘块服务
        /// </summary>
        /// <returns></returns>
        public async Task<DiskBlockService> CreateFileBlockService()
        {
            string identityHex = Identity.toHex();
            DirectoryInfo directory = string.IsNullOrEmpty(Path) ? AutoCSer.Common.ApplicationDirectory : new DirectoryInfo(Path);
            await AutoCSer.Common.Config.TryCreateDirectory(directory);
            LeftArray<KeyValue<FileInfo, long>> files = (await AutoCSer.Common.Config.DirectoryGetFiles(directory, identityHex + "*" + ExtensionName))
                .Select(p => getFileStartIndex(p))
                .Where(p => p.Key != null)
                .OrderByDescending(p => p.Value)
                .getLeftArray();
            if (files.Length == 0) files.Add(new KeyValue<FileInfo, long>(new FileInfo(System.IO.Path.Combine(directory.FullName, identityHex + "0000000000000000" + ExtensionName)), 0));

            bool isServive = false;
            var block = default(Block);
            var writeStream = default(FileStream);
            DiskBlockService service = new DiskBlockService(this, files.Length);
            int readBufferSize = Math.Max(ReadBufferSize, sizeof(int)), writeBufferSize = Math.Max(WriteBufferSize, 4 << 10);
            try
            {
                foreach (KeyValue<FileInfo, long> file in files)
                {
                    if (object.ReferenceEquals(service.Block, NullBlock.Null))
                    {
                        writeStream = await AutoCSer.Common.Config.CreateFileStream(file.Key.FullName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read, writeBufferSize);
                        await AutoCSer.Common.Config.Seek(writeStream, 0, SeekOrigin.End);
                        block = new FileBlock(service, file.Key, file.Value, file.Value + writeStream.Position, readBufferSize, writeBufferSize, writeStream);
                        writeStream = null;
                        service.Set(block);
                        block = null;
                    }
                    else service.Append(new FileBlock(service, file.Key, file.Value, file.Value + file.Key.Length, readBufferSize));
                }
                isServive = true;
            }
            finally
            {
                if (writeStream != null) await writeStream.DisposeAsync();
                if (block != null) await block.DisposeAsync();
                if (!isServive) await service.DisposeAsync();
            }
            return service;
        }
    }
}
