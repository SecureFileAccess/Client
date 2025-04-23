using Broker;
using Client;
using Grpc.Core;
using Grpc.Net.Client;

public class App
{
    private readonly BrokerProto.BrokerProtoClient _client;
    private readonly FileOperationHandler _dispatcher;

    public App(BrokerProto.BrokerProtoClient client, FileOperationHandler dispatcher)
    {
        _client = client;
        _dispatcher = dispatcher;
    }

    public async Task RunAsync()
    {
        var clientId = $"client-{Guid.NewGuid().ToString().Substring(0, 8)}";

        using var call = _client.ConnectClient();

        // Task to read incoming messages from broker
        var readTask = Task.Run(async () =>
        {
            await foreach (var message in call.ResponseStream.ReadAllAsync())
            {
                if (message.FileCommand != null)
                {
                    Console.WriteLine($"[Broker] Received command: {message.FileCommand.Action}");

                    var response = await _dispatcher.DispatchAsync(message.FileCommand);

                    await call.RequestStream.WriteAsync(new ClientMessage
                    {
                        ClientId = clientId,
                        FileResponse = new FileResponse
                        {
                            Success = true,
                            Message = $"Executed: {message.FileCommand.Action}",
                            Content = response.Content,
                        }
                    });
                }
            }
        });

        // Send initial registration + heartbeat periodically
        while (true)
        {
            await call.RequestStream.WriteAsync(new ClientMessage
            {
                ClientId = clientId,
                Heartbeat = new Heartbeat { Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() }
            });

            Console.WriteLine("[Client] Heartbeat sent...");
            await Task.Delay(3000);
        }
    }
}
