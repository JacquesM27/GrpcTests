using System.Collections.Concurrent;
using Grpc.Core;

namespace GbsServer.Services;

public class ChatService : ChatBidService.ChatBidServiceBase
{
    private static ConcurrentDictionary<string, IServerStreamWriter<ChatMessage>> _clients = new();
    public override async Task Chat(IAsyncStreamReader<ChatMessage> requestStream, IServerStreamWriter<ChatMessage> responseStream, ServerCallContext context)
    {
        while (await requestStream.MoveNext())
        {
            var message = requestStream.Current;
            _clients.TryAdd(message.UserId, responseStream);
            
            foreach (var client in _clients)
            {
                if (client.Value != responseStream)
                {
                    await client.Value.WriteAsync(new ChatMessage()
                    {
                        UserId = message.UserId,
                        Message = message.Message
                    });
                }
            }
        }
    }
}