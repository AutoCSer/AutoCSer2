using System;

namespace AutoCSer.TestCase.CommonModel.TableModel.CustomColumn
{
    /// <summary>
    /// 时间范围（建议使用字段来描述持久化数据，同时也作为内部 RPC 服务接口传参）
    /// </summary>
    [AutoCSer.ORM.CustomColumn]
    public struct DateTimeRange : AutoCSer.ORM.IVerify<DateTimeRange>, IEquatable<DateTimeRange>
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        [AutoCSer.FieldEquals.Ignore]
        public DateTime Start;
        /// <summary>
        /// 结束时间（不包含）
        /// </summary>
        [AutoCSer.FieldEquals.Ignore]
        public DateTime End;
        /// <summary>
        /// ==
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(DateTimeRange left, DateTimeRange right) { return left.Equals(right); }
        /// <summary>
        /// !=
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(DateTimeRange left, DateTimeRange right) { return left.Start != right.Start || left.End != right.End; }

        /// <summary>
        /// 自定义数据验证，验证失败需要抛出异常
        /// </summary>
        /// <returns></returns>
        public DateTimeRange Verify()
        {
            if (Start < End) return this;
            throw new InvalidCastException($"开始时间 {Start} 必须大于结束时间 {End}");
        }

        /// <summary>
        /// 检查时间是否在范围之内
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool Check(DateTime time)
        {
            return time >= Start && time < End;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(DateTimeRange other)
        {
            return Start == other.Start && End == other.End;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals((DateTimeRange)obj);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Start.GetHashCode() ^ End.GetHashCode();
        }
    }
}
