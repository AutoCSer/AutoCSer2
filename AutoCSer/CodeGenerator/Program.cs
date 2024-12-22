using AutoCSer.Extensions;
using AutoCSer.Threading;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CodeGenerator
{
    class Program 
    {
        static void Main(string[] args)
        {
            AutoCSer.Common.IsCodeGenerator = true;
            new UISynchronousTask(() => ProjectParameter.Start(args)).Wait();
        }
    }
}
