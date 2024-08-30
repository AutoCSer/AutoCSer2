using AutoCSer.Extensions;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace AutoCSer.Example.Search
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Size size = AutoCSer.Drawing.Gif.CopyScreen.CheckSize();
                FileInfo file = new FileInfo(Path.Combine(AutoCSer.Common.ApplicationDirectory.FullName, AutoCSer.IO.File.BakPrefix + ((ulong)AutoCSer.Threading.SecondTimer.Now.Ticks).toHex() + ".gif"));
                using (FileStream fileStream = new FileStream(file.FullName, FileMode.CreateNew, FileAccess.Write, FileShare.None, 1, FileOptions.WriteThrough))
                using (AutoCSer.Drawing.Gif.CopyScreen gif = new AutoCSer.Drawing.Gif.CopyScreen(fileStream, (short)size.Width, (short)size.Height))
                {
                    Console.WriteLine("Copy screen start...");
                    Console.WriteLine("Press quit to exit.");
                    while (Console.ReadLine() != "quit") ;
                }
                //Process.Start(file.FullName);
                Console.WriteLine(file.FullName);
                Console.ReadKey();
            }
            catch(Exception exception)
            {
                ConsoleWriteQueue.Breakpoint(exception.ToString());
            }
        }
    }
}
