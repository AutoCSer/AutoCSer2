using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.NetCoreWeb
{
    /// <summary>
    /// 数据视图示例，必须使用 partial 修饰符用于生成静态代码，每一个示例必须对应 类型名称.page.html 页面模板文件
    /// </summary>
    [AutoCSer.NetCoreWeb.View(IsStaticVersion = true)]
    public partial class ExampleView : View
    {
        /// <summary>
        /// 生成 LoadView 方法参数成员名称
        /// </summary>
        protected override string queryName { get { return "Parameters"; } }

        /// <summary>
        /// 视图数据成员定义必须采用 public 修饰符
        /// </summary>
        public int Sum;
        /// <summary>
        /// Task 数据成员，模拟异步调用
        /// </summary>
        public Task<int> TaskSum
        {
            get { return Task.FromResult(Sum); }
        }
        /// <summary>
        /// System.Collections.Generic.IEnumerable{T} 成员允许 LOOP 指令当成数组处理
        /// </summary>
        public IEnumerable<int> Left10
        {
            get
            {
                for (int start = Parameters.left, end = start + 10; start != end;) yield return start++;
            }
        }
        /// <summary>
        /// 对象列表
        /// </summary>
        public IEnumerable<ExampleData> DataList
        {
            get
            {
                for (int start = Parameters.left, end = start + 10; start != end;) yield return new ExampleData(start++);
            }
        }
        /// <summary>
        /// 视图数据初始化方法名称必须为 LoadView，返回值类型必须为 Task{AutoCSer.NetCoreWeb.ResponseResult}，参数则根据具体需求而定，如果不需要初始化过程则不需要定义该方法
        /// </summary>
        /// <param name="left">客户端传参</param>
        /// <param name="right">客户端传参</param>
        /// <returns>返回值状态为 Success 则正常执行返回视图数据操作，否则直接将错误信息返回给客户端</returns>
        private Task<AutoCSer.NetCoreWeb.ResponseResult> LoadView(int left, int right)
        {
            Sum = left + right;
            return SuccessResponseResultTask;
        }
    }
    /// <summary>
    /// 数据视图示例数据
    /// </summary>
    public sealed class ExampleData
    {
        /// <summary>
        /// 示例数据
        /// </summary>
        public int IntData;
        /// <summary>
        /// 示例数据
        /// </summary>
        public string StringData;
        /// <summary>
        /// 示例嵌套数据
        /// </summary>
        public ExampleData NextData { get { return new ExampleData(IntData + 1); } }
        /// <summary>
        /// 数据视图示例数据
        /// </summary>
        /// <param name="value"></param>
        public ExampleData(int value)
        {
            IntData = value;
            StringData = value.ToString();
        }
    }
}
