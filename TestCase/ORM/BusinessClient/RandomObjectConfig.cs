using AutoCSer.ORM;
using AutoCSer.TestCase.CommonModel.TableModel.CustomColumn;
using System;

namespace AutoCSer.TestCase.BusinessClient
{
    /// <summary>
    /// 随机对象生成配置
    /// </summary>
    internal class RandomObjectConfig : AutoCSer.RandomObject.Config
    {
        /// <summary>
        /// 获取自定义生成委托
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override Delegate GetCustomCreator(Type type)
        {
            if (type == typeof(Email)) return (Func<AutoCSer.RandomObject.Config, bool, Email>)createEmail;
            if (type == typeof(DateTimeRange)) return (Func<AutoCSer.RandomObject.Config, bool, DateTimeRange>)createDateTimeRange;
            return null;
        }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        /// <param name="config"></param>
        /// <param name="isNullable"></param>
        /// <returns></returns>
        private static Email createEmail(AutoCSer.RandomObject.Config config, bool isNullable)
        {
            return config.CreateString('a', 'z') + "@AutoCSer.com";
        }
        /// <summary>
        /// 时间范围
        /// </summary>
        /// <param name="config"></param>
        /// <param name="isNullable"></param>
        /// <returns></returns>
        private static DateTimeRange createDateTimeRange(AutoCSer.RandomObject.Config config, bool isNullable)
        {
            DateTime start = config.CreateDateTime();
            do
            {
                DateTime end = config.CreateDateTime();
                if (start != end)
                {
                    if (start < end) return new DateTimeRange { Start = start, End = end };
                    return new DateTimeRange { Start = end, End = start };
                }
            }
            while (true);
        }
    }
}
