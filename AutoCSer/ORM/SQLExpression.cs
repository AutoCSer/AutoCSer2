using System;
using System.Collections.Generic;

namespace AutoCSer.ORM
{
    /// <summary>
    /// SQL 表达式与函数
    /// </summary>
    public static class SQLExpression
    {
        /// <summary>
        /// SQL 函数调用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fucntionName">调用函数名称</param>
        /// <param name="parameters">调用参数</param>
        /// <returns></returns>
#if NetStandard21
        public static T? Call<T>(string fucntionName, params object[] parameters)
#else
        public static T Call<T>(string fucntionName, params object[] parameters)
#endif
        {
            return default(T);
        }

        /// <summary>
        /// COUNT(*) 计数
        /// </summary>
        /// <returns>计数</returns>
        public static int Count()
        {
            return 0;
        }
        /// <summary>
        /// COUNT(value) 计数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns>计数</returns>
        public static int Count<T>(T value)
        {
            return 0;
        }
        /// <summary>
        /// SUM(value) 求和
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns>求和</returns>
        public static T Sum<T>(T value)
        {
            return value;
        }
        /// <summary>
        /// MAX(value) 最大值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns>最大值</returns>
        public static T Max<T>(T value)
        {
            return value;
        }
        /// <summary>
        /// MIN(value) 最小值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns>最小值</returns>
        public static T Min<T>(T value)
        {
            return value;
        }
        /// <summary>
        /// DISTINCT(value) 去重
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Distinct<T>(T value)
        {
            return value;
        }
        /// <summary>
        /// GETDATE() 获取当前时间
        /// </summary>
        /// <returns>当前时间</returns>
        public static DateTime GetDate()
        {
            return AutoCSer.Threading.SecondTimer.Now;
        }
        /// <summary>
        /// SYSDATETIME() 获取当前高精度时间
        /// </summary>
        /// <returns>当前时间</returns>
        public static DateTime SysDateTime()
        {
            return AutoCSer.Threading.SecondTimer.Now;
        }
        /// <summary>
        /// value IN (values)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">data</param>
        /// <param name="values">数值集合</param>
        /// <returns>是否包含数据</returns>
        public static bool In<T>(T value, params T[] values)
        {
            return false;
        }
        /// <summary>
        /// value IN (values)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">data</param>
        /// <param name="values">数值集合</param>
        /// <returns>是否包含数据</returns>
        public static bool In<T>(T? value, params T[] values) where T : struct
        {
            return false;
        }
        /// <summary>
        /// value IN (values)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">data</param>
        /// <param name="values">数值集合</param>
        /// <returns>是否包含数据</returns>
        public static bool In<T>(T value, IEnumerable<T> values)
        {
            return false;
        }
        /// <summary>
        /// value IN (values)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">data</param>
        /// <param name="values">数值集合</param>
        /// <returns>是否包含数据</returns>
        public static bool In<T>(T? value, IEnumerable<T> values) where T : struct
        {
            return false;
        }
        /// <summary>
        /// value IN (query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">data</param>
        /// <param name="query">子查询</param>
        /// <returns>是否包含数据</returns>
        public static bool In<T>(T value, IQueryBuilder query)
        {
            return false;
        }
        /// <summary>
        /// value NOT IN (values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">data</param>
        /// <param name="values">数值集合</param>
        /// <returns>是否不包含数据</returns>
        public static bool NotIn<T>(T value, params T[] values)
        {
            return false;
        }
        /// <summary>
        /// value NOT IN (values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">data</param>
        /// <param name="values">数值集合</param>
        /// <returns>是否不包含数据</returns>
        public static bool NotIn<T>(T? value, params T[] values) where T : struct
        {
            return false;
        }
        /// <summary>
        /// value NOT IN (values)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">data</param>
        /// <param name="values">数值集合</param>
        /// <returns>是否不包含数据</returns>
        public static bool NotIn<T>(T value, IEnumerable<T> values)
        {
            return false;
        }
        /// <summary>
        /// value NOT IN (values)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">data</param>
        /// <param name="values">数值集合</param>
        /// <returns>是否不包含数据</returns>
        public static bool NotIn<T>(T? value, IEnumerable<T> values) where T : struct
        {
            return false;
        }
        /// <summary>
        /// value NOT IN (query)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">data</param>
        /// <param name="query">子查询</param>
        /// <returns>是否不包含数据</returns>
        public static bool NotIn<T>(T value, IQueryBuilder query)
        {
            return false;
        }
        /// <summary>
        /// LEN(value) 获取字符串长度
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int Len(string value)
        {
            return -1;
        }
        /// <summary>
        /// value LIKE %matchValue%
        /// </summary>
        /// <param name="value"></param>
        /// <param name="matchValue"></param>
        /// <returns></returns>
        public static bool Like(string value, string matchValue)
        {
            return false;
        }
        /// <summary>
        /// value NOT LIKE %matchValue%
        /// </summary>
        /// <param name="value"></param>
        /// <param name="matchValue"></param>
        /// <returns></returns>
        public static bool NotLike(string value, string matchValue)
        {
            return false;
        }
        /// <summary>
        /// value LIKE matchValue%
        /// </summary>
        /// <param name="value"></param>
        /// <param name="matchValue"></param>
        /// <returns></returns>
        public static bool StartsWith(string value, string matchValue)
        {
            return false;
        }
        /// <summary>
        /// value NOT LIKE matchValue%
        /// </summary>
        /// <param name="value"></param>
        /// <param name="matchValue"></param>
        /// <returns></returns>
        public static bool NotStartsWith(string value, string matchValue)
        {
            return false;
        }
        /// <summary>
        /// value LIKE %matchValue
        /// </summary>
        /// <param name="value"></param>
        /// <param name="matchValue"></param>
        /// <returns></returns>
        public static bool EndsWith(string value, string matchValue)
        {
            return false;
        }
        /// <summary>
        /// value NOT LIKE %matchValue
        /// </summary>
        /// <param name="value"></param>
        /// <param name="matchValue"></param>
        /// <returns></returns>
        public static bool NotEndsWith(string value, string matchValue)
        {
            return false;
        }
        /// <summary>
        /// value LIKE %matchValue% OR value LIKE %matchValue% ...
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="matchValueArray">匹配字符串集合</param>
        /// <returns>是否包含数值</returns>
        public static bool LikeOr(string value, string[] matchValueArray)
        {
            return false;
        }
        /// <summary>
        /// CONTAINS(value,matchValue) 全文索引
        /// </summary>
        /// <param name="value"></param>
        /// <param name="matchValue"></param>
        /// <returns></returns>
        public static bool Contains(string value, string matchValue)
        {
            return false;
        }
        /// <summary>
        /// NOT CONTAINS(value,matchValue) 全文索引
        /// </summary>
        /// <param name="value"></param>
        /// <param name="matchValue"></param>
        /// <returns></returns>
        public static bool NotContains(string value, string matchValue)
        {
            return false;
        }
        /// <summary>
        /// REPLACE(value,oldValue,newValue) 替换字串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static string Replace(string value, string oldValue, string newValue)
        {
            return value.Replace(oldValue, newValue);
        }
        /// <summary>
        /// ISNULL(value,nullValue) 空值判断
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="nullValue"></param>
        /// <returns></returns>
        public static T IsNull<T>(T value, T nullValue)
        {
            return value;
        }
        /// <summary>
        /// EXISTS(query) 存在子查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static bool Exists(IQueryBuilder query)
        {
            return false;
        }
        /// <summary>
        /// NOT EXISTS(query) 不存在子查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static bool NotExists(IQueryBuilder query)
        {
            return false;
        }
        /// <summary>
        /// DATEDIFF(dateDiffType,startTime,endTime) 计算时间差
        /// </summary>
        /// <param name="dateDiffType"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static int DateDiff(ExpressionCallDateDiffType dateDiffType, DateTime startTime, DateTime endTime)
        {
            return 0;
        }
        /// <summary>
        /// DATEDIFF(dateDiffType,startTime,endTime) 计算时间差
        /// </summary>
        /// <param name="dateDiffType"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static int DateDiff(ExpressionCallDateDiffType dateDiffType, DateTime? startTime, DateTime endTime)
        {
            return 0;
        }
        /// <summary>
        /// DATALENGTH(value) 计算长度
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int DataLength(byte[] value)
        {
            return 0;
        }
        /// <summary>
        /// CASE WHEN condition THEN trueValue ELSE falseValue END
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition"></param>
        /// <param name="trueValue"></param>
        /// <param name="falseValue"></param>
        /// <returns></returns>
        public static T Case<T>(bool condition, T trueValue, T falseValue)
        {
            return condition ? trueValue : falseValue;
        }
        /// <summary>
        /// 比较操作，比如字符串比较，仅支持 大于 GreaterThan、大于等于 GreaterThanOrEqual、小于 LessThan、小于等于 LessThanOrEqual
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="expressionType"></param>
        /// <returns></returns>
        public static bool CompareTo<T>(T left, T right, System.Linq.Expressions.ExpressionType expressionType)
        {
            return false;
        }
        /// <summary>
        /// 比较操作，比如字符串比较，仅支持 等于 Equal、不等于 NotEqual、大于 GreaterThan、大于等于 GreaterThanOrEqual、小于 LessThan、小于等于 LessThanOrEqual
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="expressionType"></param>
        /// <returns></returns>
        public static bool CompareTo<T>(T left, IQueryBuilder right, System.Linq.Expressions.ExpressionType expressionType)
        {
            return false;
        }
    }
}
