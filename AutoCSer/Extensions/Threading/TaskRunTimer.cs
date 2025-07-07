using System;
using System.Threading.Tasks;

namespace AutoCSer.Threading
{
    /// <summary>
    /// Timed task running time
    /// 定时任务执行时间
    /// </summary>
    public sealed class TaskRunTimer
    {
        /// <summary>
        /// The next running time
        /// 下一次执行时间
        /// </summary>
        private DateTime runTime;
        /// <summary>
        /// The number of seconds of execution interval
        /// 间隔执行秒数
        /// </summary>
        private double intervalSeconds;
        /// <summary>
        /// Timed task running time
        /// 定时任务执行时间
        /// </summary>
        /// <param name="intervalSeconds">The number of seconds of execution interval
        /// 间隔执行秒数</param>
        public TaskRunTimer(double intervalSeconds)
        {
            this.intervalSeconds = intervalSeconds;
            runTime = DateTime.Now;
        }
        /// <summary>
        /// Daily operating time
        /// 按天运行的定时任务运行时间
        /// </summary>
        /// <param name="hour">Start execution hours
        /// 开始执行小时</param>
        /// <param name="minute">Start execution minutes
        /// 开始执行分钟数</param>
        /// <param name="second">Start execution seconds
        /// 开始执行秒数</param>
        /// <param name="intervalSeconds">The number of seconds of execution interval
        /// 间隔执行秒数</param>
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
        /// Waiting time for execution
        /// 等待执行时间
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
