using Grpc.Core;
using GrpcServicePerformance;

namespace GrpcServicePerformance.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<AddReply> Add(AddRequest request, ServerCallContext context)
        {
            return Task.FromResult(new AddReply
            {
                Result = request.Left + request.Right
            });
        }
    }
}
