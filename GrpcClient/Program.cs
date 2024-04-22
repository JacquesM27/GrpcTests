// See https://aka.ms/new-console-template for more information

using Grpc.Net.Client;
using GrpcClient;

Console.WriteLine("Hello, World!");
Console.WriteLine("Press any key to start");
Console.ReadKey();

using var channel = GrpcChannel.ForAddress("https://localhost:7254");
var client = new Greeter.GreeterClient(channel);
var reply = await client.SayHelloAsync(new HelloRequest(){ Name = "John Doe"});

Console.WriteLine("Greeting: " + reply.Message);
Console.WriteLine("Press any key to exit...");
Console.ReadKey();