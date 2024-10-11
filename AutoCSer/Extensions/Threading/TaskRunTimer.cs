using System;
using System.Threading.Tasks;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 定时任务运行时间
    /// </summary>
    public sealed class TaskRunTimer
    {
        /// <summary>
        /// 下一次运行时间
        /// </summary>
        private DateTime runTime;
        /// <summary>
        /// 运行间隔秒数
        /// </summary>
        private double intervalSeconds;
        /// <summary>
        /// 定时任务运行时间
        /// </summary>
        /// <param name="intervalSeconds">运行间隔秒数</param>
        public TaskRunTimer(double intervalSeconds)
        {
            this.intervalSeconds = intervalSeconds;
            runTime = DateTime.Now;
        }
        /// <summary>
        /// 按天运行的定时任务运行时间
        /// </summary>
        /// <param name="hour">开始执行小时</param>
        /// <param name="minute">开始执行分钟</param>
        /// <param name="second">开始执行秒数</param>
        /// <param name="intervalSeconds">间隔执行秒数</param>
        public TaskRunTimer(int hour, int minute = 0, int second = 0, double intervalSeconds = 24 * 60 * 60)
        {
            DateTime now = DateTime.Now;
            runTime = new DateTime(now.Year, now.Month, now.Day, hour, minute, second);
            if (intervalSeconds > 0)
            {
                while (runTime < now) runTime = runTime.AddSeconds(intervalSeconds);
            }
            this.intervalSeconds = intervalSeconds;
        }
        /// <summary>
        /// 等待运行时间
        /// </summary>
        /// <returns></returns>
        public async Task Delay()
        {
            TimeSpan delayTime = runTime - DateTime.Now;
            if (delayTime.TotalMilliseconds >= 1) await Task.Delay(delayTime);
            runTime = runTime.AddSeconds(intervalSeconds);
        }
    }
}
