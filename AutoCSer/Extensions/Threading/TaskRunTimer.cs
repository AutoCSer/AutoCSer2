using System;
using System.Threading.Tasks;

namespace AutoCSer.Extensions.Threading
{
    /// <summary>
    /// 定时任务运行时间
    /// </summary>
    public sealed class TaskRunTimer
    {
        /// <summary>
        /// 下一次运行时间
        /// </summary>
        private DateTime RunTime;
        /// <summary>
        /// 运行间隔秒数
        /// </summary>
        private double IntervalSeconds;
        /// <summary>
        /// 定时任务运行时间
        /// </summary>
        /// <param name="IntervalSeconds">运行间隔秒数</param>
        public TaskRunTimer(double IntervalSeconds)
        {
            this.IntervalSeconds = IntervalSeconds;
            RunTime = DateTime.Now;
        }
        /// <summary>
        /// 按天运行的定时任务运行时间
        /// </summary>
        /// <param name="Hour">开始执行小时</param>
        /// <param name="Minute">开始执行分钟</param>
        /// <param name="Second">开始执行秒数</param>
        /// <param name="IntervalSeconds">间隔执行秒数</param>
        public TaskRunTimer(int Hour, int Minute = 0, int Second = 0, double IntervalSeconds = 24 * 60 * 60)
        {
            DateTime Now = DateTime.Now;
            RunTime = new DateTime(Now.Year, Now.Month, Now.Day, Hour, Minute, Second);
            if (IntervalSeconds > 0)
            {
                while (RunTime < Now) RunTime = RunTime.AddSeconds(IntervalSeconds);
            }
            this.IntervalSeconds = IntervalSeconds;
        }
        /// <summary>
        /// 等待运行时间
        /// </summary>
        /// <returns></returns>
        public async Task Delay()
        {
            TimeSpan DelayTime = RunTime - DateTime.Now;
            if (DelayTime.TotalMilliseconds >= 1) await Task.Delay(DelayTime);
            RunTime = RunTime.AddSeconds(IntervalSeconds);
        }
    }
}
