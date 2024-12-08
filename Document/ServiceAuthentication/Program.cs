using AutoCSer.Extensions;
using AutoCSer.Net;

namespace AutoCSer.Document.ServiceAuthentication
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine($"{nameof(TimestampVerify)} {await TimestampVerify.CommandClientSocketEvent.Test()}");
            Console.WriteLine($"{nameof(CustomVerify)} {await CustomVerify.CustomVerifyService.Test()}");
            Console.WriteLine($"{nameof(SetCommand)} {await SetCommand.CustomVerifyService.Test()}");
            Console.WriteLine($"{nameof(TransferEncoding)} {await TransferEncoding.CommandServerCompressConfig.Test()}");

            Console.WriteLine("Press quit to exit.");
            while (Console.ReadLine() != "quit") ;
        }
    }
}
