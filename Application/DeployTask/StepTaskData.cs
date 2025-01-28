using AutoCSer.Extensions;
using System;

namespace AutoCSer.CommandService.DeployTask
{
    /// <summary>
    /// 子任务信息
    /// </summary>
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false, CustomReferenceTypes = new Type[0])]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct StepTaskData : AutoCSer.BinarySerialize.ICustomSerialize<StepTaskData>
    {
        /// <summary>
        /// 任务类型
        /// </summary>
        private StepTypeEnum type;
        /// <summary>
        /// 任务类型
        /// </summary>
        public StepTypeEnum Type { get { return type; } }
        /// <summary>
        /// 子任务
        /// </summary>
        private IStepTask task;
        /// <summary>
        /// 子任务
        /// </summary>
        public IStepTask Task { get { return task; } }
        /// <summary>
        /// 子任务信息
        /// </summary>
        /// <param name="type">任务类型</param>
        /// <param name="task">子任务</param>
        internal StepTaskData(StepTypeEnum type, IStepTask task)
        {
            this.type = type;
            this.task = task;
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        unsafe void AutoCSer.BinarySerialize.ICustomSerialize<StepTaskData>.Serialize(AutoCSer.BinarySerializer serializer)
        {
            if (serializer.Stream.Write((int)(ushort)type))
            {
                switch (type)
                {
                    case StepTypeEnum.StartProcess: serializer.CustomSerialize((StartProcessTask)task); break;
                    case StepTypeEnum.UploadCompleted: serializer.CustomSerialize((UploadCompletedTask)task); break;
                }
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        unsafe void AutoCSer.BinarySerialize.ICustomSerialize<StepTaskData>.Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            var type = default(int);
            if (deserializer.Read(out type))
            {
                switch (this.type = (StepTypeEnum)(ushort)type)
                {
                    case StepTypeEnum.StartProcess:
                        var startProcessTask = default(StartProcessTask);
                        if (deserializer.CustomDeserialize(ref startProcessTask)) task = startProcessTask.notNull();
                        break;
                    case StepTypeEnum.UploadCompleted:
                        var uploadCompletedTask = default(UploadCompletedTask);
                        if (deserializer.CustomDeserialize(ref uploadCompletedTask)) task = uploadCompletedTask.notNull();
                        break;
                }
            }
        }
    }
}
