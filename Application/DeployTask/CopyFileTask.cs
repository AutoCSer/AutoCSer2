using AutoCSer.Extensions;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.DeployTask
{
    /// <summary>
    /// 复制文件任务
    /// </summary>
    internal sealed class CopyFileTask : IDeployTask
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        DeployTaskOperationTypeEnum IDeployTask.OperationType { get { return DeployTaskOperationTypeEnum.CopyFile; } }
        /// <summary>
        /// 发布任务服务端
        /// </summary>
        private readonly DeployTaskService service;
        /// <summary>
        /// 文件源路径
        /// </summary>
        private readonly string sourcePath;
        /// <summary>
        /// 文件目标路径
        /// </summary>
        private readonly string destinationPath;
        /// <summary>
        /// 切换检测文件名称，null 表示不检测
        /// </summary>
        private readonly string checkSwitchFileName;
        /// <summary>
        /// 是否备份历史文件
        /// </summary>
        private readonly bool isBackup;
        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="service">发布任务服务端</param>
        /// <param name="taskIdentity">任务ID</param>
        /// <param name="index">当前任务临时文件目录序号</param>
        /// <param name="destinationPath">文件目标路径</param>
        /// <param name="checkSwitchFileName">切换检测文件名称，null 表示不检测</param>
        /// <param name="isBackup">是否备份历史文件</param>
        internal CopyFileTask(DeployTaskService service, long taskIdentity,  int index, string destinationPath, string checkSwitchFileName, bool isBackup)
        {
            this.service = service;
            this.sourcePath = Path.Combine(service.UploadFilePath, taskIdentity.toString(), index.toString());
            this.destinationPath = destinationPath;
            this.checkSwitchFileName = checkSwitchFileName;
            this.isBackup = isBackup;
        }
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <returns></returns>
        public async Task<DeployTaskLog> Run()
        {
            var destinationDirectory = default(DirectoryInfo);
            var backupDirectory = default(DirectoryInfo);
            if (!string.IsNullOrEmpty(checkSwitchFileName))
            {
                FileInfo checkFile = new FileInfo(Path.Combine(destinationPath, checkSwitchFileName));
                if (await AutoCSer.Common.FileExists(checkFile))
                {
                    FileInfo switchFile = new FileInfo(Path.Combine(destinationPath, service.SwitchDirectoryName, checkSwitchFileName));
                    if (!await AutoCSer.Common.FileExists(switchFile) || switchFile.LastWriteTimeUtc < checkFile.LastWriteTimeUtc)
                    {
                        destinationDirectory = switchFile.Directory;
                    }
                }
            }
            if (destinationDirectory == null) await AutoCSer.Common.TryCreateDirectory(destinationDirectory = new DirectoryInfo(destinationPath));
            if (isBackup)
            {
                backupDirectory = new DirectoryInfo(Path.Combine(destinationPath, service.BackupDirectoryName, AutoCSer.Threading.SecondTimer.Now.ToString("yyyyMMddHHmmss")));
                await AutoCSer.Common.TryCreateDirectory(backupDirectory);
            }
            await copy(new DirectoryInfo(sourcePath), destinationDirectory, backupDirectory);
            return new DeployTaskLog { OperationState = DeployTaskOperationStateEnum.Completed };
        }
        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="sourceDirectory"></param>
        /// <param name="destinationDirectory"></param>
        /// <param name="backupDirectory"></param>
        /// <returns></returns>
#if NetStandard21
        private async Task copy(DirectoryInfo sourceDirectory, DirectoryInfo destinationDirectory, DirectoryInfo? backupDirectory)
#else
        private async Task copy(DirectoryInfo sourceDirectory, DirectoryInfo destinationDirectory, DirectoryInfo backupDirectory)
#endif
        {
            string destinationPath = destinationDirectory.FullName;
            var backupPath = backupDirectory?.FullName;
            foreach (DirectoryInfo nextSourceDirectory in await AutoCSer.Common.GetDirectories(sourceDirectory))
            {
                DirectoryInfo nextDestinationDirectory = new DirectoryInfo(Path.Combine(destinationPath, nextSourceDirectory.Name));
                var nextBackupDirectory = default(DirectoryInfo);
                await AutoCSer.Common.TryCreateDirectory(nextDestinationDirectory);
                if (backupPath != null) await AutoCSer.Common.TryCreateDirectory(nextBackupDirectory = new DirectoryInfo(Path.Combine(backupPath, nextSourceDirectory.Name)));
                await copy(nextSourceDirectory, nextDestinationDirectory, nextBackupDirectory);
            }
            foreach (FileInfo file in await AutoCSer.Common.DirectoryGetFiles(sourceDirectory))
            {
                string destinationFileName = Path.Combine(destinationPath, file.Name);
                if (await AutoCSer.Common.FileExists(destinationFileName))
                {
                    if (backupPath != null) await AutoCSer.Common.FileMove(destinationFileName, Path.Combine(backupPath, file.Name));
                    else await AutoCSer.Common.TryDeleteFile(destinationFileName);
                }
                await AutoCSer.Common.FileMove(file, destinationFileName);
            }
        }

        /// <summary>
        /// 取消任务
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task Cancel() 
        {
            return AutoCSer.Common.TryDeleteDirectory(sourcePath, true);
        }
    }
}
