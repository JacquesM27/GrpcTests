// See https://aka.ms/new-console-template for more information

using GbsServer;
using Grpc.Core;
using Grpc.Net.Client;

Console.WriteLine("Hello, in chat! To exit type 'exit'.");

var channel = GrpcChannel.ForAddress("https://localhost:7114");
var client = new ChatBidService.ChatBidServiceClient(channel);

var call = client.Chat();
var responseTask = Task.Run(async () =>
{
    await foreach (var message in call.ResponseStream.ReadAllAsync())
    {
        Console.WriteLine($"{message.UserId}: {message.Message}");
    }
});

Console.WriteLine("Enter your name:");
string userId = Console.ReadLine() ?? throw new InvalidOperationException();

try
{
    await call.RequestStream.WriteAsync(new ChatMessage()
    {
        UserId = userId,
        Message = "has joined the chat"
    });

    while (true)
    {
        var message = Console.ReadLine() ?? throw new InvalidOperationException();
        Console.WriteLine($"You: {message}");
        if (message.Equals("exit"))
        {
            await call.RequestStream.WriteAsync(new ChatMessage()
            {
                UserId = userId,
                Message = "has left the chat"
            });
            break;
        }

        await call.RequestStream.WriteAsync(new ChatMessage()
        {
            UserId = userId,
            Message = message
        });
    }

}
catch (RpcException exception)
{
    Console.WriteLine($"RPC failed: {exception}");
}
finally
{
    await channel.ShutdownAsync();
}


