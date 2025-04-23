using Broker;
using Client;
using Client.Contracts;
using Client.Implementation;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

class Program
{
    static async Task Main(string[] args)
    {
        using IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton(_ => GrpcChannel.ForAddress("http://localhost:5000"));
                services.AddSingleton(sp =>
                {
                    var channel = sp.GetRequiredService<GrpcChannel>();
                    return new BrokerProto.BrokerProtoClient(channel);
                });

                services.AddSingleton<IFileOperation, ReadOperation>();
                services.AddSingleton<IFileOperation, WriteOperation>();
                services.AddSingleton<FileOperationHandler>();
                services.AddTransient<App>(); // Main app class
            })
            .Build();

        var app = host.Services.GetRequiredService<App>();
        await app.RunAsync();
    }
}
