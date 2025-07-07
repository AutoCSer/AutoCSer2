using AutoCSer.Extensions;

namespace AutoCSer.Document.ServiceAuthentication
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            Console.WriteLine($"{nameof(TimestampVerify)} {await TimestampVerify.Server.Test()}");
            Console.WriteLine($"{nameof(CustomVerify)} {await CustomVerify.CustomVerifyService.Test()}");
            Console.WriteLine($"{nameof(SetCommand)} {await SetCommand.CustomVerifyService.Test()}");
            Console.WriteLine($"{nameof(TransferEncoding)} {await TransferEncoding.CommandServerCompressConfig.Test()}");

            Console.WriteLine("Completed");
            Console.ReadKey();
        }
    }
}
