using Grpc.Core;
using GrpcService;

namespace GrpcService.Services;

public class GreeterService(ILogger<GreeterService> logger) : Greeter.GreeterBase
{
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        Console.WriteLine("Request author: " + request.Name);
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        });
    }
}