using Grpc.Net.Client;

namespace Game.Client.Services;

public class GameClientService : IAsyncDisposable
{
    public UserService UserService { get; }

    private readonly GrpcChannel _channel;
    private bool _disposed;

    public GameClientService(string serverAddress)
    {
        _channel = GrpcChannel.ForAddress(serverAddress);
        UserService = new UserService(_channel);
    }

    public async ValueTask DisposeAsync()
    {
        if (!_disposed)
        {
            await _channel.ShutdownAsync();
            _channel.Dispose();
            _disposed = true;
        }
    }
}