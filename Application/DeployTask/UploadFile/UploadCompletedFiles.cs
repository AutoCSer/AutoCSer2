using System;
using System.IO;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.DeployTask
{
    /// <summary>
    /// 文件上传完成文件集合
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    internal sealed class UploadCompletedFiles
    {
        /// <summary>
        /// 上传完成文件集合
        /// </summary>
        internal readonly UploadCompletedFileName[] Files;
        /// <summary>
        /// 等待删除的文件集合
        /// </summary>
        internal readonly UploadCompletedFileName[] DeleteFiles;
        /// <summary>
        /// 等待删除的目录集合
        /// </summary>
        internal readonly UploadCompletedFileName[] DeleteDirectorys;
        /// <summary>
        /// 文件上传完成文件集合
        /// </summary>
        private UploadCompletedFiles()
        {
#if NetStandard21
            Files = DeleteFiles = DeleteDirectorys = EmptyArray<UploadCompletedFileName>.Array;
#endif
        }
        /// <summary>
        /// 文件上传完成文件集合
        /// </summary>
        /// <param name="fileUploader"></param>
        internal UploadCompletedFiles(FileUploader fileUploader)
        {
            Files = fileUploader.UploadCompletedFiles.GetArray(p => new UploadCompletedFileName(p));
            DeleteFiles = fileUploader.DeleteFiles.GetArray(p => new UploadCompletedFileName(p));
            DeleteDirectorys = fileUploader.DeleteDirectorys.GetArray(p => new UploadCompletedFileName(p));
        }
        /// <summary>
        /// 上传完成最后移动文件操作
        /// </summary>
        /// <returns></returns>
        private async Task completed()
        {
            foreach (UploadCompletedFileName file in Files)
            {
                if (await AutoCSer.Common.FileExists(file.FileName))
                {
                    string movBackupFileName = file.BackupFileName + AutoCSer.Threading.SecondTimer.Now.ToString(".yyyyMMddHHmmss") + ".bak";
                    await AutoCSer.Common.FileMove(file.FileName, movBackupFileName);
                    await AutoCSer.Common.FileMove(file.BackupFileName, file.FileName);
                    await AutoCSer.Common.FileMove(movBackupFileName, file.BackupFileName);
                }
                else await AutoCSer.Common.FileMove(file.BackupFileName, file.FileName);
            }
            foreach (UploadCompletedFileName file in DeleteFiles) await AutoCSer.Common.FileMove(file.FileName, file.BackupFileName);
            foreach (UploadCompletedFileName directory in DeleteDirectorys) await AutoCSer.Common.DirectoryMove(directory.FileName, directory.BackupFileName);
        }

        /// <summary>
        /// 上传文件最后移动文件操作
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        internal static async Task<OperationStateEnum> Completed(string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            if(!await AutoCSer.Common.FileExists(fileInfo)) return OperationStateEnum.NotFoundFile;
            byte[] fileData = await AutoCSer.Common.ReadFileAllBytes(fileName);
            SubArray<byte> compressData = new SubArray<byte>(fileData, sizeof(int), fileData.Length - sizeof(int));
            SubArray<byte> data = new SubArray<byte>(AutoCSer.Common.GetUninitializedArray<byte>(BitConverter.ToInt32(fileData, 0)));
            if (!AutoCSer.Common.Config.Decompress(ref compressData, ref data)) return OperationStateEnum.DecompressFailed;
            var files = AutoCSer.BinaryDeserializer.Deserialize<UploadCompletedFiles>(data.Array);
            if (files == null) return OperationStateEnum.DeserializeFailed;
            await files.completed();
            await AutoCSer.Common.DeleteFile(fileInfo);
            return OperationStateEnum.Success;
        }
    }
}
