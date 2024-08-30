using AutoCSer.Threading;
using System;

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
