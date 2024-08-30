using System;

namespace AutoCSer.TestCase.NetCoreWeb
{
    /// <summary>
    /// 
    /// </summary>
    public class Program 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            AutoCSer.NetCoreWeb.Startup<ViewMiddleware>.CreateHostBuilder(args);
        }
    }
}
