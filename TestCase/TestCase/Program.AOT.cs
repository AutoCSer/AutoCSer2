using AutoCSer;
using AutoCSer.Extensions;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace AutoCSer.TestCase
{
    class Program
    {
        static async Task Main(string[] args)
        {
            AutoCSer.FieldEquals.Comparor.IsBreakpoint = true;
            await AutoCSer.Threading.SwitchAwaiter.Default;
            Type errorType = typeof(Program);
            do
            {
                Task<bool> reusableDictionaryTask = ThreadPool.TinyBackground.RunTask(ReusableDictionary.TestCase);
                Task<bool> searchTreeTask = ThreadPool.TinyBackground.RunTask(SearchTree.TestCase);
                Task<bool> binarySerializeTask = ThreadPool.TinyBackground.RunTask(BinarySerialize.TestCase);
                Task<bool> jsonTask = ThreadPool.TinyBackground.RunTask(Json.TestCase);
                //Task<bool> xmlTask = ThreadPool.TinyBackground.RunTask(Xml.TestCase);
                if (!await binarySerializeTask) { errorType = typeof(BinarySerialize); break; }
                if (!await jsonTask) { errorType = typeof(Json); break; }
                //if (!await xmlTask) { errorType = typeof(Xml); break; }
                if (!await searchTreeTask) { errorType = typeof(SearchTree); break; }
                if (!await reusableDictionaryTask) { errorType = typeof(ReusableDictionary); break; }
                Console.Write('.');
            }
            while (true);
            ConsoleWriteQueue.WriteLine(errorType.FullName + " ERROR", ConsoleColor.Red);

        }
    }
}
