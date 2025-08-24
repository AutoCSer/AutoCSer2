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
            try
            {
                Type errorType = typeof(Program);
                do
                {
                    Task<bool> defaultControllerTask = CommandClientDefaultController.TestCase();
                    Task<bool> shortLinkCommandServerTask = ShortLinkCommandServer.TestCase();
                    Task<bool> reusableDictionaryTask = ThreadPool.TinyBackground.RunTask(ReusableDictionary.TestCase);
                    Task<bool> sortTreeTask = ThreadPool.TinyBackground.RunTask(Sort.TestCase);
                    Task<bool> searchTreeTask = ThreadPool.TinyBackground.RunTask(SearchTree.TestCase);
                    Task<bool> binarySerializeTask = BinarySerialize.TestCase();
                    Task<bool> jsonTask = ThreadPool.TinyBackground.RunTask(Json.TestCase);
                    Task<bool> xmlTask = ThreadPool.TinyBackground.RunTask(Xml.TestCase);
                    if (!await shortLinkCommandServerTask) { errorType = typeof(ShortLinkCommandServer); break; }
                    Task<bool> commandServerTask = CommandServer.TestCase();
                    if (!await binarySerializeTask) { errorType = typeof(BinarySerialize); break; }
                    if (!await jsonTask) { errorType = typeof(Json); break; }
                    if (!await xmlTask) { errorType = typeof(Xml); break; }
                    if (!await searchTreeTask) { errorType = typeof(SearchTree); break; }
                    if (!await sortTreeTask) { errorType = typeof(Sort); break; }
                    if (!await reusableDictionaryTask) { errorType = typeof(ReusableDictionary); break; }
                    if (!await defaultControllerTask) { errorType = typeof(CommandClientDefaultController); break; }
                    if (!await commandServerTask) { errorType = typeof(CommandServer); break; }
                    Console.Write('.');
                }
                while (true);
                ConsoleWriteQueue.WriteLine(errorType.FullName + " ERROR", ConsoleColor.Red);
            }
            catch (Exception exception) 
            { 
                Console.WriteLine(exception.ToString());
            }
            Console.ReadKey();

            if (AutoCSer.TestCase.AotMethod.Call())
            {
                AutoCSer.Extensions.AotMethod.Call();
            }
        }
    }
}
