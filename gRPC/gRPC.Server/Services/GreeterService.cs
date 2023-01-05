using System.Text;
using Grpc.Core;

namespace gRPC.Server.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append($"Hello {request.First}");
        if (request.HasLast)
        {
            sb.Append($" {request.Last}");
        }
        
        return Task.FromResult(new HelloReply
        {
            Message = sb.ToString()
        });
    }
}
